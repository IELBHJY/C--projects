using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace 厦航大数据
{
    class bad_airport_count
    {
        public List<int> bad_airport_ID = new List<int>();
        public List<int[]> lianchenglazhi = new List<int[]>();
        public List<int> bad_jiangluo = new List<int>();
        public List<int> bad_qifei = new List<int>();
        public List<int[]> yanchi_10hours = new List<int[]>();
        public List<int> airplane_ID_10Hours = new List<int>();
        public static string str = "";
        public bad_airport_count(string str1)
        {
            str = str1;
        }
        ReadData rd = new ReadData(str);
        public int show_count()
        {
            rd.read_Input();
            int count = 0;
            #region
            for (int i = 0; i < rd.AI.Length; i++)
            {
                if (rd.EP[i] == 49)
                {
                    if (rd.ET[i, 0] == 6)
                    {
                        if (rd.ET[i, 1] >= 14)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                        if (rd.ET[i, 1] == 14)
                        {
                            if (rd.ET[i, 2] == 0)
                            {
                                count--;
                                bad_airport_ID.Remove(i + 1);
                            }
                        }
                    }
                    if (rd.ET[i, 0] == 7)
                    {
                        if (rd.ET[i, 1] <= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                    }
                }
                if (rd.SP[i] == 49)
                {
                    if (rd.ST[i, 0] == 6)
                    {
                        if (rd.ST[i, 1] >= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                        if (rd.ST[i, 1] == 16)
                        {
                            if (rd.ST[i, 2] == 0)
                            {
                                count--;
                                bad_airport_ID.Remove(i + 1);
                            }
                        }
                    }
                    if (rd.ST[i, 0] == 7)
                    {
                        if (rd.ST[i, 1] <= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                    }
                }
                if (rd.EP[i] == 50)
                {
                    if (rd.ET[i, 0] == 6)
                    {
                        if (rd.ET[i, 1] >= 14)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                        if (rd.ET[i, 1] == 14)
                        {
                            if (rd.ET[i, 2] == 0)
                            {
                                count--;
                                bad_airport_ID.Remove(i + 1);
                            }
                        }
                    }
                    if (rd.ET[i, 0] == 7)
                    {
                        if (rd.ET[i, 1] <= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                    }
                }
                if (rd.SP[i] == 50)
                {
                    if (rd.ST[i, 0] == 6)
                    {
                        if (rd.ST[i, 1] >= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                        if (rd.ST[i, 1] == 16)
                        {
                            if (rd.ST[i, 2] == 0)
                            {
                                count--;
                                bad_airport_ID.Remove(i + 1);
                            }
                        }
                    }
                    if (rd.ST[i, 0] == 7)
                    {
                        if (rd.ST[i, 1] <= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                    }
                }
                if (rd.EP[i] == 61)
                {
                    if (rd.ET[i, 0] == 6)
                    {
                        if (rd.ET[i, 1] >= 14)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                        if (rd.ET[i, 1] == 14)
                        {
                            if (rd.ET[i, 2] == 0)
                            {
                                count--;
                                bad_airport_ID.Remove(i + 1);
                            }
                        }
                    }
                    if (rd.ET[i, 0] == 7)
                    {
                        if (rd.ET[i, 1] <= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                    }
                }
                if (rd.SP[i] == 61)
                {
                    if (rd.ST[i, 0] == 6)
                    {
                        if (rd.ST[i, 1] >= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                        if (rd.ST[i, 1] == 16)
                        {
                            if (rd.ST[i, 2] == 0)
                            {
                                count--;
                                bad_airport_ID.Remove(i + 1);
                            }
                        }
                    }
                    if (rd.ST[i, 0] == 7)
                    {
                        if (rd.ST[i, 1] <= 16)
                        {
                            count++;
                            bad_airport_ID.Add(i + 1);
                        }
                    }
                }
            }
            #endregion
            for (int i = 0; i < rd.AI.Length; i++)
            {
                if ((rd.SP[i] == 49 && rd.EP[i] == 50) || (rd.SP[i] == 49 && rd.EP[i] == 61))
                {
                    if (rd.ST[i, 0] >= 6 && rd.ET[i, 0] <= 7)
                    {
                        if (rd.ST[i, 1] >= 14 && rd.ET[i, 1] <= 16)
                        {
                            count--;
                            bad_airport_ID.Remove(i + 1);
                        }
                    }
                }
                if ((rd.SP[i] == 50 && rd.EP[i] == 49) || (rd.SP[i] == 50 && rd.EP[i] == 61))
                {
                    if (rd.ST[i, 0] >= 6 && rd.ET[i, 0] <= 7)
                    {
                        if (rd.ST[i, 1] >= 14 && rd.ET[i, 1] <= 16)
                        {
                            count--;
                            bad_airport_ID.Remove(i + 1);
                        }
                    }
                }
                if ((rd.SP[i] == 61 && rd.EP[i] == 49) || (rd.SP[i] == 61 && rd.EP[i] == 50))
                {
                    if (rd.ST[i, 0] >= 6 && rd.ET[i, 0] <= 7)
                    {
                        if (rd.ST[i, 1] >= 14 && rd.ET[i, 1] <= 16)
                        {
                            count--;
                            bad_airport_ID.Remove(i + 1);
                        }
                    }
                }
            }
            //求解停机航班数
            for (int i = 0; i < 142; i++)
            {
                for (int j = 0; j < rd.AI_of_PI[i].Count; j++)
                {
                    if (rd.EP[rd.AI_of_PI[i][j] - 1] == 49)
                    {
                        if (rd.ET[rd.AI_of_PI[i][j] - 1, 0] < 6)
                        {
                            if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] > 7)
                            {
                                count++;
                            }
                            else if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] == 7)
                            {
                                if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 1] >= 17)
                                {
                                    count++;
                                }
                            }
                        }
                        else if (rd.ET[rd.AI_of_PI[i][j] - 1, 0] == 6)
                        {
                            if (rd.ET[rd.AI_of_PI[i][j] - 1, 1] <= 15)
                            {
                                if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] > 7)
                                {
                                    count++;
                                }
                                else if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] == 7)
                                {
                                    if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 1] >= 17)
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                    if (rd.EP[rd.AI_of_PI[i][j] - 1] == 61)
                    {
                        if (rd.ET[rd.AI_of_PI[i][j] - 1, 0] < 6)
                        {
                            if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] > 7)
                            {
                                count++;
                            }
                            else if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] == 7)
                            {
                                if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 1] >= 17)
                                {
                                    count++;
                                }
                            }
                        }
                        else if (rd.ET[rd.AI_of_PI[i][j] - 1, 0] == 6)
                        {
                            if (rd.ET[rd.AI_of_PI[i][j] - 1, 1] <= 15)
                            {
                                if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] > 7)
                                {
                                    count++;
                                }
                                else if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] == 7)
                                {
                                    if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 1] >= 17)
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                    if (rd.EP[rd.AI_of_PI[i][j] - 1] == 50)
                    {
                        if (rd.ET[rd.AI_of_PI[i][j] - 1, 0] < 6)
                        {
                            if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] > 7)
                            {
                                count++;
                            }
                            else if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] == 7)
                            {
                                if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 1] >= 17)
                                {
                                    count++;
                                }
                            }
                        }
                        else if (rd.ET[rd.AI_of_PI[i][j] - 1, 0] == 6)
                        {
                            if (rd.ET[rd.AI_of_PI[i][j] - 1, 1] <= 15)
                            {
                                if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] > 7)
                                {
                                    count++;
                                }
                                else if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 0] == 7)
                                {
                                    if (rd.ST[rd.AI_of_PI[i][j + 1] - 1, 1] >= 17)
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Console.WriteLine(count);
            return count;
        }

        public int show_badairport()
        {
            rd.read_Input();
            int count = 0;
            #region//起飞降落数
            for (int i = 0; i < rd.AI.Length; i++)
            {
                if (rd.EP[i] == 49)
                {
                    if (rd.et[i] > rd.TF_S[0][0] && rd.et[i] < rd.TF_E[0][0])
                    {
                        bad_airport_ID.Add(i + 1);
                        bad_jiangluo.Add(i + 1);
                        count++;
                    }
                }
                if (rd.SP[i] == 49)
                {
                    if (rd.st[i] > rd.TF_S[0][1] && rd.st[i] < rd.TF_E[0][1])
                    {
                        bad_airport_ID.Add(i + 1);
                        bad_qifei.Add(i + 1);
                        count++;
                    }
                }
                if (rd.EP[i] == 50)
                {
                    if (rd.et[i] > rd.TF_S[1][0] && rd.et[i] < rd.TF_E[1][0])
                    {
                        bad_airport_ID.Add(i + 1);
                        bad_jiangluo.Add(i + 1);
                        count++;
                    }
                }
                if (rd.SP[i] == 50)
                {
                    if (rd.st[i] > rd.TF_S[1][1] && rd.st[i] < rd.TF_E[1][1])
                    {
                        bad_airport_ID.Add(i + 1);
                        bad_qifei.Add(i + 1);
                        count++;
                    }
                }
                if (rd.EP[i] == 61)
                {
                    if (rd.et[i] > rd.TF_S[2][0] && rd.et[i] < rd.TF_E[2][0])
                    {
                        bad_airport_ID.Add(i + 1);
                        bad_jiangluo.Add(i + 1);
                        count++;
                    }
                }
                if (rd.SP[i] == 61)
                {
                    if (rd.st[i] > rd.TF_S[2][1] && rd.st[i] < rd.TF_E[2][1])
                    {
                        bad_airport_ID.Add(i + 1);
                        bad_qifei.Add(i + 1);
                        count++;
                    }
                }
            }
            #endregion//
            #region//停机数量
            for(int i=0;i<142;i++)
            {
                for(int j=0;j<rd.AI_of_PI[i].Count;j++)
                {
                    if(rd.EP[rd.AI_of_PI[i][j]-1]==49&&rd.et[rd.AI_of_PI[i][j]-1]<=rd.TF_S[0][2])
                    {
                        if(rd.st[rd.AI_of_PI[i][j+1]-1]>=rd.TF_E[0][2])
                        {
                            count++;
                            bad_airport_ID.Add(rd.AI_of_PI[i][j]);
                        }
                    }
                    if (rd.EP[rd.AI_of_PI[i][j] - 1] == 50 && rd.et[rd.AI_of_PI[i][j] - 1] <= rd.TF_S[1][2])
                    {
                        if (rd.st[rd.AI_of_PI[i][j + 1] - 1] >= rd.TF_E[1][2])
                        {
                            count++;
                            bad_airport_ID.Add(rd.AI_of_PI[i][j]);
                        }
                    }
                    if (rd.EP[rd.AI_of_PI[i][j] - 1] == 61 && rd.et[rd.AI_of_PI[i][j] - 1] <= rd.TF_S[2][2])
                    {
                        if (rd.st[rd.AI_of_PI[i][j + 1] - 1] >= rd.TF_E[2][2])
                        {
                            count++;
                            bad_airport_ID.Add(rd.AI_of_PI[i][j]);
                        }
                    }
                }
            }
            #endregion
            return count;
        }
        public void show_information()
        {
            for (int i = 0; i < bad_airport_ID.Count; i++)
            {
                Console.WriteLine("航班号：" + bad_airport_ID[i] + " " + "飞机号：" + rd.AP[bad_airport_ID[i] - 1] + " " + "起飞机场：" + rd.SP[bad_airport_ID[i] - 1] + " " + "降落机场：" + rd.EP[bad_airport_ID[i] - 1] +
                " " + "起飞时间：" + rd.ST[bad_airport_ID[i] - 1, 0] + "日" + " " + rd.ST[bad_airport_ID[i] - 1, 1] + ":" + rd.ST[bad_airport_ID[i] - 1, 2] +
                " " + " 降落时间：" + rd.ET[bad_airport_ID[i] - 1, 0] + "日" + " " + rd.ET[bad_airport_ID[i] - 1, 1] + ":" + rd.ET[bad_airport_ID[i] - 1, 2]);
            }
        }
        public void show_yanchi()
        {
            for (int i = 0; i < bad_airport_ID.Count; i++)
            {
                if (rd.SP[bad_airport_ID[i] - 1] == 61)
                {
                    Console.WriteLine("航班号：" + bad_airport_ID[i] + " " + "飞机号：" + rd.AP[bad_airport_ID[i] - 1] + " " + "起飞机场：" + rd.SP[bad_airport_ID[i] - 1] + " " + "降落机场：" + rd.EP[bad_airport_ID[i] - 1] +
                " " + "起飞时间：" + rd.ST[bad_airport_ID[i] - 1, 0] + "日" + " " + rd.ST[bad_airport_ID[i] - 1, 1] + ":" + rd.ST[bad_airport_ID[i] - 1, 2] +
                " " + " 降落时间：" + rd.ET[bad_airport_ID[i] - 1, 0] + "日" + " " + rd.ET[bad_airport_ID[i] - 1, 1] + ":" + rd.ET[bad_airport_ID[i] - 1, 2]);
                }
            }
        }
        public void show_plane_information()
        {
            for (int i = 0; i < rd.AI_of_PI.Count; i++)
            {
                for (int j = 0; j < rd.AI_of_PI[i].Count; j++)
                {
                    Console.WriteLine("飞机ID:" + (i + 1) + " " + "航班号：" + rd.AI_of_PI[i][j] + " " + "起飞机场：" + rd.SP[rd.AI_of_PI[i][j] - 1] +
                        " " + "降落机场：" + rd.EP[rd.AI_of_PI[i][j] - 1] + " " + "起飞时间：" + rd.ST[rd.AI_of_PI[i][j] - 1, 0] + "日" +
                        rd.ST[rd.AI_of_PI[i][j] - 1, 1] + ":" + rd.ST[rd.AI_of_PI[i][j] - 1, 2] + " " +
                        "降落时间：" + rd.ET[rd.AI_of_PI[i][j] - 1, 0] + "日" +
                        rd.ET[rd.AI_of_PI[i][j] - 1, 1] + ":" + rd.ET[rd.AI_of_PI[i][j] - 1, 2] + " ");
                }
            }
        }      
        public int show_lianchenglazhi()
        {
            int count = 0;
            for(int i=0;i<bad_jiangluo.Count;i++)
            {
                for(int j=0;j<rd.AI.Length;j++)
                {
                    if(rd.AN[bad_jiangluo[i]-1]==rd.AN[rd.AI[j]-1]&&rd.Date[bad_jiangluo[i]-1]==rd.Date[j]&&bad_jiangluo[i]!=rd.AI[j]&&rd.is_country[bad_jiangluo[i] - 1]==1&&rd.is_country[rd.AI[j]-1]==1)
                    {
                       if(rd.SP[rd.AI[j]-1]==rd.EP[bad_jiangluo[i]-1])
                        {
                            count++;
                            lianchenglazhi.Add(new int[] { bad_jiangluo[i], rd.AI[j] });
                            //Console.WriteLine(bad_jiangluo[i]+"--->>>"+rd.AI[j]);
                        }
                    }
                }
            }
            return count;
        }
        public int show_yanchigeshu()
        {
           int count = 0;
           for(int i=0;i<bad_jiangluo.Count;i++)
            {
                TimeSpan ts1 = new TimeSpan(24, 0, 0);
                TimeSpan ts2 = new TimeSpan(36, 0, 0);
                if(rd.is_country[bad_jiangluo[i]-1]==1)
                {
                    int index1 = rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1].IndexOf(bad_jiangluo[i]);
                    int num = 0;
                    if (!bad_qifei.Contains(rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1][index1-1]))
                    {
                        if (rd.TF_E[0][0] - rd.et[bad_jiangluo[i] - 1] <= ts1)
                        {
                            for (int j = index1; j < rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1].Count; j++)
                            {
                                if (bad_airport_ID.Contains(rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1][j]))
                                {
                                    num++;
                                }
                            }
                            if (num == 0)
                            {
                                count++;
                                Console.WriteLine("航班ID:" + bad_jiangluo[i] + " " + "飞机ID:" + rd.AP[bad_jiangluo[i] - 1]);
                            }
                            else
                            {

                            }
                        }
                    }
                }
                else if(rd.is_country[bad_jiangluo[i]-1]==2)
                {
                    int index = rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1].IndexOf(bad_jiangluo[i]);
                    int num = 0;
                    if (!bad_qifei.Contains(rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1][index - 1]))
                    {
                        if (rd.TF_E[0][0] - rd.et[bad_jiangluo[i] - 1] <= ts2)
                        {
                            for (int j = index; j < rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1].Count; j++)
                            {
                                if(bad_airport_ID.Contains(rd.AI_of_PI[rd.AP[bad_jiangluo[i] - 1] - 1][j]))
                                {
                                    num++;
                                }
                            }
                            if (num == 0)
                            {
                                count++;
                                Console.WriteLine("航班ID:" + bad_jiangluo[i] + " " + "飞机ID:" + rd.AP[bad_jiangluo[i] - 1]);
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            return count;
        }
        public int show_yanchi_10hours()//80个
        {
            int count = 0;
            TimeSpan ts1 = new TimeSpan(10, 50, 20);
            for(int i=0;i<rd.AI_of_PI.Count;i++)
            {
                for(int j=0;j<rd.AI_of_PI[i].Count;j++)
                {
                    if(bad_jiangluo.Contains(rd.AI_of_PI[i][j]))
                    {
                       if(rd.TF_E[0][0]-rd.et[rd.AI_of_PI[i][j]-1]<=ts1)
                        {
                            count++;
                            yanchi_10hours.Add(new int[] { rd.AI_of_PI[i][j], (i + 1) });
                        }
                    }
                }
            }
            Console.WriteLine("wancheng!");
            return count;
        }
        public void cal_yanchi_10hours()
        {
            int position = 0;
            TimeSpan ts1 = new TimeSpan(0, 50, 0);
            TimeSpan ts2 = new TimeSpan(24, 0, 0);
            TimeSpan ts3 = new TimeSpan(36, 0, 0);
            for(int i=0;i<yanchi_10hours.Count;i++)
            {
                position = rd.AI_of_PI[yanchi_10hours[i][1] - 1].IndexOf(yanchi_10hours[i][0]);
                rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1] = rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1] + (rd.TF_E[0][0] - rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position]-1]);
                rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1] = rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1] + (rd.TF_E[0][0] - rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position]-1]);
                airplane_ID_10Hours.Add(rd.AI_of_PI[yanchi_10hours[i][1] - 1][position]);
                if (bad_airport_ID.Contains(rd.AI_of_PI[yanchi_10hours[i][1] - 1][position + 1]))
                {
                    airplane_ID_10Hours.Add(rd.AI_of_PI[yanchi_10hours[i][1] - 1][position + 1]);
                }
                //Console.WriteLine("飞机ID:" + yanchi_10hours[i][1] + " " + "航班ID：" + rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] + " " +
                //                       "起飞地:" + rd.SP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1] + " " + rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1] + " " +
                //                       rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1] + " " + rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][position] - 1]);
                for (int j=position+1;j< rd.AI_of_PI[yanchi_10hours[i][1] - 1].Count;j++)
                {
                    TimeSpan temp = rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] - rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1];
                    if (rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j - 1] - 1] + ts1 > rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1])
                    {
                        if (rd.is_country[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 1)
                        {
                            if (rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j - 1] - 1] + ts1 - rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] <= ts2)
                            {
                                if (rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 49 || rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 50 || rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 61)
                                {
                                    if (rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j-1] - 1] + ts1 + temp >= rd.TF_E[0][0])
                                    {
                                        rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j - 1] - 1] + ts1;
                                        rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + temp;
                                       
                                    }
                                    else 
                                    {
                                        rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + (rd.TF_E[0][0] - rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1]);
                                        rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + (rd.TF_E[0][0] - rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1]);
                                        // Console.WriteLine("飞机ID:" + yanchi_10hours[i][1] + " " + "航班ID：" + rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] + " " +
                                        //"起飞地:" + rd.SP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + " " + rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + " " +
                                        //rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + " " + rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1]);
                                        airplane_ID_10Hours.Add(rd.AI_of_PI[yanchi_10hours[i][1] - 1][j]);
                                        if (bad_airport_ID.Contains(rd.AI_of_PI[yanchi_10hours[i][1] - 1][j + 1]))
                                        {
                                            airplane_ID_10Hours.Add(rd.AI_of_PI[yanchi_10hours[i][1] - 1][j + 1]);
                                        }
                                    }
                                }
                                else
                                {
                                    rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j - 1] - 1] + ts1;
                                    rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + temp;
                                    //Console.WriteLine("飞机ID:" + yanchi_10hours[i][1] + " " + "航班ID：" + rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] + " " +
                                    //"起飞地:" + rd.SP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + " " + rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + " " +
                                    //rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + " " + rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1]);
                                }
                            }
                            else
                            {
                                Console.WriteLine("超出国内延迟限制！");
                            }
                        }
                        else if (rd.is_country[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 2)
                        {
                            if (rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j - 1] - 1] + ts1 - rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] <= ts3)
                            {
                                if (rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 49 || rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 50 || rd.EP[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] == 61)
                                {
                                    if (rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j - 1] - 1] + ts1 + temp >= rd.TF_E[0][0])
                                    {
                                        rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j - 1] - 1] + ts1;
                                        rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + temp;
                                       
                                    }
                                    else
                                    {
                                        rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.st[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + (rd.TF_E[0][0] - rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1]);
                                        rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] = rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1] + (rd.TF_E[0][0] - rd.et[rd.AI_of_PI[yanchi_10hours[i][1] - 1][j] - 1]);
                                        airplane_ID_10Hours.Add(rd.AI_of_PI[yanchi_10hours[i][1] - 1][j]);
                                        if (bad_airport_ID.Contains(rd.AI_of_PI[yanchi_10hours[i][1] - 1][j + 1]))
                                        {
                                            airplane_ID_10Hours.Add(rd.AI_of_PI[yanchi_10hours[i][1] - 1][j + 1]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("超出国际延迟限制！");
                            }
                        }
                    }
                    else
                    { }
                }
            }
        }
        public void isright()
        {
            for(int i=0;i<airplane_ID_10Hours.Count;i++)
            {
                if(bad_airport_ID.Contains(airplane_ID_10Hours[i]))
                {
                    
                }
                else
                {
                    Console.WriteLine("Wrong!");
                }
            }
        }
        public int show_tiqiangeshu()
        {
            int count = 0;
            TimeSpan ts1 = new TimeSpan(0, 50, 0);
            TimeSpan ts2 = new TimeSpan(6, 0, 0);
            TimeSpan ts = new TimeSpan(0, 0, 0);
            //提前该航班的起飞时间，又要满足时间差约束
            for(int i=0;i<rd.AI_of_PI.Count;i++)
            {
                for(int j=0;j<rd.AI_of_PI[i].Count;j++)
                {
                    if(bad_qifei.Contains(rd.AI_of_PI[i][j])&&j!=0)
                    {
                        if (rd.is_country[rd.AI_of_PI[i][j] - 1] == 1)
                        {
                            if (rd.st[rd.AI_of_PI[i][j] - 1] - rd.TF_S[0][1] < ts2&& rd.et[rd.AI_of_PI[i][j - 1] - 1]-rd.TF_S[0][1]<ts)
                            {
                                if (rd.et[rd.AI_of_PI[i][j - 1] - 1] - rd.TF_S[0][1] >= ts1+ (rd.st[rd.AI_of_PI[i][j] - 1] - rd.TF_S[0][1]))
                                {
                                    count++;
                                    Console.WriteLine("飞机："+(i+1)+"  "+"航班ID:"+rd.AI_of_PI[i][j]);
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }
        public void savedExcel()
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = false;
            app.UserControl = true;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = app.Workbooks;
            Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(@"C:\Users\DELL\Desktop\baseline_result.csv");
            Microsoft.Office.Interop.Excel.Sheets sheets = workbook.Sheets;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = (Microsoft.Office.Interop.Excel._Worksheet)sheets.get_Item(1); //第一个工作薄。
            if (worksheet == null)
                return;  //工作薄中没有工作表.
            for (int i = 1; i < 10; i++)
            {
              worksheet.Cells[i, 1] =rd.st[0];
            }
            Range rang = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[9, 1]];
            rang.NumberFormat= @"yyyy - mm - dd hh:mm:ss";
            string savaPath = @"C:\Users\DELL\Desktop\" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".csv";
            workbook.SaveAs(savaPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            //4.关闭Excel对象
            workbook.Close(Missing.Value, Missing.Value, Missing.Value);
            app.Quit();
            Console.WriteLine("修改完成！");
        }
        public void change() 
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = false;
            app.UserControl = true;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = app.Workbooks;
            Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(@"C:\Users\DELL\Desktop\result.csv");
            Microsoft.Office.Interop.Excel.Sheets sheets = workbook.Sheets;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = (Microsoft.Office.Interop.Excel._Worksheet)sheets.get_Item(1); //第一个工作薄。
            if (worksheet == null)
                return;
            int row_ = 0;
            for (int i = 0; i < lianchenglazhi.Count; i++)
            {
                row_ = lianchenglazhi[i][0];
                worksheet.Cells[row_, 8] = 1;
                row_ = lianchenglazhi[i][1];
                worksheet.Cells[row_, 7] = 1;
                worksheet.Cells[row_, 8] = 1;
            }
            for (int i = 0; i < yanchi_10hours.Count; i++)
            {
                row_ = yanchi_10hours[i][0];
                worksheet.Cells[row_, 4] = rd.st[yanchi_10hours[i][0] - 1];
                worksheet.Cells[row_, 5] = rd.et[yanchi_10hours[i][0] - 1];
            }
            
            Range range = worksheet.Range[worksheet.Cells[1, 4], worksheet.Cells[rd.AI.Count(), 4]];
            range.NumberFormat = @"yyyy - mm - dd hh:mm:ss"; ; //日期格式
            range = worksheet.Range[worksheet.Cells[1, 5], worksheet.Cells[rd.AI.Count(), 5]];
            range.NumberFormat = @"yyyy - mm - dd hh:mm:ss"; ; //日期格式
            string savaPath = @"C:\Users\DELL\Desktop\result\" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".csv";
            workbook.SaveAs(savaPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            workbook.Close(Missing.Value, Missing.Value, Missing.Value);
            app.Quit();
            Console.WriteLine("修改完成！");
        }
    }
}
