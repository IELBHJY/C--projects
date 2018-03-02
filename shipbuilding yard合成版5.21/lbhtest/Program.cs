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
namespace lbhtest
{
    class Program
    {
        //目标函数：转向次数，每次转向前行驶的距离，时间（每次转弯前等待的时间和行驶时间）
        //转向次数和每次转向前行驶的距离可以通过distance函数求得。
        //需要求解转弯时的等待时间，即路径上有平板车行驶时，转弯需要等待。
        public static int crossno = 16;
        public static int[,] xy=new int[crossno+2,crossno+2];//路口坐标数组
        public static int[] arrayCrossno = new int[crossno + 2];//路径数组，是由初始位置和路口和终点位置组成。
        public static int[,] c=new int[crossno + 2, crossno + 2];//路口间距离
        public static int disCross = 0;//求得路径的长路。
        public static int turnCount = 0;//求得路径的转向次数。
        public static int function = 0;//求得路径的目标函数值。
        public static int[,] tabutable;//禁忌表
        public static List<double> f = new List<double>();//存取每次搜索时的目标函数值
        /// <summary>
        /// 从数据库中读取路口坐标，读取任务序列（还未添加）
        /// </summary>
        public static void InitalDate()
        {
            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\lbh\\Documents\\Visual Studio 2012\\Projects\\shipbuilding yard合成版5.21\\input database.accdb";
            OleDbConnection conn = new OleDbConnection(connstr);
            OleDbDataReader reader;
            for (int i = 1; i < crossno+1; i++)//读取路口坐标信息
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

        }

        /// <summary>
        /// 求得各个路口之间的距离
        /// </summary>
        public static void crossdis()
        {
            #region//求距离初始点和终点最近的点
            int i;
            int j;
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
                //if (roadcapacity[i, start0] > 0)
                //{
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
                //}
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
                //if (roadcapacity[i, end0] > 0)
                //{
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
                //}
            }
            #endregion
            #region//路口间距离    
            for (i = 1; i < crossno + 1; i++)
            {
                if (i == 1)
                {
                    for (j = 1; j < 8; j++)
                    {
                        
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                       
                    }
                    for (j = 8; j < 11; j++)
                    {
                       
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 11]) + Math.Abs(xy[1, 1] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                       
                    }
                    for (j = 11; j < 13; j++)
                    {
                        
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        
                    }
                    for (j = 13; j < 16; j++)
                    {
                       
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 1] - xy[0, 16]) + Math.Abs(xy[1, 1] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                        
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                       
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        
                    }
                }
                if (i == 3)
                {
                    for (j = 1; j < 8; j++)
                    {
                        
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        
                    }
                    for (j = 8; j < 11; j++)
                    {
                       
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 11]) + Math.Abs(xy[1, 3] - xy[1, 11])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                       
                    }
                    for (j = 11; j < 13; j++)
                    {
                        
                            c[j, i] = c[i, j] = Math.Abs(xy[0, i] - xy[0, j]) + Math.Abs(xy[1, i] - xy[1, j]);
                        
                    }
                    for (j = 13; j < 16; j++)
                    {
                        
                            c[j, i] = c[i, j] = (Math.Abs(xy[0, 3] - xy[0, 16]) + Math.Abs(xy[1, 3] - xy[1, 16])) * 2 - Math.Abs(xy[0, i] - xy[0, j]) - Math.Abs(xy[1, i] - xy[1, j]);
                       
                    }
                    for (j = 16; j < crossno + 1; j++)
                    {
                        
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
            #endregion//
            #region//求取初始位置，结束位置与其他路口的距离
            //求取初始位置与其他路口的距离
            for (i = 1; i < crossno + 1; i++)
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
            //求取终点位置与其他路口的距离
            for (i = 1; i < crossno + 1; i++)
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
            //求取初始位置与结束位置之间的距离
            c[0, crossno + 1] = c[0, start1] + c[start1, end1] + c[end1, crossno + 1];

            #endregion
        }
       /// <summary>
       /// 计算求得路径的目标函数值
       /// </summary>
       /// <param name="arrayCrossno">路径数组</param>
       /// <returns>目标函数值</returns>
        public static double calFunction(int[] arrayCrossno)
        {
            for (int i = 1; i <=arrayCrossno.Length; i++)
            { 
            disCross+=c[i-1,i];
            }
            for (int i = 1; i < arrayCrossno.Length; i++)
            {
                for (i = 1; i < crossno + 2; i++)
                {
                    if ((xy[0, arrayCrossno[i - 1]] - xy[0, arrayCrossno[i]] == 0) && (xy[1, arrayCrossno[i]] - xy[1, arrayCrossno[i + 1]]) == 0)
                    {
                        turnCount += 1;
                    }
                    if ((xy[1, arrayCrossno[i - 1]] - xy[1, arrayCrossno[i]] == 0) && (xy[0, arrayCrossno[i]] - xy[0, arrayCrossno[i + 1]] == 0))
                    {
                        turnCount += 1;
                    }
                }
            }
            function = disCross + turnCount;
            return function;
        }

        public static bool countMin(int[] x,int num)
        {
            int xtemp=0;
            int ytemp=0;
            double min = 10000;
            for (int i = 1; i < num - 1; i++)
            {
                for (int j = i + 1; j < num; j++)
                {
                    int temp = x[i];
                    x[i] = x[j];
                    x[j] = temp;
                    double function = calFunction(x);
                    if (function < min&&tabutable[x[i],x[j]]==0&&tabutable[x[j],x[i]]==0)
                    {
                        min = function;
                        xtemp = i;
                        ytemp = j;
                    }
                    temp = x[i];
                    x[i] = x[j];
                    x[j] = temp;
                }
            }
            if (min == 10000)
            {
                return false;
            }
            else
            { 
            int temp=x[xtemp];
            x[xtemp] = x[ytemp];
            x[ytemp] = temp;
            tabutable[x[xtemp],x[ytemp]]=1;
            tabutable[x[ytemp],x[xtemp]]=1;
            return true;
            }
        }

        /// <summary>
        /// 禁忌搜索
        /// </summary>
        /// <param name="x">待搜索的数组</param>
        /// <param name="num">数组元素个数</param>
        /// <param name="bestcount">最优值</param>
        /// <param name="firstpoint">初始点</param>
        /// <returns>返回最优值</returns>
        public static double tabusearch(int[] x,int num,double bestcount,int firstpoint)
        {
            
            int temp = x[0];
            x[0]=x[firstpoint];
            x[firstpoint] = temp;
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = i + 1; j < x.Length; j++)
                {
                    tabutable[x[i], x[j]] = 0;
                    tabutable[x[j], x[i]] = 0;
                }
            }
            f.Add(calFunction(x));
            while (countMin(x, num))
            {
                f.Add(calFunction(x));
            }
            f.Sort();
            bestcount = f[0];
            return bestcount;
        }
        static void Main(string[] args)
        {
            InitalDate();
            crossdis();
            //int[] arrayCrossno = new int[16]{1,2,3,4,5,6,7,8,9,10,11,12,12,13,14,15,16};
        }
    }
}
