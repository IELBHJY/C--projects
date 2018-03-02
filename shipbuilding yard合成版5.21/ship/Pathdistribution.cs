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
    class Pathdistribution
    {

        public void get_path(ref string temppath2)//输出结果
        {
            double best = 10000;
            int count = 0;
            int i = 0;
            int tasknumber = 28;//任务数量
            int populationsize = 100;//种群数量
            int crossnumber = 16;//路口数量
            int trucklength = 14;//平板车数量
            int duiweinumber = 231;//堆位数量
            int bestgeneration = 0;
            int bestdistance = 0;
            int j = 0;
            j = countlength();//获取临时运输指令表个数
            if (tasknumber > j)
            {
                tasknumber = j;
            }
            tasknumber = j;
            duiweinumber = countlength1();
            Genetic GA = new Genetic(populationsize, tasknumber, crossnumber, trucklength, duiweinumber);
            GA.initialtest();//读取基础数据
            GA.createfirstpop();//产生第一代种群
            GA.rearrange();//计算任务间先后关系
            //tinzhi = 0;
            //do
            while (count < 100)
            {
                GA.modify();//修复任务序列
                GA.modifytruck();//修复平板车序列
                for (i = 0; i < populationsize; i++)
                {
                    GA.calpath(GA.truckpregegeration, GA.pregegeration, i);//计算适应度
                    if (best > GA.fitness[i])//记录最优值
                    {
                        bestgeneration = count;
                        best = GA.fitness[i];
                        bestdistance = GA.distance[i];
                        for (int k = 0; k < tasknumber; k++)
                        {
                            GA.bestgene[k] = GA.pregegeration[i, k];
                            GA.besttruck[k] = GA.truckpregegeration[i, k];
                        }
                    }
                }
                GA.wheelselect();
                GA.createnextpop();//交叉变异
                GA.producenext();//生成下一代种群
                //tinzhi = count - bestgeneration;
                count++;
                Console.WriteLine("最优值" + best + "循环代数" + count);
                Console.ReadKey();
            }// while (count<10);
            Console.WriteLine("最优距离" + bestdistance);
            Console.WriteLine("最优时间" + (best - bestdistance));
            GA.output(GA.besttruck, GA.bestgene, ref temppath2);//输出结果
        }
        int countlength()//获取临时运输指令表个数
        {
            int i = 0;
            string exePath;//路径
            exePath = System.Windows.Forms.Application.ExecutablePath;
            //string exePath = exePath1
            int index;
            for (i = 0; i < 4; i++)
            {
                index = exePath.LastIndexOf("\\");
                exePath = exePath.Substring(0, index);
            }
            i = 0;
            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exePath + @"\input database.accdb";
            OleDbConnection conn = new OleDbConnection(connstr);
            string query = "select * from 临时运输指令表";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                }
                reader.Close();
                conn.Close();
            }
            return i;
        }
        int countlength1()//获取堆位信息个数
        {
            int i = 0;
            string exePath;//路径
            exePath = System.Windows.Forms.Application.ExecutablePath;
            //string exePath = exePath1
            int index;
            for (i = 0; i < 4; i++)
            {
                index = exePath.LastIndexOf("\\");
                exePath = exePath.Substring(0, index);
            }
            i = 0;
            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exePath + @"\input database.accdb";
            OleDbConnection conn = new OleDbConnection(connstr);
            string query = "select * from 堆位信息";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                }
                reader.Close();
                conn.Close();
            }
            return i;
        }
    }
}
