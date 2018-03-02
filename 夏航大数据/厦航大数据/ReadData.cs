using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace 厦航大数据
{
    class ReadData
    {
        public string path = "";
        public int[] AN = new int[2364];//航班号
        public int[] AI = new int[2364];//航班ID
        public int[] AP = new int[2364];//飞机ID
        public int[] SP = new int[2364];//起飞机场
        public int[] EP = new int[2364];//降落机场
        public int[,] ST = new int[2364,3];//起飞时间
        public int[,] ET = new int[2364,3];//降落时间
        public double[] I = new double[2364];//重要系数
        public int[] PT = new int[2364];//机型
        public int[] date = new int[2364];//航班时间
        public int[] is_country = new int[2364];//国际/国内(2/1)
        public string[] temp = new string[2364];
        List<int[]> Y = new List<int[]>();//决策变量,起飞机场-到达机场-飞机的限制。
        int[] table = new int[3];
        public int[] ConAirport = new int[6];//关闭的机场
        public int[,] closedTime = new int[6, 2];//关闭时间
        public int[,] openTime = new int[6, 2];//开放时间
        public List<int[]> closedDate = new List<int[]>();//生效日期
        public List<int[]> openDate = new List<int[]>();//失效日期
        //public List<int> FlyTime_PT = new List<int>();//飞行时间里的飞机机型
        //public List<int> FlyTime_SP = new List<int>();//起飞机场
        //public List<int> FlyTime_EP = new List<int>();//降落机场
        //public List<int> FlyTime = new List<int>();//飞行时间
        public List<List<int>> AI_of_PI = new List<List<int>>();//每个飞机上的航班号  
        public List<List<int>> tempAOP = new List<List<int>>();
        public DateTime[] st = new DateTime[2364];
        public DateTime[] et = new DateTime[2364];
        public DateTime[] Date = new DateTime[2364];
        public List<DateTime[]> TF_S = new List<DateTime[]>();//49 50 61 机场的台风开始时间和关闭时间
        public List<DateTime[]> TF_E = new List<DateTime[]>();//49 50 61 机场的台风结束时间和关闭时间
        public List<int[]> Fly_Time = new List<int[]>();
        public ReadData(string str)
        {
            path = @"C:\Users\DELL\Desktop\厦航大数据.accdb";
        }
        public void read_Input()
        {
            string AccessConnection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+path;
            OleDbConnection odcConnection = new OleDbConnection(AccessConnection);
            odcConnection.Open();
            OleDbCommand odCommand = odcConnection.CreateCommand();
            odCommand.CommandText = "select 航班ID,日期,国际国内,航班号,起飞机场,降落机场,起飞时间,降落时间,飞机ID,机型,重要系数 from 航班";
            OleDbDataReader Reader = odCommand.ExecuteReader();
            int i = 0;
            while (Reader.Read())
            {
                AI[i] = i+1;
                Date[i] = Convert.ToDateTime(Reader["日期"]);
                temp[i] = Reader["日期"].ToString();
                string[] temp1 = temp[i].Split(' ');
                string[] temp4 = temp1[0].Split('/');
                date[i] = Convert.ToInt32(temp4[2]);
                temp[i] = Reader["国际国内"].ToString();
                if(temp[i]=="国内")
                {
                    is_country[i] = 1;
                }
                else if(temp[i]=="国际")
                {
                    is_country[i] = 2;
                }
                AN[i] = Convert.ToInt32(Reader["航班号"]);
                SP[i] = Convert.ToInt32(Reader["起飞机场"]);
                EP[i] = Convert.ToInt32(Reader["降落机场"]);
                st[i] = Convert.ToDateTime(Reader["起飞时间"]);
                temp[i] = Reader["起飞时间"].ToString();
                temp1 = temp[i].Split(' ');
                string[] temp2 = temp1[0].Split('/');
                string[] temp3 = temp1[1].Split(':');
                ST[i,0] = Convert.ToInt32(temp2[2]);
                ST[i, 1] = Convert.ToInt32(temp3[0]);
                ST[i, 2] = Convert.ToInt32(temp3[1]);
                et[i] = Convert.ToDateTime(Reader["降落时间"]);
                temp[i] = Reader["降落时间"].ToString();
                temp1 = temp[i].Split(' ');
                temp2 = temp1[0].Split('/');
                temp3 = temp1[1].Split(':');
                ET[i, 0] = Convert.ToInt32(temp2[2]);
                ET[i, 1] = Convert.ToInt32(temp3[0]);
                ET[i, 2] = Convert.ToInt32(temp3[1]);
                AP[i] = Convert.ToInt32(Reader["飞机ID"]);
                PT[i] = Convert.ToInt32(Reader["机型"]);
                I[i] = Convert.ToInt32(Reader["重要系数"]);
                i++;
            }
            Reader.Close();
            odCommand.CommandText = "select ID,起飞机场,降落机场,飞机ID from 航线飞机限制";
            Reader = odCommand.ExecuteReader();
            i = 0;
            while (Reader.Read())
            {
                table[0] = Convert.ToInt32(Reader["起飞机场"]);
                table[1] = Convert.ToInt32(Reader["降落机场"]);
                table[2] = Convert.ToInt32(Reader["飞机ID"]);
                int[] temparray1 = {table[0],table[1],table[2] };
                Y.Add(temparray1);
                i++;
            }
            Reader.Close();
            odCommand.CommandText = "select 机场,关闭时间,开放时间,生效日期,失效日期 from 机场关闭限制";
            Reader = odCommand.ExecuteReader();
            i = 0;
            while (Reader.Read())
            {
                ConAirport[i] = Convert.ToInt32(Reader["机场"]);
                string temp =Reader["关闭时间"].ToString();
                string[] temp1 = temp.Split(' ');
                string[] temp2 = temp1[1].Split(':');
                closedTime[i, 0] = Convert.ToInt32(temp2[0]);
                closedTime[i, 0] = Convert.ToInt32(temp2[1]);
                temp = Reader["开放时间"].ToString();
                temp1 = temp.Split(' ');
                temp2 = temp1[1].Split(':');
                openTime[i, 0] = Convert.ToInt32(temp2[0]);
                openTime[i, 0] = Convert.ToInt32(temp2[1]);
                temp = Reader["生效日期"].ToString();
                temp1 = temp.Split(' ');
                temp2 = temp1[0].Split('/');
                int[] temparray = {Convert.ToInt32(temp2[0]), Convert.ToInt32(temp2[1]),Convert.ToInt32(temp2[2])};
                closedDate.Add(temparray);
                temp = Reader["失效日期"].ToString();
                temp1 = temp.Split(' ');
                temp2 = temp1[0].Split('/');
                int[] temparray1 ={ Convert.ToInt32(temp2[0]), Convert.ToInt32(temp2[1]), Convert.ToInt32(temp2[2]) };
                openDate.Add(temparray1);
                i++;
            }
            Reader.Close();
            odCommand.CommandText = "select 飞机机型,起飞机场,降落机场,飞行时间（分钟） from 飞行时间";
            Reader = odCommand.ExecuteReader();
            i = 0;
            while (Reader.Read())
            {
                //FlyTime_PT.Add(Convert.ToInt32(Reader["飞机机型"]));
                //FlyTime_SP.Add(Convert.ToInt32(Reader["起飞机场"]));
                //FlyTime_EP.Add(Convert.ToInt32(Reader["降落机场"]));
                //FlyTime.Add(Convert.ToInt32(Reader["飞行时间(分钟)"]));
                int[] temp = { Convert.ToInt32(Reader["飞机机型"]), Convert.ToInt32(Reader["起飞机场"]), Convert.ToInt32(Reader["降落机场"]), Convert.ToInt32(Reader["飞行时间(分钟)"]) };
                Fly_Time.Add(temp);
                i++;
            }
            Reader.Close();
            odCommand.CommandText = "select 开始时间,结束时间,影响类型,机场 from 台风场景";
            Reader = odCommand.ExecuteReader();
            i = 0;
            DateTime[] temp7 = new DateTime[9];
            DateTime[] temp8 = new DateTime[9];
            while (Reader.Read())
            {
                temp7[i] = Convert.ToDateTime(Reader["开始时间"]);
                temp8[i] = Convert.ToDateTime(Reader["结束时间"]);
                i++;
            }
            Reader.Close();
            odcConnection.Close();
            TF_S.Add(new DateTime[3] { temp7[0], temp7[1], temp7[6] });
            TF_S.Add(new DateTime[3] { temp7[2], temp7[3], temp7[7] });
            TF_S.Add(new DateTime[3] { temp7[4], temp7[5], temp7[8] });
            TF_E.Add(new DateTime[3] { temp8[0], temp8[1], temp8[6] });
            TF_E.Add(new DateTime[3] { temp8[2], temp8[3], temp8[7] });
            TF_E.Add(new DateTime[3] { temp8[4], temp8[5], temp8[8] });
            for (int l=0;l<142;l++)
            {
                AI_of_PI.Add(new List<int>());
                tempAOP.Add(new List<int>());
            }
            for(int j=0;j<2364;j++)
            {
                for(int l=0;l<142;l++)
                {
                    if(AP[j]==l+1)
                    {
                        tempAOP[l].Add(AI[j]);
                    }
                }
            }     
            for(int j=0;j<142;j++)//求每个飞机上的任务序列
            {
                while (tempAOP[j].Count > 0)
                {
                    int label1 = 0;
                    int early_date = 9;
                    int early_hour = 25;
                    int early_min = 61;
                    int k = 0;
                    for (k = 0; k < tempAOP[j].Count; k++)
                    {
                        if (early_date > ST[tempAOP[j][k]-1,0])
                        {
                            early_date = ST[tempAOP[j][k]-1, 0];
                            early_hour = ST[tempAOP[j][k]-1, 1];
                            early_min = ST[tempAOP[j][k]-1, 2];
                            label1 = k;
                        }
                        else if (early_date==ST[tempAOP[j][k] - 1, 0])
                        {
                            if (early_hour > ST[tempAOP[j][k] - 1, 1])
                            {
                                early_hour = ST[tempAOP[j][k] - 1, 1];
                                early_min = ST[tempAOP[j][k] - 1, 2];
                                label1 = k;
                            }
                            else if (early_hour == ST[tempAOP[j][k] - 1, 1])
                            {
                                if (early_min > ST[tempAOP[j][k] - 1, 2])
                                {
                                    early_min = ST[tempAOP[j][k] - 1, 2];
                                    label1 = k;
                                }
                            }
                        }                      
                    }
                    int temp1 = tempAOP[j][label1];
                    AI_of_PI[j].Add(temp1);
                    tempAOP[j].RemoveAt(label1);
                }
            }
        }
    }
}
