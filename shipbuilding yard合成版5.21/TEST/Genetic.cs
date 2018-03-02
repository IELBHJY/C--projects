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
//using MSExcel = Microsoft.Office.Interop.Excel;


namespace TEST
{
    class Genetic
    {
        int popsize;//种群数量
        int chromolenth;//任务数量
        int trucklenth;//平板车数量
        int duiweino;//堆位数量
        int crossno;//路口数量
        double pc;//交叉率
        double pm;//变异率
        //double pr;//再生率
        string[] taskID;//指令ID
        string[] fenduanID;//分段ID
        int[] cross;//路口数量
        int[,] constrain;//任务先后约束
        //int[,] position;//分段在序列中的位置
        double[] f;//适应度值
        public int[] selected;//用于交叉变异再生操作的染色体
        double[] wheel;//轮盘
        public int[] taskweight;//该任务所运输分段的吨位
        int[] taskwidth;//分段任务运输所需道路宽度
        public int[] truckcapablity;//平板车运输能力
        int[] truckavail;//平板车是否可用，0为不可用，1为可用
        public int[,] pregegeration;//上一代
        public int[,] truckpregegeration;//平板车上一代
        public int[,] nextgeneration;//下一代
        public int[,] trucknextgeneration;//平板车下一代
        public int[] bestgene;//定义最优解
        public int[] besttruck;//定义平板车最优解
        public int[] distance;//空载行驶距离
        public double[] fitness;//适应度
        string[] duiweiID;//堆位ID
        public List<string> pathnumber;//路径对应的任务号
        //string[] path1;//第一段堆场内路径
        //string[] path2;//堆场间路径
        //string[] path3;//第二段堆场内路径
        public string[] originpath1;//原有第一段堆场内路径
        public string[] originpath3;//原有第二段堆场内路径
        //int[] origindistance1;//原有第一段堆场内路径距离
        //int[] origindistance3;//原有第二段堆场内路径距离
        //int[] distance1;//第一段堆场内路径距离
        double[] kongzaitimearray;
        double[] fuzaitimearray;
        double fuzaitime;
        double kongzaitime;
        //int[] distance3;//第二段堆场内路径距离
        int[,] xy;//路口坐标
        int[,] duiweixy;//堆位坐标
        int[,] roadcapacity;//道路理论通行能力        
        int[,] roadsituation;//道路实际占用情况
        public string bestfuzaipath;
        public string bestkongzaipath;
        public List<string> fuzaipath;
        public List<string> kongzaipath;
        public List<List<string>> path;
        public List<List<List<string>>> popsizepath;
        public List<List<double>> taskstarttime;
        public List<List<double>> taskendtime;
        public int[] pretask;
        public int[] sickpopsize;
        public int[,] temppre;
        public int[,] temptruckpre;
        public int[] tabutemppre;
        public int[] tabutemptruckpre;
        public double[] estime;
        public double[] letime;
        int[] count;
        int[] deport_X = new int[3];
        int[] deport_Y = new int[3];
        public List<int> tabutable = new List<int>();//第一种禁忌表
        public List<string> tabutablestring = new List<string>();//第二种禁忌表
        public List<int> tabucount = new List<int>();//禁忌次数
        public List<int> candidateCollection = new List<int>();//第一种候选基因集合
        public List<string> candidatestringCollection = new List<string>();//第二种候选基因集合
        public List<string> teshetable = new List<string>();//特赦表
        string exePath = @"C:\Users\lbh\Documents\Visual Studio 2012\Projects\shipbuilding yard合成版5.21";//路径
        string exePath1= @"C:\Users\lbh\Desktop";
        public Genetic(int populationsize, int tasknumber, int crossnumber, int trucklength, int duiweinumber)//Genetic构造函数
        {
            duiweino = duiweinumber;
            popsize = populationsize;
            chromolenth = tasknumber;
            crossno = crossnumber;
            pc = 0.3;
            pm = 0.2;
            fuzaitime = 0;
            kongzaitime = 0;
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
            originpath1 = new string[chromolenth];
            originpath3 = new string[chromolenth];
            taskwidth = new int[chromolenth];
            taskweight = new int[chromolenth];
            bestgene = new int[chromolenth];
            besttruck = new int[chromolenth];
            cross = new int[crossno];
            distance = new int[popsize * 2];
            fitness = new double[popsize];
            truckcapablity = new int[trucklenth];
            truckavail = new int[trucklenth];
            pathnumber = new List<string>();
            xy = new int[2, crossno + 2];
            roadcapacity = new int[crossno + 2, crossno + 2];
            roadsituation = new int[crossno + 2, crossno + 2];
            duiweixy = new int[2, duiweino];
            duiweiID = new string[duiweino];
            //distance2 = new int[chromolenth*trucklenth];
            kongzaitimearray = new double[chromolenth];
            fuzaitimearray= new double[chromolenth];
            bestfuzaipath = "";
            bestkongzaipath = "";
            fuzaipath = new List<string>();
            kongzaipath = new List<string>();
            path = new List<List<string>>();
            popsizepath = new List<List<List<string>>>();
            pretask = new int[chromolenth];
            taskstarttime = new List<List<double>>();
            taskendtime = new List<List<double>>();
            sickpopsize = new int[populationsize];
            temppre = new int[popsize, chromolenth];
            temptruckpre = new int[popsize, chromolenth];
            tabutemppre = new int[chromolenth];
            tabutemptruckpre = new int[chromolenth];
            estime = new double[chromolenth];
            letime = new double[chromolenth];
            count = new int[7];
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        public void initialtest()
        {
            int i = 1;
            int j = 0;
            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exePath + @"\input database 20.accdb";
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
            for (i = 1; i < chromolenth + 1; i++)
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
                        estime[i - 1] = Convert.ToDouble(reader["时间窗始点"]);
                        letime[i - 1] = Convert.ToDouble(reader["时间窗终点"]);
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
            for (i = 1; i < chromolenth + 1; i++)
            {
                try
                {
                    string strCom = "Select * from 任务约束 where 任务ID= " + i;
                    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
                    conn.Open();
                    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
                    if (reader.Read())
                    {
                        pretask[i - 1] = (int)reader["前置任务"]-1;
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
            for (i = 0; i < crossno + 2; i++)
            {
                for (j = 0; j < crossno + 2; j++)
                {
                    roadcapacity[i, j] = 0;
                    roadsituation[i, j] = 0;
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
        }

        /// <summary>
        /// 产生第一代种群
        /// </summary>
        public void createfirstpop()
        {
            List<int> b = new List<int>();
            List<int> b1 = new List<int>();
            List<int> b2 = new List<int>();
            List<int> b3 = new List<int>();
            List<int> b4 = new List<int>();
            List<int> b5 = new List<int>();
            List<int> b6 = new List<int>();
            List<int> b7 = new List<int>();
            List<int> b8 = new List<int>();
            int k = 0;
            int n = 0;
            int[] s = new int[chromolenth];
            for (k = 0; k < truckcapablity.Length; k++)
            {
                switch (truckcapablity[k])
                {
                    case 90:
                           b1.Add(k);
                           b.Add(k);
                           break;
                    case 130:
                            b2.Add(k);
                            b.Add(k);
                            break;
                    case 170:
                            b3.Add(k);
                            b.Add(k);
                            break;
                    case 220:
                            b4.Add(k);
                            b.Add(k);
                            break;
                    case 380:
                            b5.Add(k);
                            b.Add(k);
                            break;
                }
            }
            count[0] = b.Count; //3
            count[1] = b1.Count;//1
            count[2] = count[1] + b2.Count;//1
            count[3] = count[2] + b3.Count;//1
            count[4] = count[3] + b4.Count;//2
            count[5] = count[4] + b5.Count;//3
            count[6] = count[5] + b6.Count;//6
            List<int> a = new List<int>();//将任务储存在a中
            List<int> randomtask = new List<int>();
            for (k = 0; k < chromolenth; k++)
            {
                a.Add(k);
            }
            for (int i = 0; i < popsize; i++)
            {
                for (int j = 0; j < chromolenth; j++)
                {
                    if (j ==0)
                    {
                        k = 3;
                        randomtask.Add(k);
                        pregegeration[i, j] = a[k];
                    }
                    else if (j == 1)
                    {
                        k = 5;
                        randomtask.Add(k);
                        pregegeration[i, j] = a[k];
                    }
                    else if (j == chromolenth - 2)
                    {
                        k = 11;
                        randomtask.Add(k);
                        pregegeration[i, j] = a[k];
                    }
                    else if (j == chromolenth - 1)
                    {
                        k = 14;
                        randomtask.Add(k);
                        pregegeration[i, j] = a[k];
                    }
                    else
                    {
                        Random rd = new Random();
                        k = rd.Next(0, a.Count);
                        if (!randomtask.Contains(k)&&k!=11&&k!=14)
                        {
                            randomtask.Add(k);
                            pregegeration[i, j] = a[k];
                        }
                        else
                        {
                            j--;
                            continue;
                        }
                    }
                    if (taskweight[a[k]]<=90)
                    {
                        Random rd1 = new Random();
                        n = rd1.Next(0, trucklenth);
                        truckpregegeration[i, j] = b[n];
                    }
                    else if (taskweight[a[k]]<=130)
                    {
                        Random rd3 = new Random();
                        n = rd3.Next(count[1], trucklenth);
                        truckpregegeration[i, j] = b[n];
                    }
                    else if (taskweight[a[k]]<=170)
                    {
                        Random rd4 = new Random();
                        n = rd4.Next(count[2], trucklenth);
                        truckpregegeration[i, j] = b[n];
                    }
                    else if (taskweight[a[k]]<=220)
                    {
                        Random rd4 = new Random();
                        n = rd4.Next(count[3], trucklenth);
                        truckpregegeration[i, j] = b[n];
                    }
                    else if (taskweight[a[k]]<=380)
                    {
                        Random rd5 = new Random();
                        n = rd5.Next(count[4], trucklenth);
                        truckpregegeration[i, j] = b[n];
                    }
                }
                for (int z = 0; z < chromolenth; z++)
                {
                    randomtask.RemoveAt(0);
                }
                double fit=calfitness(i);
                int k1 = -1;
                int k2 = -1;
                for (int j= 0; j < chromolenth; j++)
                {
                    if (pretask[j] != -1)
                    {
                        k1 = j;
                    }
                }
                for (int j = 0; j < chromolenth; j++)
                {
                    if (pretask[j] != -1 && j != k1)
                    {
                        k2 = j;
                    }
                }
                if (taskstarttime[i][11] <taskendtime[i][3])
                {
                    taskstarttime.RemoveAt(i);
                    taskendtime.RemoveAt(i);
                    i--;
                    continue;
                }
                if (taskstarttime[i][14] < taskendtime[i][5])
                {
                    taskstarttime.RemoveAt(i);
                    taskendtime.RemoveAt(i);
                    i--;
                    continue;
                }
                if ((taskendtime[i][3] <= taskstarttime[i][11]) && taskendtime[i][5] <= taskstarttime[i][14])
                {
                    Console.WriteLine("初始种群任务约束成立");
                }
                else
                {
                    Console.WriteLine("初始种群任务约束不成立");
                }
                for (int j = 0; j < chromolenth; j++)
                {
                    taskstarttime[i].RemoveAt(0);
                    taskendtime[i].RemoveAt(0);
                }
                for (int j = 0; j < trucklenth; j++)
                {
                    popsizepath[i].RemoveAt(0);
                }
            }
        }
        /// <summary>
        /// 求某个个体的目标函数
        /// </summary>
        /// <param name="popsize">个体</param>
        /// <returns>目标函数</returns>
        public double calfitness(int thisone)
        {
            List<List<string>> collection = new List<List<string>>();
            popsizepath.Add(collection);
            List<double> collection1 = new List<double>();
            taskstarttime.Add(collection1);
            List<double> collection2 = new List<double>();
            taskendtime.Add(collection2);
            int j = 0;
            List<List<int>> a = new List<List<int>>();
            for (j = 0; j < trucklenth; j++)
            {
                List<int> a1 = new List<int>();
                a.Add(a1);
            }
            for (j = 0; j < chromolenth; j++)//将任务分配到各平板车
            {
                for (int k = 0; k < trucklenth; k++)
                {
                    if (truckpregegeration[thisone, j] == k)
                    {
                        a[k].Add(pregegeration[thisone,j]);
                    }
                }
            }
            int countnumber = 0;
            kongzaitime = 0;
            fuzaitime= 0;
            for (int i = 0; i <chromolenth;i++ )
            {
                taskstarttime[thisone].Add(-1);
                taskendtime[thisone].Add(-1);
            }
            for (j = 0; j < trucklenth; j++)
            {
                List<string> t = new List<string>();
                popsizepath[thisone].Add(t);
                if (a[j].Count() != 0)
                {
                    taskstarttime[thisone][a[j][0]] =0;
                    for (int k = 0; k < a[j].Count(); k++)
                    {
                        popsizepath[thisone][j].Add(" ");
                        if (k < a[j].Count() - 1)
                        {
                            fuzaitimearray[countnumber]=loadPath(a[j][k], a[j][k]);//此处需要计算任务k，起始堆场到目标堆场之间的负载路径
                            taskendtime[thisone][a[j][k]] = taskstarttime[thisone][a[j][k]] + fuzaitimearray[countnumber]/6;
                            fuzaitime += fuzaitimearray[countnumber];
                            if (a[j][k] != a[j][k + 1])
                            {
                                kongzaitimearray[countnumber] = unloadPath(a[j][k], a[j][k + 1]);//此处需要计算任务k目标堆场到下一个任务的起始堆场的空载路径
                                taskstarttime[thisone][a[j][k + 1]] = taskendtime[thisone][a[j][k]] + kongzaitimearray[countnumber]/10;
                                popsizepath[thisone][j][k] = bestfuzaipath + " " + bestkongzaipath;
                                kongzaitime+= kongzaitimearray[countnumber];
                            }
                            else
                            {
                                Console.WriteLine("任务分配有错！");
                            }
                            countnumber++;
                        }
                        else
                        {
                            fuzaitimearray[countnumber] = loadPath(a[j][k], a[j][k]);
                            taskendtime[thisone][a[j][k]] = taskstarttime[thisone][a[j][k]] + fuzaitimearray[countnumber]/6;
                            popsizepath[thisone][j][k] = bestfuzaipath;
                            fuzaitime += fuzaitimearray[countnumber];
                            countnumber++;
                        }
                    }
                }
            }
            //判断是否满足前后任务约束
            if ((taskendtime[thisone][3] <= taskstarttime[thisone][11]) && (taskendtime[thisone][5] <= taskstarttime[thisone][14]))
            {
                sickpopsize[thisone] = 0;//满足约束条件
            }
            else
            {
                sickpopsize[thisone] = 1;//不满足约束条件
            }
            double constarttime = 0;
            double conendtime = 0;
            for (int i = 0; i < chromolenth; i++)
            {
                if (taskstarttime[thisone][i] >= estime[i])
                {
                    constarttime += 0;
                }
                else
                {
                    constarttime += estime[i] - Convert.ToDouble((taskstarttime[thisone][i]).ToString("0.0000"));
                }
                if (taskendtime[thisone][i] <= letime[i])
                {
                    conendtime += 0;
                }
                else
                {
                    conendtime += Convert.ToDouble(taskendtime[thisone][i].ToString("0.0000")) - letime[i];
                }
            }
            fitness[thisone] = kongzaitime * 0.4 / 10 + constarttime*0.3 + conendtime*0.3;
            return fitness[thisone];
        }

        /// <summary>
        /// 轮盘赌选择
        /// </summary>
        /// <param name="n">交叉变异的个体</param>
        public void wheelselect(int n)//轮盘赌选择较好染色体
        {
            Random rand = new Random();
            int i, j, r;
            double sum = 0;
            for (i = 0; i < popsize; i++)
            {
                wheel[0] = 0;
                sum = sum + (5000- fitness[i]);//目标函数越大，适应度应该越低
                wheel[i + 1] = wheel[i] + (5000- fitness[i]);
            }
            for (i = 0; i < n; i++)
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
            }
            Dictionary<int, double> dic = new Dictionary<int, double>();
            List<double> list = new List<double>();
            for (j = 0; j < popsize; j++)
            {
                dic.Add(j, fitness[j]);
                list.Add(fitness[j]);
            }
            list.Sort();//目标函数从小到大排序。
            int chooseone = n;
            List<int> reproducechoose = new List<int>();
            for ( j = 0; j <popsize-n; j++)
            {
              for ( int k = 0; k<popsize; k++)
              {
                if (list[j] == dic[k]&& !reproducechoose.Contains(k))
                 {
                   reproducechoose.Add(k);
                   selected[chooseone] = k;
                   chooseone++;
                   break;
                 }
              }
            }
            for (i = 0; i < popsize - n; i++)
            {
                reproducechoose.RemoveAt(0);
            }
            for (i = 0; i < popsize; i++)
            {
              list.RemoveAt(0);
              dic.Remove(i);
            }
        }
        /// <summary>
        /// 单点交叉
        /// </summary>
        /// <param name="p1">父代个体p1</param>
        /// <param name="p2">父代个体p2</param>
        /// <param name="c1">产生的子代c1</param>
        /// <param name="c2">产生的子代c2</param>
        public void crossover(int p1, int p2, int c1, int c2)//交叉
        {
            int mutation1;
            Random r = new Random();
            List<int> templist1 = new List<int>();
            List<int> templist2 = new List<int>();
            for (int i = 0; i < chromolenth; i++)
            {
                nextgeneration[c1, i] = pregegeration[p1, i];
                nextgeneration[c2, i] = pregegeration[p2, i];
                trucknextgeneration[c1, i] = truckpregegeration[p1, i];
                trucknextgeneration[c2, i] = truckpregegeration[p2, i];
            }
            mutation1 = r.Next(0, chromolenth);
            for (int i = mutation1 + 1; i < chromolenth; i++)
            {
                templist1.Add(pregegeration[p1,i]);
                templist2.Add(pregegeration[p2, i]);
            }
            for (int i = mutation1 + 1; i < chromolenth; i++)
            {
                for (int j = 0; j < chromolenth; j++)
                {
                    if (templist1.Contains(pregegeration[p2, j]))
                    {
                        nextgeneration[c1, i] = pregegeration[p2, j];
                        trucknextgeneration[c1, i] = truckpregegeration[p2, j];
                        templist1.Remove(pregegeration[p2, j]);
                        break;
                    }
                }
            }
            for (int i = mutation1 + 1; i < chromolenth; i++)
            {
                for (int j = 0; j < chromolenth; j++)
                {
                    if (templist2.Contains(pregegeration[p1, j]))
                    {
                        nextgeneration[c2, i] = pregegeration[p1, j];
                        trucknextgeneration[c2, i] = truckpregegeration[p1, j];
                        templist2.Remove(pregegeration[p1, j]);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 变异
        /// </summary>
        /// <param name="p1">父代中个体p1</param>
        /// <param name="c1">产生的子代c1</param>
       public void mutation(int p1,int c1)//变异
        {
            int mutation1;
            int mutation2;
            int fornow;
            int k1 = -1;
            int k2 = -1;
            for (int j = 0; j < chromolenth; j++)
            {
                if (pretask[j] != -1)
                {
                    k1 = j;
                }
            }
            for (int j = 0; j < chromolenth; j++)
            {
                if (pretask[j] != -1 && j != k1)
                {
                    k2 = j;
                }
            }
            Random r = new Random();
            do
            {
              mutation1 = r.Next(0, chromolenth);
            } while(mutation1==k1 ||mutation1==k2||mutation1==pretask[k1]||mutation1==pretask[k2]);
             do
             {
               mutation2 = r.Next(0, chromolenth);
             } while (mutation2 == mutation1 || mutation2 == k1 || mutation2 == k2||mutation2==pretask[k1]||mutation2==pretask[k2]);
             if (mutation1 > mutation2)
             {
                 fornow = mutation1;
                 mutation1 = mutation2;
                 mutation2 = fornow;
             }
             for (int i = 0; i < chromolenth; i++)
             {
                 nextgeneration[c1, i] = pregegeration[p1, i];
                 trucknextgeneration[c1, i] = truckpregegeration[p1, i];
             }
            //任务单点交换
             nextgeneration[c1, mutation1] = pregegeration[p1, mutation2];
             nextgeneration[c1, mutation2] = pregegeration[p1, mutation1];
            //平板车需要判断交换的任务是否满足平板车承重要求
             if (taskweight[nextgeneration[c1, mutation1]] > truckcapablity[trucknextgeneration[c1, mutation1]])
             {
                 if (taskweight[nextgeneration[c1, mutation1]] <=130)
                 {
                     Random suijishu2 = new Random();
                     trucknextgeneration[c1, mutation1] = suijishu2.Next(count[1], trucklenth);
                 }
                 else if (taskweight[nextgeneration[c1, mutation1]] <=170)
                 {
                     Random suijishu3 = new Random();
                     trucknextgeneration[c1, mutation1] = suijishu3.Next(count[2], trucklenth);
                 }
                 else if (taskweight[nextgeneration[c1, mutation1]] <=220)
                 {
                     Random suijishu4 = new Random();
                     trucknextgeneration[c1, mutation1] = suijishu4.Next(count[3], trucklenth);
                 }
                 else if (taskweight[nextgeneration[c1, mutation1]] <=380)
                 {
                     Random suijishu7 = new Random();
                     trucknextgeneration[c1, mutation1] = suijishu7.Next(count[4], trucklenth);
                 }
             }
             if (taskweight[nextgeneration[c1, mutation2]] > truckcapablity[trucknextgeneration[c1, mutation2]])
             {
                 if (taskweight[nextgeneration[c1, mutation2]] <=130)
                 {
                     Random suijishu2 = new Random();
                     trucknextgeneration[c1, mutation2] = suijishu2.Next(count[1], trucklenth);
                 }
                 else if (taskweight[nextgeneration[c1, mutation2]] <=170)
                 {
                     Random suijishu3 = new Random();
                     trucknextgeneration[c1, mutation2] = suijishu3.Next(count[2], trucklenth);
                 }
                 else if (taskweight[nextgeneration[c1, mutation2]] <=220)
                 {
                     Random suijishu4 = new Random();
                     trucknextgeneration[c1, mutation2] = suijishu4.Next(count[3], trucklenth);
                 }
                 else if (taskweight[nextgeneration[c1, mutation2]] <=380)
                 {
                     Random suijishu7 = new Random();
                     trucknextgeneration[c1, mutation2] = suijishu7.Next(count[4], trucklenth);
                 }
             }
        }
       /// <summary>
       /// 再生
       /// </summary>
       /// <param name="p1">父代中个体p1</param>
       /// <param name="c1">产生的子代c1</param>
        public void reproduction(int p1,int c1)
        {
            for (int i = 0; i < chromolenth; i++)
            {
                nextgeneration[c1, i] = pregegeration[p1, i];
                trucknextgeneration[c1, i] = truckpregegeration[p1, i];
            }
        }
        /// <summary>
        /// 交叉，变异，再生
        /// </summary>
        public void createnextpop()
        {
            int i=0;
            for (i = 0; i <(int)(popsize*pc); i+=2)
            {
                crossover(selected[i], selected[i + 1], i, i + 1);
                for (int j = 0; j < chromolenth; j++)
                {
                    pregegeration[selected[i], j] = nextgeneration[i, j];
                    truckpregegeration[selected[i], j] = trucknextgeneration[i, j];
                    pregegeration[selected[i+1], j] = nextgeneration[i + 1, j];
                    truckpregegeration[selected[i+1], j] = trucknextgeneration[i + 1, j];
                }
                calfitness(selected[i]);
                calfitness(selected[i + 1]);
                if ((taskendtime[selected[i]][3] <= taskstarttime[selected[i]][11]) && (taskendtime[selected[i]][5] <= taskstarttime[selected[i]][14]) && (taskendtime[selected[i+1]][3] <= taskstarttime[selected[i+1]][11]) && (taskendtime[selected[i+1]][5] <= taskstarttime[selected[i+1]][14]))
                {

                }
                else
                {
                    for (int j = 0; j < chromolenth; j++)
                    {
                        pregegeration[selected[i], j] = temppre[selected[i], j];
                        truckpregegeration[selected[i], j] = temptruckpre[selected[i], j];
                        pregegeration[selected[i+1], j] = temppre[selected[i + 1], j];
                        truckpregegeration[selected[i+1], j] = temptruckpre[selected[i + 1], j];
                    }
                    i = i - 2;
                    continue;
                }
                for (int j = 0; j < chromolenth; j++)
                {
                    pregegeration[selected[i], j] = temppre[selected[i], j];
                    truckpregegeration[selected[i], j] = temptruckpre[selected[i], j];
                    pregegeration[selected[i+1], j] = temppre[selected[i + 1], j];
                    truckpregegeration[selected[i+1], j] = temptruckpre[selected[i + 1], j];
                }
            }
            for (i = (int)(popsize * pc); i < (int)(popsize * (pc + pm)); i++)
            {
                mutation(selected[i], i);
                for (int j = 0; j < chromolenth; j++)
                {
                    pregegeration[selected[i], j] = nextgeneration[i, j];
                    truckpregegeration[selected[i], j] = trucknextgeneration[i, j];
                }
                calfitness(selected[i]);
                if ((taskendtime[selected[i]][3] <= taskstarttime[selected[i]][11]) && (taskendtime[selected[i]][5] <= taskstarttime[selected[i]][14]))
                {
                }
                else
                {
                    for (int j = 0; j < chromolenth; j++)
                    {
                        pregegeration[selected[i], j] = temppre[selected[i], j];
                        truckpregegeration[selected[i], j] = temptruckpre[selected[i], j];
                    }
                    i = i - 1;
                    continue;
                }
                for (int j = 0; j < chromolenth; j++)
                {
                    pregegeration[selected[i], j] = temppre[selected[i], j];
                    truckpregegeration[selected[i], j] = temptruckpre[selected[i], j];
                }
            }
            for (i = (int)(popsize * (pc + pm)); i < popsize; i++)
            {
                reproduction(selected[i], i);    
            }
        }
        /// <summary>
        /// 产生下一代种群
        /// </summary>
        public void producenext()//将子代赋值给父代
        {
            int[,] temgeneration = new int[2 * popsize, chromolenth];
            int[,] trucktemgeneration = new int[2 * popsize, chromolenth];
            //将父代放入临时种群
            for (int i = 0; i <popsize; i++)
            {
                for (int j = 0; j <chromolenth; j++)
                {
                    temgeneration[i, j] = pregegeration[i, j];
                    trucktemgeneration[i, j] = truckpregegeration[i, j];
                }
            }
            //将子代放入临时种群
            for (int i = 0; i < popsize; i++)
            {
                for (int j = 0; j < chromolenth; j++)
                {
                    temgeneration[i + popsize, j] = nextgeneration[i, j];
                    trucktemgeneration[i + popsize, j] = trucknextgeneration[i, j];
                }
            }
            for (int i = 0; i <popsize; i++)
            {
                for (int j = 0; j <chromolenth; j++)
                {
                    pregegeration[i, j] = temgeneration[i+popsize, j];
                    truckpregegeration[i, j] = trucktemgeneration[i+popsize, j];
                }
            }
        }

        public void savepre()
        {
            for (int i = 0; i < popsize; i++)
            {
                for (int j = 0; j < chromolenth; j++)
                {
                    temppre[i, j] = pregegeration[i, j];
                    temptruckpre[i, j] = truckpregegeration[i, j];
                }
            }
        }

        public void tabusavepre(int thisone)
        {
            for (int i = 0; i < chromolenth; i++)
            {
                tabutemppre[i] = pregegeration[thisone, i];
                tabutemptruckpre[i] = truckpregegeration[thisone, i];
            }
        }

        public int modify()
        {
            int sick = -1;
            for (int i = 0; i < popsize; i++)
            {
                if (sickpopsize[i] == 1)
                {
                    sick = i;
                    return sick;
                }
            }
            return sick;
        }

       /// <summary>
       /// 第一种禁忌搜索
       /// </summary>
       /// <param name="thisone">当前个体</param>
       /// <returns>任务序列中的位置，任务号，产生的平板车，适应度</returns>
        public double[] TabuSearch_mutation(int thisone,bool isteshe)
        {
            bool is_or_not = isteshe;
            if (!is_or_not)
            {
                double[] value_array = new double[7];
                #region
                Random r = new Random();
                int number1 = r.Next(0, chromolenth);
                int number2 = r.Next(0, chromolenth);
                while (number2 == number1)
                {
                    number2 = r.Next(0, chromolenth);
                }
                int tasknumber1 = pregegeration[thisone, number1];
                int tasknumber2 = pregegeration[thisone, number2];
                string str = tasknumber1.ToString() + "-" + tasknumber2.ToString();
                while (tabutablestring.Contains(str) || candidatestringCollection.Contains(str)||str.Equals("3-11")||str.Equals("11-3")||str.Equals("5-14")||str.Equals("14-5"))
                {
                    Random rd = new Random();
                    number1 = rd.Next(0, chromolenth);
                    number2 = r.Next(0, chromolenth);
                    while (number2 == number1)
                    {
                        number2 = r.Next(0, chromolenth);
                    }
                    tasknumber1 = pregegeration[thisone, number1];
                    tasknumber2 = pregegeration[thisone, number2];
                    str = tasknumber1.ToString() + "-" + tasknumber2.ToString();
                }
                if (taskweight[pregegeration[thisone, number1]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number1] = num1;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number1] = num3;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number1] = num4;
                }
                if (taskweight[pregegeration[thisone, number2]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number2] = num1;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number2] = num3;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number2] = num4;
                }
                #endregion
                double tempfit = calfitness(thisone);
                int sick = modify();
                while (sick >= 0)
                {
                    for (int i = 0; i < chromolenth; i++)
                    {
                        truckpregegeration[thisone, i] = tabutemptruckpre[i];
                    }
                    #region
                    r = new Random();
                    number1 = r.Next(0, chromolenth);
                    number2 = r.Next(0, chromolenth);
                    while (number2 == number1)
                    {
                        number2 = r.Next(0, chromolenth);
                    }
                    tasknumber1 = pregegeration[thisone, number1];
                    tasknumber2 = pregegeration[thisone, number2];
                    str = tasknumber1.ToString() + "-" + tasknumber2.ToString();
                    while (tabutablestring.Contains(str) || candidatestringCollection.Contains(str) || str.Equals("3-11") || str.Equals("11-3") || str.Equals("5-14") || str.Equals("14-5"))
                    {
                        Random rd = new Random();
                        number1 = rd.Next(0, chromolenth);
                        number2 = r.Next(0, chromolenth);
                        while (number2 == number1)
                        {
                            number2 = r.Next(0, chromolenth);
                        }
                        tasknumber1 = pregegeration[thisone, number1];
                        tasknumber2 = pregegeration[thisone, number2];
                        str = tasknumber1.ToString() + "-" + tasknumber2.ToString();
                    }
                    if (taskweight[pregegeration[thisone, number1]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number1] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 220)
                    {
                        Random r3 = new Random();
                        int num3 = r3.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number1] = num3;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number1] = num4;
                    }
                    if (taskweight[pregegeration[thisone, number2]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number2] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 220)
                    {
                        Random r3 = new Random();
                        int num3 = r3.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number2] = num3;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number2] = num4;
                    }
                    #endregion
                    tempfit = calfitness(thisone);
                    sick = modify();
                }
                if (sick < 0)
                {
                    value_array[0] = number1;
                    value_array[1] = number2;
                    value_array[2] = tasknumber1;
                    value_array[3] = tasknumber2;
                    value_array[4] = truckpregegeration[thisone, number1];
                    value_array[5] = truckpregegeration[thisone, number2];
                    value_array[6] = tempfit;
                }
                return value_array;
            }
            else
            {
                double[] value_array = new double[7];
                #region
                int j = 0;
                string str1 = tabutablestring[j];
                string[] num = str1.Split('-');
                int tasknumber1 = Convert.ToInt32(num[0]);
                int tasknumber2 = Convert.ToInt32(num[1]);
                string str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                int number1 = -1;
                int number2 = -1;
                for (int i = 0; i < chromolenth; i++)
                {
                    if (tasknumber1 == pregegeration[thisone,i])
                    {
                        number1 = i;
                    }
                    if (tasknumber2 == pregegeration[thisone,i])
                    {
                        number2 = i;
                    }
                }
                while (teshetable.Contains(str))
                {
                    j += 2;
                    str1 = tabutablestring[j];
                    num = str1.Split('-');
                    tasknumber1 = Convert.ToInt32(num[0]);
                    tasknumber2 = Convert.ToInt32(num[1]);
                    str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                    number1 = -1;
                    number2 = -1;
                    for (int i = 0; i < chromolenth; i++)
                    {
                        if (tasknumber1 == pregegeration[thisone,i])
                        {
                            number1 = i;
                        }
                        if (tasknumber2 == pregegeration[thisone,i])
                        {
                            number2 = i;
                        }
                    }
                }
                if (taskweight[pregegeration[thisone, number1]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number1] = num1;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number1] = num3;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number1] = num4;
                }
                if (taskweight[pregegeration[thisone, number2]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number2] = num1;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number2] = num3;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number2] = num4;
                }
                #endregion
                double tempfit = calfitness(thisone);
                int sick = modify();
                while (sick >= 0)
                {
                    for (int i = 0; i < chromolenth; i++)
                    {
                        truckpregegeration[thisone, i] = tabutemptruckpre[i];
                    }
                    #region
                    str1 = tabutablestring[j];
                    num = str1.Split('-');
                    tasknumber1 = Convert.ToInt32(num[0]);
                    tasknumber2 = Convert.ToInt32(num[1]);
                    str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                    number1 = -1;
                    number2 = -1;
                    for (int i = 0; i < chromolenth; i++)
                    {
                        if (tasknumber1 == pregegeration[thisone,i])
                        {
                            number1 = i;
                        }
                        if (tasknumber2 == pregegeration[thisone,i])
                        {
                            number2 = i;
                        }
                    }
                    if (taskweight[pregegeration[thisone, number1]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number1] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 220)
                    {
                        Random r3 = new Random();
                        int num3 = r3.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number1] = num3;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number1] = num4;
                    }
                    if (taskweight[pregegeration[thisone, number2]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number2] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 220)
                    {
                        Random r3 = new Random();
                        int num3 = r3.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number2] = num3;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number2] = num4;
                    }
                    #endregion
                    tempfit = calfitness(thisone);
                    sick = modify();
                }
                if (sick < 0)
                {
                    value_array[0] = number1;
                    value_array[1] = number2;
                    value_array[2] = tasknumber1;
                    value_array[3] = tasknumber2;
                    value_array[4] = truckpregegeration[thisone, number1];
                    value_array[5] = truckpregegeration[thisone, number2];
                    value_array[6] = tempfit;
                }
                return value_array;
            }
        }

        /// <summary>
        /// 第二种禁忌搜索
        /// </summary>
        /// <param name="thisone">当前个体</param>
        /// isteshe 是true为特赦，false为不是特赦
        /// <returns>第一个位置，第二个位置，第一个任务号，第二个任务号，第一个平板车号，第二个平板车号，目标函数</returns>
        public double[] TabuSearch_cross(int thisone,bool isteshe)
        {
            bool is_or_not=isteshe;
            if (!is_or_not)
            {
                #region
                double[] value_array = new double[7];//储存 选中的两个基因，变异后的两个平板车，还有最后的目标函数
                Random r = new Random();
                int number1 = r.Next(0, chromolenth);
                int number2 = r.Next(0, chromolenth);
                while (number2 == number1)
                {
                    number2 = r.Next(0, chromolenth);
                }
                int tasknumber1 = pregegeration[thisone, number1];
                int tasknumber2 = pregegeration[thisone, number2];
                //任务号-任务号  编码
                string str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                //判断选中的任务号是否在临时禁忌表和全局禁忌表中
                while (candidatestringCollection.Contains(str) || tabutablestring.Contains(str) || str.Equals("3-11") || str.Equals("11-3") || str.Equals("4-15") || str.Equals("15-4"))
                {
                    Random rd = new Random();
                    number1 = rd.Next(0, chromolenth);
                    number2 = rd.Next(0, chromolenth);
                    tasknumber1 = pregegeration[thisone, number1];
                    tasknumber2 = pregegeration[thisone, number2];
                    str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                }
                pregegeration[thisone, number1] = tabutemppre[number2];
                if (taskweight[pregegeration[thisone, number1]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number1] = num1;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number1] = num3;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number1] = num4;
                }
                pregegeration[thisone, number2] = tabutemppre[number1];
                if (taskweight[pregegeration[thisone, number2]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number2] = num1;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number2] = num3;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number2] = num4;
                }
                double tempfitness = calfitness(thisone);
                int sick = modify();
                while (sick >= 0)
                {
                    for (int i = 0; i < chromolenth; i++)
                    {
                        pregegeration[thisone, i] = tabutemppre[i];
                        truckpregegeration[thisone, i] = tabutemptruckpre[i];
                    }
                    Random rt = new Random();
                    number1 = rt.Next(0, chromolenth);
                    number2 = rt.Next(0, chromolenth);
                    tasknumber1 = pregegeration[thisone, number1];
                    tasknumber2 = pregegeration[thisone, number2];
                    str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                    while (candidatestringCollection.Contains(str) || tabutablestring.Contains(str) || str.Equals("3-11") || str.Equals("11-3") || str.Equals("4-15") || str.Equals("15-4"))
                    {
                        Random rd = new Random();
                        number1 = rd.Next(0, chromolenth);
                        number2 = rd.Next(0, chromolenth);
                        tasknumber1 = pregegeration[thisone, number1];
                        tasknumber2 = pregegeration[thisone, number2];
                        str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                    }
                    pregegeration[thisone, number1] = tabutemppre[number2];
                    if (taskweight[pregegeration[thisone, number1]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number1] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 220)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number1] = num4;
                    }
                    pregegeration[thisone, number2] = tabutemppre[number1];
                    if (taskweight[pregegeration[thisone, number2]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number2] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 220)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number2] = num4;
                    }
                    tempfitness = calfitness(thisone);
                    sick = modify();
                }
                if (sick < 0)
                {
                    value_array[0] = number1;
                    value_array[1] = number2;
                    value_array[2] = pregegeration[thisone, number1];
                    value_array[3] = pregegeration[thisone, number2];
                    value_array[4] = truckpregegeration[thisone, number1];
                    value_array[5] = truckpregegeration[thisone, number2];
                    value_array[6] = tempfitness;
                }
                return value_array;
                #endregion
            }
            else
            {
                #region
                double[] value_array = new double[7];//储存 选中的两个基因，变异后的两个平板车，还有最后的目标函数
                int j = 0;
                string str1=tabutablestring[j];
                string[] num = str1.Split('-');
                int tasknumber1 = Convert.ToInt32(num[0]);
                int tasknumber2 = Convert.ToInt32(num[1]);
                string str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                int number1=-1;
                int number2=-1;
                for (int i = 0; i < chromolenth; i++)
                {
                    if (tasknumber1 == pregegeration[thisone,i])
                    {
                        number1 = i;
                    }
                    if (tasknumber2 == pregegeration[thisone,i])
                    {
                        number2 = i;
                    }
                }
               while (teshetable.Contains(str))
               {
                   j += 2;
                   str1 = tabutablestring[j];
                   num = str1.Split('-');
                   tasknumber1 = Convert.ToInt32(num[0]);
                   tasknumber2 = Convert.ToInt32(num[1]);
                   str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                   number1 = -1;
                   number2 = -1;
                   for (int i = 0; i < chromolenth; i++)
                   {
                       if (tasknumber1 == pregegeration[thisone,i])
                       {
                           number1 = i;
                       }
                       if (tasknumber2 == pregegeration[thisone,i])
                       {
                           number2 = i;
                       }
                   }
               }
                pregegeration[thisone, number1] = tabutemppre[number2];
                if (taskweight[pregegeration[thisone, number1]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number1] = num1;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number1] = num2;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number1] = num3;
                }
                else if (taskweight[pregegeration[thisone, number1]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number1] = num4;
                }
                pregegeration[thisone, number2] = tabutemppre[number1];
                if (taskweight[pregegeration[thisone, number2]] <= 90)
                {
                    Random r1 = new Random();
                    int num1 = r1.Next(0, trucklenth);
                    truckpregegeration[thisone, number2] = num1;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 130)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[1], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 170)
                {
                    Random r2 = new Random();
                    int num2 = r2.Next(count[2], trucklenth);
                    truckpregegeration[thisone, number2] = num2;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 220)
                {
                    Random r3 = new Random();
                    int num3 = r3.Next(count[3], trucklenth);
                    truckpregegeration[thisone, number2] = num3;
                }
                else if (taskweight[pregegeration[thisone, number2]] <= 380)
                {
                    Random r4 = new Random();
                    int num4 = r4.Next(count[4], trucklenth);
                    truckpregegeration[thisone, number2] = num4;
                }
                double tempfitness = calfitness(thisone);
                int sick = modify();
                int circle_count = 0;
                while (sick >= 0)
                {
                     if(circle_count > 50)
                    {
                        for (int i = 0; i < chromolenth; i++)
                        {
                            pregegeration[thisone, i] = tabutemppre[i];
                            truckpregegeration[thisone, i] = tabutemptruckpre[i];
                        }
                        tempfitness = calfitness(thisone);
                        sick = modify();
                        break;
                    }
                    for (int i = 0; i < chromolenth; i++)
                    {
                        pregegeration[thisone, i] = tabutemppre[i];
                        truckpregegeration[thisone, i] = tabutemptruckpre[i];
                    }
                    str1 = tabutablestring[j];
                    num = str1.Split('-');
                    tasknumber1 = Convert.ToInt32(num[0]);
                    tasknumber2 = Convert.ToInt32(num[1]);
                    str = (tasknumber1).ToString() + "-" + (tasknumber2).ToString();
                    number1 = -1;
                    number2 = -1;
                    for (int i = 0; i < chromolenth; i++)
                    {
                        if (tasknumber1 == pregegeration[thisone,i])
                        {
                            number1 = i;
                        }
                        if (tasknumber2 == pregegeration[thisone,i])
                        {
                            number2 = i;
                        }
                    }
                    pregegeration[thisone, number1] = tabutemppre[number2];
                    if (taskweight[pregegeration[thisone, number1]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number1] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 220)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number1] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number1]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number1] = num4;
                    }
                    pregegeration[thisone, number2] = tabutemppre[number1];
                    if (taskweight[pregegeration[thisone, number2]] <= 90)
                    {
                        Random r1 = new Random();
                        int num1 = r1.Next(0, trucklenth);
                        truckpregegeration[thisone, number2] = num1;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 130)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[1], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 170)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[2], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 220)
                    {
                        Random r2 = new Random();
                        int num2 = r2.Next(count[3], trucklenth);
                        truckpregegeration[thisone, number2] = num2;
                    }
                    else if (taskweight[pregegeration[thisone, number2]] <= 380)
                    {
                        Random r4 = new Random();
                        int num4 = r4.Next(count[4], trucklenth);
                        truckpregegeration[thisone, number2] = num4;
                    }
                    tempfitness = calfitness(thisone);
                    sick = modify();
                    circle_count++;
                }
                if (sick < 0)
                {
                    value_array[0] = number1;
                    value_array[1] = number2;
                    value_array[2] = pregegeration[thisone, number1];
                    value_array[3] = pregegeration[thisone, number2];
                    value_array[4] = truckpregegeration[thisone, number1];
                    value_array[5] = truckpregegeration[thisone, number2];
                    value_array[6] = tempfitness;
                }
                return value_array;
                #endregion
            }
        }

        /// <summary>
        /// 求空载运行的距离
        /// </summary>
        /// <param name="taskno0">起始任务</param>
        /// <param name="tasknon">目标任务</param>
        /// <returns>空载距离</returns>
        public double unloadPath(int taskno0,int tasknon)
        {
            int i = 0;
            int j = 0;
            string[] e;
            e = new string[2];
            int[,] p;
            p = new int[2, 2];
            if (taskno0 == tasknon)//计算一个任务中堆场间负载时的情况
            {
                Console.WriteLine("需要计算空载时的最优路径，请使用空载路径求解算法！");
            }
            else//计算两个任务之间的空载情况
            {
                e[0] = getFirstPath(originpath3[taskno0]);
                e[1] = getLastPath(originpath1[tasknon]);
            }
            for (i = 0; i < e.Count(); i++)//读取堆位坐标
            {
                for (int l = 1; l < duiweino + 1; l++)
                {
                    if (e[i] == duiweiID[l - 1])
                    {
                        p[0, i] = duiweixy[0, l - 1];
                        p[1, i] = duiweixy[1, l - 1];
                    }
                }
            }
            xy[0, 0] = p[0, 0];
            xy[1, 0] = p[1, 0];
            xy[0, crossno + 1] = p[0, 1];
            xy[1, crossno + 1] = p[1, 1];
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
            int[,] c;
            c = new int[crossno + 2, crossno + 2];
            for (i = 1; i < crossno + 1; i++)
            {
                if (i == 1)
                {
                    for (j = 1; j < 8; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 8; j < 11; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 11]) + Math.Abs(xy[1, 1] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 11; j < 13; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 13; j < 16; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 16]) + Math.Abs(xy[1, 1] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                }
                if (i == 3)
                {
                    for (j = 1; j < 8; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 8; j < 11; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 11]) + Math.Abs(xy[1, 3] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 11; j < 13; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 13; j < 16; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 16]) + Math.Abs(xy[1, 3] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                        c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);

                    }
                }
                else
                {
                    for (j = 1; j < crossno + 1; j++)
                    {
                        c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                }
            }
            //for (i = 1; i < crossno + 1; i++)
            //{
            //    if (slength == 0)
            //    {
            //        c[i, 0] = c[0, i] = c[start1, i] - c[0, start1];
            //    }
            //    else if (slength1 != 0)
            //    {
            //        c[i, 0] = c[0, i] = c[0, start1] + c[start1, i];
            //    }
            //}
            //求取终点位置与其他路口的距离
            //for (i = 1; i < crossno + 1; i++)
            //{
            //    if (elength1 == 0)
            //    {
            //        c[crossno + 1, i] = c[i, crossno + 1] = c[i, end1] - c[end1, crossno + 1];
            //    }
            //    else if (elength != 0)
            //    {
            //        c[crossno + 1, i] = c[i, crossno + 1] = c[i, end1] + c[end1, crossno + 1];
            //    }
            //}
            c[0, start0] = c[start0, 0] = Math.Abs(xy[0, 0] - xy[0, start0]) + Math.Abs(xy[1, 0] - xy[1, start0]);
            for (i = 1; i < crossno + 1; i++)
            {
                c[i, 0] = c[0, i] = c[start0, i] + c[0, start0];
            }
            c[end0, crossno + 1] = c[crossno + 1, end0] = Math.Abs(xy[0, end0] - xy[0, crossno + 1]) + Math.Abs(xy[1, end0] - xy[1, crossno + 1]);
            for (i = 1; i < crossno + 1; i++)
            {
                c[i, crossno + 1] = c[crossno + 1, i] = c[i, end0] + c[end0, crossno + 1];
            }
            //求取初始位置与结束位置之间的距离
            c[0, crossno + 1] = c[0, start0] + c[start0, end0] + c[end0, crossno + 1];
            AdjacencyList<string> adjacency = new AdjacencyList<string>();
            adjacency.AddVertex(start0.ToString());
            for (int lukou = 1; lukou < 17; lukou++)
            {
                if (lukou != start0 && lukou != end0)
                {
                    adjacency.AddVertex(lukou.ToString());
                }
            }
            if (start0 != end0)
            {
                adjacency.AddVertex(end0.ToString());
            }
            else
            {
                bestkongzaipath = "";
                bestkongzaipath = (start0).ToString();
                return 0;
            }
            //构建道路网络图
            adjacency.AddEdge("1", "2");
            adjacency.AddEdge("1", "3");
            adjacency.AddEdge("2", "4");
            adjacency.AddEdge("3", "4");
            adjacency.AddEdge("4", "5");
            adjacency.AddEdge("4", "6");
            adjacency.AddEdge("5", "7");
            adjacency.AddEdge("6", "7");
            adjacency.AddEdge("6", "11");
            adjacency.AddEdge("7", "12");
            adjacency.AddEdge("12", "11");
            adjacency.AddEdge("11", "10");
            adjacency.AddEdge("11", "16");
            adjacency.AddEdge("10", "9");
            adjacency.AddEdge("10", "15");
            adjacency.AddEdge("16", "15");
            adjacency.AddEdge("9", "14");
            adjacency.AddEdge("14", "15");
            adjacency.AddEdge("9", "8");
            adjacency.AddEdge("8", "13");
            adjacency.AddEdge("13", "14");
            string[] sumpath = adjacency.DFSTraverse();
            int unloaddistance = 0;
            int unloadturn = 0;
            List<int> unloaddis = new List<int>();
            List<int> unloadt = new List<int>();
            for (int s = 0; s < sumpath.Length; s++)
            {
                if (sumpath[s] == null)
                {
                    break;
                }
                else
                {
                    if (sumpath[s].Contains('-'))
                    {
                        string[] temp = sumpath[s].Split('-');
                        int[,] patharray = new int[sumpath.Length, temp.Length];
                        for (int arr = 0; arr < temp.Length; arr++)
                        {
                            int num = Convert.ToInt32(temp[arr]);
                            patharray[s, arr] = num;
                        }
                        unloaddistance = 0;
                        for (int arr = 0; arr < temp.Length - 1; arr++)
                        {
                            unloaddistance += c[patharray[s, arr], patharray[s, arr + 1]];
                        }
                        unloaddis.Add(unloaddistance);
                        unloadturn = 0;
                        for (int arr = 0; arr < temp.Length - 2; arr++)
                        {
                            if ((xy[0, patharray[s, arr]] - xy[0, patharray[s, arr + 1]] == 0) && (xy[1, patharray[s, arr+1]] - xy[1, patharray[s, arr + 2]]) == 0)
                            {
                                unloadturn += 1;
                            }
                            if ((xy[1, patharray[s, arr]] - xy[1, patharray[s, arr + 1]] == 0) && (xy[0, patharray[s, arr+1]] - xy[0, patharray[s, arr + 2]]) == 0)
                            {
                                unloadturn += 1;
                            }
                        }
                        unloadt.Add(unloadturn);
                    }
                }
            }
            double t = 1000;
            double k = 0;
            bestkongzaipath = "";
            for (int load = 0; load < unloaddis.Count; load++)
            {
                if (t > (unloaddis[load]+unloadt[load]*1.5))
                {
                    t =  unloaddis[load]+unloadt[load]*3;
                    k = unloaddis[load];
                    bestkongzaipath = sumpath[load];
                }
            }
            return k;
        }

        /// <summary>
        /// 采用深度优先遍历算法求解负载时的最优路径
        /// </summary>
        /// <param name="taskwidth">任务宽度</param>
        /// <param name="taskno0">起始任务</param>
        /// <param name="tasknon">目标任务</param>
        /// <returns>负载距离（包含转向）</returns>
        public double loadPath(int taskno0, int tasknon)
        {
            #region
            int i = 0;
            int j = 0;
            string[] e;
             e= new string[2];
            int[,] p;
            p = new int[2, 2];
            if (taskno0 == tasknon)//计算一个任务中堆场间负载时的情况
            {
                e[0] = getLastPath(originpath1[taskno0]);
                e[1] = getFirstPath(originpath3[tasknon]);
            }
            else//计算两个任务之间的空载情况
            {
                Console.WriteLine("需要计算空载时的最优路径，请使用空载路径求解算法！");
            }
            for (i = 0; i < e.Count(); i++)//读取堆位坐标
            {
                for (int l = 1; l < duiweino + 1; l++)
                {
                    if (e[i] == duiweiID[l - 1])
                    {
                        p[0, i] = duiweixy[0, l - 1];
                        p[1, i] = duiweixy[1, l - 1];
                    }
                }
            }
            //将起始任务和终点任务的位置作为第1个路口和第17个路口
            xy[0, 0] = p[0, 0];
            xy[1, 0] = p[1, 0];
            xy[0, crossno + 1] = p[0, 1];
            xy[1, crossno + 1] = p[1, 1];
            //初始化道路通行情况
            //for (j = 0; j < crossno + 2; j++)
            //{
            //    roadcapacity[0, j] = roadcapacity[j, 0] = 0;
            //}
            //for (j = 0; j < crossno + 2; j++)
            //{
            //    roadcapacity[crossno + 1, j] = roadcapacity[j, crossno + 1] = 0;
            //}
            //求取距离起始位置距离最近的路口，并将其路口号赋值给start0。
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
            //求取距离结束位置最近的点，并将其赋值给end0。
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
            //Console.WriteLine(e[0]+" "+e[1]+" "+start0+" "+start1+" "+end0+" "+end1);
            //int index = 0;
            //if ((start0 == end0) && (start1 == end1))
            //{
            //    index = 1;
            //}
            //if ((start0 == end1) && (start1 == end0))
            //{
            //    index = 1;
            //}
            //
            //roadcapacity[0, start0] = roadcapacity[0, start1] = roadcapacity[start0, 0] = roadcapacity[start1, 0] = roadcapacity[start0, start1];
            //roadcapacity[end0, crossno + 1] = roadcapacity[end1, crossno + 1] = roadcapacity[crossno + 1, end0] = roadcapacity[crossno + 1, end1] = roadcapacity[end0, end1];
            //起点终点道路插入
            //int[,] roadchoice;
            //roadchoice = new int[crossno + 2, crossno + 2];//可通行道路情况
            //for (i = 0; i < crossno + 2; i++)
            //{
            //    for (j = 0; j < crossno + 2; j++)
            //    {
            //        roadchoice[i, j] = 0;
            //    }
            //}
            //for (i = 0; i < crossno + 2; i++)
            //{
            //    for (j = 0; j < crossno + 2; j++)
            //    {
            //        if ((roadcapacity[i, j] - roadsituation[i, j]) > roadwidth)
            //            roadchoice[i, j] = 1;
            //        else
            //            roadchoice[i, j] = 0;
            //    }
            //}
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
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 8; j < 11; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 11]) + Math.Abs(xy[1, 1] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 11; j < 13; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 13; j < 16; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 16]) + Math.Abs(xy[1, 1] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                }
                if (i == 3)
                {
                    for (j = 1; j < 8; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 8; j < 11; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 11]) + Math.Abs(xy[1, 3] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 11; j < 13; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 13; j < 16; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 16]) + Math.Abs(xy[1, 3] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                        //if (roadchoice[i, j] == 1)
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
     
                    }
                }
                else
                {
                    for (j = 1; j < crossno + 1; j++)
                    {
                       c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                    }
                }
            }
            #endregion
            #region//求取初始位置，结束位置与其他路口的距离
            //求取初始位置与其他路口的距离
            //for (i = 1; i < crossno + 1; i++)
            //{
            //            if (slength == 0)
            //            {
            //                c[i, 0] = c[0, i] = c[start1, i] - c[0, start1];
            //            }
            //            else if (slength1 != 0)
            //            {
            //                c[i, 0] = c[0, i] = c[0, start1] + c[start1, i];
            //            }
            //}
            c[0, start0] = c[start0, 0] = Math.Abs(xy[0, 0] - xy[0, start0]) + Math.Abs(xy[1,0]-xy[1,start0]);
            for (i = 1; i < crossno + 1; i++)
            {
                c[i, 0] = c[0, i] = c[start0, i] + c[0, start0];
            }
                //求取终点位置与其他路口的距离
                //for (i = 1; i < crossno + 1; i++)
                //{
                //    if (elength1 == 0)
                //    {
                //        c[crossno + 1, i] = c[i, crossno + 1] = c[i, end1] - c[end1, crossno + 1];
                //    }
                //    else if (elength != 0)
                //    {
                //        c[crossno + 1, i] = c[i, crossno + 1] = c[i, end1] + c[end1, crossno + 1];
                //    }
                //}
            c[end0, crossno + 1] = c[crossno + 1, end0] = Math.Abs(xy[0, end0] - xy[0, crossno + 1]) + Math.Abs(xy[1,end0]-xy[1,crossno+1]);
            for (i = 1; i < crossno + 1; i++)
            {
                c[i, crossno + 1] = c[crossno + 1, i] = c[i, end0] + c[end0, crossno + 1];
            }
                //求取初始位置与结束位置之间的距离
                c[0, crossno + 1] = c[0, start0] + c[start0, end0] + c[end0, crossno + 1];
            #endregion
            #region //使用深度优先遍历算法求解最优路径
            AdjacencyList<string> adjacency= new AdjacencyList<string>();
            adjacency.AddVertex(start0.ToString());
            for (int lukou = 1; lukou < 17; lukou++)
            {
                if (lukou !=start0 && lukou!=end0)
                {
                    adjacency.AddVertex(lukou.ToString());
                }
            }
            if (start0 != end0)
            {
                adjacency.AddVertex(end0.ToString());
            }
            else
            {
                bestfuzaipath = "";
                bestfuzaipath = (start0).ToString();
                return 0;
            }
            //构建道路网络图
            adjacency.AddEdge("1", "2");
            adjacency.AddEdge("1", "3");
            adjacency.AddEdge("2", "4");
            adjacency.AddEdge("3", "4");
            adjacency.AddEdge("4", "5");
            adjacency.AddEdge("4", "6");
            adjacency.AddEdge("5", "7");
            adjacency.AddEdge("6", "7");
            adjacency.AddEdge("6", "11");
            adjacency.AddEdge("7", "12");
            adjacency.AddEdge("12", "11");
            adjacency.AddEdge("11", "10");
            adjacency.AddEdge("11", "16");
            adjacency.AddEdge("10", "9");
            adjacency.AddEdge("10", "15");
            adjacency.AddEdge("16", "15");
            adjacency.AddEdge("9", "14");
            adjacency.AddEdge("14", "15");
            adjacency.AddEdge("9", "8");
            adjacency.AddEdge("8", "13");
            adjacency.AddEdge("13", "14");
            string[] sumpath=adjacency.DFSTraverse();
            int loaddistance = 0;
            int loadturn = 0;
            List<int> loaddis = new List<int>();
            List<int> turn = new List<int>();
            for (int s = 0; s < sumpath.Length; s++)
            {
                if (sumpath[s] == null)
                {
                    break;
                }
                else
                {
                    if (sumpath[s].Contains('-'))
                    {
                        string[] temp = sumpath[s].Split('-');
                        int[,] patharray = new int[sumpath.Length, temp.Length];
                        for (int arr = 0; arr < temp.Length; arr++)
                        {
                            int num = Convert.ToInt32(temp[arr]);
                            patharray[s, arr] = num;
                        }
                        loaddistance = 0;
                        for (int arr = 0; arr < temp.Length - 1; arr++)
                        {
                            loaddistance += c[patharray[s, arr], patharray[s, arr + 1]];
                        }
                        loaddis.Add(loaddistance);
                        loadturn = 0;
                        for (int arr = 0; arr < temp.Length - 2; arr++)
                        {
                            if ((xy[0, patharray[s, arr]] - xy[0, patharray[s, arr + 1]] == 0) && (xy[1, patharray[s, arr+1]] - xy[1, patharray[s, arr + 2]]) == 0)
                            {
                                loadturn += 1;
                            }
                            if ((xy[1, patharray[s, arr]] - xy[1, patharray[s, arr + 1]] == 0) && (xy[0, patharray[s, arr+1]] - xy[0, patharray[s, arr + 2]]) == 0)
                            {
                                loadturn += 1;
                            }
                        }
                        turn.Add(loadturn);
                    }
                }
            }
            double t = 1000;
            double k = 0;
            bestfuzaipath = "";
            for (int load = 0; load < loaddis.Count; load++)
            {
                if (t > (loaddis[load] + turn[load]*1.5))
                {
                    t = loaddis[load] + turn[load]*1.5;
                    k = loaddis[load]+turn[load];
                    bestfuzaipath= sumpath[load];
                }
            }
            return k;
            #endregion
        }


        /// <summary>
        /// 求堆场内路径的最后节点
        /// </summary>
        /// <param name="path">输入路径</param>
        /// <returns>路径中最后一个点</returns>
        public string getLastPath(string path)
        {
            string pointPath = "";
            int number;
            number=path.LastIndexOf('-');
            if (number == -1)
            {
                pointPath = path;
            }
            else
            {
                pointPath = path.Substring(number+1);
            }
            return pointPath;
        }
        /// <summary>
        /// 求堆场内路径的第一个节点
        /// </summary>
        /// <param name="path">输入路径</param>
        /// <returns>路径中第一个节点</returns>
        public string getFirstPath(string path)
        {
            string pointPath = "";
            int number;
            number = path.IndexOf('-');
            if (number == -1)
            {
                pointPath = path;
            }
            else
            {
                pointPath = path.Substring(0,number);
            }
            return pointPath;
        }
    }
}
