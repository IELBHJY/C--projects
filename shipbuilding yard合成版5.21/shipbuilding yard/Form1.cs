using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using Microsoft.Office.Core;
//using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace shipbuilding_yard
{
    public partial class  Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            string exePath = System.Windows.Forms.Application.ExecutablePath;//取程序debug的路径
            
            int index;//得到sln文件的地址
            for (int i = 0; i < 4; i++)
            {
                index = exePath.LastIndexOf("\\");
                exePath = exePath.Substring(0, index);
            }
            DataTable dtExcel = new DataTable();
            //数据表
            DataSet ds = new DataSet();
            //获取文件扩展名

            string input_path = textBox1.Text.Replace("\\", "\\\\");//输入excel文件路径

            string strExtension = System.IO.Path.GetExtension(input_path);//excel文件类型
            string strFileName = System.IO.Path.GetFileName(input_path);//excel文件名


            ////Excel的连接
            OleDbConnection objConn = null;
            switch (strExtension)
            {
                case ".xls":
                    objConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + input_path + ";" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\"");
                    break;
                case ".xlsx":
                    objConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + input_path + ";" + "Extended Properties=\"Excel 12.0;HDR=False;IMEX=1;\"");
                    break;
                default:
                    objConn = null;
                    break;
            }           
            
            objConn.Open();
            
            string strSql = "select * from [Sheet1$]";
            //获取Excel指定Sheet表中的信息
            OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
            OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
            ds.Clear();
            dtExcel.Clear();
            myData.Fill(ds, "Sheet1");//填充数据
            objConn.Close();
            //dtExcel即为excel文件中指定表中存储的信息
            dtExcel = ds.Tables["Sheet1"];

            DataTable dtCopy = dtExcel.Copy();//将输入数据的格式赋到dtCopy
            DataView dv = dtExcel.DefaultView;

            dv.Sort = "任务时间,下一工艺阶段 DESC,时间余量";   //排序：1 按任务日期升序；2 下一工艺阶段降序，3时间余量升序
            dtCopy = dv.ToTable();//dtCopy为经过排序后的输入数据

            for (int gy = 0; gy < dtCopy.Rows.Count; gy++)
            {
                if (dtCopy.Rows[gy]["下一工艺阶段"].ToString() == "1")
                { dtCopy.Rows[gy]["下一工艺阶段"] = "预舾装"; }
                if (dtCopy.Rows[gy]["下一工艺阶段"].ToString() == "2")
                { dtCopy.Rows[gy]["下一工艺阶段"] = "涂装"; }
                if (dtCopy.Rows[gy]["下一工艺阶段"].ToString() == "3")
                { dtCopy.Rows[gy]["下一工艺阶段"] = "总组"; }
            }

                                 
            //========
            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exePath + @"\input database.accdb";
            OleDbConnection conn = new OleDbConnection(connstr);
            conn.Open();
            //读取堆场初始占用状态
            OleDbDataAdapter occupied = new OleDbDataAdapter("select * from 当前全局堆场占用信息", conn);
            DataTable occupiedTable = new DataTable();
            occupiedTable.Clear();
            occupied.Fill(occupiedTable);
            //读取堆位信息
            OleDbDataAdapter cell_info = new OleDbDataAdapter("select * from 堆位信息", conn);
            DataTable cellTable = new DataTable();
            cellTable.Clear();
            cell_info.Fill(cellTable);
            //清空临时运输指令表
            string strInsert = "DELETE  FROM 临时运输指令表";
            OleDbCommand inst = new OleDbCommand(strInsert, conn);
            inst.ExecuteNonQuery();

            //输出数据的定义
            string tempstr = "";//界面上的分段堆位分配输出（该算法的最终输出）
            //数据库里的分段堆位分配输出（该算法的最终输出）
            DataTable task = new DataTable("临时运输指令表");
            OleDbDataAdapter adapater1 = new OleDbDataAdapter("select * from 临时运输指令表", conn);//?
            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapater1);//?
            adapater1.Fill(task);//?

            int task_id = 1;//任务序列号
            string path = null;

            for (int i = 0; i < dtCopy.Rows.Count; i++)
            {
                string s0 = ""; //任务分段所在堆场相连的道路
                string e0 = "";//任务分段所在堆位
                string yid0 = "";//任务分段所在场地编号
                string yid1 = "";//任务分段目标场地编号
                float len = 0;//总的移动度

                string pa0 = null;//出堆场路径

                string pa1 = "";//入堆场路径
                double plen1 = 0;//入堆场移动度
                double plen0;//出堆场移动度
                Path myPath = new Path();//path要出该堆场的路径
                path_2 myPath2 = new path_2();//myPath2在同一个堆场内的路径

                string b_po1 = null;//分段在堆位内放置位置{0,1,2,3,4}，大分段0，小分段1-4对应上下左右位置
                string cell_ID1 = dtCopy.Rows[i]["当前位置"].ToString();//分段当前堆位
                string path1 = "";
                string path2 = "";
                BlockMv myblockmv = new BlockMv();//黄颖函数

                //分段放置姿态赋值给b_po1
                DataRow[] dr_po1 = occupiedTable.Select("[分段ID]='" + dtCopy.Rows[i]["分段编号"].ToString() + "'");
                DataTable Table_po1 = occupiedTable.Clone();
                Table_po1.Clear();
                for (int po = 0; po < dr_po1.Length; po++)//最多一次赋值。。。。
                {
                    Table_po1.ImportRow(dr_po1[po]);
                }
                if (Table_po1.Rows.Count > 0)
                {
                    b_po1 = Table_po1.Rows[0]["分段放置位置"].ToString();
                }

                //========================================================出堆场阶段上线

                //对起始位置是车间的情况，分别赋值
                if (dtCopy.Rows[i]["当前位置"].ToString() == "平直车间")
                {
                    pa0 = "平直车间-";
                    plen0 = 0.001;
                }
                else
                {
                    if (dtCopy.Rows[i]["当前位置"].ToString() == "曲面车间")
                    {
                        pa0 = "曲面车间-";
                        plen0 = 0.001;
                    }
                    else
                    {
                        if (dtCopy.Rows[i]["当前位置"].ToString() == "8号平台")
                        {
                            pa0 = "8号平台-";
                            plen0 = 0.001;
                        }
                        else//不是车间，是堆场
                        {
                            e0 = dtCopy.Rows[i]["当前位置"].ToString();
                            DataRow[] cell1_dr = cellTable.Select("[堆位ID]='" + dtCopy.Rows[i]["当前位置"].ToString() + "'");
                            DataTable cell1_table = cellTable.Clone();
                            cell1_table.Clear();
                            for (int c1 = 0; c1 < cell1_dr.Length; c1++)
                            {
                                cell1_table.ImportRow(cell1_dr[c1]);//当前堆位的信息
                            }

                            DataTable ds1 = cell1_table.DefaultView.ToTable(false, new string[] { "场地ID" });
                            yid0 = ds1.Rows[0]["场地ID"].ToString();//yid0=当前堆位所在场地的ID

                            s0 = "R" + yid0;//
                            Vertex vts0 = new Vertex(s0);//出堆场时的起始堆位
                            Vertex vte0 = new Vertex(e0);//出堆场时的终止堆位/路
                            //求出堆场的最短路径
                            myPath.getConn(vts0, vte0, yid0, cellTable, occupiedTable, cell_ID1, null, b_po1);


                            string filepath = exePath + "\\" + yid0;//分段出初始堆场路径的文件的路径                               
                            string text0 = System.IO.File.ReadAllText(@"" + filepath + "路径.txt");

                            string[] sArray0 = text0.Split('-');
                            for (int j = sArray0.Length - 2; j > 0; j--)
                            {
                                pa0 = pa0 + sArray0[j] + "-";//得到顺序的出场路径
                            }
                            //路径长度，移动度值
                            string[] str0 = sArray0[sArray0.Length - 1].Split(':');
                            string len0 = str0[str0.Length - 1];
                            plen0 = float.Parse(len0);
                        }
                    }
                }

                //========================================================出堆场阶段下线

                //判断入场的目标堆场的类型
                if (dtCopy.Rows[i]["目标场地"].ToString() == "总组平台")
                {
                    pa1 = "总组平台";
                    plen1 = 0.001;
                }

                if (dtCopy.Rows[i]["目标场地"].ToString() == "涂装车间")
                {
                    pa1 = "涂装车间";
                    plen1 = 0.001;
                }

                if (dtCopy.Rows[i]["目标场地"].ToString() == "预舾装车间")
                {
                    pa1 = "预舾装车间";
                    plen1 = 0.001;
                }

                DataTable tempTable1 = new DataTable();
                DataTable tempTable2 = new DataTable();
                DataTable tempTable3 = new DataTable();
                DataTable temp_tb2 = new DataTable();
                DataTable tb2 = new DataTable();

                //获取存放小分段的堆位
                DataRow[] oc1_dr = occupiedTable.Select("[分段尺寸]='小'");
                DataTable oc1_table = occupiedTable.Clone();
                oc1_table.Clear();
                for (int o1 = 0; o1 < oc1_dr.Length; o1++)
                {
                    oc1_table.ImportRow(oc1_dr[o1]);
                }
                DataTable oc2_table = oc1_table.DefaultView.ToTable(false, new string[] { "堆位ID" });



                if (dtCopy.Rows[i]["目标场地"].ToString() == "堆场")
                {                  
                    DataRow[] cell2_dr = cellTable.Select("[场地等级]='A'");
                    temp_tb2 = cellTable.Clone();
                    temp_tb2.Clear();
                    for (int c1 = 0; c1 < cell2_dr.Length; c1++)
                    {
                        temp_tb2.ImportRow(cell2_dr[c1]);//temp_tb2=场地级别为A的堆位信息表
                    }

                    tb2 = temp_tb2.Clone();//A级场地存放小分段的堆位信息表
                    tb2.Clear();
                    for (int o2 = 0; o2 < temp_tb2.Rows.Count; o2++)
                    {
                        for (int o3 = 0; o3 < oc2_table.Rows.Count; o3++)
                        {
                            if (temp_tb2.Rows[o2]["堆位ID"].ToString() == oc2_table.Rows[o3]["堆位ID"].ToString())
                            {
                                DataRow ocr1_dr = temp_tb2.Rows[o2];
                                tb2.Rows.Add(ocr1_dr.ItemArray);
                            }
                        }
                    }


                    int tb1;
                   
                    DataRow[] dr_tb;

                    DataTable tempTablet_b1 = new DataTable();

                    //得到在ABC的最高级堆场上只存放一个小分段的堆位信息 tempTable2
                    if (tb2.Rows.Count != 0)
                    {                        
                        tb2.Columns.Add("分段个数", typeof(int));
                        for (tb1 = 0; tb1 < tb2.Rows.Count; tb1++)
                        {                         
                            DataRow[] tb1_dr = occupiedTable.Select("[堆位ID]='" + tb2.Rows[tb1]["堆位ID"].ToString() + "'");
                            tempTablet_b1 = occupiedTable.Clone();
                            tempTablet_b1.Clear();
                            for (int o2 = 0; o2 < tb1_dr.Length; o2++)
                            {
                                tempTablet_b1.ImportRow(tb1_dr[o2]);
                            }
                            tb2.Rows[tb1]["分段个数"] = tempTablet_b1.Rows.Count;
                        }
                        dr_tb = tb2.Select("分段个数 = 1");
                        tempTable2 = tb2.Clone();
                        tempTable2 = dr_tb.CopyToDataTable();
                        tempTable2.Columns.Remove("分段个数");
                    }

                    else
                    {                       
                        DataRow[] cell3_dr = cellTable.Select("[场地等级]='B'");
                        temp_tb2 = cellTable.Clone();
                        temp_tb2.Clear();
                        for (int c1 = 0; c1 < cell3_dr.Length; c1++)
                        {
                            temp_tb2.ImportRow(cell3_dr[c1]);
                        }

                        tb2 = temp_tb2.Clone();
                        tb2.Clear();
                        for (int o2 = 0; o2 < temp_tb2.Rows.Count; o2++)
                        {
                            for (int o3 = 0; o3 < oc2_table.Rows.Count; o3++)
                            {
                                if (temp_tb2.Rows[o2]["堆位ID"].ToString() == oc2_table.Rows[o3]["堆位ID"].ToString())
                                {
                                    DataRow ocr1_dr = temp_tb2.Rows[o2];
                                    tb2.Rows.Add(ocr1_dr.ItemArray);
                                }
                            }
                        }


                        if (tb2.Rows.Count != 0)
                        {
                            tb2.Columns.Add("分段个数", typeof(int));
                            for (tb1 = 0; tb1 < tb2.Rows.Count; tb1++)
                            {                              
                                DataRow[] tb1_dr = occupiedTable.Select("[堆位ID]='" + tb2.Rows[tb1]["堆位ID"].ToString() + "'");
                                tempTablet_b1 = occupiedTable.Clone();
                                tempTablet_b1.Clear();
                                for (int o2 = 0; o2 < tb1_dr.Length; o2++)
                                {
                                    tempTablet_b1.ImportRow(tb1_dr[o2]);
                                }


                                tb2.Rows[tb1]["分段个数"] = tempTablet_b1.Rows.Count;
                            }
                            dr_tb = tb2.Select("分段个数 = 1");
                            tempTable2 = tb2.Clone();
                            tempTable2 = dr_tb.CopyToDataTable();
                            tempTable2.Columns.Remove("分段个数");
                        }
                        else
                        {
                            DataRow[] cell4_dr = cellTable.Select("[场地等级]='C'");
                            temp_tb2 = cellTable.Clone();
                            temp_tb2.Clear();
                            for (int c1 = 0; c1 < cell4_dr.Length; c1++)
                            {
                                temp_tb2.ImportRow(cell4_dr[c1]);
                            }

                            tb2 = temp_tb2.Clone();
                            tb2.Clear();
                            for (int o2 = 0; o2 < temp_tb2.Rows.Count; o2++)
                            {
                                for (int o3 = 0; o3 < oc2_table.Rows.Count; o3++)
                                {
                                    if (temp_tb2.Rows[o2]["堆位ID"].ToString() == oc2_table.Rows[o3]["堆位ID"].ToString())
                                    {
                                        DataRow ocr1_dr = temp_tb2.Rows[o2];
                                        tb2.Rows.Add(ocr1_dr.ItemArray);
                                    }
                                }
                            }


                            if (tb2.Rows.Count != 0)
                            {
                                tb2.Columns.Add("分段个数", typeof(int));
                                for (tb1 = 0; tb1 < tb2.Rows.Count; tb1++)
                                {
                                    DataRow[] tb1_dr = occupiedTable.Select("[堆位ID]='" + tb2.Rows[tb1]["堆位ID"].ToString() + "'");
                                    tempTablet_b1 = occupiedTable.Clone();
                                    tempTablet_b1.Clear();
                                    for (int o2 = 0; o2 < tb1_dr.Length; o2++)
                                    {
                                        tempTablet_b1.ImportRow(tb1_dr[o2]);
                                    }


                                    tb2.Rows[tb1]["分段个数"] = tempTablet_b1.Rows.Count;
                                }
                                dr_tb = tb2.Select("分段个数 = 1");
                                tempTable2 = tb2.Clone();
                                tempTable2 = dr_tb.CopyToDataTable();
                                tempTable2.Columns.Remove("分段个数");
                            }
                        }

                    }
                    //获取ABC的最高级的空堆位集 tempTable1
                    DataRow[] cell5_dr = cellTable.Select("[场地等级]='A'");
                    DataTable cell2_tb = cellTable.Clone();
                    cell2_tb.Clear();
                    for (int c1 = 0; c1 < cell5_dr.Length; c1++)
                    {
                        cell2_tb.ImportRow(cell5_dr[c1]);
                    }

                    tempTable1 = cellTable.Clone();
                    tempTable1.Clear();

                    for (int o2 = 0; o2 < cell2_tb.Rows.Count; o2++)
                    {
                        int cc1 = 0;
                        for (int o3 = 0; o3 < occupiedTable.Rows.Count; o3++)
                        {
                            if (cell2_tb.Rows[o2]["堆位ID"].ToString() == occupiedTable.Rows[o3]["堆位ID"].ToString())
                            {
                                cc1 = cc1 + 1;
                            }
                        }
                        if (cc1 == 0)
                        {
                            DataRow oc2_dr = cell2_tb.Rows[o2];
                            tempTable1.Rows.Add(oc2_dr.ItemArray);
                        }
                    }


                    if (tempTable1.Rows.Count == 0)
                    {
                        DataRow[] cell6_dr = cellTable.Select("[场地等级]='B'");
                        DataTable cell3_tb = cellTable.Clone();
                        cell3_tb.Clear();
                        for (int c1 = 0; c1 < cell6_dr.Length; c1++)
                        {
                            cell3_tb.ImportRow(cell6_dr[c1]);
                        }

                        tempTable1 = cellTable.Clone();
                        tempTable1.Clear();

                        for (int o2 = 0; o2 < cell3_tb.Rows.Count; o2++)
                        {
                            int cc1 = 0;
                            for (int o3 = 0; o3 < occupiedTable.Rows.Count; o3++)
                            {
                                if (cell3_tb.Rows[o2]["堆位ID"].ToString() == occupiedTable.Rows[o3]["堆位ID"].ToString())
                                {
                                    cc1 = cc1 + 1;
                                }
                            }
                            if (cc1 == 0)
                            {
                                DataRow oc2_dr = cell3_tb.Rows[o2];
                                tempTable1.Rows.Add(oc2_dr.ItemArray);
                            }
                        }


                        if (tempTable1.Rows.Count == 0)
                        {
                            DataRow[] cell7_dr = cellTable.Select("[场地等级]='C'");
                            DataTable cell4_tb = cellTable.Clone();
                            cell4_tb.Clear();
                            for (int c1 = 0; c1 < cell7_dr.Length; c1++)
                            {
                                cell4_tb.ImportRow(cell7_dr[c1]);
                            }

                            tempTable1 = cellTable.Clone();
                            tempTable1.Clear();

                            for (int o2 = 0; o2 < cell4_tb.Rows.Count; o2++)
                            {
                                int cc1 = 0;
                                for (int o3 = 0; o3 < occupiedTable.Rows.Count; o3++)
                                {
                                    if (cell4_tb.Rows[o2]["堆位ID"].ToString() == occupiedTable.Rows[o3]["堆位ID"].ToString())
                                    {
                                        cc1 = cc1 + 1;
                                    }
                                }
                                if (cc1 == 0)
                                {
                                    DataRow oc2_dr = cell4_tb.Rows[o2];
                                    tempTable1.Rows.Add(oc2_dr.ItemArray);
                                }
                            }
                        }

                    }
                    //===================初始候选堆位集搜索结束 



                    DateTime dt = Convert.ToDateTime(dtCopy.Rows[i]["任务时间"].ToString());//
                    dt = dt.AddDays(Convert.ToInt32(dtCopy.Rows[i]["时间余量"].ToString()));//计划出场时间
                    string dt1 = dt.ToString("yyyy/M/d");

                    DataRow[] d1_dr = occupiedTable.Select("[分段预计出场时间]='" + dt1 + "'");
                    DataTable d1_tempTable3 = occupiedTable.Clone();
                    d1_tempTable3.Clear();
                    for (int o3 = 0; o3 < d1_dr.Length; o3++)
                    {
                        d1_tempTable3.ImportRow(d1_dr[o3]);
                    }
                    tempTable3 = d1_tempTable3.DefaultView.ToTable(false, new string[] { "堆位ID" });//当前堆场上分段出场时间和任务分段计划出场时间一致的堆位信息表


                    DataRow result1 = tempTable2.NewRow();//取tempTable2的格式
                    DataTable new_tempTable2;
                    new_tempTable2 = tempTable2.Clone();
                    for (int rt1 = 0; rt1 < tempTable2.Rows.Count; rt1++)
                    {
                        for (int rt2 = 0; rt2 < tempTable3.Rows.Count; rt2++)
                        {

                            if (tempTable2.Rows[rt1]["堆位ID"].ToString() == tempTable3.Rows[rt2]["堆位ID"].ToString())
                            {

                                result1 = tempTable2.Rows[rt1];

                                new_tempTable2.Rows.Add(result1.ItemArray);//小分段堆位里时间一致的堆位信息
                            }
                        }
                    }


                    //计算时间权重



                    DataRow[] result2;
                    DataTable new2_tempTable2;
                    string cell_ID2 = null;
                    string b_po2 = null;

                    if (dtCopy.Rows[i]["分段尺寸"].ToString() == "小" && tempTable2 != null && new_tempTable2.Rows.Count > 0)
                    //两个小分段放在一个堆位上
                    {

                        result2 = new_tempTable2.Select("工艺优先级1 = '" + dtCopy.Rows[i]["下一工艺阶段"].ToString() + "'");//判断工艺优先级
                        new2_tempTable2 = tempTable2.Clone(); ;

                        if (result2.Length > 0)
                        {
                            new2_tempTable2 = result2.CopyToDataTable();//new2_tempTable2 工艺优先级匹配的
                        }

                        else
                        {
                            result2 = new_tempTable2.Select("工艺优先级2 = '" + dtCopy.Rows[i]["下一工艺阶段"].ToString() + "'");
                            if (result2.Length > 0)
                            {
                                new2_tempTable2 = result2.CopyToDataTable();
                            }
                            else
                            {
                                result2 = new_tempTable2.Select("工艺优先级3 = '" + dtCopy.Rows[i]["下一工艺阶段"].ToString() + "'");
                                new2_tempTable2 = result2.CopyToDataTable();
                            }
                        }

                        //比较当前候选堆位和周围堆位分段离场的时间
                        new2_tempTable2.Columns.Add("时间差", typeof(double));//候选堆位集
                        for (int l = 0; l < new2_tempTable2.Rows.Count; l++)
                        {
                            double t;
                            double t_t = 0;
                            int n = 0;
                            DataTable tempTable4 = new DataTable();

                            for (int m = new2_tempTable2.Columns.IndexOf("东堆位ID"); m <= new2_tempTable2.Columns.IndexOf("北堆位ID"); m++)
                            {
                                DataRow[] d2_dr = occupiedTable.Select("[堆位ID]='" + new2_tempTable2.Rows[l][m].ToString() + "'");
                                DataTable d2_tempTable3 = occupiedTable.Clone();
                                d2_tempTable3.Clear();
                                for (int o4 = 0; o4 < d2_dr.Length; o4++)
                                {
                                    d2_tempTable3.ImportRow(d2_dr[o4]);
                                }
                                tempTable4 = d2_tempTable3.DefaultView.ToTable(false, new string[] { "分段预计出场时间" });




                                if (tempTable4.Rows.Count > 0)
                                {
                                    for (int p = 0; p < tempTable4.Rows.Count; p++)//一个堆位有两个分段
                                    {
                                        DateTime dt2 = Convert.ToDateTime(tempTable4.Rows[p]["分段预计出场时间"].ToString());

                                        if (DateTime.Compare(dt2, dt) >= 0)
                                        {
                                            t = (dt2 - dt).TotalDays;
                                        }
                                        else
                                        {
                                            t = (dt - dt2).TotalDays;
                                        }
                                        t_t = t_t + t * t;
                                        n = n + 1;

                                    }

                                }

                            }
                            new2_tempTable2.Rows[l]["时间差"] = t_t / (n + 1);

                        }
                        new2_tempTable2.DefaultView.Sort = "时间差 asc";//候选堆位集按时间差升序排
                        DataTable new2_tempTable2_Sort = new2_tempTable2.DefaultView.ToTable();

                        //==========================================0509就到这儿了，下次分解


                        string filepath = null;

                        double[] len_ps = new double[5];

                        if (new2_tempTable2_Sort.Rows.Count >= 5) //取前5个
                        {
                            for (int q = 0; q < 5; q++)
                            {


                                string yid1_p = new2_tempTable2_Sort.Rows[q]["场地ID"].ToString();
                                string s1_p = "R" + yid1_p;
                                Vertex vts1_p = new Vertex(s1_p);
                                cell_ID2 = new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString();
                                Vertex vte1_p = new Vertex(new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString());

                                DataRow[] po2_dr1 = occupiedTable.Select("[堆位ID]='" + new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString() + "'");//读取已放小分段的占用信息
                                DataTable ocs_tempTable1 = occupiedTable.Clone();
                                ocs_tempTable1.Clear();

                                for (int o5 = 0; o5 < po2_dr1.Length; o5++)
                                {
                                    ocs_tempTable1.ImportRow(po2_dr1[o5]);
                                }
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "1") b_po2 = "2";//1上2下3左4右
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "2") b_po2 = "1";
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "3") b_po2 = "4";
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "4") b_po2 = "3";


                                if (yid0 == yid1_p)
                                {
                                    myPath2.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, cell_ID1, cell_ID2, b_po2);//堆场内部移动

                                }
                                else
                                { myPath.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, null, cell_ID2, b_po2); }//跨堆场移动
                                                               
                                //读取生成的路径和自由度，修改格式
                                filepath = exePath + "\\" + yid1_p;

                                //string text1_p = System.IO.File.ReadAllText(@"C:\\Users\\chenkai\\Desktop\\smart shipbuilding\\program\\shipbuilding yard\\" + yid1_p + "路径.txt");
                                string text1_p = System.IO.File.ReadAllText(@"" + filepath + "路径.txt");

                                string[] sArray1_p = text1_p.Split('-');
                                string pa1_p = null;
                                for (int r = 1; r < sArray1_p.Length - 2; r++)
                                {
                                    pa1_p = pa1_p + sArray1_p[r] + "-";
                                }
                                pa1_p = pa1_p + sArray1_p[sArray1_p.Length - 2];//R2-T1804-T1105- --->  T1804


                                string[] str1_p = sArray1_p[sArray1_p.Length - 1].Split(':');
                                string len1_p = str1_p[str1_p.Length - 1];
                                float plen1_p = float.Parse(len1_p);
                                double len_p = plen0 + plen1_p;
                                len_ps[q] = len_p;   //   总自由度
                            }
                        }
                        else
                        {
                            for (int q = 0; q < new2_tempTable2_Sort.Rows.Count; q++)
                            {


                                string yid1_p = new2_tempTable2_Sort.Rows[q]["场地ID"].ToString();
                                string s1_p = "R" + yid1_p;
                                Vertex vts1_p = new Vertex(s1_p);


                                cell_ID2 = new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString();
                                Vertex vte1_p = new Vertex(new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString());

                                DataRow[] po2_dr1 = occupiedTable.Select("[堆位ID]='" + new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString() + "'");
                                DataTable ocs_tempTable1 = occupiedTable.Clone();
                                ocs_tempTable1.Clear();

                                for (int o5 = 0; o5 < po2_dr1.Length; o5++)
                                {
                                    ocs_tempTable1.ImportRow(po2_dr1[o5]);
                                }
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "1") b_po2 = "2";
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "2") b_po2 = "1";
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "3") b_po2 = "4";
                                if (ocs_tempTable1.Rows[0]["分段放置位置"].ToString() == "4") b_po2 = "3";

                                if (yid0 == yid1_p)
                                { myPath2.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, cell_ID1, cell_ID2, b_po2); }
                                else
                                { myPath.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, null, cell_ID2, b_po2); }

                                                               
                                filepath = exePath + "\\" + yid1_p;
                              
                                string text1_p = System.IO.File.ReadAllText(@"" + filepath + "路径.txt");
                                string[] sArray1_p = text1_p.Split('-');
                                string pa1_p = null;
                                for (int r = 1; r < sArray1_p.Length - 2; r++)
                                {
                                    pa1_p = pa1_p + sArray1_p[r] + "-";
                                }
                                pa1_p = pa1_p + sArray1_p[sArray1_p.Length - 2];


                                string[] str1_p = sArray1_p[sArray1_p.Length - 1].Split(':');
                                string len1_p = str1_p[str1_p.Length - 1];
                                float plen1_p = float.Parse(len1_p);
                                double len_p = plen0 + plen1_p;
                                len_ps[q] = len_p;
                            }
                        }
                        //len_ps[position]自由度最小
                        int position = 0;
                        for (int t = 0; t < len_ps.Length; t++)
                        {
                            if (len_ps[t] != 0 && len_ps[t] < len_ps[position])
                            {
                                position = t;

                            }

                        }
                       
                        yid1 = new2_tempTable2_Sort.Rows[position]["场地ID"].ToString();
                        string s1 = "R" + yid1;
                        Vertex vts1 = new Vertex(s1);
                        Vertex vte1 = new Vertex(new2_tempTable2_Sort.Rows[position]["堆位ID"].ToString());

                        cell_ID2 = new2_tempTable2_Sort.Rows[position]["堆位ID"].ToString();


                        DataRow[] po2_dr2 = occupiedTable.Select("[堆位ID]='" + new2_tempTable2_Sort.Rows[position]["堆位ID"].ToString() + "'");
                        DataTable ocs_tempTable2 = occupiedTable.Clone();
                        ocs_tempTable2.Clear();

                       
                         ocs_tempTable2.ImportRow(po2_dr2[0]);
                       
                        if (ocs_tempTable2.Rows[0]["分段放置位置"].ToString() == "1") b_po2 = "2";
                        if (ocs_tempTable2.Rows[0]["分段放置位置"].ToString() == "2") b_po2 = "1";
                        if (ocs_tempTable2.Rows[0]["分段放置位置"].ToString() == "3") b_po2 = "4";
                        if (ocs_tempTable2.Rows[0]["分段放置位置"].ToString() == "4") b_po2 = "3";

                        if (yid0 == yid1)
                        {
                            myPath2.getConn(vts1, vte1, yid1, cellTable, occupiedTable, cell_ID1, cell_ID2, b_po2);
                        }
                        else
                        {
                            myPath.getConn(vts1, vte1, yid1, cellTable, occupiedTable, null, cell_ID2, b_po2);
                        }
                        
                        filepath = exePath + "\\" + yid1;


                      
                        string text1 = System.IO.File.ReadAllText(@"" + filepath + "路径.txt");
                        string[] sArray1 = text1.Split('-');
                        for (int r = 1; r < sArray1.Length - 2; r++)
                        {
                            pa1 = pa1 + sArray1[r] + "-";
                        }
                        pa1 = pa1 + sArray1[sArray1.Length - 2];


                        string[] str1 = sArray1[sArray1.Length - 1].Split(':');
                        string len1 = str1[str1.Length - 1];
                        plen1 = float.Parse(len1); //确定的进场自由度
                       
                    }
               
                    else//没有已经有小分段放置的堆位
                    {
                        if (tempTable1 != null)//在空堆位集上选堆位
                        {
                            result2 = tempTable1.Select("工艺优先级1 = '" + dtCopy.Rows[i]["下一工艺阶段"].ToString() + "'");
                            new2_tempTable2 = tempTable1.Clone(); ;

                            if (result2.Length > 0)
                            {
                                new2_tempTable2 = result2.CopyToDataTable();
                            }

                            else
                            {
                                result2 = tempTable1.Select("工艺优先级2 = '" + dtCopy.Rows[i]["下一工艺阶段"].ToString() + "'");
                                if (result2.Length > 0)
                                {
                                    new2_tempTable2 = result2.CopyToDataTable();
                                }
                                else
                                {
                                    result2 = tempTable1.Select("工艺优先级3 = '" + dtCopy.Rows[i]["下一工艺阶段"].ToString() + "'");
                                    new2_tempTable2 = result2.CopyToDataTable();
                                }
                            }
                            new2_tempTable2.Columns.Add("时间差", typeof(double));//

                            for (int l = 0; l < new2_tempTable2.Rows.Count; l++)
                            {
                                double t;
                                double t_t = 0;
                                int n = 0;
                                DataTable tempTable4 = new DataTable();
                                for (int m = new2_tempTable2.Columns.IndexOf("东堆位ID"); m <= new2_tempTable2.Columns.IndexOf("北堆位ID"); m++)
                                {                                    
                                    DataRow[] d2_dr = occupiedTable.Select("[堆位ID]='" + new2_tempTable2.Rows[l][m].ToString() + "'");
                                    DataTable d2_tempTable3 = occupiedTable.Clone();
                                    d2_tempTable3.Clear();
                                    for (int o4 = 0; o4 < d2_dr.Length; o4++)
                                    {
                                        d2_tempTable3.ImportRow(d2_dr[o4]);
                                    }
                                    tempTable4 = d2_tempTable3.DefaultView.ToTable(false, new string[] { "分段预计出场时间" });


                                    if (tempTable4.Rows.Count > 0)
                                    {
                                        for (int p = 0; p < tempTable4.Rows.Count; p++)//一个堆位有两个分段
                                        {
                                            DateTime dt2 = Convert.ToDateTime(tempTable4.Rows[p]["分段预计出场时间"].ToString());

                                            if (DateTime.Compare(dt2, dt) >= 0)
                                            {
                                                t = (dt2 - dt).TotalDays;
                                            }
                                            else
                                            {
                                                t = (dt - dt2).TotalDays;
                                            }
                                            t_t = t_t + t * t;
                                            n = n + 1;

                                        }

                                    }

                                }
                                new2_tempTable2.Rows[l]["时间差"] = t_t / (n + 1);

                            }
                            new2_tempTable2.DefaultView.Sort = "时间差 asc";
                            DataTable new2_tempTable2_Sort = new2_tempTable2.DefaultView.ToTable();




                            double[] len_ps = new double[5];
                            string filepath = null;

                            if (new2_tempTable2_Sort.Rows.Count >= 5)
                            {
                                for (int q = 0; q < 5; q++)
                                {


                                    string yid1_p = new2_tempTable2_Sort.Rows[q]["场地ID"].ToString();
                                    string s1_p = "R" + yid1_p;
                                    Vertex vts1_p = new Vertex(s1_p);
                                    Vertex vte1_p = new Vertex(new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString());


                                    cell_ID2 = new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString();
                                  
                                    if (yid0 == yid1_p)
                                    { myPath2.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, cell_ID1, cell_ID2, b_po2); }
                                    else
                                    { myPath.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, null, cell_ID2, b_po2); }
                              
                                    filepath = exePath + "\\" + yid1_p;
                                    string text1_p = System.IO.File.ReadAllText(@"" + filepath + "路径.txt");
                                    string[] sArray1_p = text1_p.Split('-');
                                    string pa1_p = null;
                                    for (int r = 1; r < sArray1_p.Length - 2; r++)
                                    {
                                        pa1_p = pa1_p + sArray1_p[r] + "-";
                                    }
                                    pa1_p = pa1_p + sArray1_p[sArray1_p.Length - 2];


                                    string[] str1_p = sArray1_p[sArray1_p.Length - 1].Split(':');
                                    string len1_p = str1_p[str1_p.Length - 1];
                                    float plen1_p = float.Parse(len1_p);
                                    double len_p = plen0 + plen1_p;
                                    len_ps[q] = len_p;
                                }
                            }
                            else
                            {
                                for (int q = 0; q < new2_tempTable2_Sort.Rows.Count; q++)
                                {


                                    string yid1_p = new2_tempTable2_Sort.Rows[q]["场地ID"].ToString();
                                    string s1_p = "R" + yid1_p;
                                    Vertex vts1_p = new Vertex(s1_p);
                                    Vertex vte1_p = new Vertex(new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString());

                                    cell_ID2 = new2_tempTable2_Sort.Rows[q]["堆位ID"].ToString();

                                    if (yid0 == yid1_p)
                                    { myPath2.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, cell_ID1, cell_ID2, b_po2); }
                                    else
                                    { myPath.getConn(vts1_p, vte1_p, yid1_p, cellTable, occupiedTable, null, cell_ID2, b_po2); }

                                    filepath = exePath + "\\" + yid1_p;

                                    string text1_p = System.IO.File.ReadAllText(@"" + filepath + "路径.txt");
                                    string[] sArray1_p = text1_p.Split('-');
                                    string pa1_p = null;
                                    for (int r = 1; r < sArray1_p.Length - 2; r++)
                                    {
                                        pa1_p = pa1_p + sArray1_p[r] + "-";
                                    }
                                    pa1_p = pa1_p + sArray1_p[sArray1_p.Length - 2];


                                    string[] str1_p = sArray1_p[sArray1_p.Length - 1].Split(':');
                                    string len1_p = str1_p[str1_p.Length - 1];
                                    float plen1_p = float.Parse(len1_p);
                                    double len_p = plen0 + plen1_p;
                                    len_ps[q] = len_p;
                                }
                            }

                            int position = 0;
                            for (int t = 0; t < len_ps.Length; t++)
                            {
                                if (len_ps[t] != 0 && len_ps[t] < len_ps[position])
                                {
                                    position = t;

                                }

                            }
                            //自由度最小
                            yid1 = new2_tempTable2_Sort.Rows[position]["场地ID"].ToString();
                            string s1 = "R" + yid1;
                            Vertex vts1 = new Vertex(s1);
                            Vertex vte1 = new Vertex(new2_tempTable2_Sort.Rows[position]["堆位ID"].ToString());

                            cell_ID2 = new2_tempTable2_Sort.Rows[position]["堆位ID"].ToString();

                            if (yid0 == yid1)
                            { myPath2.getConn(vts1, vte1, yid1, cellTable, occupiedTable, cell_ID1, cell_ID2, b_po2); }
                            else
                            { myPath.getConn(vts1, vte1, yid1, cellTable, occupiedTable, null, cell_ID2, b_po2); }


                            filepath = exePath + "\\" + yid1;

                            string text1 = System.IO.File.ReadAllText(@"" + filepath + "路径.txt");
                            string[] sArray1 = text1.Split('-');
                            for (int r = 1; r < sArray1.Length - 2; r++)
                            {
                                pa1 = pa1 + sArray1[r] + "-";
                            }
                            pa1 = pa1 + sArray1[sArray1.Length - 2];


                            string[] str1 = sArray1[sArray1.Length - 1].Split(':');
                            string len1 = str1[str1.Length - 1];
                            plen1 = float.Parse(len1);

                        }
                        else
                        {
                            tempstr = "堆位已满";
                        }
                    }
                }//堆位选择算法结束

                //调整任务执行顺序：1 阻挡分段在路径上&在任务序列中&在同一天，阻挡分段提前到当前任务前一个；2 其他情况，调用黄颖的程序
                string[] sArray1_ID = pa0.Split('-'); //分解出场路径，得到堆位数组
                string[] sArray2_ID = pa1.Split('-');//分解进场路径，得到堆位数组
                DataTable ob1_table = new DataTable();


                string[] obs1_ID = new string[20];//用于存储阻挡分段的ID
                int ob1 = 0;//路径上阻挡分段在后续任务序列中的个数
                //出场阶段
                for (int r1 = sArray1_ID.Length - 1; r1 >= 0; r1--)//判断路径上每个堆位上的分段是否在后续任务序列中
                {                   
                    DataRow[] ob1_dr = occupiedTable.Select("[堆位ID]='" + sArray1_ID[r1] + "'");
                    DataTable ob1_tempTable = occupiedTable.Clone();
                    ob1_tempTable.Clear();
                    for (int o5 = 0; o5 < ob1_dr.Length; o5++)
                    {
                        ob1_tempTable.ImportRow(ob1_dr[o5]);
                    }
                    ob1_table = ob1_tempTable.DefaultView.ToTable(false, new string[] { "分段ID" });//所选路径上所有堆位上的分段ID


                    if (ob1_table.Rows.Count == 1)//堆位上有一个阻挡分段
                    {
                        for (int i2 = i + 1; i2 < dtCopy.Rows.Count; i2++)//阻挡分段是否在后续任务序列中
                        {
                            if (dtCopy.Rows[i2]["分段编号"].ToString() == ob1_table.Rows[0]["分段ID"].ToString() && dtCopy.Rows[i2]["任务时间"].ToString() == dtCopy.Rows[i]["任务时间"].ToString())
                            {
                                obs1_ID[ob1] = dtCopy.Rows[i2]["分段编号"].ToString();
                                DataRow dr1 = dtCopy.NewRow();
                                for (int mm2 = 0; mm2 < dtCopy.Columns.Count; mm2++)
                                {
                                    dr1[mm2] = dtCopy.Rows[i2][mm2].ToString();
                                }
                          
                                dtCopy.Rows[i2].Delete();
                            
                                dtCopy.Rows.InsertAt(dr1, i + ob1);
                                ob1 = ob1 + 1;
                            }
                        }
                    }
                    else if (ob1_table.Rows.Count == 2)//堆位上有2个阻挡分段
                    {
                        for (int i2 = i + 1; i2 < dtCopy.Rows.Count; i2++)
                        {
                            if (dtCopy.Rows[i2]["任务时间"].ToString() == dtCopy.Rows[i]["任务时间"].ToString() && ((dtCopy.Rows[i2]["分段编号"].ToString() == ob1_table.Rows[0]["分段ID"].ToString()) || (dtCopy.Rows[i2]["分段编号"].ToString() == ob1_table.Rows[1]["分段ID"].ToString())))
                            {
                                obs1_ID[ob1] = dtCopy.Rows[i2]["分段编号"].ToString();
                                DataRow dr1 = dtCopy.NewRow();
                                for (int mm2 = 0; mm2 < dtCopy.Columns.Count; mm2++)
                                {
                                    dr1[mm2] = dtCopy.Rows[i2][mm2].ToString();
                                }
                                dtCopy.Rows[i2].Delete();
                                dtCopy.Rows.InsertAt(dr1, i + ob1);
                                ob1 = ob1 + 1;

                            }
                        }
                    }
                }
                //入场阶段
                for (int r2 = 0; r2 < sArray2_ID.Length; r2++)
                {
                    DataRow[] ob1_dr = occupiedTable.Select("[堆位ID]='" + sArray2_ID[r2] + "'");
                    DataTable ob1_tempTable = occupiedTable.Clone();
                    ob1_tempTable.Clear();
                    for (int o5 = 0; o5 < ob1_dr.Length; o5++)
                    {
                        ob1_tempTable.ImportRow(ob1_dr[o5]);
                    }
                    ob1_table = ob1_tempTable.DefaultView.ToTable(false, new string[] { "分段ID" });

                    if (ob1_table.Rows.Count == 1)
                    {
                        for (int i2 = i + 1; i2 < dtCopy.Rows.Count; i2++)
                        {
                            if (dtCopy.Rows[i2]["分段编号"].ToString() == ob1_table.Rows[0]["分段ID"].ToString() && dtCopy.Rows[i2]["任务时间"].ToString() == dtCopy.Rows[i]["任务时间"].ToString())
                            {
                                obs1_ID[ob1] = dtCopy.Rows[i2]["分段编号"].ToString();
                                DataRow dr1 = dtCopy.NewRow();
                                for (int mm2 = 0; mm2 < dtCopy.Columns.Count; mm2++)
                                {
                                    dr1[mm2] = dtCopy.Rows[i2][mm2].ToString();
                                }
                                dtCopy.Rows[i2].Delete();
                                dtCopy.Rows.InsertAt(dr1, i + ob1);
                                ob1 = ob1 + 1;
                            }
                        }
                    }
                    else if (ob1_table.Rows.Count == 2)
                    {
                        for (int i2 = i + 1; i2 < dtCopy.Rows.Count; i2++)
                        {
                            if (dtCopy.Rows[i2]["任务时间"].ToString() == dtCopy.Rows[i]["任务时间"].ToString() && ((dtCopy.Rows[i2]["分段编号"].ToString() == ob1_table.Rows[0]["分段ID"].ToString()) || (dtCopy.Rows[i2]["分段编号"].ToString() == ob1_table.Rows[1]["分段ID"].ToString())))
                            {
                                obs1_ID[ob1] = dtCopy.Rows[i2]["分段编号"].ToString();
                                DataRow dr1 = dtCopy.NewRow();
                                for (int mm2 = 0; mm2 < dtCopy.Columns.Count; mm2++)
                                {
                                    dr1[mm2] = dtCopy.Rows[i2][mm2].ToString();
                                }
                                dtCopy.Rows[i2].Delete();
                                dtCopy.Rows.InsertAt(dr1, i + ob1);
                                ob1 = ob1 + 1;

                            }
                        }
                    }
                }
                //得到路径上阻挡分段在任务序列中的数量ob1
                string blockpo = null;

                if (ob1 >=1)
                {
                    i = i - 1;
                }
                else//如果没有阻挡分段在后续任务序列中
                {

                    if (yid0 == yid1 && yid0.Length > 0 && yid1.Length > 0)//在堆场内部移动
                    {
                        path = pa1;
                        path = path.Replace("R" + yid1, "R");
                        len = (float)Math.Round((float)(plen1), 3);
                    }
                    else//跨堆场移动时的路径和移动度
                    {
                        path = pa0 + "R-" + pa1;
                        len = (float)Math.Round((float)(plen0 + plen1), 3);
                    }


                    index = path.IndexOf("-R-");
                    if (index > -1)//跨堆场运输或一个堆场内部先出去再进来
                    {
                        path1 = path.Substring(0, index);
                        path2 = path.Substring(index + 3, path.Length - index - 3);

                        //阻挡分段
                        string[] dr_ob1 = path1.Split('-');
                        for (int i1 = dr_ob1.Length - 1; i1 > 0; i1--)
                        {
                            for (int o1 = 0; o1 < occupiedTable.Rows.Count; o1++)
                            {
                                if (occupiedTable.Rows[o1]["堆位ID"].ToString() == dr_ob1[i1] && occupiedTable.Rows[o1]["分段ID"].ToString() != dtCopy.Rows[i]["分段编号"].ToString())
                                {



                                    //调用黄颖 getconn(celltable,occupiedTable,occupiedTable.Rows[o1]["分段ID"].ToString())
                                    //Block bk = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    //Vertex vt1 = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    //myblockmv.getConn(vt1, yid0, bk, cellTable, occupiedTable);

                                    Block bk = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    Vertex vt1 = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    string output_str = "";
                                    int Number_Block = 0;
                                    Block bk_second = null;
                                    Vertex vx_second = null;
                                 
                                    int o_second = 0;
                                    myblockmv.getConn(vt1, yid0, bk, cellTable, occupiedTable, ref output_str, ref Number_Block, ref bk_second,ref vx_second,ref o_second);

                                    for (int block_number = Number_Block; block_number > 0; block_number--)
                                    {
                                        myblockmv.getConn(vx_second, yid0, bk_second, cellTable, occupiedTable, ref output_str, ref Number_Block, ref bk_second, ref vx_second, ref o_second);
                                        string filepath = exePath + "\\" + yid0;

                                        string x1 = bk_second.id;
                                        string x2 = vx_second.id;
                                        int x3 = o_second;

                                        string[] txt_content0 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径.txt");
                                        string[] txt_content1 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p1.txt");
                                        string[] txt_content2 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p2.txt");

                                        string path_b1 = "";
                                        string path_b2 = "";
                                        string blockpo_b = "";
                                        string de_cell = "";

                                        if (output_str == "")
                                        {
                                            Console.WriteLine("当前堆场不存在满足条件的空堆位存放阻挡分段" + bk.id);
                                        }//若所有的A/B/C类空堆位都不能无障碍到达


                                        else
                                        {
                                            if (txt_content0.Length == 0)
                                            {
                                                //截取不定长度的字符串
                                                string output_str1 = output_str;
                                                int y1 = output_str1.IndexOf("/") + 1;
                                                int z1 = output_str1.IndexOf("R" + yid0);
                                                string str1 = output_str1.Substring(y1, z1 - y1);//截取字符串“/”以后、R之前的部分，即出场路径."+2"输出结果会包括R2，没有"+2"的话就不包括R2。
                                                //Console.WriteLine(str1);

                                                //截取不定长度的字符串
                                                int y2 = output_str1.IndexOf("R" + yid0) + 1;
                                                int z2 = output_str1.IndexOf("*");
                                                string str2 = output_str1.Substring(y2 + 2, z2 - y2 - 2);//截取字符串的R以后、“*”之前的部分，即入场路径
                                                //Console.WriteLine(str2);

                                                //截取字符串的最后一位
                                                string str3 = output_str1.Substring(output_str1.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str3);

                                                //截取固定长度的字符串的第一种方法
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(z4 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str4);


                                                /* //截取固定长度的字符串的第二种方法
                                                int y4 = output_str1.IndexOf("R" + yid) + 1;
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(y4 + 2, z4 - y4 - 2);//截取字符串“R”以后、"*"之前的部分，即目标堆位
                                                Console.WriteLine(str4);
                                                */

                                                path_b1 = str1;
                                                path_b2 = str2;
                                                blockpo_b = str3;
                                                de_cell = str4;
                                            }

                                            else
                                            {
                                                string output_str0 = output_str;
                                                int y5 = output_str0.IndexOf("/") + 1;
                                                int z5 = output_str0.IndexOf("*");
                                                string str5 = output_str0.Substring(y5, z5 - y5);//截取字符串“/”以后、*之前的部分，即堆场内的路径
                                                //Console.WriteLine(str5);

                                                string str6 = null;//不存在第二段，因此为空

                                                string str7 = output_str0.Substring(output_str0.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str7);

                                                int z8 = output_str0.IndexOf("*");
                                                string str8 = output_str0.Substring(z8 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str8);

                                                path_b1 = str5;
                                                path_b2 = str6;
                                                blockpo_b = str7;
                                                de_cell = str8;
                                            }
                                        }



                                        //string path_b1 = "";
                                        //string path_b2 = "";
                                        //string blockpo_b = "";
                                        //string de_cell = "";
                                        // tempstr = tempstr + occupiedTable.Rows[o1]["分段ID"].ToString() + "   " + path + "    " + len + "    " + blockpo + "\r\n";

                                        task.Rows.Add(new object[] { task_id, bk_second.id, task_id, null, path_b1, 0, null, 0, path_b2, blockpo_b, 0, null, null, 0, 0 });
                                        task_id = task_id + 1;




                                        //更新堆场占用信息                      


                                        if (de_cell != "预舾装车间" && de_cell != "涂装车间" && de_cell != "总组平台")
                                        {
                                            DataRow oc2_dr = occupiedTable.NewRow();
                                            oc2_dr["分段ID"] = occupiedTable.Rows[o_second]["分段ID"].ToString();
                                            oc2_dr["分段尺寸"] = occupiedTable.Rows[o_second]["分段尺寸"].ToString();
                                            oc2_dr["堆位ID"] = de_cell;

                                            oc2_dr["分段放置位置"] = blockpo;

                                            oc2_dr["分段预计出场时间"] = occupiedTable.Rows[o_second]["分段预计出场时间"].ToString();
                                            occupiedTable.Rows.Add(oc2_dr);

                                        }
                                        
                                        occupiedTable.Rows.RemoveAt(o_second);
                                        
                                    }
                                    
                                    
                                        string filepath1 = exePath + "\\" + yid0;
                                        string[] txt_content01 = File.ReadAllLines(@"" + filepath1 + "阻挡分段移动_路径.txt");
                                        string[] txt_content11 = File.ReadAllLines(@"" + filepath1 + "阻挡分段移动_路径p1.txt");
                                        string[] txt_content21 = File.ReadAllLines(@"" + filepath1 + "阻挡分段移动_路径p2.txt");

                                        string path_b11 = "";
                                        string path_b21 = "";
                                        string blockpo_b1 = "";
                                        string de_cell1 = "";

                                        if (output_str == "")
                                        {
                                            Console.WriteLine("当前堆场不存在满足条件的空堆位存放阻挡分段" + bk.id);
                                        }//若所有的A/B/C类空堆位都不能无障碍到达


                                        else
                                        {
                                            if (txt_content01.Length == 0)
                                            {
                                                //截取不定长度的字符串
                                                string output_str1 = output_str;
                                                int y1 = output_str1.IndexOf("/") + 1;
                                                int z1 = output_str1.IndexOf("R" + yid0);
                                                string str1 = output_str1.Substring(y1, z1 - y1);//截取字符串“/”以后、R之前的部分，即出场路径."+2"输出结果会包括R2，"-1"的话就不包括R2。
                                                //Console.WriteLine(str1);

                                                //截取不定长度的字符串
                                                int y2 = output_str1.IndexOf("R" + yid0) + 1;
                                                int z2 = output_str1.IndexOf("*");
                                                string str2 = output_str1.Substring(y2 + 2, z2 - y2 - 2);//截取字符串的R以后、“*”之前的部分，即入场路径
                                                //Console.WriteLine(str2);

                                                //截取字符串的最后一位
                                                string str3 = output_str1.Substring(output_str1.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str3);

                                                //截取固定长度的字符串的第一种方法
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(z4 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str4);


                                                /* //截取固定长度的字符串的第二种方法
                                                int y4 = output_str1.IndexOf("R" + yid) + 1;
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(y4 + 2, z4 - y4 - 2);//截取字符串“R”以后、"*"之前的部分，即目标堆位
                                                Console.WriteLine(str4);
                                                */

                                                path_b11 = str1;
                                                path_b21 = str2;
                                                blockpo_b1 = str3;
                                                de_cell1 = str4;
                                            }

                                            else
                                            {
                                                string output_str0 = output_str;
                                                int y5 = output_str0.IndexOf("/") + 1;
                                                int z5 = output_str0.IndexOf("*");
                                                string str5 = output_str0.Substring(y5, z5 - y5);//截取字符串“/”以后、*之前的部分，即堆场内的路径
                                                //Console.WriteLine(str5);

                                                string str6 = null;//不存在第二段，因此为空

                                                string str7 = output_str0.Substring(output_str0.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str7);

                                                int z8 = output_str0.IndexOf("*");
                                                string str8 = output_str0.Substring(z8 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str8);

                                                path_b11 = str5;
                                                path_b21 = str6;
                                                blockpo_b1 = str7;
                                                de_cell1 = str8;
                                            }
                                        }



                                        //string path_b1 = "";
                                        //string path_b2 = "";
                                        //string blockpo_b = "";
                                        //string de_cell = "";
                                        // tempstr = tempstr + occupiedTable.Rows[o1]["分段ID"].ToString() + "   " + path + "    " + len + "    " + blockpo + "\r\n";

                                        task.Rows.Add(new object[] { task_id, occupiedTable.Rows[o1]["分段ID"].ToString(), task_id, null, path_b11, 0, null, 0, path_b21, blockpo_b1, 0, null, null, 0, 0 });
                                        task_id = task_id + 1;




                                        //更新堆场占用信息                      

                                        



                                        if (de_cell1 != "预舾装车间" && de_cell1 != "涂装车间" && de_cell1 != "总组平台")
                                        {
                                            DataRow oc2_dr = occupiedTable.NewRow();
                                            oc2_dr["分段ID"] = occupiedTable.Rows[o1]["分段ID"].ToString();
                                            oc2_dr["分段尺寸"] = occupiedTable.Rows[o1]["分段尺寸"].ToString();
                                            oc2_dr["堆位ID"] = de_cell1;

                                            oc2_dr["分段放置位置"] = blockpo;

                                            oc2_dr["分段预计出场时间"] = occupiedTable.Rows[o1]["分段预计出场时间"].ToString();
                                            occupiedTable.Rows.Add(oc2_dr);

                                        }
                                        occupiedTable.Rows.RemoveAt(o1);
                                    
                                    
                                   


                                }
                            }
                        }
                        string[] dr_ob2 = path2.Split('-');
                        for (int i1 = 0; i1 < dr_ob1.Length - 1; i1++)
                        {
                            for (int o1 = 0; o1 < occupiedTable.Rows.Count; o1++)
                            {
                                if (occupiedTable.Rows[o1]["堆位ID"].ToString() == dr_ob1[i1] && occupiedTable.Rows[o1]["分段ID"].ToString() != dtCopy.Rows[i]["分段编号"].ToString())
                                {


                                    //调用黄颖 getconn(celltable,occupiedTable,occupiedTable.Rows[o1]["分段ID"].ToString())
                                    //Block bk = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    //Vertex vt1 = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    //myblockmv.getConn(vt1, yid0, bk, cellTable, occupiedTable);

                                    Block bk = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    Vertex vt1 = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    string output_str = "";
                                    int Number_Block = 0;
                                    Block bk_second = null;
                                    Vertex vx_second = null;
                                    //Block bk2;
                                    int o_second = 0;
                                    myblockmv.getConn(vt1, yid0, bk, cellTable, occupiedTable, ref output_str, ref Number_Block, ref bk_second, ref vx_second, ref o_second);

                                    if (Number_Block == 1)
                                    {
                                        myblockmv.getConn(vx_second, yid0, bk_second, cellTable, occupiedTable, ref output_str, ref Number_Block, ref bk_second, ref vx_second, ref o_second);
                                        string filepath = exePath + "\\" + yid0;
                                        string[] txt_content0 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径.txt");
                                        string[] txt_content1 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p1.txt");
                                        string[] txt_content2 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p2.txt");

                                        string path_b1 = "";
                                        string path_b2 = "";
                                        string blockpo_b = "";
                                        string de_cell = "";

                                        if (output_str == "")
                                        {
                                            Console.WriteLine("当前堆场不存在满足条件的空堆位存放阻挡分段" + bk.id);
                                        }//若所有的A/B/C类空堆位都不能无障碍到达


                                        else
                                        {
                                            if (txt_content0.Length == 0)
                                            {
                                                //截取不定长度的字符串
                                                string output_str1 = output_str;
                                                int y1 = output_str1.IndexOf("/") + 1;
                                                int z1 = output_str1.IndexOf("R" + yid0);
                                                string str1 = output_str1.Substring(y1, z1 - y1);//截取字符串“/”以后、R之前的部分，即出场路径."+2"输出结果会包括R2，没有"+2"的话就不包括R2。
                                                //Console.WriteLine(str1);

                                                //截取不定长度的字符串
                                                int y2 = output_str1.IndexOf("R" + yid0) + 1;
                                                int z2 = output_str1.IndexOf("*");
                                                string str2 = output_str1.Substring(y2 + 2, z2 - y2 - 2);//截取字符串的R以后、“*”之前的部分，即入场路径
                                                //Console.WriteLine(str2);

                                                //截取字符串的最后一位
                                                string str3 = output_str1.Substring(output_str1.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str3);

                                                //截取固定长度的字符串的第一种方法
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(z4 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str4);


                                                /* //截取固定长度的字符串的第二种方法
                                                int y4 = output_str1.IndexOf("R" + yid) + 1;
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(y4 + 2, z4 - y4 - 2);//截取字符串“R”以后、"*"之前的部分，即目标堆位
                                                Console.WriteLine(str4);
                                                */

                                                path_b1 = str1;
                                                path_b2 = str2;
                                                blockpo_b = str3;
                                                de_cell = str4;
                                            }

                                            else
                                            {
                                                string output_str0 = output_str;
                                                int y5 = output_str0.IndexOf("/") + 1;
                                                int z5 = output_str0.IndexOf("*");
                                                string str5 = output_str0.Substring(y5, z5 - y5);//截取字符串“/”以后、*之前的部分，即堆场内的路径
                                                //Console.WriteLine(str5);

                                                string str6 = null;//不存在第二段，因此为空

                                                string str7 = output_str0.Substring(output_str0.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str7);

                                                int z8 = output_str0.IndexOf("*");
                                                string str8 = output_str0.Substring(z8 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str8);

                                                path_b1 = str5;
                                                path_b2 = str6;
                                                blockpo_b = str7;
                                                de_cell = str8;
                                            }
                                        }



                                        //string path_b1 = "";
                                        //string path_b2 = "";
                                        //string blockpo_b = "";
                                        //string de_cell = "";
                                        // tempstr = tempstr + occupiedTable.Rows[o1]["分段ID"].ToString() + "   " + path + "    " + len + "    " + blockpo + "\r\n";

                                        task.Rows.Add(new object[] { task_id, bk_second.id, task_id, null, path_b1, 0, null, 0, path_b2, blockpo_b, 0, null, null, 0, 0 });
                                        task_id = task_id + 1;




                                        //更新堆场占用信息                      





                                        if (de_cell != "预舾装车间" && de_cell != "涂装车间" && de_cell != "总组平台")
                                        {
                                            DataRow oc2_dr = occupiedTable.NewRow();
                                            oc2_dr["分段ID"] = occupiedTable.Rows[o_second]["分段ID"].ToString();
                                            oc2_dr["分段尺寸"] = occupiedTable.Rows[o_second]["分段尺寸"].ToString();
                                            oc2_dr["堆位ID"] = de_cell;

                                            oc2_dr["分段放置位置"] = blockpo;

                                            oc2_dr["分段预计出场时间"] = occupiedTable.Rows[o_second]["分段预计出场时间"].ToString();
                                            occupiedTable.Rows.Add(oc2_dr);



                                        }
                                        occupiedTable.Rows.RemoveAt(o_second);

                                    }
                                    else
                                    {
                                        string filepath = exePath + "\\" + yid0;
                                        string[] txt_content0 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径.txt");
                                        string[] txt_content1 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p1.txt");
                                        string[] txt_content2 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p2.txt");

                                        string path_b1 = "";
                                        string path_b2 = "";
                                        string blockpo_b = "";
                                        string de_cell = "";

                                        if (output_str == "")
                                        {
                                            Console.WriteLine("当前堆场不存在满足条件的空堆位存放阻挡分段" + bk.id);
                                        }//若所有的A/B/C类空堆位都不能无障碍到达


                                        else
                                        {
                                            if (txt_content0.Length == 0)
                                            {
                                                //截取不定长度的字符串
                                                string output_str1 = output_str;
                                                int y1 = output_str1.IndexOf("/") + 1;
                                                int z1 = output_str1.IndexOf("R" + yid0);
                                                string str1 = output_str1.Substring(y1, z1 - y1);//截取字符串“/”以后、R之前的部分，即出场路径."+2"输出结果会包括R2，没有"+2"的话就不包括R2。
                                                //Console.WriteLine(str1);

                                                //截取不定长度的字符串
                                                int y2 = output_str1.IndexOf("R" + yid0) + 1;
                                                int z2 = output_str1.IndexOf("*");
                                                string str2 = output_str1.Substring(y2 + 2, z2 - y2 - 2);//截取字符串的R以后、“*”之前的部分，即入场路径
                                                //Console.WriteLine(str2);

                                                //截取字符串的最后一位
                                                string str3 = output_str1.Substring(output_str1.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str3);

                                                //截取固定长度的字符串的第一种方法
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(z4 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str4);


                                                /* //截取固定长度的字符串的第二种方法
                                                int y4 = output_str1.IndexOf("R" + yid) + 1;
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(y4 + 2, z4 - y4 - 2);//截取字符串“R”以后、"*"之前的部分，即目标堆位
                                                Console.WriteLine(str4);
                                                */

                                                path_b1 = str1;
                                                path_b2 = str2;
                                                blockpo_b = str3;
                                                de_cell = str4;
                                            }

                                            else
                                            {
                                                string output_str0 = output_str;
                                                int y5 = output_str0.IndexOf("/") + 1;
                                                int z5 = output_str0.IndexOf("*");
                                                string str5 = output_str0.Substring(y5, z5 - y5);//截取字符串“/”以后、*之前的部分，即堆场内的路径
                                                //Console.WriteLine(str5);

                                                string str6 = null;//不存在第二段，因此为空

                                                string str7 = output_str0.Substring(output_str0.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str7);

                                                int z8 = output_str0.IndexOf("*");
                                                string str8 = output_str0.Substring(z8 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str8);

                                                path_b1 = str5;
                                                path_b2 = str6;
                                                blockpo_b = str7;
                                                de_cell = str8;
                                            }
                                        }



                                        //string path_b1 = "";
                                        //string path_b2 = "";
                                        //string blockpo_b = "";
                                        //string de_cell = "";
                                        // tempstr = tempstr + occupiedTable.Rows[o1]["分段ID"].ToString() + "   " + path + "    " + len + "    " + blockpo + "\r\n";

                                        task.Rows.Add(new object[] { task_id, occupiedTable.Rows[o1]["分段ID"].ToString(), task_id, null, path_b1, 0, null, 0, path_b2, blockpo_b, 0, null, null, 0, 0 });
                                        task_id = task_id + 1;




                                        //更新堆场占用信息                      





                                        if (de_cell != "预舾装车间" && de_cell != "涂装车间" && de_cell != "总组平台")
                                        {
                                            DataRow oc2_dr = occupiedTable.NewRow();
                                            oc2_dr["分段ID"] = occupiedTable.Rows[o1]["分段ID"].ToString();
                                            oc2_dr["分段尺寸"] = occupiedTable.Rows[o1]["分段尺寸"].ToString();
                                            oc2_dr["堆位ID"] = de_cell;

                                            oc2_dr["分段放置位置"] = blockpo;

                                            oc2_dr["分段预计出场时间"] = occupiedTable.Rows[o1]["分段预计出场时间"].ToString();
                                            occupiedTable.Rows.Add(oc2_dr);



                                        }
                                        occupiedTable.Rows.RemoveAt(o1);

                                    }
                                   
                                }
                            }
                        }
                    }
                    else
                    {
                        path1 = path;

                        string[] dr_ob1 = path1.Split('-');
                        for (int i1 = dr_ob1.Length - 2; i1 > 0; i1--)
                        {
                            for (int o1 = 0; o1 < occupiedTable.Rows.Count; o1++)
                            {
                                if (occupiedTable.Rows[o1]["堆位ID"].ToString() == dr_ob1[i1] && occupiedTable.Rows[o1]["分段ID"].ToString() != dtCopy.Rows[i]["分段编号"].ToString())
                                {


                                    //调用黄颖 getconn(celltable,occupiedTable,occupiedTable.Rows[o1]["分段ID"].ToString())
                                    //Block bk = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    //Vertex vt1 = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    //myblockmv.getConn(vt1, yid0, bk, cellTable, occupiedTable);

                                    Block bk = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    Vertex vt1 = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                                    string output_str = "";
                                    int Number_Block = 0;
                                    Block bk_second = null;
                                    Vertex vx_second = null;
                                    Block bk2;
                                    int o_second = 0;
                                    myblockmv.getConn(vt1, yid0, bk, cellTable, occupiedTable, ref output_str, ref Number_Block, ref bk_second, ref vx_second, ref o_second);

                                    if (Number_Block == 1)
                                    {
                                        myblockmv.getConn(vx_second, yid0, bk_second, cellTable, occupiedTable, ref output_str, ref Number_Block, ref bk_second, ref vx_second, ref o_second);
                                        //string xxxx = bk_second.id;
                                        //string xxxxx = vx_second.id;
                                        string filepath = exePath + "\\" + yid0;
                                        string[] txt_content0 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径.txt");
                                        string[] txt_content1 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p1.txt");
                                        string[] txt_content2 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p2.txt");

                                        string path_b1 = "";
                                        string path_b2 = "";
                                        string blockpo_b = "";
                                        string de_cell = "";

                                        if (output_str == "")
                                        {
                                            Console.WriteLine("当前堆场不存在满足条件的空堆位存放阻挡分段" + bk.id);
                                        }//若所有的A/B/C类空堆位都不能无障碍到达


                                        else
                                        {
                                            if (txt_content0.Length == 0)
                                            {
                                                //截取不定长度的字符串
                                                string output_str1 = output_str;
                                                int y1 = output_str1.IndexOf("/") + 1;
                                                int z1 = output_str1.IndexOf("R" + yid0);
                                                string str1 = output_str1.Substring(y1, z1 - y1);//截取字符串“/”以后、R之前的部分，即出场路径."+2"输出结果会包括R2，没有"+2"的话就不包括R2。
                                                //Console.WriteLine(str1);

                                                //截取不定长度的字符串
                                                int y2 = output_str1.IndexOf("R" + yid0) + 1;
                                                int z2 = output_str1.IndexOf("*");
                                                string str2 = output_str1.Substring(y2 + 2, z2 - y2 - 2);//截取字符串的R以后、“*”之前的部分，即入场路径
                                                //Console.WriteLine(str2);

                                                //截取字符串的最后一位
                                                string str3 = output_str1.Substring(output_str1.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str3);

                                                //截取固定长度的字符串的第一种方法
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(z4 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str4);


                                                /* //截取固定长度的字符串的第二种方法
                                                int y4 = output_str1.IndexOf("R" + yid) + 1;
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(y4 + 2, z4 - y4 - 2);//截取字符串“R”以后、"*"之前的部分，即目标堆位
                                                Console.WriteLine(str4);
                                                */

                                                path_b1 = str1;
                                                path_b2 = str2;
                                                blockpo_b = str3;
                                                de_cell = str4;
                                            }

                                            else
                                            {
                                                string output_str0 = output_str;
                                                int y5 = output_str0.IndexOf("/") + 1;
                                                int z5 = output_str0.IndexOf("*");
                                                string str5 = output_str0.Substring(y5, z5 - y5);//截取字符串“/”以后、*之前的部分，即堆场内的路径
                                                //Console.WriteLine(str5);

                                                string str6 = null;//不存在第二段，因此为空

                                                string str7 = output_str0.Substring(output_str0.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str7);

                                                int z8 = output_str0.IndexOf("*");
                                                string str8 = output_str0.Substring(z8 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str8);

                                                path_b1 = str5;
                                                path_b2 = str6;
                                                blockpo_b = str7;
                                                de_cell = str8;
                                            }
                                        }



                                        //string path_b1 = "";
                                        //string path_b2 = "";
                                        //string blockpo_b = "";
                                        //string de_cell = "";
                                        // tempstr = tempstr + occupiedTable.Rows[o1]["分段ID"].ToString() + "   " + path + "    " + len + "    " + blockpo + "\r\n";

                                        task.Rows.Add(new object[] { task_id, bk_second.id, task_id, null, path_b1, 0, null, 0, path_b2, blockpo_b, 0, null, null, 0, 0 });
                                        task_id = task_id + 1;




                                        //更新堆场占用信息                      





                                        if (de_cell != "预舾装车间" && de_cell != "涂装车间" && de_cell != "总组平台")
                                        {
                                            DataRow oc2_dr = occupiedTable.NewRow();
                                            oc2_dr["分段ID"] = occupiedTable.Rows[o_second]["分段ID"].ToString();
                                            oc2_dr["分段尺寸"] = occupiedTable.Rows[o_second]["分段尺寸"].ToString();
                                            oc2_dr["堆位ID"] = de_cell;

                                            oc2_dr["分段放置位置"] = blockpo;

                                            oc2_dr["分段预计出场时间"] = occupiedTable.Rows[o_second]["分段预计出场时间"].ToString();
                                            occupiedTable.Rows.Add(oc2_dr);



                                        }
                                        occupiedTable.Rows.RemoveAt(o_second);

                                    }
                                    else
                                    {
                                        string filepath = exePath + "\\" + yid0;
                                        string[] txt_content0 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径.txt");
                                        string[] txt_content1 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p1.txt");
                                        string[] txt_content2 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p2.txt");

                                        string path_b1 = "";
                                        string path_b2 = "";
                                        string blockpo_b = "";
                                        string de_cell = "";

                                        if (output_str == "")
                                        {
                                            Console.WriteLine("当前堆场不存在满足条件的空堆位存放阻挡分段" + bk.id);
                                        }//若所有的A/B/C类空堆位都不能无障碍到达


                                        else
                                        {
                                            if (txt_content0.Length == 0)
                                            {
                                                //截取不定长度的字符串
                                                string output_str1 = output_str;
                                                int y1 = output_str1.IndexOf("/") + 1;
                                                int z1 = output_str1.IndexOf("R" + yid0);
                                                string str1 = output_str1.Substring(y1, z1 - y1);//截取字符串“/”以后、R之前的部分，即出场路径."+2"输出结果会包括R2，没有"+2"的话就不包括R2。
                                                //Console.WriteLine(str1);

                                                //截取不定长度的字符串
                                                int y2 = output_str1.IndexOf("R" + yid0) + 1;
                                                int z2 = output_str1.IndexOf("*");
                                                string str2 = output_str1.Substring(y2 + 2, z2 - y2 - 2);//截取字符串的R以后、“*”之前的部分，即入场路径
                                                //Console.WriteLine(str2);

                                                //截取字符串的最后一位
                                                string str3 = output_str1.Substring(output_str1.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str3);

                                                //截取固定长度的字符串的第一种方法
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(z4 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str4);


                                                /* //截取固定长度的字符串的第二种方法
                                                int y4 = output_str1.IndexOf("R" + yid) + 1;
                                                int z4 = output_str1.IndexOf("*");
                                                string str4 = output_str1.Substring(y4 + 2, z4 - y4 - 2);//截取字符串“R”以后、"*"之前的部分，即目标堆位
                                                Console.WriteLine(str4);
                                                */

                                                path_b1 = str1;
                                                path_b2 = str2;
                                                blockpo_b = str3;
                                                de_cell = str4;
                                            }

                                            else
                                            {
                                                string output_str0 = output_str;
                                                int y5 = output_str0.IndexOf("/") + 1;
                                                int z5 = output_str0.IndexOf("*");
                                                string str5 = output_str0.Substring(y5, z5 - y5);//截取字符串“/”以后、*之前的部分，即堆场内的路径
                                                //Console.WriteLine(str5);

                                                string str6 = null;//不存在第二段，因此为空

                                                string str7 = output_str0.Substring(output_str0.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                                                //Console.WriteLine(str7);

                                                int z8 = output_str0.IndexOf("*");
                                                string str8 = output_str0.Substring(z8 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                                                //Console.WriteLine(str8);

                                                path_b1 = str5;
                                                path_b2 = str6;
                                                blockpo_b = str7;
                                                de_cell = str8;
                                            }
                                        }



                                        //string path_b1 = "";
                                        //string path_b2 = "";
                                        //string blockpo_b = "";
                                        //string de_cell = "";
                                        // tempstr = tempstr + occupiedTable.Rows[o1]["分段ID"].ToString() + "   " + path + "    " + len + "    " + blockpo + "\r\n";

                                        task.Rows.Add(new object[] { task_id, occupiedTable.Rows[o1]["分段ID"].ToString(), task_id, null, path_b1, 0, null, 0, path_b2, blockpo_b, 0, null, null, 0, 0 });
                                        task_id = task_id + 1;




                                        //更新堆场占用信息                      





                                        if (de_cell != "预舾装车间" && de_cell != "涂装车间" && de_cell != "总组平台")
                                        {
                                            DataRow oc2_dr = occupiedTable.NewRow();
                                            oc2_dr["分段ID"] = occupiedTable.Rows[o1]["分段ID"].ToString();
                                            oc2_dr["分段尺寸"] = occupiedTable.Rows[o1]["分段尺寸"].ToString();
                                            oc2_dr["堆位ID"] = de_cell;

                                            oc2_dr["分段放置位置"] = blockpo;

                                            oc2_dr["分段预计出场时间"] = occupiedTable.Rows[o1]["分段预计出场时间"].ToString();
                                            occupiedTable.Rows.Add(oc2_dr);



                                        }
                                        occupiedTable.Rows.RemoveAt(o1);

                                    }
                                   
                                }
                            }
                        }

                    }





                    if (dtCopy.Rows[i]["当前位置"].ToString() != "平直车间" && dtCopy.Rows[i]["当前位置"].ToString() != "曲面车间" && dtCopy.Rows[i]["当前位置"].ToString() != "8号平台") // && dtCopy.Rows[i]["目标场地"].ToString() == "堆场"
                    {
                        //len = (float)Math.Round((float)(plen0 + plen1), 3);

                        //更新堆场占用信息                      
                        int cc2 = 0;
                        int oc1;
                        int row_ID = 0;
                        DataTable row_tempTable = occupiedTable.Copy();
                        for (oc1 = 0; oc1 < row_tempTable.Rows.Count; oc1++)
                        {
                            if (dtCopy.Rows[i]["分段编号"].ToString() == row_tempTable.Rows[oc1]["分段ID"].ToString())
                            {
                                cc2 = cc2 + 1;
                                row_ID = oc1;
                                break;
                            }
                        }
                        if (cc2 > 0)
                        {
                            occupiedTable.Rows.RemoveAt(row_ID);
                        }


                        if (sArray2_ID[sArray2_ID.Length - 1] != "预舾装车间" && sArray2_ID[sArray2_ID.Length - 1] != "涂装车间" && sArray2_ID[sArray2_ID.Length - 1] != "总组平台")
                        {
                            DataRow oc2_dr = occupiedTable.NewRow();
                            oc2_dr["分段ID"] = dtCopy.Rows[i]["分段编号"].ToString();
                            oc2_dr["分段尺寸"] = dtCopy.Rows[i]["分段尺寸"].ToString();
                            oc2_dr["堆位ID"] = sArray2_ID[sArray2_ID.Length - 1];
                            if (dtCopy.Rows[i]["分段尺寸"].ToString() == "小")
                            {
                                DataRow[] ocs_dr = occupiedTable.Select("[堆位ID]='" + sArray2_ID[sArray2_ID.Length - 1].ToString() + "'");
                                DataTable ocs_tempTable = occupiedTable.Clone();
                                ocs_tempTable.Clear();

                                if (sArray2_ID.Length < 2) oc2_dr["分段放置位置"] = "1";
                                else
                                {
                                    if (ocs_dr.Length == 0)
                                    {
                                        DataRow[] po_dr2 = cellTable.Select("[堆位ID]='" + sArray2_ID[sArray2_ID.Length - 1].ToString() + "'");
                                        DataTable po_tempTable1 = cellTable.Clone();
                                        po_tempTable1.Clear();
                                        for (int po = 0; po < po_dr2.Length; po++)
                                        {
                                            po_tempTable1.ImportRow(po_dr2[po]);
                                        }

                                        if (po_tempTable1.Rows[0]["东堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "3"; }
                                        else if (po_tempTable1.Rows[0]["南堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "1"; }
                                        else if (po_tempTable1.Rows[0]["西堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "4"; }
                                        else if (po_tempTable1.Rows[0]["北堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "2"; }
                                        else oc2_dr["分段放置位置"] = "1";


                                    }
                                    else
                                    {
                                        for (int o5 = 0; o5 < ocs_dr.Length; o5++)
                                        {
                                            ocs_tempTable.ImportRow(ocs_dr[o5]);
                                        }
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "1") oc2_dr["分段放置位置"] = "2";
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "2") oc2_dr["分段放置位置"] = "1";
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "3") oc2_dr["分段放置位置"] = "4";
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "4") oc2_dr["分段放置位置"] = "3";
                                    }
                                }







                            }
                            else
                            { oc2_dr["分段放置位置"] = "0"; }
                            DateTime dt = Convert.ToDateTime(dtCopy.Rows[i]["任务时间"].ToString());
                            dt = dt.AddDays(Convert.ToInt32(dtCopy.Rows[i]["时间余量"].ToString()));
                            string dt1 = dt.ToString("yyyy/M/d");

                            oc2_dr["分段预计出场时间"] = dt1;
                            occupiedTable.Rows.Add(oc2_dr);

                            blockpo = oc2_dr["分段放置位置"].ToString();
                        }


                    }
                    else
                    {
                        //len = (float)Math.Round((float)(plen0 + plen1), 3);

                        //更新堆场占用信息   
                        if (sArray2_ID[sArray2_ID.Length - 1] != "预舾装车间" && sArray2_ID[sArray2_ID.Length - 1] != "涂装车间" && sArray2_ID[sArray2_ID.Length - 1] != "总组平台")
                        {
                            DataRow oc2_dr = occupiedTable.NewRow();
                            oc2_dr["分段ID"] = dtCopy.Rows[i]["分段编号"].ToString();
                            oc2_dr["分段尺寸"] = dtCopy.Rows[i]["分段尺寸"].ToString();
                            oc2_dr["堆位ID"] = sArray2_ID[sArray2_ID.Length - 1];

                            if (dtCopy.Rows[i]["分段尺寸"].ToString() == "小")
                            {
                                DataRow[] ocs_dr = occupiedTable.Select("[堆位ID]='" + sArray2_ID[sArray2_ID.Length - 1] + "'");
                                DataTable ocs_tempTable = occupiedTable.Clone();
                                ocs_tempTable.Clear();

                                if (sArray2_ID.Length < 2) oc2_dr["分段放置位置"] = "1";
                                else
                                {
                                    if (ocs_dr.Length == 0)
                                    {
                                        DataRow[] po_dr2 = cellTable.Select("[堆位ID]='" + sArray2_ID[sArray2_ID.Length - 1].ToString() + "'");
                                        DataTable po_tempTable1 = cellTable.Clone();
                                        po_tempTable1.Clear();
                                        for (int po = 0; po < po_dr2.Length; po++)
                                        {
                                            po_tempTable1.ImportRow(po_dr2[po]);
                                        }

                                        if (po_tempTable1.Rows[0]["东堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "3"; }
                                        else if (po_tempTable1.Rows[0]["南堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "1"; }
                                        else if (po_tempTable1.Rows[0]["西堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "4"; }
                                        else if (po_tempTable1.Rows[0]["北堆位ID"].ToString() == sArray2_ID[sArray2_ID.Length - 2].ToString())
                                        { oc2_dr["分段放置位置"] = "2"; }
                                        else oc2_dr["分段放置位置"] = "1";
                                    }
                                    else
                                    {
                                        for (int o5 = 0; o5 < ocs_dr.Length; o5++)
                                        {
                                            ocs_tempTable.ImportRow(ocs_dr[o5]);
                                        }
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "1") oc2_dr["分段放置位置"] = "2";
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "2") oc2_dr["分段放置位置"] = "1";
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "3") oc2_dr["分段放置位置"] = "4";
                                        if (ocs_tempTable.Rows[0]["分段放置位置"].ToString() == "4") oc2_dr["分段放置位置"] = "3";
                                    }
                                }


                            }
                            else
                            { oc2_dr["分段放置位置"] = "0"; }
                            DateTime dt = Convert.ToDateTime(dtCopy.Rows[i]["任务时间"].ToString());
                            dt = dt.AddDays(Convert.ToInt32(dtCopy.Rows[i]["时间余量"].ToString()));
                            string dt1 = dt.ToString("yyyy/M/d");

                            oc2_dr["分段预计出场时间"] = dt1;
                            occupiedTable.Rows.Add(oc2_dr);

                            blockpo = oc2_dr["分段放置位置"].ToString();
                        }

                    }

                    tempstr = tempstr + dtCopy.Rows[i]["分段编号"].ToString() + "   " + path + "    " + len + "    " + blockpo + "\r\n";
                    task.Rows.Add(new object[] { task_id, dtCopy.Rows[i]["分段编号"].ToString(), task_id, null, path1, 0, null, 0, path2, blockpo, 0, null, null, 0, 0 });
                    task_id = task_id + 1;

                }


            }



            //删除重复行


            string[] path_id = path.Split('-');


            for (int j1 = 0; j1 < task.Rows.Count; j1++)
            {
                for (int j2 = j1 + 1; j2 < task.Rows.Count; j2++)
                {
                    if (task.Rows[j1]["分段ID"].ToString() == task.Rows[j2]["分段ID"].ToString())
                    {

                        DataRow[] dr_de1 = occupiedTable.Select("[分段ID]='" + task.Rows[j1]["分段ID"].ToString() + "'");
                        DataTable table_de1 = occupiedTable.Clone();
                        table_de1.Clear();
                        for (int c1 = 0; c1 < dr_de1.Length; c1++)
                        {
                            table_de1.ImportRow(dr_de1[c1]);
                        }
                        DataRow[] dr_de2 = occupiedTable.Select("[分段ID]='" + task.Rows[j1 + 1]["分段ID"].ToString() + "'");
                        DataTable table_de2 = occupiedTable.Clone();
                        table_de2.Clear();
                        for (int c1 = 0; c1 < dr_de2.Length; c1++)
                        {
                            table_de2.ImportRow(dr_de2[c1]);
                        }
                        for (int j3 = 0; j3 < occupiedTable.Rows.Count; j3++)
                        {
                            //DataRow[] dr_de = occupiedTable.Select("[堆位ID]='" + path_id[0].ToString() + "'");
                            //DataTable table_de = occupiedTable.Clone();
                            //table_de.Clear();
                            //for (int c1 = 0; c1 < dr_de.Length; c1++)
                            //{
                            //    table_de.ImportRow(dr_de[c1]);
                            //}


                            // if (path_id[0].ToString() == occupiedTable.Rows[j3]["堆位ID"].ToString() && table_de2.Rows[0]["分段预计出场时间"].ToString() != table_de1.Rows[0]["分段预计出场时间"].ToString())
                            if (occupiedTable.Rows[j3]["分段ID"].ToString() == task.Rows[j1]["分段ID"].ToString() && occupiedTable.Rows[j3]["分段尺寸"].ToString() == "小" && path_id[0].ToString() == occupiedTable.Rows[j3]["堆位ID"].ToString())
                            //occupiedTable.Rows[j3]["分段ID"].ToString() == task.Rows[j1]["分段ID"].ToString() &&
                            {
                                task.Rows.RemoveAt(j2);
                            }


                            else if (occupiedTable.Rows[j3]["分段ID"].ToString() == task.Rows[j1]["分段ID"].ToString())
                            {
                                task.Rows[j1]["起始堆场内路径"] = task.Rows[j2]["起始堆场内路径"];
                                task.Rows[j1]["目标堆场内路径"] = task.Rows[j2]["目标堆场内路径"];
                                task.Rows[j1]["分段放置位置"] = task.Rows[j2]["分段放置位置"];
                                task.Rows.RemoveAt(j1);
                            }
                        }



                    }
                }
            }
            //同一堆场内路径分为两部分

            for (int t_id = 0; t_id < task.Rows.Count; t_id++)
            {
                task.Rows[t_id]["指令序号"] = task.Rows[t_id]["指令ID"] = t_id + 1;
                if (task.Rows[t_id]["起始堆场内路径"].ToString().Length > 0 && task.Rows[t_id]["目标堆场内路径"].ToString().Length == 0)
                {
                    string[] p1 = task.Rows[t_id]["起始堆场内路径"].ToString().Split('-');
                    task.Rows[t_id]["目标堆场内路径"] = p1[p1.Length - 1];
                    int p2 = task.Rows[t_id]["起始堆场内路径"].ToString().LastIndexOf("-");
                    task.Rows[t_id]["起始堆场内路径"] = task.Rows[t_id]["起始堆场内路径"].ToString().Substring(0, p2);
                }

            }





            DataView dv2 = task.DefaultView;
            dv2.Sort = "指令序号";   //第一种排序
            DataTable taskCopy = dv2.ToTable();
            adapater1.Update(taskCopy);


            textBox3.Clear();
            textBox3.Text = tempstr;
            conn.Close();

        }       
                    

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        } 
           
               
       
        
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter =  "all files (*.*)|*.*"; 

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = this.openFileDialog1.FileName;
                // 你的处理文件路径代码 
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            path_road mypath_road = new path_road();
            string temppath="";
            mypath_road.get_path(ref temppath);
            //textBox2.Clear();
            textBox2.Text = temppath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            //frm.MdiParent = this;
            frm.Show();
        }      

        
    }
}
