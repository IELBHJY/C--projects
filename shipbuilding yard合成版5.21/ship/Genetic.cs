using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Collections;
using System.Reflection;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace ship
{
    class Genetic
    {
        Random rand = new Random();
        int popsize;//种群数量
        int chromolenth;//任务数量
        int trucklenth;//平板车数量
        int duiweino;//堆位数量
        int crossno;//路口数量
        double crossrate;//交叉率
        double mutaterate;//变异率
        string[] taskID;//指令ID
        string[] fenduanID;//分段ID
        int[] cross;//路口数量
        int[,] constrain;//任务先后约束
        int[,] position;//分段在序列中的位置
        double[] f;//适应度值
        int[] selected;//用于交叉操作的染色体
        double[] wheel;//轮盘
        int[] taskweight;//该任务所运输分段的吨位
        int[] taskwidth;//分段任务运输所需道路宽度
        int[] truckcapablity;//平板车运输能力
        int[] truckavail;//平板车是否可用，0为不可用，1为可用
        public int[,] pregegeration;//上一代
        public int[,] truckpregegeration;//平板车上一代
        int[,] nextgeneration;//下一代
        int[,] trucknextgeneration;//平板车下一代
        public int[] bestgene;//定义最优解
        public int[] besttruck;//定义平板车最优解
        public int[] distance;//空载行驶距离
        public double[] fitness;//适应度
        string[] duiweiID;//堆位ID
        string[] pathnumber;//路径对应的任务号
        string[] path1;//第一段堆场内路径
        string[] path2;//堆场间路径
        string[] path3;//第二段堆场内路径
        string[] originpath1;//原有第一段堆场内路径
        string[] originpath3;//原有第二段堆场内路径
        int[] origindistance1;//原有第一段堆场内路径距离
        int[] origindistance3;//原有第二段堆场内路径距离
        int[] distance1;//第一段堆场内路径距离
        int[] distance2;//堆场间路径距离
        int[] distance3;//第二段堆场内路径距离
        int[,] xy;//路口坐标
        int[,] duiweixy;//堆位坐标
        int[,] roadcapacity;//道路理论通行能力        
        int[,] roadsituation;//道路实际占用情况
        string exePath;//路径
        string pathd = "->";
        public Genetic(int populationsize, int tasknumber, int crossnumber, int trucklength, int duiweinumber)//Genetic构造函数
        {
            duiweino = duiweinumber;
            popsize = populationsize;
            chromolenth = tasknumber;
            crossno = crossnumber;
            crossrate = 0.8;
            mutaterate = 0.6;
            trucklenth = trucklength;
            f = new double[2 * popsize];
            selected = new int[popsize];
            wheel = new double[popsize + 1];
            pregegeration = new int[popsize, chromolenth];
            nextgeneration = new int[popsize, chromolenth];
            truckpregegeration = new int[popsize, chromolenth];
            trucknextgeneration = new int[popsize, chromolenth];
            constrain = new int[chromolenth, chromolenth];
            taskID = new string[chromolenth];
            fenduanID = new string[chromolenth];
            taskwidth = new int[chromolenth];
            taskweight = new int[chromolenth];
            bestgene = new int[chromolenth];
            besttruck = new int[chromolenth];
            cross = new int[crossno];
            distance = new int[popsize * 2];
            fitness = new double[2 * popsize];
            truckcapablity = new int[trucklenth];
            truckavail = new int[trucklenth];
            pathnumber = new string[2 * tasknumber];
            originpath1 = new string[tasknumber];
            originpath3 = new string[tasknumber];
            origindistance1 = new int[chromolenth];
            origindistance3 = new int[chromolenth];
            path1 = new string[2 * tasknumber];
            path2 = new string[2 * tasknumber];
            path3 = new string[2 * tasknumber];
            distance1 = new int[2 * tasknumber];
            distance2 = new int[2 * tasknumber];
            distance3 = new int[2 * tasknumber];
            xy = new int[2, crossno + 2];
            roadcapacity = new int[crossno + 2, crossno + 2];
            roadsituation = new int[crossno + 2, crossno + 2];
            duiweixy = new int[2, duiweino];
            duiweiID = new string[duiweino];


            //string exePath = System.Environment.CurrentDirectory;//本程序所在路径
            exePath = System.Windows.Forms.Application.ExecutablePath;
            //string exePath = exePath1
            int index;
            for (int i = 0; i < 4; i++)
            {
                index = exePath.LastIndexOf("\\");//exePath中最后一个“\\”的索引
                exePath = exePath.Substring(0, index);//取exePath中从0到index的字符串
            }
            //exePath = exePath + "\\";
        }
        public void initialtest()//读取access数据表数据
        {
            int i = 1;
            int j = 0;

            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exePath + @"\input database.accdb";
            OleDbConnection conn = new OleDbConnection(connstr);
            OleDbDataReader reader;
            for (i = 1; i < trucklenth + 1; i++)//读取平板车相关信息，平板车状态和额定载重
            {
                try
                {
                    string strCom = "Select * from 平板车信息 where 平板车ID='" + i.ToString() + "'";
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        truckavail[i - 1] = (int)reader["平板车状态"];
                        truckcapablity[i - 1] = (int)reader["额定载重"];
                    }
                    else
                    {
                        throw (new Exception("当前没有该记录！"));
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw (new Exception("数据库出错:" + e.Message));
                }
            }
            for (i = 1; i < chromolenth + 1; i++)//读取运输指令相关信息，将指令ID存储到taskID中，起始堆场内路径，目标堆场内路径
            {
                try
                {
                    string strCom = "Select * from 临时运输指令表 where 指令序号= " + i;
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        taskID[i - 1] = reader["指令ID"].ToString();
                        originpath1[i - 1] = reader["起始堆场内路径"].ToString();
                        originpath3[i - 1] = reader["目标堆场内路径"].ToString();
                        if ((originpath1[i - 1] == "") || (originpath3[i - 1] == ""))
                        {
                            throw (new Exception("当前没有该记录！"));
                        }
                        taskweight[i - 1] = (int)reader["分段所需运载能力"];
                        taskwidth[i - 1] = (int)reader["任务所需道路宽度"];
                        fenduanID[i - 1] = reader["分段ID"].ToString();
                        //Console.WriteLine("" + taskID[i - 1] + "分段所需运载能力" + taskweight[i-1] + "任务所需道路宽度" + taskwidth[i - 1]);
                    }
                    else
                    {
                        throw (new Exception("当前没有该记录！"));
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw (new Exception("数据库出错:" + e.Message));
                }
            }

            for (i = 1; i < chromolenth + 1; i++)//读取分段相关信息
            {

                //string query = "select * from 分段信息";
                //using (OleDbCommand cmd = new OleDbCommand(query, conn))
                //{
                //    conn.Open();
                //    OleDbDataReader reader1 = cmd.ExecuteReader();
                //    while (reader1.Read())
                //    {
                //        taskweight[i - 1] = (int)reader1["分段重量"];
                //        i++;
                //    }
                //    reader1.Close();
                //    conn.Close();
                //}
                try
                {
                    if (fenduanID[i - 1].Count() == 1)
                        fenduanID[i - 1] = "00" + fenduanID[i - 1];
                    if (fenduanID[i - 1].Count() == 2)
                        fenduanID[i - 1] = "0" + fenduanID[i - 1];
                    string strCom = "Select * from 分段信息 where 分段ID= '" + fenduanID[i - 1] + "'";
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        taskweight[i - 1] = (int)reader["分段重量"];
                    }
                    else
                    {
                        throw (new Exception("当前没有该分段'" + taskweight[i - 1] + "'记录！"));
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw (new Exception("数据库出错:" + e.Message));
                }
            }
            for (i = 1; i < crossno + 1; i++)//读取路口坐标信息
            {
                try
                {
                    string strCom = "Select * from 路口坐标信息 where 路口ID='" + i.ToString() + "'";
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        xy[0, i] = (int)reader["横坐标"];
                        xy[1, i] = (int)reader["纵坐标"];
                    }
                    else
                    {
                        throw (new Exception("当前没有该记录！"));
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw (new Exception("数据库出错:" + e.Message));
                }
            }
            int k = 0;
            int l = 0;
            for (i = 0; i < crossno + 2; i++)
            {
                for (j = 0; j < crossno + 2; j++)
                {
                    roadcapacity[i, j] = 0;
                    roadsituation[i, j] = 0;
                }
            }
            for (i = 1; i < 22; i++)//读取路段通行能力相关信息，路段ID
            {
                try
                {
                    string strCom = "Select * from 厂区道路信息 where 路段ID='" + i.ToString() + "'";
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        k = (int)reader["路口1"];
                        l = (int)reader["路口2"];
                        roadcapacity[k, l] = roadcapacity[l, k] = (int)reader["通行能力"];
                        roadsituation[k, l] = roadsituation[l, k] = (int)reader["实时占用情况"];
                    }
                    else
                    {
                        throw (new Exception("当前没有该记录！"));
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw (new Exception("数据库出错:" + e.Message));
                }
            }

            for (i = 1; i < duiweino + 1; i++)//读取堆位坐标
            {
                try
                {
                    string strCom = "Select * from 堆位信息 where 序号='" + "X" + i.ToString("0000") + "'";
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        duiweixy[0, i - 1] = (int)reader["堆位横坐标"];
                        duiweixy[1, i - 1] = (int)reader["堆位纵坐标"];
                        duiweiID[i - 1] = (string)reader["堆位ID"];
                    }
                    else
                    {
                        throw (new Exception("当前没有该记录！"));
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw (new Exception("数据库出错:" + e.Message));
                }
            }
            for (i = 0; i < chromolenth; i++)//读取运输指令相关信息
            {
                originpath1[i] = transferstart(originpath1[i]);
                originpath3[i] = transferstart(originpath3[i]);
            }
            for (i = 1; i < chromolenth; i++)//读取运输指令相关信息，i=1改成i=0.
            {
                origindistance1[i] = caldistance(originpath1[i]);
                origindistance3[i] = caldistance(originpath3[i]);
            }
        }
        /*string transferstart(string path)//
        {
            string transferpath = "";
            string each = "";
            string each1 = "";
            string x = "X";
            int number = 0;
            int i = 0;
            int j = 0;
            do
            {
                number = path.IndexOf("-");//查询第一个字符的位置
                j = 0;
                if (number == -1)
                {

                    number = path.Count();
                    j = 1;
                }
                each = path.Substring(0, number);//从第1个字符串开始取number个字符串
                for (i = 0; i < duiweino; i++)
                {
                    if (each == duiweiID[i])
                        break;
                }
                i = i + 1;
                each1 = i.ToString("0000");//将i转换成000i
                each1 = string.Concat(x, each1);//X000i
                transferpath = transferpath + each1;
                if (j == 1)
                    path = "";
                else
                    path = path.Substring(number+1);
            } while (path.Count() > 0);
            return transferpath;
        }  */
        string transferstart(string path)
        {
            //P7104-P7103-P6215-P6214-P6208-P6207
            string transferpath = "";
            string each = "";
            string each1 = "";
            string x = "X";
            int number = 0;
            int i = 0;
            int j = 0;
            do
            {
                number = path.IndexOf("-");//查询第一个字符的位置
                j = 0;
                if (number == -1)
                {
                    number = path.Count();
                    each = path.Substring(0, number);
                    j = 1;
                }
                each = path.Substring(0, number);//从第1个字符串开始取number个字符串  
                for (i = 0; i < duiweino; i++)
                {
                    if (each == duiweiID[i])
                        each1 = (i + 1).ToString("0000");
                    continue;
                }
                each1 = string.Concat(x, each1);//X000i
                transferpath = transferpath + each1;
                if (j == 1)
                {
                    path = "";
                }
                else
                {
                    path = path.Substring(number + 1);
                }
            } while (path.Count() > 0);
            return transferpath;
        }
        public void createfirstpop()//产生第一代种群
        {
            List<int> b = new List<int>();
            List<int> b1 = new List<int>();
            List<int> b2 = new List<int>();
            List<int> b3 = new List<int>();
            List<int> b4 = new List<int>();
            List<int> b5 = new List<int>();
            int k = 0;
            int n = 0;
            int[] s;
            s = new int[chromolenth];
            #region//平板车按运输能力分组
            for (k = 0; k < truckcapablity.Length; k++)
            {
                switch (truckcapablity[k])
                {
                    case 90:
                        if (truckavail[k] == 1)
                        {
                            b1.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 130:
                        if (truckavail[k] == 1)
                        {
                            b2.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 250:
                        if (truckavail[k] == 1)
                        {
                            b3.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 380:
                        if (truckavail[k] == 1)
                        {
                            b4.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 420:
                        if (truckavail[k] == 1)
                        {
                            b5.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 0:
                        break;

                }
            }
            #endregion
            //各种运输能力平板车的数量
            int[] count;
            count = new int[5];
            count[0] = b.Count;
            count[1] = b1.Count;
            count[2] = count[1] + b2.Count;
            count[3] = count[2] + b3.Count;
            count[4] = count[3] + b4.Count;
            //for (k = 0; k < truckcapablity.Length; k++)//平板车按运输能力分组
            //{
            //    Console.WriteLine("可用平板车" + truckavail[k]);
            //}
            List<int> a = new List<int>();//将任务储存在a中
            Random rd = new Random();
            for (int i = 0; i < popsize; i++)
            {
                for (k = 0; k < chromolenth; k++)
                {
                    a.Add(k);
                }
                for (k = 0; k < chromolenth; k++)
                {
                    s[k] = 0;
                }
                for (int j = 0; j < chromolenth; j++)
                {
                    //for (int l = 0; l < a.Count; l++)
                    //{
                    //    Console.Write(a[l] + " ");
                    //}
                    //Console.WriteLine(" ");
                    k = rd.Next(0, a.Count - 1);
                    //Console.WriteLine(a.Count + " " + k + " " + a[k]); 
                    pregegeration[i, j] = a[k];//------------------------------任务序列生成
                    #region//原代码注释掉的部分
                    /*
                if (b1.Count > 0)//平板车序列生成
                    if (taskweight[a[k]] < 90)
                    {
                        n = rd.Next(0, b1.Count);
                        truckpregegeration[i, j] = b1[n];
                        s[j] = 1;//已分配的任务标记为1
                    }
                if (b2.Count > 0)
                    if ((taskweight[a[k]] < 130) && (s[j] == 0))
                    {
                        n = rd.Next(0, b2.Count);
                        truckpregegeration[i, j] = b2[n];
                        s[j] = 1;
                    }
                if (b3.Count > 0)
                    if ((taskweight[a[k]] < 250) && (s[j] == 0))
                    {
                        n = rd.Next(0, b3.Count);
                        truckpregegeration[i, j] = b3[n];
                        s[j] = 1;
                    }
                if (b4.Count > 0)
                    if ((taskweight[a[k]] < 380) && (s[j] == 0))
                    {
                        n = rd.Next(0, b4.Count);
                        truckpregegeration[i, j] = b4[n];
                        s[j] = 1;
                    }
                if (b5.Count > 0)
                    if ((taskweight[a[k]] < 420) && (s[j] == 0))
                    {
                        n = rd.Next(0, b5.Count);
                        truckpregegeration[i, j] = b5[n];
                        s[j] = 1;
                    }*/
                    #endregion
                    if (b1.Count > 0)//----------------------------------------平板车序列生成
                        if (taskweight[a[k]] < 90)
                        {
                            n = rd.Next(0, count[0]);
                            truckpregegeration[i, j] = b[n];
                            s[j] = 1;//已分配的任务标记为1
                        }
                    if (b2.Count > 0)
                        if ((taskweight[a[k]] < 130) && (s[j] == 0))
                        {
                            n = rd.Next(count[1], count[0]);
                            truckpregegeration[i, j] = b[n];
                            s[j] = 1;
                        }
                    if (b3.Count > 0)
                        if ((taskweight[a[k]] < 250) && (s[j] == 0))
                        {
                            n = rd.Next(count[2], count[0]);
                            truckpregegeration[i, j] = b[n];
                            s[j] = 1;
                        }
                    if (b4.Count > 0)
                        if ((taskweight[a[k]] < 380) && (s[j] == 0))
                        {
                            n = rd.Next(count[3], count[0]);
                            truckpregegeration[i, j] = b[n];
                            s[j] = 1;
                        }
                    if (b5.Count > 0)
                        if ((taskweight[a[k]] < 420) && (s[j] == 0))
                        {
                            n = rd.Next(count[4], count[0]);
                            truckpregegeration[i, j] = b[n];
                            s[j] = 1;
                        }


                    if (s[j] == 0)
                    {
                        Console.WriteLine("任务" + (a[k] + 1) + "无可用平板车");
                        Environment.Exit(0);
                    }
                    a.RemoveAt(k);//将已分配的任务从列表中去除
                }
            }
        }
        public void rearrange()//求取任务间先后关系
        {
            int i, j, k, l, k1, k2;
            string[] yardnum;//------------------------------------------堆场内路径,与任务数量一致。
            yardnum = new string[chromolenth];
            string j1;
            string j2 = "X";
            string j3;
            int[] intertask;//-------------------------------------------干涉任务编号。
            intertask = new int[chromolenth];
            //初始化干涉任务
            for (i = 0; i < chromolenth; i++)
            {
                for (j = 0; j < chromolenth; j++)
                {
                    constrain[i, j] = 0;//------------------------------任务i是任务j的前置任务，则等于1。
                    //Console.WriteLine( i );
                }
            }
            for (j = 0; j < chromolenth; j++)//-------------------------将两段堆场内路径合并,最终储存在yardnum[] 中。
            {
                yardnum[j] = string.Concat(originpath1[j], originpath3[j]);
            }
            for (j = 1; j < duiweino + 1; j++)//堆位
            {
                k = 0;
                k1 = k2 = -1;
                j3 = j.ToString("0000");//-----------------------------------将j转换成000j.
                j1 = string.Concat(j2, j3);//--------------------------------j1=X000j
                //Console.Write("堆位号" + j1);
                for (i = 0; i < chromolenth; i++)//分段运输任务
                {
                    //yardnum = getpath(i); 
                    if (yardnum[i].Contains(j1))//若路径含有该堆位
                    {
                        //Console.WriteLine("任务号" + i+"任务路径" + yardnum[i]);
                        intertask[k] = i;
                        if (yardnum[i].IndexOf(j1) == yardnum[i].Length - j1.Length)//若该堆位是路径的目标堆位
                        {
                            k1 = k;
                            if (k2 < 0)
                                for (l = 0; l < k1; l++)
                                {
                                    constrain[intertask[l], intertask[k1]] = 1;
                                }
                            if (k2 >= 0)
                                for (l = k2 + 1; l < k1; l++)
                                {
                                    constrain[intertask[l], intertask[k1]] = 1;
                                    constrain[intertask[k2], intertask[l]] = 1;
                                }
                        }
                        if (yardnum[i].IndexOf(j1) == 0)//若该堆位是路径的起始堆位
                        {
                            k2 = k;
                            if (k2 > 0)
                            {
                                constrain[intertask[k - 1], i] = 1;
                            }
                        }
                        k = k + 1;
                    }
                }
                //Console.WriteLine("");
                //Console.WriteLine("堆位" + j + "的干涉为 ");
                //for (k = 0; k < intertask.Length; k++)
                //{
                //     Console.Write("任务" + (intertask[k] + 1));
                //}
                //Console.WriteLine(" ");          
                if (k2 > k1)//最后部分的先后关系
                {
                    if (k2 < chromolenth - 1)
                    {
                        for (l = k2 + 1; l < k; l++)
                        {
                            //Console.WriteLine(intertask[k2 - 1] + "" + intertask[l - 1]);
                            if (l > 0)
                                constrain[intertask[k2], intertask[l]] = 1;//先后约束
                        }
                    }
                }
            }
            for (i = 0; i < chromolenth; i++)//分段运输任务先后关系输出
            {
                Console.WriteLine("任务" + (i + 1) + "的前置任务为: ");
                for (j = 0; j < chromolenth; j++)
                {
                    if (constrain[j, i] > 0)
                        Console.Write("任务" + (j + 1) + " ");
                }
                Console.WriteLine(" ");
            }
            //for (i = 0; i < chromolenth; i++)//分段运输任务
            //{
            //    for (j = 0; j < chromolenth; j++)
            //    {
            //        Console.Write(constrain[i, j] + " ");
            //    }
            //    Console.WriteLine(" ");
            //}      
        }
        public void wheelselect()//轮盘赌选择较好染色体
        {
            int i, j, r;
            double sum = 0;
            for (i = 0; i < popsize; i++)
            {
                //sum = sum + (1000 - fitness[i]);
                //wheel[i + 1] = wheel[i] + (500 - fitness[i]);
                sum += fitness[i];
                wheel[i + 1] = wheel[i] + fitness[i];
            }
            for (i = 0; i < popsize; i++)
            {
                r = rand.Next((int)sum);
                for (j = 0; j < popsize; j++)
                {
                    if (r > wheel[j] && r < wheel[j + 1])
                    {
                        selected[i] = j;
                        break;
                    }
                }
                Console.WriteLine("selected" + i + " " + selected[i]);
            }
        }
        public void createnextpop()//产生下一代种群
        {
            //Console.WriteLine("休整后");
            //for (int i = 0; i < popsize; i++)
            //{
            //    //for (int j = 0; j < chromolenth; j++)
            //    //{
            //    //    Console.Write(pregeneration[i, j]);
            //    //}
            //    //Console.WriteLine(" ");
            //    for (int j = 0; j < chromolenth; j++)
            //    {
            //        Console.Write(truckpregegeration[i, j]);
            //    }
            //    Console.WriteLine(" ");
            //}
            for (int i = 0; i < popsize; i += 2)//交叉
            {
                //crossover(selected[i], selected[i + 1], i, i + 1);//将序号为selected[i]和selected[i + 1]的染色体进行交叉，产生的子代放在nextgeneration中i和i+1的位置
                crossover(i, i + 1, i, i + 1);//将序号为selected[i]和selected[i + 1]的染色体进行交叉，产生的子代放在nextgeneration中i和i+1的位置
            }
            //Console.WriteLine("交叉后");
            //for (int i = 0; i < popsize; i++)
            //{
            //    //for (int j = 0; j < chromolenth; j++)
            //    //{
            //    //    Console.Write(nextgeneration[i, j]);
            //    //}
            //    //Console.WriteLine(" ");
            //    for (int j = 0; j < chromolenth; j++)
            //    {
            //        Console.Write(trucknextgeneration[i, j]);
            //    }
            //    Console.WriteLine(" ");
            //}
            mutation(ref nextgeneration);//变异
            //Console.WriteLine("变异后");
            //for (int i = 0; i < popsize; i++)
            //{
            //    //for (int j = 0; j < chromolenth; j++)
            //    //{
            //    //    Console.Write(nextgeneration[i, j]);
            //    //}
            //    //Console.WriteLine(" ");
            //    for (int j = 0; j < chromolenth; j++)
            //    {
            //        Console.Write(trucknextgeneration[i, j]);
            //    }
            //    Console.WriteLine(" ");

            //}
        }
        void crossover(int p1, int p2, int c1, int c2)//交叉
        {
            int mutation1;
            int mutation2;
            int fornow;
            double dr = rand.NextDouble();
            //Console.WriteLine("交叉概率" + dr);
            for (int i = 0; i < chromolenth; i++)
            {
                nextgeneration[c1, i] = pregegeration[p1, i];
                nextgeneration[c2, i] = pregegeration[p2, i];
                trucknextgeneration[c1, i] = truckpregegeration[p1, i];
                trucknextgeneration[c2, i] = truckpregegeration[p2, i];
            }
            if (dr < crossrate)//判断是否小于交叉概率
            {
                mutation1 = rand.Next(0, chromolenth);//随机选取两个位置
                //Console.WriteLine("mutation1" + mutation1);
                do
                {
                    mutation2 = rand.Next(0, chromolenth);
                } while (mutation2 == mutation1);
                //Console.WriteLine("mutation1" + mutation1 + "mutation2" + mutation2 + "chromolenth" + chromolenth);
                if (mutation1 > mutation2)//保证mutation1较小
                {
                    fornow = mutation1;
                    mutation1 = mutation2;
                    mutation2 = fornow;
                }
                for (int i = mutation1; i < mutation2; i++)
                {
                    //nextgeneration[c1, i] = pregegeration[p2, i];
                    //nextgeneration[c2, i] = pregegeration[p1, i];
                    trucknextgeneration[c1, i] = truckpregegeration[p2, i];
                    trucknextgeneration[c2, i] = truckpregegeration[p1, i];
                }
            }
        }
        void mutation(ref int[,] curgeneration)//变异
        {
            int mutation1;
            int mutation2;
            int fornow;
            double dr;
            dr = rand.NextDouble();
            for (int i = 0; i < popsize; i++)
            {
                if (dr < 1)
                {
                    mutation1 = rand.Next(0, chromolenth);//随机选取两个位置
                    //Console.WriteLine("mutation1" + mutation1);
                    do
                    {
                        mutation2 = rand.Next(0, chromolenth);
                    } while (mutation2 == mutation1);
                    //Console.WriteLine("mutation1" + mutation1 + "mutation2" + mutation2 + "chromolenth" + chromolenth);
                    if (mutation1 > mutation2)//保证mutation1较小
                    {
                        fornow = mutation1;
                        mutation1 = mutation2;
                        mutation2 = fornow;
                    }
                    trucknextgeneration[i, mutation1] = truckpregegeration[i, mutation2];//平板车序列变异
                    trucknextgeneration[i, mutation2] = truckpregegeration[i, mutation1];//交换位置
                    for (int j = mutation1 + 1; j < mutation2 + 1; j++)
                    {
                        nextgeneration[i, j] = pregegeration[i, j - 1];//任务序列变异  
                        //trucknextgeneration[i, j] = truckpregegeration[i, j - 1];//平板车序列变异  
                    }
                }
            }
        }
        public void producenext()//产生下一代
        {
            int[,] temgeneration = new int[2 * popsize, chromolenth];
            int[,] trucktemgeneration = new int[2 * popsize, chromolenth];
            //将父代放入临时种群
            for (int i = 0; i <= popsize - 1; i++)
            {
                for (int j = 0; j <= chromolenth - 1; j++)
                {
                    temgeneration[i, j] = pregegeration[i, j];
                    trucktemgeneration[i, j] = truckpregegeration[i, j];
                }
            }
            //将子代放入临时种群
            for (int i = 0; i <= popsize - 1; i++)
            {
                for (int j = 0; j <= chromolenth - 1; j++)
                {
                    temgeneration[i + popsize, j] = nextgeneration[i, j];
                    trucktemgeneration[i + popsize, j] = trucknextgeneration[i, j];
                }
            }
            for (int i = popsize; i < 2 * popsize; i++)//计算适应度
            {
                calpath(trucktemgeneration, temgeneration, i);
            }
            int[] tem = new int[chromolenth];
            int[] temtruck = new int[chromolenth];
            double tem_f = 0;
            for (int i = 0; i <= 2 * popsize - 1; i++)//按适应度排序，最小的在最前面
            {
                for (int j = i + 1; j <= 2 * popsize - 1; j++)
                {
                    if (fitness[i] > fitness[j])
                    {
                        tem_f = fitness[i];
                        fitness[i] = fitness[j];
                        fitness[j] = tem_f;
                        for (int k = 0; k < chromolenth; k++)
                        {
                            tem[k] = temgeneration[i, k];
                            temgeneration[i, k] = temgeneration[j, k];
                            temgeneration[j, k] = tem[k];
                            temtruck[k] = trucktemgeneration[i, k];
                            trucktemgeneration[i, k] = trucktemgeneration[j, k];
                            trucktemgeneration[j, k] = temtruck[k];
                        }
                    }
                }
            }
            for (int i = 0; i <= popsize - 1; i++)//生成下一代
            {
                for (int j = 0; j <= chromolenth - 1; j++)
                {
                    pregegeration[i, j] = temgeneration[i, j];
                    truckpregegeration[i, j] = trucktemgeneration[i, j];
                }
            }
            //Console.WriteLine("挑选后");
            //for (int i = 0; i < popsize; i++)
            //{
            //    //for (int j = 0; j < chromolenth; j++)
            //    //{
            //    //    Console.Write(nextgeneration[i, j]);
            //    //}
            //    //Console.WriteLine(" ");
            //    for (int j = 0; j < chromolenth; j++)
            //    {
            //        Console.Write(truckpregegeration[i,j]);
            //    }
            //    Console.WriteLine(" ");
            //}
        }
        public void calpath(int[,] truck, int[,] task, int generationnumber)//通过path()函数计算距离
        {
            int j = 0;
            int i = 0;
            int geneno = generationnumber;
            int taskwidth1;
            int countnumber = 0;//计数
            int[] taskp;//任务在序列中的位置
            int[] cheno;//任务执行车的编号
            int[] position;//任务在平板车任务序列中的位置
            cheno = new int[chromolenth];
            position = new int[chromolenth];
            taskp = new int[chromolenth];
            List<List<int>> a = new List<List<int>>();//存任务号
            for (j = 0; j < trucklenth + 1; j++)
            {
                List<int> a1 = new List<int>();
                a.Add(a1);
            }
            for (j = 0; j < chromolenth; j++)//将任务分配到各平板车
            {
                for (int k = 0; k < trucklenth; k++)
                {
                    if (truck[geneno, j] == k)
                        a[k].Add(task[geneno, j]);//将各任务号分到各平板车
                }
            }
            int countdistance = 0;
            for (j = 0; j < trucklenth + 1; j++)//将任务编号及堆场距离存入
            {
                for (int k = 0; k < a[j].Count(); k++)
                {
                    pathnumber[countnumber] = a[j][k].ToString();//记录任务号
                    distance2[countnumber] = path(taskwidth[a[j][k]], a[j][k], a[j][k]);//计算堆场间距离
                    taskp[a[j][k]] = countnumber;//记录任务位置
                    cheno[a[j][k]] = j;//记录车号
                    position[a[j][k]] = k;//记录在序列中的位置
                    countnumber++;
                    if (k < a[j].Count() - 1)
                    {
                        pathnumber[countnumber] = a[j][k].ToString() + "to" + a[j][k + 1].ToString();//记录任务号
                        distance2[countnumber] = path(taskwidth[a[j][k]], a[j][k], a[j][k + 1]);//计算堆场间距离
                        countdistance = countdistance + distance2[countnumber];
                        countnumber++;
                    }
                }
            }
            taskwidth1 = 0;//空载平板车所需道路宽度
            int l = 0;
            for (j = 0; j < countnumber; j++)//负载任务堆场内路径赋值
            {
                if (!pathnumber[j].Contains("to"))
                {
                    l = Convert.ToInt32(pathnumber[j]);//取得任务号
                    path1[j] = originpath1[l];//取得路径
                    path3[j] = originpath3[l];
                    distance1[j] = origindistance1[l];//取得距离
                    distance3[j] = origindistance3[l];
                    //Console.WriteLine("堆场内道路" + path3[j] + "序号" + pathnumber[j]);
                }
            }
            for (j = 0; j < countnumber; j++)//空载任务堆场内路径赋值
            {
                if (pathnumber[j].Contains("to"))
                {
                    //Console.WriteLine("堆场内道路" + path3[j - 1] + "序号" + pathnumber[j]);
                    path1[j] = reversepath(path3[j - 1]);//取得路径
                    path3[j] = reversepath(path1[j - 1]);
                    distance1[j] = distance3[j - 1];//取得距离
                    distance3[j] = distance1[j + 1];
                }
            }
            double time1 = 0;//临时时间变量
            int task1 = 0;//临时任务变量
            int[] s;//该任务是否已计算完成
            s = new int[chromolenth];
            double[] starttime;//任务开始时间
            starttime = new double[chromolenth];
            double[] endtime;//任务结束时间
            endtime = new double[chromolenth];
            double[] trucktime;//各平板车时间
            trucktime = new double[countnumber];
            // int time2 = 0;//平板车起始工作时间
            List<int> tasklist = new List<int>();//任务序列，供寻找用
            for (i = 0; i < chromolenth; i++)//初始化
            {
                s[i] = 0;
                starttime[i] = 0;
                endtime[i] = 0;
                tasklist.Add(i);
            }
            for (i = 0; i < chromolenth; i++)//初始化
            {
                trucktime[i] = 0;
            }
            int count = 0;
            do
            {
                for (i = 0; i < tasklist.Count; i++)//寻找无前置任务的平板车运输任务
                {
                    if (caltask(s, tasklist[i]) == 0)
                    {
                        task1 = tasklist[i];
                        break;
                    }
                }
                count++;
                for (i = 0; i < chromolenth; i++)//计算前置任务的最大完成时间
                {
                    if (constrain[i, task1] == 1)
                        if (endtime[i] >= time1)
                            time1 = endtime[i];

                }
                if (position[task1] == 0)//判断是否是该平板车起始任务,为true则该任务是平板车第一个任务
                {
                    if (trucktime[cheno[task1]] >= time1)//计算任务开始时间
                        starttime[task1] = trucktime[cheno[task1]];
                    else
                        starttime[task1] = time1;
                    endtime[task1] = starttime[task1] + caltime(distance1[taskp[task1]], distance2[taskp[task1]], distance3[taskp[task1]], true, taskwidth[task1]);//计算任务结束时间
                    trucktime[cheno[task1]] = endtime[task1];
                    if (position[task1] < a[cheno[task1]].Count() - 1)//判断后一个是否是空载任务,为true则该任务是空载任务
                        trucktime[cheno[task1]] = trucktime[cheno[task1]] + caltime(distance1[taskp[task1] + 1], distance2[taskp[task1] + 1], distance3[taskp[task1] + 1], false, taskwidth1);//更新平板车可用时间
                }
                else
                {
                    if (trucktime[cheno[task1]] >= time1)//计算任务开始时间
                        starttime[task1] = trucktime[cheno[task1]];
                    else
                        starttime[task1] = time1;
                    endtime[task1] = starttime[task1] + caltime(distance1[taskp[task1]], distance2[taskp[task1]], distance3[taskp[task1]], true, taskwidth[task1]);//计算任务结束时间
                    trucktime[cheno[task1]] = endtime[task1];
                    if (position[task1] < a[cheno[task1]].Count() - 1)
                        trucktime[cheno[task1]] = trucktime[cheno[task1]] + caltime(distance1[taskp[task1] + 1], distance2[taskp[task1] + 1], distance3[taskp[task1] + 1], false, taskwidth1);//更新平板车可用时间
                }
                //Console.WriteLine("任务" + task1 + "开始时间" + starttime[task1]);
                //Console.WriteLine("任务" + task1 + "结束时间" + endtime[task1]);
                //Console.WriteLine("平板车" + cheno[task1] + "结束时间" + trucktime[cheno[task1]]);
                //Console.WriteLine("任务位置" + position[task1] + "判定位置" + (a[cheno[task1]].Count() - 1));
                s[task1] = 1;//标记任务已完成
                tasklist.Remove(task1);//从列表中清除该任务
                //计算任务开始时间与结束时间
            } while ((tasklist.Count() > 0) && (count < chromolenth + 2));
            if (tasklist.Count() > 0)
            {
                Console.WriteLine("任务先后关系有误");
                Environment.Exit(0);
            }
            double maxtime = 0;
            for (i = 0; i < chromolenth; i++)
            {
                if (endtime[i] >= maxtime)
                    maxtime = endtime[i];
            }
            distance[geneno] = countdistance;//总空载距离
            fitness[geneno] = maxtime + countdistance;//适应度
            //Console.WriteLine("适应度" + fitness[geneno] + "最大时间" + maxtime + "空载距离" + countdistance);
        }
        public void output(int[] truck, int[] task, ref string temppath22)//计算路径及距离
        {
            int j = 0;
            int i = 0;
            int taskwidth1;
            int countnumber = 0;//计数
            int[] taskp;//任务在序列中的位置
            int[] cheno;//任务执行车的编号
            int[] position;//任务在平板车任务序列中的位置
            cheno = new int[chromolenth];
            position = new int[chromolenth];
            taskp = new int[chromolenth];
            List<List<int>> a = new List<List<int>>();//存任务号
            MSExcel.Application app = null;
            MSExcel.Workbook wb = null;
            MSExcel.Worksheet ws = null;
            // MSExcel.Range r = null;
            //打开excel
            app = new Microsoft.Office.Interop.Excel.Application();


            string pathroad = exePath + @"\output.xlsx";
            if (File.Exists((string)pathroad))
            {
                File.Delete((string)pathroad);
            }
            //wb = app.Workbooks.Open(pathroad, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);//打开已有
            wb = app.Workbooks.Add(Type.Missing);//新建
            app.Visible = false;//读Excel不显示出来影响用户体验
            ws = (MSExcel.Worksheet)wb.Worksheets.get_Item(1); //得到WorkSheet对象
            /// <summary>
            /// 读取Excel中的内容
            /// </summary>
            ////读取A1单元格内容
            //r = ws.get_Range("A1", Type.Missing);
            //string strA1 = r.Value;
            ws.Cells[1, 1] = "任务号";
            ws.Cells[1, 2] = "开始时间";
            ws.Cells[1, 3] = "结束时间";
            ws.Cells[1, 4] = "平板车号";
            ws.Cells[1, 5] = "起始堆场内路径";
            ws.Cells[1, 6] = "堆场间路径";
            ws.Cells[1, 7] = "目标堆场内路径";
            for (j = 0; j < trucklenth + 1; j++)
            {
                List<int> a1 = new List<int>();
                a.Add(a1);
            }
            for (j = 0; j < chromolenth; j++)//将任务分配到各平板车
            {
                for (int k = 0; k < trucklenth; k++)
                {
                    if (truck[j] == k)
                        a[k].Add(task[j]);//将各任务号分到各平板车
                }
            }
            List<List<int>> c = new List<List<int>>();//存堆场内路径1
            for (j = 0; j < trucklenth + 1; j++)
            {
                List<int> c1 = new List<int>();
                c.Add(c1);
            }
            int countdistance = 0;
            for (j = 0; j < trucklenth + 1; j++)//将任务编号及堆场距离存入
            {
                for (int k = 0; k < a[j].Count(); k++)
                {
                    pathnumber[countnumber] = a[j][k].ToString();
                    c[j].Add(path(taskwidth[a[j][k]], a[j][k], a[j][k]));
                    distance2[countnumber] = path(taskwidth[a[j][k]], a[j][k], a[j][k]);
                    path2[countnumber] = pathd;
                    taskp[a[j][k]] = countnumber;
                    cheno[a[j][k]] = j;
                    position[a[j][k]] = k;
                    countnumber++;
                    if (k < a[j].Count() - 1)
                    {
                        pathnumber[countnumber] = a[j][k].ToString() + "to" + a[j][k + 1].ToString();
                        c[j].Add(path(taskwidth[a[j][k]], a[j][k], a[j][k + 1]));
                        distance2[countnumber] = path(taskwidth[a[j][k]], a[j][k], a[j][k + 1]);
                        path2[countnumber] = pathd;
                        countdistance = countdistance + distance2[countnumber];
                        countnumber++;
                    }
                }
            }
            taskwidth1 = 0;//空载平板车所需道路宽度    
            int l = 0;
            for (j = 0; j < countnumber; j++)//负载任务堆场内路径赋值
            {
                if (!pathnumber[j].Contains("to"))
                {
                    l = Convert.ToInt32(pathnumber[j]);
                    path1[j] = originpath1[l];
                    path3[j] = originpath3[l];
                    distance1[j] = origindistance1[l];
                    distance3[j] = origindistance3[l];
                    //Console.WriteLine("堆场内道路" + path3[j] + "序号" + pathnumber[j]);
                }
            }
            for (j = 0; j < countnumber; j++)//
            {
                if (pathnumber[j].Contains("to"))//空载任务堆场内路径赋值
                {
                    //Console.WriteLine("堆场内道路" + path3[j - 1] + "序号" + pathnumber[j]);
                    path1[j] = reversepath(path3[j - 1]);
                    path3[j] = reversepath(path1[j - 1]);
                    distance1[j] = distance3[j - 1];
                    distance3[j] = distance1[j + 1];
                }
            }
            double time1 = 0;//临时时间变量
            int task1 = 0;//临时任务变量
            int[] s;//该任务是否已计算完成
            s = new int[chromolenth];
            double[] starttime;//任务开始时间
            starttime = new double[chromolenth];
            double[] endtime;//任务结束时间
            endtime = new double[chromolenth];
            double[] trucktime;//各平板车时间
            trucktime = new double[countnumber];
            // int time2 = 0;//平板车起始工作时间
            List<int> tasklist = new List<int>();//任务序列，供寻找用
            for (i = 0; i < chromolenth; i++)//初始化
            {
                s[i] = 0;
                starttime[i] = 0;
                endtime[i] = 0;
                tasklist.Add(i);
            }
            for (i = 0; i < chromolenth; i++)//初始化
            {
                trucktime[i] = 0;
            }
            int count = 1;
            do
            {
                for (i = 0; i < tasklist.Count; i++)//寻找无前置任务的平板车运输任务
                {
                    if (caltask(s, tasklist[i]) == 0)
                    {
                        task1 = tasklist[i];
                        break;
                    }
                }
                count++;
                for (i = 0; i < chromolenth; i++)//计算前置任务的最大完成时间
                {
                    if (constrain[i, task1] == 1)
                        if (endtime[i] >= time1)
                            time1 = endtime[i];
                }
                if (position[task1] == 0)//判断是否是该平板车起始任务,为true则该任务是平板车第一个任务
                {
                    if (trucktime[cheno[task1]] >= time1)
                        starttime[task1] = trucktime[cheno[task1]];
                    else
                        starttime[task1] = time1;
                    endtime[task1] = starttime[task1] + caltime(distance1[taskp[task1]], distance2[taskp[task1]], distance3[taskp[task1]], true, taskwidth[task1]);
                    trucktime[cheno[task1]] = endtime[task1];
                    if (position[task1] < a[cheno[task1]].Count() - 1)//判断后一个是否是空载任务,为true则该任务是空载任务
                        trucktime[cheno[task1]] = trucktime[cheno[task1]] + caltime(distance1[taskp[task1] + 1], distance2[taskp[task1] + 1], distance3[taskp[task1] + 1], false, taskwidth1);
                }
                else
                {
                    if (trucktime[cheno[task1]] >= time1)
                        starttime[task1] = trucktime[cheno[task1]];
                    else
                        starttime[task1] = time1;
                    endtime[task1] = starttime[task1] + caltime(distance1[taskp[task1]], distance2[taskp[task1]], distance3[taskp[task1]], true, taskwidth[task1]);
                    trucktime[cheno[task1]] = endtime[task1];
                    if (position[task1] < a[cheno[task1]].Count() - 1)
                        trucktime[cheno[task1]] = trucktime[cheno[task1]] + caltime(distance1[taskp[task1] + 1], distance2[taskp[task1] + 1], distance3[taskp[task1] + 1], false, taskwidth1);
                }
                string finaldistance = "";
                finaldistance = string.Concat(path1[taskp[task1]], path2[taskp[task1]], path3[taskp[task1]]);
                ws.Cells[count, 1] = task1 + 1;
                ws.Cells[count, 2] = starttime[task1];
                ws.Cells[count, 3] = endtime[task1];
                ws.Cells[count, 4] = cheno[task1];
                ws.Cells[count, 5] = transfer(path1[taskp[task1]]);
                ws.Cells[count, 6] = path2[taskp[task1]];
                ws.Cells[count, 7] = transfer(path3[taskp[task1]]);
                Console.WriteLine("任务" + (task1 + 1) + "开始时间" + starttime[task1].ToString().PadLeft(5, ' ') + "结束时间" + endtime[task1].ToString().PadLeft(5, ' ') + "平板车" + cheno[task1]);
                Console.WriteLine("路径" + finaldistance);
                //Console.WriteLine("任务" + task1 + "结束时间" + endtime[task1]);
                //Console.WriteLine("平板车" + cheno[task1] + "结束时间" + trucktime[cheno[task1]]);
                //Console.WriteLine("任务位置" + position[task1] + "判定位置" + (a[cheno[task1]].Count() - 1));

                //输出到界面
                temppath22 = temppath22 + (task1 + 1).ToString() + "  " + starttime[task1]
                    + "  " + endtime[task1] + "  " + cheno[task1]
                    + "  " + transfer(path1[taskp[task1]]) + "  " + path2[taskp[task1]]
                    + "  " + transfer(path3[taskp[task1]]) + "\r\n";

                s[task1] = 1;
                tasklist.Remove(task1);
                //计算任务开始时间与结束时间
            } while (tasklist.Count() > 0);
            ws.SaveAs(pathroad, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.Close(null, null, null);
            app.Workbooks.Close();
            app.Application.Quit();
            app.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            ws = null;
            wb = null;
            app = null;
        }
        string transfer(string path)
        {
            string transferpath = "";
            string each = "";
            int number = 0;
            do
            {
                each = path.Substring(1, 4);
                number = Convert.ToInt32(each);
                transferpath = transferpath + duiweiID[number - 1] + "-";
                path = path.Substring(5);
            } while (path.Count() > 0);
            //int i = transferpath.Count() - 1;
            //transferpath = transferpath.Substring(0,i);
            return transferpath;
        }
        int caldistance(string path)
        {
            List<string> a = new List<string>();
            string identification = "X";
            string each;
            int output = 0;
            int startpoint = 0;
            startpoint = path.IndexOf(identification);
            while (startpoint < path.Length)
            {
                each = path.Substring(startpoint, 5);
                a.Add(each);
                startpoint = startpoint + 5;
            }
            int[,] blockcoordinate;//堆位坐标
            blockcoordinate = new int[2, a.Count()];
            int i = 0;
            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exePath + @"\input database.accdb";
            OleDbConnection conn = new OleDbConnection(connstr);
            OleDbDataReader reader;
            for (i = 0; i < a.Count(); i++)//读取堆位坐标
            {
                try
                {
                    string strCom = "Select * from 堆位信息 where 序号='" + a[i] + "'";
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        blockcoordinate[0, i] = (int)reader["堆位横坐标"];
                        blockcoordinate[1, i] = (int)reader["堆位纵坐标"];
                    }
                    else
                    {
                        throw (new Exception("当前没有该记录！"));
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw (new Exception("数据库出错:" + e.Message));
                }
            }
            for (i = 1; i < a.Count(); i++)//读取堆位坐标
            {
                output = output + Math.Abs(blockcoordinate[0, i] - blockcoordinate[0, i - 1]) + Math.Abs(blockcoordinate[1, i] - blockcoordinate[1, i - 1]);
            }
            return output;
        }
        string reversepath(string path)
        {
            string identification = "X";
            string each;
            string output = "";
            int startpoint = 0;
            startpoint = path.IndexOf(identification);
            do
            {
                each = path.Substring(startpoint, 5);
                output = string.Concat(each, output);
                startpoint = startpoint + 5;
            } while (startpoint < path.Length);
            return output;
        }
        double caltime(int distance1, int distance2, int distance3, bool task, double width)//计算任务时间，distance1、distance3是堆场内路径，distance2是堆场间路径，task是该任务是负载还是空载，width是指分段宽度
        {
            int i = 0;
            int j = 0;
            int k = 0;
            bool task1;
            i = distance1;
            j = distance2;
            k = distance3;
            task1 = task;
            double time;
            double ve;//平板车空载速度
            double vl;//平板车负载速度
            ve = 1;
            vl = 1;
            if (task1)//如果task1为true，表明任务为负载任务
                time = (distance1 + distance2 + distance3) / vl;
            else
                time = (distance1 + distance2 + distance3) / ve;
            return time;
        }
        int caltask(int[] s, int taskno)//计算某任务的先任务是否完成
        {
            int[] s1;//该任务是否已在计算
            s1 = new int[chromolenth];
            s1 = s;
            int j = taskno;
            int c = 0;
            for (int i = 0; i < chromolenth; i++)
            {
                if (constrain[i, j] == 1)
                    c = c + Math.Abs(s1[i] - constrain[i, j]);
            }
            return c;
        }
        public void modify()//修复任务基因
        {
            int i = 0;
            int j = 0;
            int l = 0;
            int max = 0;
            int i1 = 0;
            int j1 = 0;
            int[] k = new int[chromolenth];
            for (l = 0; l < popsize; l++)
            {
                do
                {
                    for (j = 0; j < chromolenth; j++)
                    {
                        k[pregegeration[l, j]] = j;//任务pregegeration[l, j]在任务序列中的位置
                    }
                    //for (i = 0; i < chromolenth; i++)
                    //{
                    //    for (j = 0; j < chromolenth; j++)
                    //    {
                    //        if (constrain[i, j] == 1)
                    //            Console.Write(k[i] - k[j] + " ");
                    //        else
                    //            Console.Write(0 + " ");
                    //    }
                    //    Console.WriteLine(" ");
                    //}
                    max = 0;
                    for (i = 0; i < chromolenth; i++)
                    {
                        for (j = 0; j < chromolenth; j++)
                        {
                            if (constrain[i, j] == 1)//任务i应该在任务j之前
                                if (k[i] > k[j])//如果任务j在任务i之前
                                    if (max < (k[i] - k[j]))
                                    {
                                        i1 = i;
                                        j1 = j;
                                        max = k[i] - k[j];//两个任务的位置之差
                                    }
                            //position[i, j] = -(k[i] - k[j]);//两个任务的位置之差，正说明i任务在前，负值说明j任务在后
                        }
                    }
                    if (max > 0)
                    {
                        int x = 0;
                        x = pregegeration[l, k[i1]];//将任务i1，i2互换
                        pregegeration[l, k[i1]] = pregegeration[l, k[j1]];
                        pregegeration[l, k[j1]] = x;
                        x = truckpregegeration[l, k[i1]];
                        truckpregegeration[l, k[i1]] = truckpregegeration[l, k[j1]];
                        truckpregegeration[l, k[j1]] = x;
                    }
                    //Console.WriteLine(max);
                    //for (i = 0; i < popsize; i++)
                    //{
                    //    for (j = 0; j < chromolenth; j++)
                    //    {
                    //        Console.Write(pregegeration[i, j] + " ");
                    //    }
                    //    Console.WriteLine(" ");
                    //    for (j = 0; j < chromolenth; j++)
                    //    {
                    //        Console.Write(truckpregegeration[i, j] + " ");
                    //    }
                    //    Console.WriteLine(" ");
                    //}
                } while (max > 0);
            }
            //for (i = 0; i < popsize; i++)
            //{
            //    for (j = 0; j < chromolenth; j++)
            //    {
            //        Console.Write(pregegeration[i, j] + " ");
            //    }
            //    Console.WriteLine(" ");
            //    for (j = 0; j < chromolenth; j++)
            //    {
            //        Console.Write(truckpregegeration[i, j] + " ");
            //    }
            //    Console.WriteLine(" ");
            //}

        }
        public void modifytruck()//修复平板车基因
        {
            List<int> b = new List<int>();
            List<int> b1 = new List<int>();
            List<int> b2 = new List<int>();
            List<int> b3 = new List<int>();
            List<int> b4 = new List<int>();
            List<int> b5 = new List<int>();
            int k = 0;
            int n = 0;
            Random rd = new Random();
            for (k = 0; k < truckcapablity.Length; k++)//平板车按运输能力分组
            {
                switch (truckcapablity[k])
                {
                    case 90:
                        if (truckavail[k] == 1)
                        {
                            b1.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 130:
                        if (truckavail[k] == 1)
                        {
                            b2.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 250:
                        if (truckavail[k] == 1)
                        {
                            b3.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 380:
                        if (truckavail[k] == 1)
                        {
                            b4.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 420:
                        if (truckavail[k] == 1)
                        {
                            b5.Add(k + 1);
                            b.Add(k + 1);
                        }
                        break;
                    case 0:
                        break;
                }
            }
            int[] count;
            count = new int[5];
            count[0] = b.Count;
            count[1] = b1.Count;
            count[2] = count[1] + b2.Count;
            count[3] = count[2] + b3.Count;
            count[4] = count[3] + b4.Count;
            for (int i = 0; i < popsize; i++)
            {
                for (int j = 0; j < chromolenth; j++)
                {
                    if (taskwidth[pregegeration[i, j]] > truckcapablity[truckpregegeration[i, j]])
                        do
                        {
                            /*
                            if (b1.Count > 0)
                                if (taskweight[pregegeration[i, j]] < 90)
                                {
                                    //n = rd.Next(0, b1.Count);
                                    n = b.Count - b1.Count;
                                    n1 = rd.Next(0, b.Count);
                                    truckpregegeration[i, j] = b1[n];
                                }
                            if (b2.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 130) && (taskweight[pregegeration[i, j]] >= 90))
                                {
                                    n = rd.Next(0, b2.Count);
                                    truckpregegeration[i, j] = b2[n];
                                }
                            if (b3.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 250) && (taskweight[pregegeration[i, j]] >= 130))
                                {
                                    n = rd.Next(0, b3.Count);
                                    truckpregegeration[i, j] = b3[n];
                                }
                            if (b4.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 380) && (taskweight[pregegeration[i, j]] >= 250))
                                {
                                    n = rd.Next(0, b4.Count);
                                    truckpregegeration[i, j] = b4[n];
                                }
                            if (b5.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 420) && (taskweight[pregegeration[i, j]] >= 380))
                                {
                                    n = rd.Next(0, b5.Count);
                                    truckpregegeration[i, j] = b5[n];
                                }*/
                            if (b1.Count > 0)
                                if (taskweight[pregegeration[i, j]] < 90)
                                {
                                    n = rd.Next(0, count[0]);
                                    truckpregegeration[i, j] = b[n];
                                }
                            if (b2.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 130) && (taskweight[pregegeration[i, j]] >= 90))
                                {
                                    n = rd.Next(count[1], count[0]);
                                    truckpregegeration[i, j] = b[n];
                                }
                            if (b3.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 250) && (taskweight[pregegeration[i, j]] >= 130))
                                {
                                    n = rd.Next(count[2], count[0]);
                                    truckpregegeration[i, j] = b[n];
                                }
                            if (b4.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 380) && (taskweight[pregegeration[i, j]] >= 250))
                                {
                                    n = rd.Next(count[3], count[0]);
                                    truckpregegeration[i, j] = b[n];
                                }
                            if (b5.Count > 0)
                                if ((taskweight[pregegeration[i, j]] < 420) && (taskweight[pregegeration[i, j]] >= 380))
                                {
                                    n = rd.Next(count[4], count[0]);
                                    truckpregegeration[i, j] = b[n];
                                }
                        } while (taskwidth[pregegeration[i, j]] <= truckcapablity[truckpregegeration[i, j]]);
                }
            }
        }

        private int Random(int trucklenth)
        {
            throw new NotImplementedException();
        }
        //原程序的path函数
        /*int path(double taskwidth, int taskno0, int tasknon)//计算距离。 tasknoO是任务号；
         {
             int i = 0;
             int j = 0;
             double roadwidth;//当前任务所需的道路宽度
             string[] a;
             a = new string[2];
             roadwidth = taskwidth;
             int[,] p;
             p = new int[2, 2];
             //for (i = 0; i < 2; i++)
             //{
             //    for (j = 1; j < crossno + 1; j++)
             //    {
             //        xy[i, j] = 0;
             //    }
             //}
             if (taskno0 == tasknon)
             {
                 //Console.WriteLine("任务号" + taskno0+"任务路径" + originpath1[taskno0]);
                 a[0] = originpath1[taskno0].Substring(originpath1[taskno0].Count() - 5, 5);
                 a[1] = originpath3[taskno0].Substring(0, 5);
             }
             else
             {
                 a[0] = originpath3[taskno0].Substring(0, 5);
                 //Console.WriteLine("任务号" + tasknon + "任务路径" + originpath1[tasknon]);
                 a[1] = originpath1[tasknon].Substring(originpath1[tasknon].Count() - 5, 5);
             }
             int d;
             string each;
             for (i = 0; i < a.Count(); i++)//读取堆位坐标
             {
                 each = a[i].Substring(a[i].Count() - 4, 4);
                 d = Convert.ToInt32(each) - 1;
                 p[0, i] = duiweixy[0, d];
                 p[1, i] = duiweixy[1, d];
                 //Console.WriteLine("任务号" + a[i] + "横坐标" + p[0, i] + "纵坐标" + p[1, i]);        
             }
             //将起始任务和终点任务的位置作为第1个路口和第17个路口
             xy[0, 0] = p[0, 0];
             xy[1, 0] = p[1, 0];
             xy[0, crossno + 1] = p[0, 1];
             xy[1, crossno + 1] = p[1, 1];
             //xy[0, 0] = xy[1, 0] = xy[0, crossno + 1] = xy[1, crossno + 1] = 0;//暂时对起点终点赋值
             //Console.WriteLine("起点横坐标" + xy[0, 0] + "起点纵坐标" + xy[1, 0] + "终点横坐标" + xy[0, 15] + "终点纵坐标" + xy[1, 15]);      
             int start0 = 0;
             int start1 = 0;
             int maxl = 1000;
             int length;
             int length1;
             for (i = 1; i < crossno + 1; i++)//求取距离起始位置距离最近的路口，并将其路口号赋值给start0。
             {
                 length = Math.Abs(xy[0, 0] - xy[0, i]) + Math.Abs(xy[1, 0] - xy[1, i]);
                 if (maxl >= length)
                 {
                     maxl = length;
                     start0 = i;
                 }
             }
             maxl = 1000;
             for (j = 0; j < crossno + 2; j++)
             {
                 roadcapacity[0, j] = roadcapacity[j, 0] = 0;
             }
             for (j = 0; j < crossno + 2; j++)
             {
                 roadcapacity[crossno + 1, j] = roadcapacity[j, crossno + 1] = 0;
             }
            for (i = 1; i < crossno + 1; i++)
             {
                 if (roadcapacity[i, start0] > 0)
                 {
                     if (xy[0, start0] == xy[0, i])//如果第i点和start0点横坐标相同
                     {
                         length = Math.Abs(xy[0, 0] - xy[0, start0]);
                         length1 = Math.Abs(xy[1, i] - xy[1, start0]) - Math.Abs(xy[1, 0] - xy[1, start0]) - Math.Abs(xy[1, 0] - xy[1, i]);
                     }
                     else
                     {
                         length = Math.Abs(xy[1, 0] - xy[1, start0]);//还需要把路口限定在两路口之间，赋值有问题；
                         length1 = Math.Abs(xy[0, i] - xy[0, start0]) - Math.Abs(xy[0, 0] - xy[0, start0]) - Math.Abs(xy[0, 0] - xy[0, i]);
                     }
                     if (length1 == 0)//初始位置的点位于第i点和start0点之间。但不一定在一条直线上。
                     {
                         if (maxl >= length)//在距离初始距离最近的点中，求取横坐标或者纵坐标距离最近的点 赋值给start1。
                         {
                             maxl = length;
                             start1 = i;
                         }
                     }
                 }
             }  
             int end0 = 0;
             int end1 = 0;
             maxl = 1000;
             for (i = 1; i < crossno + 1; i++)
             {
                 length = Math.Abs(xy[0, crossno + 1] - xy[0, i]) + Math.Abs(xy[1, crossno + 1] - xy[1, i]);
                 if (maxl >= length)//搜索距离结束位置最小的点，将其赋值给end0。
                 {
                     maxl = length;
                     end0 = i;
                 }
             }
             maxl = 1000;
             for (i = 1; i < crossno + 1; i++)
             {
                 if (roadcapacity[i, end0] > 0)
                 {
                     if (xy[0, end0] == xy[0, i])
                     {
                         length = Math.Abs(xy[0, crossno + 1] - xy[0, end0]);
                         length1 = Math.Abs(xy[1, i] - xy[1, end0]) - Math.Abs(xy[1, crossno + 1] - xy[1, end0]) - Math.Abs(xy[1, crossno + 1] - xy[1, i]);
                     }
                     else
                     {
                         length = Math.Abs(xy[1, crossno + 1] - xy[1, end0]);//还需要把路口限定在两路口之间，赋值有问题
                         length1 = Math.Abs(xy[0, i] - xy[0, end0]) - Math.Abs(xy[0, crossno + 1] - xy[0, end0]) - Math.Abs(xy[0, crossno + 1] - xy[0, i]);
                     }
                     if (length1 == 0)//如果结束位置位于第i点和end0点之间，搜索距离结束位置的横坐标或者纵坐标距离最小的点，将其赋值给end1。
                     {
                         if (maxl >= length)
                         {
                             maxl = length;
                             end1 = i;
                         }
                     }
                 }
             }
             int index = 0;
             if ((start0 == end0) && (start1 == end1))
             {
                 index = 1;
             }
             if ((start0 == end1) && (start1 == end0))
             {
                 index = 1;
             }
             //Console.WriteLine(start0 + " " + start1 + " " + end0 + " " + end1);
             //道路理论通行能力
             roadcapacity[0, start0] = roadcapacity[0, start1] = roadcapacity[start0, 0] = roadcapacity[start1, 0] = roadcapacity[start0, start1];
             roadcapacity[end0, crossno + 1] = roadcapacity[end1, crossno + 1] = roadcapacity[crossno + 1, end0] = roadcapacity[crossno + 1, end1] = roadcapacity[end0, end1];
             //道路理论通行能力
             //for (i = 0; i < crossno; i++)
             //{
             //    for (j = 0; j < crossno; j++)
             //    {
             //        roadcapacity[i, j] = 0;
             //    }
             //} 
             //起点终点的插入
             int[,] roadchoice;
             roadchoice = new int[crossno + 2, crossno + 2];//可通行道路情况
             for (i = 0; i < crossno + 2; i++)
             {
                 for (j = 0; j < crossno + 2; j++)
                 {
                     roadchoice[i, j] = 0;
                 }
             }
             for (i = 0; i < crossno + 2; i++)
             {
                 for (j = 0; j < crossno + 2; j++)
                 {
                     if ((roadcapacity[i, j] - roadsituation[i, j]) > roadwidth)
                         roadchoice[i, j] = 1;
                     else
                         roadchoice[i, j] = 0;
                 }
             }
             //for (i = 0; i < crossno + 2; i++)//输出可通行道路情况
             //{

             //    for (j = 0; j < crossno + 2; j++)
             //    {

             //        Console.Write(roadchoice[i, j] + " ");
             //    }
             //    Console.WriteLine(" ");
             //}
             int[,] c;
             c = new int[crossno + 2, crossno + 2];
             #region //路口间距离
             for (i = 1; i < crossno + 1; i++)
             {
                 if (i==1)
                 {
                     for (j = 1; j < 8; j++)
                     {
                         if (roadchoice[i, j] == 1)
                             c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
                         for (j = 8; j < 11; j++)
                         {
                             if (roadchoice[i, j] == 1)
                                 c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 11]) + Math.Abs(xy[1, 1] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                             else
                                 c[i, j] = 10000;
                         }
                         for (j = 11; j < 13; j++)
                         {
                             if (roadchoice[i, j] == 1)
                                 c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                             else
                                 c[i, j] = 10000;
                         }
                             for (j = 13; j < 16; j++)
                             {
                                 if (roadchoice[i, j] == 1)
                                     c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 16]) + Math.Abs(xy[1, 1] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                                 else
                                     c[i, j] = 10000;
                             }
                             for (j = 16; j < crossno + 1; j++)
                             {
                                 if (roadchoice[i, j] == 1)
                                     c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                                 else
                                     c[i, j] = 10000;
                             }
                 }
                 if (i == 3)
                 {
                     for (j = 1; j < 8; j++)
                     {
                         if (roadchoice[i, j] == 1)
                             c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
                     for (j = 8; j < 11; j++)
                     {
                         if (roadchoice[i, j] == 1)
                             c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 11]) + Math.Abs(xy[1, 3] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
                     for (j = 11; j < 13; j++)
                     {
                         if (roadchoice[i, j] == 1)
                             c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
                     for (j = 13; j < 16; j++)
                     {
                         if (roadchoice[i, j] == 1)
                             c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 16]) + Math.Abs(xy[1, 3] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
                     for (j = 16; j < crossno + 1; j++)
                     {
                         if (roadchoice[i, j] == 1)
                             c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
                 }
                 else
                 {
                     for (j = 1; j < crossno + 1; j++)
                     {
                         if (roadchoice[i, j] == 1)
                             c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
                 }
             }
             /*
               for (i = 0; i < crossno + 2; i++)
              {
                 for (j = 0; j < crossno + 2; j++)
                     {
                         if (roadchoice[i, j] == 1)//此处定义距离有错误。
                             c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                         else
                             c[i, j] = 10000;
                     }
              }
             //求取初始位置与其他路口的距离
             for (i = 1; i < crossno + 1; i++)
             {
                 if (roadcapacity[0, i] == 1)
                 {
                     if (roadcapacity[i, start0] > 0)
                     {
                         if (xy[0, start0] == xy[0, i])//如果第i点和start0点横坐标相同
                         {
                             length = Math.Abs(xy[0, 0] - xy[0, start0]);
                             length1 = Math.Abs(xy[1, i] - xy[1, start0]) - Math.Abs(xy[1, 0] - xy[1, start0]) - Math.Abs(xy[1, 0] - xy[1, i]);
                         }
                         else
                         {
                             length = Math.Abs(xy[1, 0] - xy[1, start0]);//还需要把路口限定在两路口之间，赋值有问题；
                             length1 = Math.Abs(xy[0, i] - xy[0, start0]) - Math.Abs(xy[0, 0] - xy[0, start0]) - Math.Abs(xy[0, 0] - xy[0, i]);
                         }
                         if (length1 == 0)//初始位置的点位于第i点和start0点之间。但不一定在一条直线上。
                         {
                             if (maxl >= length)//在距离初始距离最近的点中，求取横坐标或者纵坐标距离最近的点 赋值给start1。
                             {
                                 maxl = length;
                                 start1 = i;
                             }
                           c[0, i] = c[start1, i] - c[0, start1];
                         }
                         else if (length1 != 0)
                         { 
                         c[0,i]=c[0,start1]+c[start1,i];
                         }
                     }
                 }
             }
             //求取终点位置与其他路口的距离
             for (i = 1; i < crossno + 1; i++)
             {
                 if (roadcapacity[i, end0] > 0)
                 {
                     if (xy[0, end0] == xy[0, i])
                     {
                         length = Math.Abs(xy[0, crossno + 1] - xy[0, end0]);
                         length1 = Math.Abs(xy[1, i] - xy[1, end0]) - Math.Abs(xy[1, crossno + 1] - xy[1, end0]) - Math.Abs(xy[1, crossno + 1] - xy[1, i]);
                     }
                     else
                     {
                         length = Math.Abs(xy[1, crossno + 1] - xy[1, end0]);//还需要把路口限定在两路口之间，赋值有问题
                         length1 = Math.Abs(xy[0, i] - xy[0, end0]) - Math.Abs(xy[0, crossno + 1] - xy[0, end0]) - Math.Abs(xy[0, crossno + 1] - xy[0, i]);
                     }
                     if (length1 == 0)//如果结束位置位于第i点和end0点之间，搜索距离结束位置的横坐标或者纵坐标距离最小的点，将其赋值给end1。
                     {
                         if (maxl >= length)
                         {
                             maxl = length;
                             end1 = i;
                         }
                         c[i,crossno+1]=c[i,end1]-c[end1,crossno+1];
                     }
                     else if (length != 0)
                     { 
                     c[i,crossno+1]=c[i,end1]+c[end1,crossno+1];
                     }
                 }
             } 

             int[] dist = new int[crossno + 2];
             int[] prev = new int[crossno + 2];
             int[] s = new int[crossno + 2];
             for (i = 0; i <= crossno + 1; i++)
             {
                 dist[i] = c[0, i];
                 s[i] = 0;     //初始都未用过该点
                 if (dist[i] == 10000)
                     prev[i] = 100;
                 else
                     prev[i] = 0;
             }
             dist[0] = 0;
             s[0] = 1;
             //依次将未放入S集合的结点中，取dist[]最小值的结点，放入结合S中
             //一旦S包含了所有V中顶点，dist就记录了从源点到所有其他顶点之间的最短路径长度
             for (i = 1; i <= crossno + 1; i++)
             {
                 int tmp = 10000;
                 int u = 0;
                 //找出当前未使用的点j的dist[j]最小值
                 for (j = 1; j <= crossno + 1; j++)
                     if ((s[j] == 0) && dist[j] < tmp)
                     {
                         u = j;              //u保存当前邻接点中距离最小的点的号码
                         tmp = dist[j];
                     }
                 s[u] = 1;    //表示u点已存入S集合中
                 //更新dist
                 for (j = 1; j <= crossno + 1; j++)
                 {
                     if ((s[j] == 0) && c[u, j] < 10000)
                     {
                         int newdist = dist[u] + c[u, j];
                         if (newdist < dist[j])
                         {
                             dist[j] = newdist;
                             prev[j] = u;
                         }
                     }
                 }
             }
             //for (i = 0; i <= crossno + 1; i++)
             //{
             //    for (j = 0; j <= crossno + 1; j++)
             //    {
             //        if(c[i,j]<10000)
             //            Console.Write(c[i,j]+ " ");
             //        else
             //            Console.Write("0 ");

             //    }
             //    Console.WriteLine();
             //}
             //for (j = 1; j <= crossno + 1; j++)
             //{
             //    Console.WriteLine("平板车行驶距离" +j+" "+ dist[j]);
             //}
             int[] que = new int[crossno + 2];
             int tot = 1;
             que[tot] = crossno + 1;
             tot++;
             int tp = prev[crossno + 1];
             if (prev[crossno + 1] == 100)
             {
                 if (taskno0 == tasknon)
                 {
                     Console.WriteLine("任务号" + (taskno0 + 1) + "路径无法通行");
                 }
                 else
                 {
                     Console.WriteLine("任务号" + (taskno0 + 1) + "与任务" + (tasknon + 1) + "路径无法通行");
                 }
                 Environment.Exit(0);
             }
             while ((tp != 0) && (tp < 20))
             {
                 que[tot] = tp;
                 tot++;
                 //Console.WriteLine("标识" + tp);
                 tp = prev[tp];
             }
             que[tot] = 0;
             pathd = "";
             string lukou;
             for (i = tot - 1; i > 1; i--)
             {
                 lukou = string.Concat(que[i].ToString(), ">");
                 pathd = string.Concat(pathd, lukou);
             }
             i = pathd.Count() - 1;
             pathd = pathd.Substring(0, i);
             if (index == 1)
             {
                 pathd = "";
             }
             //Console.WriteLine("平板车行驶距离" + dist[crossno + 1]);
             //for (i = tot; i >= 1; i--)
             //{
             //    if (i != 1)
             //        Console.Write("路口" + que[i] + "->");
             //    else
             //        Console.WriteLine("路口" + que[i]);

             //}
             return dist[crossno + 1];
         }  */
        //我自己编写的path函数
        /// <summary>
        /// 计算堆场间距离
        /// </summary>
        /// <param name="taskwidth">任务需要的道路宽度</param>
        /// <param name="tasknoO"></param>
        /// <param name="tasknon"></param>
        /// <returns>距离值</returns>
        int path(double taskwidth, int taskno0, int tasknon)
        {
            #region
            int i = 0;
            int j = 0;
            double roadwidth;//当前任务所需的道路宽度
            string[] a;
            a = new string[2];
            roadwidth = taskwidth;
            int[,] p;
            p = new int[2, 2];
            if (taskno0 == tasknon)
            {
                //Console.WriteLine("任务号" + taskno0+"任务路径" + originpath1[taskno0]);
                a[0] = originpath1[taskno0].Substring(originpath1[taskno0].Count() - 5, 5);
                a[1] = originpath3[taskno0].Substring(0, 5);
            }
            else
            {
                a[0] = originpath3[taskno0].Substring(0, 5);
                //Console.WriteLine("任务号" + tasknon + "任务路径" + originpath1[tasknon]);
                a[1] = originpath1[tasknon].Substring(originpath1[tasknon].Count() - 5, 5);
            }
            int d;
            string each;
            for (i = 0; i < a.Count(); i++)//读取堆位坐标
            {
                each = a[i].Substring(a[i].Count() - 4, 4);
                d = Convert.ToInt32(each) - 1;
                p[0, i] = duiweixy[0, d];
                p[1, i] = duiweixy[1, d];
            }
            //将起始任务和终点任务的位置作为第1个路口和第17个路口
            xy[0, 0] = p[0, 0];
            xy[1, 0] = p[1, 0];
            xy[0, crossno + 1] = p[0, 1];
            xy[1, crossno + 1] = p[1, 1];
            //初始化道路通行情况
            for (j = 0; j < crossno + 2; j++)
            {
                roadcapacity[0, j] = roadcapacity[j, 0] = 0;
            }
            for (j = 0; j < crossno + 2; j++)
            {
                roadcapacity[crossno + 1, j] = roadcapacity[j, crossno + 1] = 0;
            }
            //求取距离起始位置距离最近的路口，并将其路口号赋值给start1。
            int start0 = 0;
            int start1 = 0;
            int maxl = 1000;
            int slength = 0;
            int slength1 = 0;
            for (i = 1; i < crossno + 1; i++)
            {
                slength = Math.Abs(xy[0, 0] - xy[0, i]) + Math.Abs(xy[1, 0] - xy[1, i]);
                if (maxl >= slength)
                {
                    maxl = slength;
                    start0 = i;//start0记录距离初始位置距离最近的路口号
                }
            }
            maxl = 1000;
            for (i = 1; i < crossno + 1; i++)
            {
                if (roadcapacity[i, start0] > 0)
                {
                    if (xy[0, start0] == xy[0, i])//如果第i点和start0点横坐标相同
                    {
                        slength = Math.Abs(xy[0, 0] - xy[0, start0]);
                        slength1 = Math.Abs(xy[1, i] - xy[1, start0]) - Math.Abs(xy[1, 0] - xy[1, start0]) - Math.Abs(xy[1, 0] - xy[1, i]);
                    }
                    else if (xy[1, start0] == xy[1, i])
                    {
                        slength = Math.Abs(xy[1, 0] - xy[1, start0]);//还需要把路口限定在两路口之间，赋值有问题；
                        slength1 = Math.Abs(xy[0, i] - xy[0, start0]) - Math.Abs(xy[0, 0] - xy[0, start0]) - Math.Abs(xy[0, 0] - xy[0, i]);
                    }
                    if (slength1 == 0)//初始位置的点位于第i点和start0点之间。但不一定在一条直线上。
                    {
                        if (maxl >= slength)//在距离初始距离最近的点中，求取横坐标或者纵坐标距离最近的点 赋值给start1。
                        {
                            maxl = slength;
                            start1 = i;
                        }
                    }
                }
            }
            //求取距离结束位置最近的点，并将其赋值给end1。
            int end0 = 0;
            int end1 = 0;
            maxl = 1000;
            int elength = 0;
            int elength1 = 0;
            for (i = 1; i < crossno + 1; i++)
            {
                elength = Math.Abs(xy[0, crossno + 1] - xy[0, i]) + Math.Abs(xy[1, crossno + 1] - xy[1, i]);
                if (maxl >= elength)//搜索距离结束位置最小的点，将其赋值给end0。
                {
                    maxl = elength;
                    end0 = i;
                }
            }
            maxl = 1000;
            for (i = 1; i < crossno + 1; i++)
            {
                if (roadcapacity[i, end0] > 0)
                {
                    if (xy[0, end0] == xy[0, i])
                    {
                        elength = Math.Abs(xy[0, crossno + 1] - xy[0, end0]);
                        elength1 = Math.Abs(xy[1, i] - xy[1, end0]) - Math.Abs(xy[1, crossno + 1] - xy[1, end0]) - Math.Abs(xy[1, crossno + 1] - xy[1, i]);
                    }
                    else
                    {
                        elength = Math.Abs(xy[1, crossno + 1] - xy[1, end0]);//还需要把路口限定在两路口之间，赋值有问题
                        elength1 = Math.Abs(xy[0, i] - xy[0, end0]) - Math.Abs(xy[0, crossno + 1] - xy[0, end0]) - Math.Abs(xy[0, crossno + 1] - xy[0, i]);
                    }
                    if (elength1 == 0)//如果结束位置位于第i点和end0点之间，搜索距离结束位置的横坐标或者纵坐标距离最小的点，将其赋值给end1。
                    {
                        if (maxl >= elength)
                        {
                            maxl = elength;
                            end1 = i;
                        }
                    }
                }
            }
            int index = 0;
            if ((start0 == end0) && (start1 == end1))
            {
                index = 1;
            }
            if ((start0 == end1) && (start1 == end0))
            {
                index = 1;
            }
            //
            roadcapacity[0, start0] = roadcapacity[0, start1] = roadcapacity[start0, 0] = roadcapacity[start1, 0] = roadcapacity[start0, start1];
            roadcapacity[end0, crossno + 1] = roadcapacity[end1, crossno + 1] = roadcapacity[crossno + 1, end0] = roadcapacity[crossno + 1, end1] = roadcapacity[end0, end1];
            //起点终点道路插入
            int[,] roadchoice;
            roadchoice = new int[crossno + 2, crossno + 2];//可通行道路情况
            for (i = 0; i < crossno + 2; i++)
            {
                for (j = 0; j < crossno + 2; j++)
                {
                    roadchoice[i, j] = 0;
                }
            }
            for (i = 0; i < crossno + 2; i++)
            {
                for (j = 0; j < crossno + 2; j++)
                {
                    if ((roadcapacity[i, j] - roadsituation[i, j]) > roadwidth)
                        roadchoice[i, j] = 1;
                    else
                        roadchoice[i, j] = 0;
                }
            }
            #endregion
            #region //路口间距离
            int[,] c;
            c = new int[crossno + 2, crossno + 2];
            for (i = 1; i < crossno + 1; i++)
            {
                if (i == 1)
                {
                    for (j = 1; j < 8; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 8; j < 11; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 11]) + Math.Abs(xy[1, 1] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 11; j < 13; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 13; j < 16; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 16]) + Math.Abs(xy[1, 1] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                }
                if (i == 3)
                {
                    for (j = 1; j < 8; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 8; j < 11; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 11]) + Math.Abs(xy[1, 3] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 11; j < 13; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 13; j < 16; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 16]) + Math.Abs(xy[1, 3] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                }
                else
                {
                    for (j = 1; j < crossno + 1; j++)
                    {
                        if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        else
                            c[j, i] = c[i, j] = 10000;
                    }
                }
            }
            #endregion
            #region//求取初始位置，结束位置与其他路口的距离
            //求取初始位置与其他路口的距离
            for (i = 1; i < crossno + 1; i++)
            {
                if (roadcapacity[0, i] == 1)
                {
                    if (roadcapacity[i, start1] > 0)
                    {
                        if (slength == 0)
                        {
                            c[i, 0] = c[0, i] = c[start1, i] - c[0, start1];
                        }
                        else if (slength1 != 0)
                        {
                            c[i, 0] = c[0, i] = c[0, start1] + c[start1, i];
                        }
                    }
                }
            }
            //求取终点位置与其他路口的距离
            for (i = 1; i < crossno + 1; i++)
            {
                if (roadcapacity[i, crossno + 1] == 1)
                {

                    if (roadcapacity[i, end1] > 0)
                    {
                        if (elength1 == 0)
                        {
                            c[crossno + 1, i] = c[i, crossno + 1] = c[i, end1] - c[end1, crossno + 1];
                        }
                        else if (elength != 0)
                        {
                            c[crossno + 1, i] = c[i, crossno + 1] = c[i, end1] + c[end1, crossno + 1];
                        }
                    }
                }
            }
            //求取初始位置与结束位置之间的距离
            c[0, crossno + 1] = c[0, start1] + c[start1, end1] + c[end1, crossno + 1];

            #endregion
            #region //搜索最小距离
            int[] dist = new int[crossno + 2];
            int[] prev = new int[crossno + 2];
            int[] s = new int[crossno + 2];
            for (i = 0; i <= crossno + 1; i++)
            {
                dist[i] = c[0, i];
                s[i] = 0;     //初始都未用过该点
                if (dist[i] == 10000)
                    prev[i] = 100;
                else
                    prev[i] = 0;
            }
            dist[0] = 0;
            s[0] = 1;
            //初始化转向次数
            int turnCount = 0;
            //arrayU数组用于存放每次搜索时得到的最优点
            int[] arrayU = new int[crossno + 2];
            //arrayU数组的第一个值是初始位置点
            arrayU[0] = 0;
            //依次将未放入S集合的结点中，取dist[]最小值的结点，放入集合S中
            //一旦S包含了所有V中顶点，dist就记录了从源点到所有其他顶点之间的最短路径长度
            for (i = 1; i <= crossno + 1; i++)
            {
                int tmp = 10000;
                int u = 0;
                //找出当前未使用的点j的dist[j]最小值
                for (j = 1; j <= crossno + 1; j++)
                {
                    if ((s[j] == 0) && dist[j] < tmp)
                    {
                        u = j;              //u保存当前邻接点中距离最小的点的号码
                        tmp = dist[j];
                    }
                    s[u] = 1;    //表示u点已存入S集合中
                }
                //将第i次搜索到的点j存入arrayU数组中。
                arrayU[i] = u;
                //更新dist
                for (j = 1; j <= crossno + 1; j++)
                {
                    if ((s[j] == 0) && c[u, j] < 10000)
                    {
                        int newdist = dist[u] + c[u, j];
                        if (newdist < dist[j])
                        {
                            dist[j] = newdist;
                            prev[j] = u;
                            dist[j] = newdist;
                        }
                    }
                }
            }
            //依据求取的最优路径集合arrayU[]。计算最优路径中转向次数。
            for (i = 1; i < crossno + 1; i++)
            {
                if ((xy[0, arrayU[i - 1]] - xy[0, arrayU[i]] == 0) && (xy[1, arrayU[i]] - xy[1, arrayU[i + 1]]) == 0)
                {
                    turnCount += 1;
                }
                if ((xy[1, arrayU[i - 1]] - xy[1, arrayU[i]] == 0) && (xy[0, arrayU[i]] - xy[0, arrayU[i + 1]]) == 0)
                {
                    turnCount += 1;
                }
            }
            int[] que = new int[crossno + 2];
            int tot = 1;
            que[tot] = crossno + 1;
            tot++;
            int tp = prev[crossno + 1];
            if (prev[crossno + 1] == 100)
            {
                if (taskno0 == tasknon)
                {
                    Console.WriteLine("任务号" + (taskno0 + 1) + "路径无法通行");
                }
                else
                {
                    Console.WriteLine("任务号" + (taskno0 + 1) + "与任务" + (tasknon + 1) + "路径无法通行");
                }
                Environment.Exit(0);
            }
            while ((tp != 0) && (tp < 20))
            {
                que[tot] = tp;
                tot++;
                tp = prev[tp];
            }
            que[tot] = 0;
            pathd = "";
            string lukou;
            for (i = tot - 1; i > 1; i--)
            {
                lukou = string.Concat(que[i].ToString(), ">");
                pathd = string.Concat(pathd, lukou);
            }
            i = pathd.Count() - 1;
            pathd = pathd.Substring(0, i);
            if (index == 1)
            {
                pathd = "";
            }
            //更新最优路径值,转向一次暂定为可以行驶30。
            return dist[crossno + 1] + turnCount * 30;
            #endregion
        }
    }
}
