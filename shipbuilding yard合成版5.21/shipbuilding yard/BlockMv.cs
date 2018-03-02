using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Windows.Forms; //命名空间引入；

namespace shipbuilding_yard
{
    class BlockMv
    {
        //public void ReadAccess(string yid, out DataTable cellTable, out DataTable occupiedTable)
        //{
        //    //连接Access字符串.把access数据库拷贝到debug目录下
        //    string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Local Database.accdb;Persist Security Info=False";
        //    OleDbConnection conn = new OleDbConnection(conStr);
        //    conn.Open();

        //    //读取堆位
        //    OleDbDataAdapter cell = new OleDbDataAdapter("select * from 堆位信息 where [场地ID]='" + yid + "'", conn);
        //    cellTable = new DataTable();//定义DataTable类型的数据cellTable.存放堆位；
        //    cellTable.Clear();
        //    cell.Fill(cellTable);


        //    DataView dv = new DataView(cellTable);
        //    dv.Sort = "堆位ID asc"; //堆位按升序显示
        //    cellTable = dv.ToTable();

        //    //读取堆位占用信息和出场时间信息
        //    OleDbDataAdapter occupied = new OleDbDataAdapter("select * from 当前全局堆场占用信息", conn);
        //    occupiedTable = new DataTable();//存放堆位上分段的占用情况
        //    occupiedTable.Clear();
        //    occupied.Fill(occupiedTable);


        //    conn.Close();
        //}





        //public void ReadExcel(DataTable cellTable, DataTable occupiedTable, out DataTable dtExcel)
        //{
        //    //已知一个堆位上的阻挡分段
        //    dtExcel = new DataTable();//数据表
        //    DataSet ds = new DataSet(); //获取文件扩展名

        //    string strExtension = System.IO.Path.GetExtension(@"C:\\Users\\HuangYing\\Desktop\\BlockMotion\\阻挡分段输入.xlsx");
        //    string strFileName = System.IO.Path.GetFileName(@"C:\\Users\\HuangYing\\Desktop\\BlockMotion\\阻挡分段输入.xlsx");
        //    OleDbConnection objConn = null;
        //    switch (strExtension)
        //    {
        //        case ".xls":
        //            objConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\HuangYing\\Desktop\\BlockMotion\\阻挡分段输入.xlsx;" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\"");
        //            break;
        //        case ".xlsx":
        //            objConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\HuangYing\\Desktop\\BlockMotion\\阻挡分段输入.xlsx;" + "Extended Properties=\"Excel 12.0;HDR=False;IMEX=1;\"");
        //            break;
        //        default:
        //            objConn = null;
        //            break;
        //    }

        //    objConn.Open();

        //    string strSql = "select * from [Sheet1$]";//获取Excel指定Sheet表中的信息
        //    OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
        //    OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
        //    ds.Clear();
        //    dtExcel.Clear();
        //    myData.Fill(ds, "Sheet1");//填充数据
        //    objConn.Close();
        //    dtExcel = ds.Tables["Sheet1"];//dtExcel即为excel文件中指定表中存储的信息

        //    string id = "";
        //    string pb = "";
        //    string dt = "";
        //    string position = "";
        //    Block bk0 = new Block(id, pb, dt, position);
        //    for (int i = 0; i < dtExcel.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < occupiedTable.Rows.Count; j++)
        //        {
        //            bk0.id = dtExcel.Rows[i]["阻挡分段ID"].ToString();//读取阻挡分段的ID号
        //            if (occupiedTable.Rows[j]["分段ID"].ToString() == bk0.id)
        //            {
        //                bk0.pb = occupiedTable.Rows[j]["堆位ID"].ToString();//查数据库
        //                bk0.dt = occupiedTable.Rows[j]["分段预计出场时间"].ToString();
        //            }
        //        }
        //    }

        //    DateTime dt0 = DateTime.Parse(Convert.ToDateTime(bk0.dt).ToString("yyyy/M/d"));
        //    DateTime Today = DateTime.Today;
        //    System.TimeSpan DT0 = dt0 - Today;
        //    int getDay0 = (int)DT0.TotalDays;
        //}



        double mobility0 = 0;//表示采用场内直达方式的移动度
        double mobility1 = 0;//表示采用先出场后入场方式时，出场过程中的移动度
        double mobility2 = 0;//表示采用先出场后入场方式时，入场过程中的移动度

        //int Number_Block = 0;
        int Number_Block;//作为判断依据，阻挡分段移动路径上还有Number_Block个额外的阻挡分段；
        Block bk_second = null;//声明第二个阻挡分段
        Vertex vx_second = null;//声明第二个阻挡分段所在的堆位
        int o_second = 0;//声明第二个阻挡分段的放置姿态
        //string output_str = "";

        //BlockMv的主函数getConn
        public void getConn(Vertex v_start, string yid, Block bk0, DataTable cellTable, DataTable occupiedTable, ref string output_str, ref int Number_Block, ref Block bk_second, ref Vertex vx_second, ref int o_second)
        //public getConn(Vertex v_start, Vertex v_end, string yid, Block bk0, DataTable cellTable, DataTable occupiedTable, DataTable dtExcel,ref string output_str)
        {
            string aa = bk0.id;//调式时看是否传值成功
            Vertex v_end = null;
            output_str = "";//声明最终返回的string
            string exePath = System.Windows.Forms.Application.ExecutablePath;//程序文件的相对位置
            //string exePath = exePath1
            int index;
            for (int i = 0; i < 4; i++)
            {
                index = exePath.LastIndexOf("\\");
                exePath = exePath.Substring(0, index);
            }


            //读取分段所在堆场的堆位信息（读取陈凯的cellTable）
            DataRow[] dr1 = cellTable.Select("[场地ID]='" + yid + "'");//选取cellTable里面的场地ID为当前所在场地的行信息
            DataTable tempTable = cellTable.Clone();
            tempTable.Clear();//tempTable保留cellTable的格式



            for (int c1 = 0; c1 < dr1.Length; c1++)//一行行往tempTable里面加入行信息
            {
                tempTable.ImportRow(dr1[c1]);
            }

            DataView dv1 = new DataView(tempTable);//排序
            dv1.Sort = "堆位Name asc";
            tempTable = dv1.ToTable();//现在完成tempTable的填值

            /*
            string id = "";
            string pb = "";
            string dt = "";
            string position = "";
            bk0 = new Block(id, pb, dt, position);
            bk0.id = "7";//测试用：陈凯给的阻挡分段ID
             */


            for (int i = 0; i < occupiedTable.Rows.Count; i++)
            {
                if (occupiedTable.Rows[i]["分段ID"].ToString() == bk0.id)
                {
                    bk0.pb = occupiedTable.Rows[i]["堆位ID"].ToString();//阻挡分段所在堆位ID
                    bk0.dt = occupiedTable.Rows[i]["分段预计出场时间"].ToString();//阻挡分段离场时间
                    bk0.position = occupiedTable.Rows[i]["分段放置位置"].ToString();//阻挡分段放置姿态
                    if (occupiedTable.Rows[i]["分段尺寸"].ToString() == "小")
                    {
                        bk0.position = "1";
                    }
                    else { bk0.position = "0"; }//阻挡分段分配到空堆位后的放置位置
                }
            }

            //将阻挡分段离场时间转换为DateTime格式，进行计算
            DateTime dt0 = DateTime.Parse(Convert.ToDateTime(bk0.dt).ToString("yyyy/M/d"));
            DateTime Today = DateTime.Today;
            System.TimeSpan DT0 = dt0 - Today;
            int getDay0 = (int)DT0.TotalDays;
            int getDay2 = 0;//声明当前堆位四周堆位上分段的离场时间





            //初始化距离矩阵。对角线为0，其他地方为90000的矩阵
            int[,] dismatrix = new int[tempTable.Rows.Count, tempTable.Rows.Count];//声明固定长度的矩阵数组
            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                for (int j = 0; j < tempTable.Rows.Count; j++)
                {
                    if (i == j)
                    {
                        dismatrix[i, j] = 0;
                    }
                    else { dismatrix[i, j] = 90000; }
                }
            }



            //定义存放堆位权重值在堆位-东-南-西-北分权重的格式
            int[,] Weight = new int[tempTable.Rows.Count, 4];
            int[] W = new int[tempTable.Rows.Count];//声明堆位权重值

            //定义堆位的权重值计算方法，即四个方向的分权重相加
            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                W[i] = 0;
                for (int j = 0; j < 4; j++)
                {
                    Weight[i, j] = 0;
                    W[i] = W[i] + Weight[i, j];
                }
            }



            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                Vertex vt = new Vertex(tempTable.Rows[i]["堆位ID"].ToString());//读取当前堆位，即数据库的第i行第2列
                for (int j = 0; j < tempTable.Rows.Count; j++)//j为局部变量
                {
                    if (tempTable.Rows[i]["东堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString())//读出该堆位值为当前堆位的东侧堆位
                    { //东侧堆位占用情况
                        Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());//读取当前堆位的东侧堆位
                        vt.east = vt1;
                        int c1 = 0, c2 = 0;

                        List<Block> occupiedby1 = new List<Block>();
                        List<Block> occupiedby2 = new List<Block>();



                        for (int k = 0; k < occupiedTable.Rows.Count; k++)
                        {
                            if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())//占用状态表的第k行第2列
                            {
                                Block bk1 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                occupiedby1.Add(bk1);
                                c1 = occupiedby1.Count;//表示当前堆位上的分段个数
                            }


                            if (tempTable.Rows[i]["东堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                            {
                                Block bk2 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());//读取东侧堆位上分段的分段号/堆位号/离场时间
                                occupiedby2.Add(bk2);
                                c2 = occupiedby2.Count;//表示当前堆位的东侧堆位上的分段个数

                                if (c2 == 2)//若一个堆位上有两个小分段
                                {
                                    Block bk21 = occupiedby2.ElementAt(0);//分别定义一个堆位上的两个分段
                                    Block bk22 = occupiedby2.ElementAt(1);


                                    DateTime dt21 = DateTime.Parse(bk21.dt);//分别定义一个堆位上的两个分段的离场时间
                                    DateTime dt22 = DateTime.Parse(bk22.dt);

                                    System.TimeSpan DT21 = dt21 - Today;
                                    System.TimeSpan DT22 = dt22 - Today;
                                    int getDay21 = (int)DT21.TotalDays;
                                    int getDay22 = (int)DT22.TotalDays;

                                    if (getDay21 > getDay22)//选择两个分段中离场时间距当前时间更长的分段的离场时间用于计算
                                    {
                                        getDay2 = getDay21;
                                    }
                                    else
                                    {
                                        getDay2 = getDay22;
                                    }
                                }
                                else if (c2 == 1)//若一个堆位上只有一个分段
                                {
                                    DateTime dt2 = DateTime.Parse(bk2.dt);
                                    System.TimeSpan DT2 = dt2 - Today;
                                    getDay2 = (int)DT2.TotalDays;
                                }
                            }
                        }

                        //定义距离矩阵，时间权重值在东侧堆位的分权重值
                        if (c1 == 0 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1; Weight[i, 0] = 10000; }
                        if (c1 == 0 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 501; ; Weight[i, 0] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }//用离场时间计算东侧分权重值
                        if (c1 == 0 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 0] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }
                        if (c1 == 1 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 501; Weight[i, 0] = 90000; }//当前堆位被占用，权重为无穷大，设为900000
                        if (c1 == 1 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 0] = 90000; }
                        if (c1 == 1 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 0] = 90000; }
                        if (c1 == 2 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 0] = 90000; }
                        if (c1 == 2 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 0] = 90000; }
                        if (c1 == 2 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 2001; Weight[i, 0] = 90000; }

                    }
                    else if (tempTable.Rows[i]["东堆位ID"].ToString() == "R") { Weight[i, 0] = 10000; }//堆场东侧边界可通行
                    else if (tempTable.Rows[i]["东堆位ID"].ToString() == "S") { Weight[i, 0] = 50000; }//堆场东侧边界不通行
                    //以上是当前堆位与其东侧堆位的关系，得到东侧分权重值


                    if (tempTable.Rows[i]["南堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString())//若第i+1行第8列的堆位与第j+1行第2列堆位相同，读出该堆位值为当前堆位的南侧堆位
                    { //南侧堆位占用情况
                        Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());//读取当前堆位的南侧堆位
                        vt.east = vt1;
                        int c1 = 0, c2 = 0;
                        List<Block> occupiedby1 = new List<Block>();
                        List<Block> occupiedby2 = new List<Block>();


                        for (int k = 0; k < occupiedTable.Rows.Count; k++)
                        {

                            if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())//占用状态表的第k行第2列
                            {
                                Block bk1 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                occupiedby1.Add(bk1);
                                c1 = occupiedby1.Count;//表示当前堆位上的分段个数为c1
                            }

                            if (tempTable.Rows[i]["南堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                            {
                                Block bk2 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());//读取南侧堆位分段信息
                                occupiedby2.Add(bk2);

                                c2 = occupiedby2.Count;//南侧堆位上存在的分段个数为c2

                                if (c2 == 2)//若南侧堆位上存在的分段个数为2个
                                {
                                    Block bk21 = occupiedby2.ElementAt(0);
                                    Block bk22 = occupiedby2.ElementAt(1);

                                    DateTime dt21 = DateTime.Parse(bk21.dt);
                                    DateTime dt22 = DateTime.Parse(bk22.dt);
                                    System.TimeSpan DT21 = dt21 - Today;
                                    System.TimeSpan DT22 = dt22 - Today;
                                    int getDay21 = (int)DT21.TotalDays;
                                    int getDay22 = (int)DT22.TotalDays;

                                    if (getDay21 > getDay22)//选两个分段中离场时间较晚的那个用于计算
                                    {
                                        getDay2 = getDay21;
                                    }
                                    else
                                    {
                                        getDay2 = getDay22;
                                    }

                                }
                                else if (c2 == 1)//南侧堆位上存在的分段个数为1个
                                {
                                    DateTime dt2 = DateTime.Parse(bk2.dt);
                                    System.TimeSpan DT2 = dt2 - Today;
                                    getDay2 = (int)DT2.TotalDays;
                                }
                            }
                        }



                        if (c1 == 0 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1; Weight[i, 1] = 10000; }
                        if (c1 == 0 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 501; ; Weight[i, 1] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }
                        if (c1 == 0 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 1] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }
                        if (c1 == 1 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 501; Weight[i, 1] = 90000; }//当前堆位被占用，权重为无穷大，设为900000
                        if (c1 == 1 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 1] = 90000; }
                        if (c1 == 1 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 1] = 90000; }
                        if (c1 == 2 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 1] = 90000; }
                        if (c1 == 2 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 1] = 90000; }
                        if (c1 == 2 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 2001; Weight[i, 1] = 90000; }

                    }
                    else if (tempTable.Rows[i]["南堆位ID"].ToString() == "R") { Weight[i, 1] = 10000; }//堆场南侧边界可通行
                    else if (tempTable.Rows[i]["南堆位ID"].ToString() == "S") { Weight[i, 1] = 50000; }//堆场南侧边界不通行
                    //以上是当前堆位与其南侧堆位的关系，得到南侧分权重值


                    if (tempTable.Rows[i]["西堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString())//若第i+1行第9列的堆位与第j+1行第2列堆位相同，读出该堆位值为当前堆位的西侧堆位
                    { //西侧堆位占用情况
                        Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());//读取当前堆位的西侧堆位
                        vt.east = vt1;
                        int c1 = 0, c2 = 0;
                        List<Block> occupiedby1 = new List<Block>();
                        List<Block> occupiedby2 = new List<Block>();


                        for (int k = 0; k < occupiedTable.Rows.Count; k++)
                        {
                            if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())//占用状态表的第k行第2列
                            {
                                Block bk1 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                occupiedby1.Add(bk1);
                                c1 = occupiedby1.Count;//当前堆位上的分段个数
                            }
                            if (tempTable.Rows[i]["西堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                            {

                                Block bk2 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                occupiedby2.Add(bk2);
                                c2 = occupiedby2.Count;//西侧堆位上的分段个数

                                if (c2 == 2)//西侧堆位上有2个分段
                                {
                                    Block bk21 = occupiedby2.ElementAt(0);
                                    Block bk22 = occupiedby2.ElementAt(1);

                                    DateTime dt21 = DateTime.Parse(bk21.dt);
                                    DateTime dt22 = DateTime.Parse(bk22.dt);
                                    System.TimeSpan DT21 = dt21 - Today;
                                    System.TimeSpan DT22 = dt22 - Today;
                                    int getDay21 = (int)DT21.TotalDays;
                                    int getDay22 = (int)DT22.TotalDays;

                                    if (getDay21 > getDay22)//选择离场时间较晚的那个分段用于计算
                                    {
                                        getDay2 = getDay21;
                                    }
                                    else
                                    {
                                        getDay2 = getDay22;
                                    }
                                }
                                else if (c2 == 1)//西侧堆位上只有一个分段
                                {
                                    DateTime dt2 = DateTime.Parse(bk2.dt);
                                    System.TimeSpan DT2 = dt2 - Today;
                                    getDay2 = (int)DT2.TotalDays;
                                }

                            }
                        }

                        if (c1 == 0 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1; Weight[i, 2] = 10000; }
                        if (c1 == 0 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 501; ; Weight[i, 2] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }
                        if (c1 == 0 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 2] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }
                        if (c1 == 1 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 501; Weight[i, 2] = 90000; }//当前堆位被占用，权重为无穷大，设为900000
                        if (c1 == 1 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 2] = 90000; }
                        if (c1 == 1 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 2] = 90000; }
                        if (c1 == 2 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 2] = 90000; }
                        if (c1 == 2 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 2] = 90000; }
                        if (c1 == 2 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 2001; Weight[i, 2] = 90000; }

                    }
                    else if (tempTable.Rows[i]["西堆位ID"].ToString() == "R") { Weight[i, 2] = 10000; }//堆场西侧边界可通行
                    else if (tempTable.Rows[i]["西堆位ID"].ToString() == "S") { Weight[i, 2] = 50000; }//堆场西侧边界不通行
                    //以上是当前堆位与其西侧堆位的关系，得到西侧分权重值



                    if (tempTable.Rows[i]["北堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString())//若第i+1行第10列的堆位与第j+1行第2列堆位相同，读出该堆位值为当前堆位的北侧堆位
                    { //北侧堆位占用情况
                        Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());//读取当前堆位的北侧堆位
                        vt.east = vt1;
                        int c1 = 0, c2 = 0;
                        List<Block> occupiedby1 = new List<Block>();
                        List<Block> occupiedby2 = new List<Block>();


                        for (int k = 0; k < occupiedTable.Rows.Count; k++)
                        {
                            if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())//占用状态表的第k行第2列
                            {

                                Block bk1 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                occupiedby1.Add(bk1);

                                c1 = occupiedby1.Count;//当前堆位上的分段个数为c1
                            }
                            if (tempTable.Rows[i]["北堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                            {

                                Block bk2 = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                occupiedby2.Add(bk2);
                                c2 = occupiedby2.Count;//北侧堆位上的分段个数为c2

                                if (c2 == 2)
                                {
                                    Block bk21 = occupiedby2.ElementAt(0);
                                    Block bk22 = occupiedby2.ElementAt(1);

                                    DateTime dt21 = DateTime.Parse(bk21.dt);
                                    DateTime dt22 = DateTime.Parse(bk22.dt);
                                    System.TimeSpan DT21 = dt21 - Today;
                                    System.TimeSpan DT22 = dt22 - Today;
                                    int getDay21 = (int)DT21.TotalDays;
                                    int getDay22 = (int)DT22.TotalDays;

                                    if (getDay21 > getDay22)//选择离场时间较晚的分段用于计算
                                    {
                                        getDay2 = getDay21;
                                    }
                                    else
                                    {
                                        getDay2 = getDay22;
                                    }

                                }
                                else if (c2 == 1)//若北侧堆位上只有一个分段
                                {
                                    DateTime dt2 = DateTime.Parse(bk2.dt);
                                    System.TimeSpan DT2 = dt2 - Today;
                                    getDay2 = (int)DT2.TotalDays;
                                }
                            }
                        }

                        if (c1 == 0 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1; Weight[i, 3] = 10000; }
                        if (c1 == 0 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 501; ; Weight[i, 3] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }
                        if (c1 == 0 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 3] = (int)(getDay0 - getDay2) * (int)(getDay0 - getDay2); }
                        if (c1 == 1 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 501; Weight[i, 3] = 90000; }//当前堆位被占用，权重为无穷大，设为900000
                        if (c1 == 1 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 3] = 90000; }
                        if (c1 == 1 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 3] = 90000; }
                        if (c1 == 2 && c2 == 0) { dismatrix[i, j] = dismatrix[j, i] = 1001; Weight[i, 3] = 90000; }
                        if (c1 == 2 && c2 == 1) { dismatrix[i, j] = dismatrix[j, i] = 1501; Weight[i, 3] = 90000; }
                        if (c1 == 2 && c2 == 2) { dismatrix[i, j] = dismatrix[j, i] = 2001; Weight[i, 3] = 90000; }

                    }
                    else if (tempTable.Rows[i]["北堆位ID"].ToString() == "R") { Weight[i, 3] = 10000; }//堆场北侧边界可通行
                    else if (tempTable.Rows[i]["北堆位ID"].ToString() == "S") { Weight[i, 3] = 50000; }//堆场北侧边界不通行
                    //以上是当前堆位与其北侧堆位的关系，得到北侧分权重值
                }
            }//以上是堆位时间权重计算步骤






            //以下是输出初始化的堆位权重表WeightTable。这个表包含当前堆场上所有的堆位（即空堆位+被占用的堆位）。
            DataTable WeightTable = tempTable.Clone();
            WeightTable.Clear();

            string filepath = exePath + "\\" + yid;//文件存储和读取的相对位置


            System.IO.StreamWriter paOut = new System.IO.StreamWriter(@"" + filepath + "阻挡分段_堆位权重值.txt", false, System.Text.Encoding.Default);

            int[] WW = new int[1000];//设个足够大的数组容量，对应存放空堆位的权重值
            int ii = 0;

            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    W[i] = W[i] + Weight[i, j];
                    paOut.WriteLine("场地ID:" + yid + "-" + "堆位ID:" + tempTable.Rows[i]["堆位ID"].ToString() + "-" + Weight[i, j]);
                }
                paOut.WriteLine("权重值为：" + W[i]);
                paOut.WriteLine(); //下次循环前空一行


                DataRow weight0 = tempTable.Rows[i];
                WeightTable.Rows.Add(weight0.ItemArray);//一行行写入WeightTable这个表中,包含堆位ID和堆位时间权重值等信息

                DataColumn weightColumn = new DataColumn();
                weightColumn.DataType = typeof(double);//列的类型
                weightColumn.ColumnName = "堆位时间权重";//列名字
                weightColumn.DefaultValue = W[i].ToString();
                if (WeightTable.Columns.Contains("堆位时间权重"))//这里没有判断的话就会列“XXXX”已属于此 DataTable报错
                {
                    WeightTable.Rows[i]["堆位时间权重"] = W[i];//WeightTable这个表中的“堆位时间权重”那一列的值等于堆位时间权重值
                }
                else
                {
                    WeightTable.Columns.Add(weightColumn);
                }

                WeightTable.DefaultView.Sort = "堆位时间权重 asc";//对所有堆位按照时间权重值排序
            }
            paOut.Flush();
            paOut.Close();




            //这一段是只挑选出当前堆场的空堆位，存入Vacant_vertex0这个表中
            DataTable Vacant_vertex0 = new DataTable();
            Vacant_vertex0 = WeightTable.Clone();
            Vacant_vertex0.Clear();
            DataRow[] vacant00 = WeightTable.Select("[堆位时间权重]<90000");//寻找所有堆位中的空堆位.WeightTable里面有堆位权重值那一列。
            DataTable Vacant0 = WeightTable.Clone();
            for (int i = 0; i < vacant00.Length; i++)
            {
                Vacant_vertex0.ImportRow(vacant00[i]);//导入空堆位；
            }



            //建立空堆位集合,并写入空堆位的权重值

            //DataRow[] vacant11 = Vacant_vertex0.Select("[场地等级]='A'");//寻找A类堆位中的空堆位.WeightTable里面有堆位权重值那一列。
            DataTable Vacant1 = Vacant_vertex0.Clone();//新建Vacant_vertex0这个datateble，保留tempTable的格式
            Vacant1.Clear();
            for (int i = 0; i < vacant00.Length; i++)
            {
                Vacant1.ImportRow(vacant00[i]);//导入所有的A、B、C类的空堆位；
            }


            DataTable Vacant_vertex1 = new DataTable();
            Vacant_vertex1 = Vacant_vertex0.Clone();
            Vacant_vertex1.Clear();

            for (int i = 0; i < Vacant1.Rows.Count; i++)
            {
                int order = 0;
                for (int j = 0; j < occupiedTable.Rows.Count; j++)
                {
                    if (Vacant1.Rows[i]["堆位ID"] == occupiedTable.Rows[j]["堆位ID"])
                    {
                        order = order + 1;//跳过被占用的堆位
                    }
                }
                if (order == 0)
                {
                    DataRow vacant12 = Vacant1.Rows[i];
                    Vacant_vertex1.Rows.Add(vacant12.ItemArray);//加入一行空堆位信息
                    WW[ii++] = W[i];
                }
            }


            /*
            if (Vacant_vertex1.Rows.Count == 0)//若不存在A类的空堆位
            {
                DataRow[] vacant21 = Vacant_vertex0.Select("[场地等级]='B'");//寻找B类道路堆场中的空堆位
                DataTable Vacant2 = Vacant_vertex0.Clone();//新建Vacant_vertex这个datateble，保留WeightTable的格式
                Vacant2.Clear();
                for (int i = 0; i < vacant21.Length; i++)
                {
                    Vacant2.ImportRow(vacant21[i]);//导入空堆位；
                }

                Vacant_vertex1 = Vacant_vertex0.Clone();
                Vacant_vertex1.Clear();

                for (int i = 0; i < Vacant2.Rows.Count; i++)
                {
                    int order = 0;

                    for (int j = 0; j < occupiedTable.Rows.Count; j++)
                    {
                        if (Vacant2.Rows[i]["堆位ID"] == occupiedTable.Rows[j]["堆位ID"])
                        {
                            order = order + 1;
                        }
                    }
                    if (order == 0)
                    {
                        DataRow vacant22 = Vacant2.Rows[i];
                        Vacant_vertex1.Rows.Add(vacant22.ItemArray);//加入一行空堆位信息
                        WW[ii++] = W[i];

                    }
                }
            }





            if (Vacant_vertex1.Rows.Count == 0)//若不存在A类和B类的空堆位
            {
                DataRow[] vacant31 = Vacant_vertex0.Select("[场地等级]='C'");//寻找C类道路堆场中的空堆位
                DataTable Vacant3 = Vacant_vertex0.Clone();//新建Vacant_vertex这个datateble，保留WeightTable的格式
                Vacant3.Clear();
                for (int i = 0; i < vacant31.Length; i++)
                {
                    Vacant3.ImportRow(vacant31[i]);//导入空堆位；
                }

                Vacant_vertex1 = Vacant_vertex0.Clone();
                Vacant_vertex1.Clear();

                for (int i = 0; i < Vacant3.Rows.Count; i++)
                {
                    int order = 0;

                    for (int j = 0; j < occupiedTable.Rows.Count; j++)
                    {
                        if (Vacant3.Rows[i]["堆位ID"] == occupiedTable.Rows[j]["堆位ID"])
                        {
                            order = order + 1;//跳过被占用的堆位，即跳过非空堆位所在行的信息
                        }
                    }
                    if (order == 0)
                    {
                        DataRow vacant32 = Vacant3.Rows[i];
                        Vacant_vertex1.Rows.Add(vacant32.ItemArray);//加入一行空堆位信息
                        WW[ii++] = W[i];
                    }
                }
            }
            */





            //输出空堆位以及空堆位权重值到一个叫Vacant_vertex1的datatable。
            Vacant_vertex1.DefaultView.Sort = "场地等级 asc";//对空堆位按照场地等级排序
            //Vacant_vertex1.DefaultView.Sort = "堆位时间权重 asc";
            DataTable Vacant0_Sort = Vacant_vertex1.DefaultView.ToTable();





            //输出修正后的距离矩阵
            System.IO.StreamWriter vxOut = new System.IO.StreamWriter(@"" + filepath + "阻挡分段_堆位距离矩阵.txt", false, System.Text.Encoding.Default);

            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                string id = "";
                Vertex vacant_vertex = new Vertex(id);

                for (int q = 0; q < Vacant0_Sort.Rows.Count; q++)
                {
                    if (Vacant0_Sort.Rows[q]["堆位ID"].ToString() == tempTable.Rows[i]["堆位ID"].ToString())
                    {
                        vacant_vertex.id = tempTable.Rows[i]["堆位ID"].ToString();

                        for (int j = 0; j < tempTable.Rows.Count; j++)
                        {
                            if (tempTable.Rows[i]["东堆位ID"].ToString() == "R" || tempTable.Rows[i]["南堆位ID"].ToString() == "R" || tempTable.Rows[i]["西堆位ID"].ToString() == "R" || tempTable.Rows[i]["北堆位ID"].ToString() == "R")
                            {
                                if (tempTable.Rows[j]["东堆位ID"].ToString() == "R" || tempTable.Rows[j]["南堆位ID"].ToString() == "R" || tempTable.Rows[j]["西堆位ID"].ToString() == "R" || tempTable.Rows[j]["北堆位ID"].ToString() == "R")
                                {
                                    if (i != j)
                                    {
                                        dismatrix[i, j] = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < tempTable.Rows.Count; j++)
                {
                    vxOut.Write(dismatrix[i, j] + "    ");
                }
                vxOut.WriteLine();
            }
            vxOut.Flush();
            vxOut.Close();


            //在所有空堆位里面，按照时间权重值从小到大选择目标堆位
            for (int q = 1; q < Vacant0_Sort.Rows.Count; q++)//从1开始，不包括道路
            {
                Vertex Vertex_end = new Vertex(Vacant0_Sort.Rows[q]["堆位ID"].ToString());//终点堆位的堆位ID。在候选空堆位集中找。
                Console.WriteLine("目标堆位候选" + q + ":" + Vacant0_Sort.Rows[q]["堆位ID"].ToString());
                v_end = Vertex_end;




                //dijkstra最短路径算法.
                int[,] dis = new int[tempTable.Rows.Count, tempTable.Rows.Count];
                for (int i = 0; i < tempTable.Rows.Count; i++)
                {
                    for (int j = 0; j < tempTable.Rows.Count; j++)
                    {
                        dis[i, j] = dismatrix[i, j];
                    }
                }


                ArrayList ways = new ArrayList();
                for (int i = 0; i < tempTable.Rows.Count; i++)  //有row个点，则从中心到各点的路有row-1条
                {
                    ArrayList w = new ArrayList();
                    int j = 0;
                    w.Add(j);
                    ways.Add(w);
                }


                ArrayList S = new ArrayList();
                ArrayList Sr = new ArrayList();
                int[] Indexof_distance = new int[tempTable.Rows.Count];



                for (int i = 0; i < tempTable.Rows.Count; i++)
                {
                    Indexof_distance[i] = i;
                }


                S.Add(Indexof_distance[0]);

                for (int i = 0; i < tempTable.Rows.Count; i++)
                {
                    Sr.Add(Indexof_distance[i]);
                }
                Sr.RemoveAt(0);

                int[] D = new int[tempTable.Rows.Count]; //存放中心点到每个点的距离
                //以上是初始化。S和Sr(里边放的都是点的编号)



                //定义阻挡分段所在的当前堆位为起点，初始化start=0
                int start = 0;
                for (int i = 0; i < occupiedTable.Rows.Count; i++)
                {
                    if (occupiedTable.Rows[i]["分段ID"].ToString() == bk0.id)//阻挡分段所在的堆位id.
                    {
                        v_start.id = occupiedTable.Rows[i]["堆位ID"].ToString();
                    }

                }

                for (int j = 0; j < tempTable.Rows.Count; j++)
                {
                    if (tempTable.Rows[j]["堆位ID"].ToString() == v_start.id)//阻挡分段所在的堆位id
                    {
                        start = Int16.Parse(tempTable.Rows[j]["堆位Name"].ToString());//start赋值为当前堆位的编号（优先考虑场内直达目标堆位的方式）
                    }
                }

                int Count = tempTable.Rows.Count - 1;
                while (Count > 0)
                {
                    //假定中心点的编号是start的贪吃法求路径
                    for (int i = 0; i < tempTable.Rows.Count; i++)//起始点编号的V0。以目标堆位为起始点，从阻挡分段当前位置向目标堆位靠拢。
                    { D[i] = dismatrix[start, i]; }

                    int min_num = (int)Sr[0];  //距中心点的最小距离点编号。Sr[]存放除了中心点以外的点的编号。
                    foreach (int s in Sr)
                    {
                        if (D[s] < D[min_num]) min_num = s;
                    }

                    S.Add(min_num);//把点从Sr里面移到S里；
                    Sr.Remove(min_num);//把最新包含进来的点也加到路径中
                    ((ArrayList)ways[min_num]).Add(min_num);

                    foreach (int element in Sr)
                    {
                        int current_position = element;
                        bool exchange = false;      //有交换标志

                        if (D[element] < D[min_num] + dismatrix[min_num, current_position])
                            D[element] = D[element];
                        else
                        {
                            D[element] = dismatrix[min_num, current_position] + D[min_num];
                            exchange = true;
                        }
                        //修改距离矩阵                   
                        dismatrix[start, element] = D[element];
                        current_position = element;
                        dismatrix[start, current_position] = D[element];

                        //修改路径---------------
                        if (exchange == true)
                        {
                            ((ArrayList)ways[element]).Clear();
                            foreach (int point in (ArrayList)ways[min_num])
                                ((ArrayList)ways[element]).Add(point);//加了新点之后距离更短
                        }
                    }
                    --Count;
                }
                //以上是dijkstra最短路径算法（场内直达的寻路方式）。下面是输出路径结果。



                //选择堆位权重值最小的堆位为目标堆位，通过距离矩阵dismatrix计算堆场内移动的最短路径，生成路径.txt
                System.IO.StreamWriter rdOut = new System.IO.StreamWriter(@"" + filepath + "阻挡分段移动_路径.txt", false, System.Text.Encoding.Default);
                foreach (ArrayList mother in ways)
                {
                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        if (tempTable.Rows[i]["堆位ID"].ToString() == v_end.id)//v_end由堆位权重值得到；
                        {
                            if (mother[mother.Count - 1].ToString() == tempTable.Rows[i]["堆位Name"].ToString())//mother路径的最后一个值是目标堆位的编号
                            {
                                foreach (int child in mother)//输出mother这条路径里的每个节点
                                {
                                    string vp;
                                    for (int j = 0; j < tempTable.Rows.Count; j++)//输出路径结果
                                    {
                                        object obj = tempTable.Rows[j]["堆位Name"];
                                        int x = Convert.ToInt32(obj);
                                        if (child == x)//匹配每个child节点与tempTable里的堆位Name
                                        {
                                            vp = tempTable.Rows[j]["堆位ID"].ToString();
                                            rdOut.Write("{0}-", vp);
                                        }
                                    }
                                }


                                List<Block> occupiedby = new List<Block>();
                                for (int k = 0; k < occupiedTable.Rows.Count; k++)
                                {
                                    if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                                    {
                                        Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                        occupiedby.Add(bk);
                                    }
                                }
                                int c = occupiedby.Count;

                                if (c.Equals(0)) rdOut.WriteLine("   mobility:{0}", (float)dismatrix[start, (int)mother[mother.Count - 1]] / 1000);
                                if (c.Equals(1)) rdOut.WriteLine("   mobility:{0}", (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000);
                                if (c.Equals(2)) rdOut.WriteLine("   mobility:{0}", (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000);


                                switch (c)
                                {
                                    case 0: mobility0 = (float)dismatrix[start, (int)mother[mother.Count - 1]] / 1000;
                                        break;
                                    case 1: mobility0 = (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000;
                                        break;
                                    case 2: mobility0 = (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000;
                                        break;
                                }


                                Console.WriteLine("mobility0:" + mobility0);
                                rdOut.Flush();
                                rdOut.Close();
                            }
                        }
                    }
                }




                if (mobility0 < 1)//mobility0 < 1表示阻挡分段在堆场内部，由当前堆位无障碍地到达目标堆位
                {
                    string text0 = System.IO.File.ReadAllText(@"" + filepath + "阻挡分段移动_路径.txt");
                    string pa0 = null;
                    string[] sArray0 = text0.Split('-');
                    for (int i = 1; i < sArray0.Length - 2; i++)//从1开始
                    {
                        pa0 = pa0 + sArray0[i] + "-";
                    }

                    output_str = output_str + bk0.id + "/" + pa0 + sArray0[sArray0.Length - 2] + "*" + bk0.position + "\n"; //输出的字符串是（分段ID，场内路径，分段放置位置）
                    Console.WriteLine(output_str);
                    File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p1.txt", "");//将两个txt文件清空（即路径p1，路径p2）
                    File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p2.txt", "");

                    break;//跳出所有循环，直接输出output_str
                }
                //以上是无障碍到达的情况——场内直达的寻路方式




                else //若场内移动的路径上还存在至少1个阻挡分段，则采用先出场，再入场的方式到达目标堆位
                {
                    File.WriteAllText(@"" + filepath + "阻挡分段移动_路径.txt", "");//将"路径.txt"文件清空

                    //以下是采用先出场，再入场的方式到达目标堆位的最短路径算法
                    int[,] dis_2 = new int[tempTable.Rows.Count, tempTable.Rows.Count];
                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < tempTable.Rows.Count; j++)
                        {
                            dis_2[i, j] = dismatrix[i, j];
                        }
                    }

                    ArrayList ways_2 = new ArrayList();
                    for (int i = 0; i < tempTable.Rows.Count; i++)  //有row个点，则从中心到各点的路有row-1条
                    {
                        ArrayList w_2 = new ArrayList();
                        int j = 0;
                        w_2.Add(j);
                        ways_2.Add(w_2);
                    }


                    ArrayList S_2 = new ArrayList();
                    ArrayList Sr_2 = new ArrayList();
                    int[] Indexof_distance_2 = new int[tempTable.Rows.Count];



                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        Indexof_distance_2[i] = i;
                    }


                    S_2.Add(Indexof_distance_2[0]);

                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        Sr_2.Add(Indexof_distance_2[i]);
                    }
                    Sr_2.RemoveAt(0);

                    int[] D_2 = new int[tempTable.Rows.Count]; //存放中心点到每个点的距离


                    Count = tempTable.Rows.Count - 1;
                    while (Count > 0)
                    {
                        //假定中心点的编号是0的贪吃法求路径
                        for (int i = 0; i < tempTable.Rows.Count; i++)
                        { D_2[i] = dismatrix[0, i]; }

                        int min_num = (int)Sr_2[0];  //距中心点的最小距离点编号。Sr[]存放除了中心点以外的点的编号。
                        foreach (int s in Sr_2)
                        {
                            if (D_2[s] < D_2[min_num]) min_num = s;
                        }

                        S_2.Add(min_num);//把点从Sr里面移到S里；
                        Sr_2.Remove(min_num);//把最新包含进来的点也加到路径中
                        ((ArrayList)ways_2[min_num]).Add(min_num);

                        foreach (int element in Sr_2)
                        {
                            int current_position = element;
                            bool exchange = false;      //有交换标志

                            if (D_2[element] < D_2[min_num] + dismatrix[min_num, current_position])
                                D_2[element] = D_2[element];
                            else
                            {
                                D_2[element] = dismatrix[min_num, current_position] + D_2[min_num];
                                exchange = true;
                            }
                            //修改距离矩阵                   
                            dismatrix[0, element] = D_2[element];
                            current_position = element;
                            dismatrix[0, current_position] = D_2[element];

                            //修改路径---------------
                            if (exchange == true)
                            {
                                ((ArrayList)ways_2[element]).Clear();
                                foreach (int point in (ArrayList)ways_2[min_num])
                                    ((ArrayList)ways_2[element]).Add(point);//加了新点之后距离更短
                            }
                        }
                        --Count;
                    }
                    //以上是dijkstra最短路径算法（先出场再入场的寻路方式）。下面是输出路径结果。



                    //选择堆位权重值最小的堆位为目标堆位，通过距离矩阵dismatrix计算堆场内移动的最短路径，生成路径.txt
                    System.IO.StreamWriter rdOut2 = new System.IO.StreamWriter(@"" + filepath + "阻挡分段移动_路径p2.txt", false, System.Text.Encoding.Default);
                    foreach (ArrayList mother in ways_2)
                    {
                        for (int i = 0; i < tempTable.Rows.Count; i++)
                        {
                            if (tempTable.Rows[i]["堆位ID"].ToString() == v_end.id)//v_end由堆位权重值得到；
                            {
                                if (mother[mother.Count - 1].ToString() == tempTable.Rows[i]["堆位Name"].ToString())//mother路径的最后一个值是目标堆位的编号
                                {
                                    foreach (int child in mother)//输出mother这条路径里的每个节点
                                    {
                                        string vp;
                                        for (int j = 0; j < tempTable.Rows.Count; j++)//输出路径结果
                                        {
                                            object obj = tempTable.Rows[j]["堆位Name"];
                                            int x = Convert.ToInt32(obj);
                                            if (child == x)//匹配每个child节点与tempTable里的堆位Name
                                            {
                                                vp = tempTable.Rows[j]["堆位ID"].ToString();
                                                rdOut2.Write("{0}-", vp);
                                            }
                                        }
                                    }


                                    List<Block> occupiedby = new List<Block>();
                                    for (int k = 0; k < occupiedTable.Rows.Count; k++)
                                    {
                                        if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                                        {
                                            Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                            occupiedby.Add(bk);
                                        }
                                    }

                                    int c = occupiedby.Count;
                                    if (c.Equals(0)) rdOut2.WriteLine("   mobility:{0}", (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000);
                                    if (c.Equals(1)) rdOut2.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);
                                    if (c.Equals(2)) rdOut2.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);

                                    switch (c)
                                    {
                                        case 0: mobility2 = (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000;
                                            break;
                                        case 1: mobility2 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                            break;
                                        case 2: mobility2 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                            break;
                                    }

                                    Console.WriteLine("mobility2:" + mobility2);//入场路径上的移动度mobility2
                                }
                            }
                        }
                    }
                    rdOut2.Flush();
                    rdOut2.Close();


                    System.IO.StreamWriter rdOut1 = new System.IO.StreamWriter(@"" + filepath + "阻挡分段移动_路径p1.txt", false, System.Text.Encoding.Default);
                    foreach (ArrayList mother in ways_2)
                    {
                        for (int i = 0; i < tempTable.Rows.Count; i++)
                        {
                            if (tempTable.Rows[i]["堆位ID"].ToString() == bk0.pb)//阻挡分段的起始位置；
                            {
                                if (mother[mother.Count - 1].ToString() == tempTable.Rows[i]["堆位Name"].ToString())//mother路径的最后一个值是目标堆位的编号
                                {
                                    foreach (int child in mother)//输出mother这条路径里的每个节点
                                    {
                                        string vp = "";
                                        for (int j = 0; j < tempTable.Rows.Count; j++)//输出路径结果
                                        {
                                            object obj = tempTable.Rows[j]["堆位Name"];
                                            int x = Convert.ToInt32(obj);

                                            if (child == x)//匹配每个child节点与tempTable里的堆位Name
                                            {
                                                vp = tempTable.Rows[j]["堆位ID"].ToString();
                                                rdOut1.Write("{0}-", vp);
                                            }

                                        }
                                    }


                                    List<Block> occupiedby = new List<Block>();
                                    for (int k = 0; k < occupiedTable.Rows.Count; k++)
                                    {
                                        if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                                        {
                                            Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                            occupiedby.Add(bk);
                                        }
                                    }
                                    int c = occupiedby.Count;
                                    if (c.Equals(0)) rdOut1.WriteLine("   mobility:{0}", (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000);
                                    if (c.Equals(1)) rdOut1.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);
                                    if (c.Equals(2)) rdOut1.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);


                                    switch (c)
                                    {
                                        case 0: mobility1 = (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000;
                                            break;
                                        case 1: mobility1 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                            break;
                                        case 2: mobility1 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                            break;
                                    }
                                    Console.WriteLine("mobility1:" + mobility1);//阻挡分段出场路径上的移动度mobility1

                                }
                            }
                        }
                    }
                    rdOut1.Flush();
                    rdOut1.Close();


                    double mobility = mobility1 + mobility2;//阻挡分段到达目标堆位的整条路径上的移动度mobility = mobility1 + mobility2
                    Console.WriteLine("mobility:" + mobility);
                    if (mobility < 1)//若阻挡分段采用先出场再入场方式，可以无障碍到达目标堆位
                    {
                        string text1 = System.IO.File.ReadAllText(@"" + filepath + "阻挡分段移动_路径p1.txt");//输出出场路径到“路径p1”
                        string pa1 = null;
                        string[] sArray1 = text1.Split('-');
                        for (int i = sArray1.Length - 2; i > 0; i--)
                        {
                            pa1 = pa1 + sArray1[i] + "-";
                            //Console.WriteLine(pa1);
                        }


                        string text2 = System.IO.File.ReadAllText(@"" + filepath + "阻挡分段移动_路径p2.txt");//输出入场路径到“路径p2”
                        string pa2 = null;
                        string[] sArray2 = text2.Split('-');
                        for (int i = 0; i < sArray2.Length - 2; i++)//从0开始，包括R标志
                        {
                            pa2 = pa2 + sArray2[i] + "-";
                            //Console.WriteLine(pa2 + sArray2[sArray2.Length - 2]);
                        }

                        output_str = output_str + bk0.id + "/" + pa1 + pa2 + sArray2[sArray2.Length - 2] + "*" + bk0.position + "\n"; //输出的字符串output_str是（分段ID，场内路径，分段放置位置）
                        Console.WriteLine(output_str);

                        break;//跳出所有循环，直接输出output_str
                    }
                    //以上是无障碍到达的情况——先出场再入场的寻路方式


                    else//若不能无障碍到达，则清空“路径p1.txt”和“路径p2.txt”
                    {
                        File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p1.txt", "");
                        File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p2.txt", "");
                    }
                }


            }



            /*
            //读取三个txt文件的所有内容，供之后判断txt内容是否为空
            string[] txt_content0 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径.txt");
            string[] txt_content1 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p1.txt");
            string[] txt_content2 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p2.txt");
            */



            //下面是阻挡分段移动路径上再允许另外一个阻挡分段的情况
            if (output_str == "")//若所有的空堆位都不能无障碍到达，则放宽条件，容许阻挡分段移动的路径上还存在另外一个阻挡分段，重新寻找路径
            {
                Number_Block = 1;//表示阻挡分段移动的路径上允许有一个额外的阻挡分段；

                //在所有空堆位里面，按照时间权重值从小到大选择目标堆位
                for (int q = 1; q < Vacant0_Sort.Rows.Count; q++)//从1开始，不包括道路
                {
                    Vertex Vertex_end = new Vertex(Vacant0_Sort.Rows[q]["堆位ID"].ToString());//终点堆位的堆位ID。在候选空堆位集中找。

                    Console.WriteLine("目标堆位候选" + q + ":" + Vacant0_Sort.Rows[q]["堆位ID"].ToString());
                    v_end = Vertex_end;




                    //dijkstra最短路径算法（优先考虑场内路径到达目标堆位——允许一个额外的阻挡分段）.
                    int[,] dis = new int[tempTable.Rows.Count, tempTable.Rows.Count];
                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < tempTable.Rows.Count; j++)
                        {
                            dis[i, j] = dismatrix[i, j];
                        }
                    }


                    ArrayList ways = new ArrayList();
                    for (int i = 0; i < tempTable.Rows.Count; i++)  //有row个点，则从中心到各点的路有row-1条
                    {
                        ArrayList w = new ArrayList();
                        int j = 0;
                        w.Add(j);
                        ways.Add(w);
                    }


                    ArrayList S = new ArrayList();
                    ArrayList Sr = new ArrayList();
                    int[] Indexof_distance = new int[tempTable.Rows.Count];



                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        Indexof_distance[i] = i;
                    }


                    S.Add(Indexof_distance[0]);

                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        Sr.Add(Indexof_distance[i]);
                    }
                    Sr.RemoveAt(0);

                    int[] D = new int[tempTable.Rows.Count]; //存放中心点到每个点的距离
                    //以上是初始化。S和Sr(里边放的都是点的编号)




                    int start = 0;
                    for (int i = 0; i < occupiedTable.Rows.Count; i++)
                    {
                        if (occupiedTable.Rows[i]["分段ID"].ToString() == bk0.id)//阻挡分段所在的堆位id.
                        {
                            v_start.id = occupiedTable.Rows[i]["堆位ID"].ToString();
                        }

                    }//定义阻挡分段所在的当前堆位为起点start
                    for (int j = 0; j < tempTable.Rows.Count; j++)
                    {
                        if (tempTable.Rows[j]["堆位ID"].ToString() == v_start.id)//阻挡分段所在的堆位id
                        {
                            start = Int16.Parse(tempTable.Rows[j]["堆位Name"].ToString());//start赋值为当前堆位的编号
                        }
                    }

                    int Count = tempTable.Rows.Count - 1;
                    while (Count > 0)
                    {
                        //假定中心点的编号是start的贪吃法求路径
                        for (int i = 0; i < tempTable.Rows.Count; i++)//起始点编号的V0。以目标堆位为起始点，从阻挡分段当前位置向目标堆位靠拢。
                        { D[i] = dismatrix[start, i]; }

                        int min_num = (int)Sr[0];  //距中心点的最小距离点编号。Sr[]存放除了中心点以外的点的编号。
                        foreach (int s in Sr)
                        {
                            if (D[s] < D[min_num]) min_num = s;
                        }

                        S.Add(min_num);//把点从Sr里面移到S里；
                        Sr.Remove(min_num);//把最新包含进来的点也加到路径中
                        ((ArrayList)ways[min_num]).Add(min_num);

                        foreach (int element in Sr)
                        {
                            int current_position = element;
                            bool exchange = false;      //有交换标志

                            if (D[element] < D[min_num] + dismatrix[min_num, current_position])
                                D[element] = D[element];
                            else
                            {
                                D[element] = dismatrix[min_num, current_position] + D[min_num];
                                exchange = true;
                            }
                            //修改距离矩阵                   
                            dismatrix[start, element] = D[element];
                            current_position = element;
                            dismatrix[start, current_position] = D[element];

                            //修改路径---------------
                            if (exchange == true)
                            {
                                ((ArrayList)ways[element]).Clear();
                                foreach (int point in (ArrayList)ways[min_num])
                                    ((ArrayList)ways[element]).Add(point);//加了新点之后距离更短
                            }
                        }
                        --Count;
                    }
                    //以上是dijkstra最短路径算法（场内路径到达目标堆位——允许一个额外的阻挡分段）。
                    //下面是输出路径结果和输出移动度。



                    //选择堆位权重值最小的堆位为目标堆位，通过距离矩阵dismatrix计算堆场内移动的最短路径，生成路径.txt
                    System.IO.StreamWriter rdOut = new System.IO.StreamWriter(@"" + filepath + "阻挡分段移动_路径.txt", false, System.Text.Encoding.Default);
                    foreach (ArrayList mother in ways)
                    {
                        for (int i = 0; i < tempTable.Rows.Count; i++)
                        {
                            if (tempTable.Rows[i]["堆位ID"].ToString() == v_end.id)//v_end由堆位权重值得到；
                            {
                                if (mother[mother.Count - 1].ToString() == tempTable.Rows[i]["堆位Name"].ToString())//mother路径的最后一个值是目标堆位的编号
                                {
                                    foreach (int child in mother)//输出mother这条路径里的每个节点
                                    {
                                        string vp;
                                        for (int j = 0; j < tempTable.Rows.Count; j++)//输出路径结果
                                        {
                                            object obj = tempTable.Rows[j]["堆位Name"];
                                            int x = Convert.ToInt32(obj);
                                            if (child == x)//匹配每个child节点与tempTable里的堆位Name
                                            {
                                                vp = tempTable.Rows[j]["堆位ID"].ToString();
                                                rdOut.Write("{0}-", vp);
                                            }
                                        }
                                    }


                                    List<Block> occupiedby = new List<Block>();
                                    for (int k = 0; k < occupiedTable.Rows.Count; k++)
                                    {
                                        if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                                        {
                                            Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                            occupiedby.Add(bk);
                                        }
                                    }
                                    int c = occupiedby.Count;

                                    if (c.Equals(0)) rdOut.WriteLine("   mobility:{0}", (float)dismatrix[start, (int)mother[mother.Count - 1]] / 1000);
                                    if (c.Equals(1)) rdOut.WriteLine("   mobility:{0}", (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000);
                                    if (c.Equals(2)) rdOut.WriteLine("   mobility:{0}", (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000);


                                    switch (c)
                                    {
                                        case 0: mobility0 = (float)dismatrix[start, (int)mother[mother.Count - 1]] / 1000;
                                            break;
                                        case 1: mobility0 = (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000;
                                            break;
                                        case 2: mobility0 = (float)(dismatrix[start, (int)mother[mother.Count - 1]] - 500) / 1000;
                                            break;
                                    }


                                    Console.WriteLine("mobility0:" + mobility0);
                                    rdOut.Flush();
                                    rdOut.Close();
                                }
                            }
                        }
                    }
                    //上面是输出场内移动路径结果到“路径.txt”和输出移动度。



                    if (mobility0 < 2)//放宽条件，允许阻挡分段的移动路径还存在另外一个阻挡分段
                    {
                        string text0 = System.IO.File.ReadAllText(@"" + filepath + "阻挡分段移动_路径.txt");
                        string pa0 = null;
                        string[] sArray0 = text0.Split('-');
                        for (int i = 1; i < sArray0.Length - 2; i++)//从1开始
                        {
                            pa0 = pa0 + sArray0[i] + "-";
                        }

                        output_str = output_str + bk0.id + "/" + pa0 + sArray0[sArray0.Length - 2] + "*" + bk0.position + "\n"; //输出的字符串是（分段ID，场内路径，分段放置位置）
                        Console.WriteLine(output_str);
                        File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p1.txt", "");//清空两个文件“路径p1.txt”和“路径p2.txt”
                        File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p2.txt", "");

                        break;
                    }



                    else //若场内移动的路径上还存在至少2个阻挡分段，则采用场外绕行方式（即先出场再入场）
                    {
                        File.WriteAllText(@"" + filepath + "阻挡分段移动_路径.txt", "");//清空“路径.txt”


                        //下面是最短路径算法（采用先出场再入场方式——允许一个额外的阻挡分段）
                        int[,] dis_2 = new int[tempTable.Rows.Count, tempTable.Rows.Count];
                        for (int i = 0; i < tempTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < tempTable.Rows.Count; j++)
                            {
                                dis_2[i, j] = dismatrix[i, j];
                            }
                        }


                        ArrayList ways_2 = new ArrayList();
                        for (int i = 0; i < tempTable.Rows.Count; i++)  //有row个点，则从中心到各点的路有row-1条
                        {
                            ArrayList w_2 = new ArrayList();
                            int j = 0;
                            w_2.Add(j);
                            ways_2.Add(w_2);
                        }


                        ArrayList S_2 = new ArrayList();
                        ArrayList Sr_2 = new ArrayList();
                        int[] Indexof_distance_2 = new int[tempTable.Rows.Count];



                        for (int i = 0; i < tempTable.Rows.Count; i++)
                        {
                            Indexof_distance_2[i] = i;
                        }


                        S_2.Add(Indexof_distance_2[0]);

                        for (int i = 0; i < tempTable.Rows.Count; i++)
                        {
                            Sr_2.Add(Indexof_distance_2[i]);
                        }
                        Sr_2.RemoveAt(0);

                        int[] D_2 = new int[tempTable.Rows.Count]; //存放中心点到每个点的距离


                        Count = tempTable.Rows.Count - 1;
                        while (Count > 0)
                        {
                            //假定中心点的编号是start的贪吃法求路径
                            for (int i = 0; i < tempTable.Rows.Count; i++)//起始点编号的V0。以目标堆位为起始点，从阻挡分段当前位置向目标堆位靠拢。
                            { D_2[i] = dismatrix[0, i]; }

                            int min_num = (int)Sr_2[0];  //距中心点的最小距离点编号。Sr[]存放除了中心点以外的点的编号。
                            foreach (int s in Sr_2)
                            {
                                if (D_2[s] < D_2[min_num]) min_num = s;
                            }

                            S_2.Add(min_num);//把点从Sr里面移到S里；
                            Sr_2.Remove(min_num);//把最新包含进来的点也加到路径中
                            ((ArrayList)ways_2[min_num]).Add(min_num);

                            foreach (int element in Sr_2)
                            {
                                int current_position = element;
                                bool exchange = false;      //有交换标志

                                if (D_2[element] < D_2[min_num] + dismatrix[min_num, current_position])
                                    D_2[element] = D_2[element];
                                else
                                {
                                    D_2[element] = dismatrix[min_num, current_position] + D_2[min_num];
                                    exchange = true;
                                }
                                //修改距离矩阵                   
                                dismatrix[0, element] = D_2[element];
                                current_position = element;
                                dismatrix[0, current_position] = D_2[element];

                                //修改路径---------------
                                if (exchange == true)
                                {
                                    ((ArrayList)ways_2[element]).Clear();
                                    foreach (int point in (ArrayList)ways_2[min_num])
                                        ((ArrayList)ways_2[element]).Add(point);//加了新点之后距离更短
                                }
                            }
                            --Count;
                        }
                        //以上是dijkstra最短路径算法（采用先出场再入场方式——允许一个额外的阻挡分段）。
                        //下面是输出路径结果。



                        //选择堆位权重值最小的堆位为目标堆位，通过距离矩阵dismatrix计算堆场内移动的最短路径，生成路径.txt
                        System.IO.StreamWriter rdOut2 = new System.IO.StreamWriter(@"" + filepath + "阻挡分段移动_路径p2.txt", false, System.Text.Encoding.Default);
                        foreach (ArrayList mother in ways_2)
                        {
                            for (int i = 0; i < tempTable.Rows.Count; i++)
                            {
                                if (tempTable.Rows[i]["堆位ID"].ToString() == v_end.id)//v_end由堆位权重值得到；
                                {
                                    if (mother[mother.Count - 1].ToString() == tempTable.Rows[i]["堆位Name"].ToString())//mother路径的最后一个值是目标堆位的编号
                                    {
                                        foreach (int child in mother)//输出mother这条路径里的每个节点
                                        {
                                            string vp;
                                            for (int j = 0; j < tempTable.Rows.Count; j++)//输出路径结果
                                            {
                                                object obj = tempTable.Rows[j]["堆位Name"];
                                                int x = Convert.ToInt32(obj);
                                                if (child == x)//匹配每个child节点与tempTable里的堆位Name
                                                {
                                                    vp = tempTable.Rows[j]["堆位ID"].ToString();
                                                    rdOut2.Write("{0}-", vp);
                                                }
                                            }
                                        }


                                        List<Block> occupiedby = new List<Block>();
                                        for (int k = 0; k < occupiedTable.Rows.Count; k++)
                                        {
                                            if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                                            {
                                                Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                                occupiedby.Add(bk);
                                            }
                                        }

                                        int c = occupiedby.Count;
                                        if (c.Equals(0)) rdOut2.WriteLine("   mobility:{0}", (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000);
                                        if (c.Equals(1)) rdOut2.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);
                                        if (c.Equals(2)) rdOut2.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);

                                        switch (c)
                                        {
                                            case 0: mobility2 = (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000;
                                                break;
                                            case 1: mobility2 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                                break;
                                            case 2: mobility2 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                                break;
                                        }

                                        Console.WriteLine("mobility2:" + mobility2);//入场路径上的移动度mobility2

                                    }
                                }
                            }
                        }
                        rdOut2.Flush();
                        rdOut2.Close();


                        System.IO.StreamWriter rdOut1 = new System.IO.StreamWriter(@"" + filepath + "阻挡分段移动_路径p1.txt", false, System.Text.Encoding.Default);
                        foreach (ArrayList mother in ways_2)
                        {
                            for (int i = 0; i < tempTable.Rows.Count; i++)
                            {
                                if (tempTable.Rows[i]["堆位ID"].ToString() == bk0.pb)//阻挡分段的起始位置；
                                {
                                    if (mother[mother.Count - 1].ToString() == tempTable.Rows[i]["堆位Name"].ToString())//mother路径的最后一个值是目标堆位的编号
                                    {
                                        foreach (int child in mother)//输出mother这条路径里的每个节点
                                        {
                                            string vp = "";
                                            for (int j = 0; j < tempTable.Rows.Count; j++)//输出路径结果
                                            {
                                                object obj = tempTable.Rows[j]["堆位Name"];
                                                int x = Convert.ToInt32(obj);

                                                if (child == x)//匹配每个child节点与tempTable里的堆位Name
                                                {
                                                    vp = tempTable.Rows[j]["堆位ID"].ToString();
                                                    rdOut1.Write("{0}-", vp);
                                                }
                                            }
                                        }


                                        List<Block> occupiedby = new List<Block>();
                                        for (int k = 0; k < occupiedTable.Rows.Count; k++)
                                        {
                                            if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                                            {
                                                Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString(), occupiedTable.Rows[k]["分段预计出场时间"].ToString(), occupiedTable.Rows[k]["分段放置位置"].ToString());
                                                occupiedby.Add(bk);
                                            }
                                        }
                                        int c = occupiedby.Count;
                                        if (c.Equals(0)) rdOut1.WriteLine("   mobility:{0}", (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000);
                                        if (c.Equals(1)) rdOut1.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);
                                        if (c.Equals(2)) rdOut1.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);


                                        switch (c)
                                        {
                                            case 0: mobility1 = (float)dismatrix[0, (int)mother[mother.Count - 1]] / 1000;
                                                break;
                                            case 1: mobility1 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                                break;
                                            case 2: mobility1 = (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000;
                                                break;
                                        }
                                        Console.WriteLine("mobility1:" + mobility1);//出场路径上的移动度mobility1

                                    }
                                }
                            }
                        }
                        rdOut1.Flush();
                        rdOut1.Close();


                        double mobility = mobility1 + mobility2;//先出场再入场方式时，整条路径上的移动度mobility = mobility1 + mobility2
                        Console.WriteLine("mobility:" + mobility);
                        if (mobility < 2)//小于2表示允许阻挡分段的移动路径上存在另外一个新的阻挡分段
                        {
                            string text1 = System.IO.File.ReadAllText(@"" + filepath + "阻挡分段移动_路径p1.txt");//输出出场路径到“路径p1”
                            string pa1 = null;
                            string[] sArray1 = text1.Split('-');
                            for (int i = sArray1.Length - 2; i > 0; i--)
                            {
                                pa1 = pa1 + sArray1[i] + "-";
                                //Console.WriteLine(pa1);
                            }


                            string text2 = System.IO.File.ReadAllText(@"" + filepath + "阻挡分段移动_路径p2.txt");//输出入场路径到“路径p2”
                            string pa2 = null;
                            string[] sArray2 = text2.Split('-');
                            for (int i = 0; i < sArray2.Length - 2; i++)//从0开始，包括R
                            {
                                pa2 = pa2 + sArray2[i] + "-";
                                //Console.WriteLine(pa2 + sArray2[sArray2.Length - 2]);
                            }

                            output_str = output_str + bk0.id + "/" + pa1 + pa2 + sArray2[sArray2.Length - 2] + "*" + bk0.position + "\n"; //输出的字符串是（分段ID，场内路径，分段放置位置）
                            Console.WriteLine(output_str);

                            break;
                        }



                        else//若采用先出场再入场方式时整条路径上的额外阻挡分段个数大于等于2，则清空“路径p1”和“路径p2”
                        {
                            File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p1.txt", "");
                            File.WriteAllText(@"" + filepath + "阻挡分段移动_路径p2.txt", "");
                        }
                    }
                }//容许阻挡分段移动的路径上还存在另外一个阻挡分段

            }//以上是允许一个额外阻挡的情况


            //若输出的string为空，四种情况都不成立
            //即（1）不允许额外阻挡+场内直达；（2）不允许额外阻挡+先出场再入场；（3）允许一个额外阻挡+场内直达：（4）允许一个额外阻挡+先出场再入场 
            if (output_str == "")
            {
                Console.WriteLine("不存在满足条件的空堆位存放阻挡分段" + bk0.id);
            }



        //若四种情况中有一种成立，即输出的output_str不为空
            //拆分output_str这个字符串为四个部分：出场路径，入场路径，放置姿态，目标堆位

            else
            {
                //读取三个txt文件的所有内容，供之后判断txt内容是否为空
                string[] txt_content0 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径.txt");
                string[] txt_content1 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p1.txt");
                string[] txt_content2 = File.ReadAllLines(@"" + filepath + "阻挡分段移动_路径p2.txt");


                ////若场内路径不存在，则是采用先出场再入场的方式寻路（拆分为：出场路径str1，入场路径str2，放置姿态str3，目标堆位str4）
                if (txt_content0.Length == 0)
                {
                    //截取不定长度的字符串
                    string output_str1 = output_str;
                    int y1 = output_str1.IndexOf("/") + 1;
                    int z1 = output_str1.IndexOf("R" + yid);
                    string str1 = output_str1.Substring(y1, z1 - y1);//截取字符串“/”以后、R之前的部分，即出场路径."+2"输出结果会包括R2，没有"+2"的话就不包括R2。
                    Console.WriteLine(str1);

                    //截取不定长度的字符串
                    int y2 = output_str1.IndexOf("R" + yid) + 1;
                    int z2 = output_str1.IndexOf("*");
                    string str2 = output_str1.Substring(y2 + 2, z2 - y2 - 2);//截取字符串的R以后、“*”之前的部分，即入场路径
                    Console.WriteLine(str2);

                    //截取字符串的最后一位
                    string str3 = output_str1.Substring(output_str1.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                    Console.WriteLine(str3);

                    //截取固定长度的字符串的第一种方法
                    int z4 = output_str1.IndexOf("*");
                    string str4 = output_str1.Substring(z4 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                    Console.WriteLine(str4);


                    /* //截取固定长度的字符串的第二种方法
                    int y4 = output_str1.IndexOf("R" + yid) + 1;
                    int z4 = output_str1.IndexOf("*");
                    string str4 = output_str1.Substring(y4 + 2, z4 - y4 - 2);//截取字符串“R”以后、"*"之前的部分，即目标堆位
                    Console.WriteLine(str4);
                    */

                    string path_b1 = str1;
                    string path_b2 = str2;
                    string blockpo_b = str3;
                    string de_cell = str4;


                    //分析路径上哪个分段是额外的阻挡分段
                    string path1 = str1;
                    string path2 = str2;
                    string path_out = str1 + str2;//将两段路径组合起来
                    Console.WriteLine(path_out);

                    //路径上的堆位集合
                    string[] vertex_set = path_out.Split('-');
                    for (int i1 = vertex_set.Length - 1; i1 > 0; i1--)
                    {
                        for (int o1 = 0; o1 < occupiedTable.Rows.Count; o1++)
                        {
                            if (occupiedTable.Rows[o1]["堆位ID"].ToString() == vertex_set[i1])//判断路径上的堆位集合中的堆位是否是占用信息表的堆位ID，如果是，则表示这个堆位上的分段是额外的阻挡分段
                            {
                                o_second = o1;
                                //返回出额外的阻挡分段的ID
                                bk_second = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                vx_second = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                            }

                        }
                    }

                }
                //以上是采用的先出场再入场的方式寻路，识别阻挡分段移动路径上额外的阻挡分段（可为0），并拆分output_str，传递给主程序



                else//场内路径不为空，即存在场内路径（拆分为：出场路径str5，入场路径str6，放置姿态str7，目标堆位str8）
                {
                    string output_str0 = output_str;
                    int y5 = output_str0.IndexOf("/") + 1;
                    int z5 = output_str0.IndexOf("*");
                    string str5 = output_str0.Substring(y5, z5 - y5);//截取字符串“/”以后、*之前的部分，即堆场内的路径
                    Console.WriteLine(str5);

                    string str6 = null;//不存在第二段，因此为空

                    string str7 = output_str0.Substring(output_str0.Length - 2, 1);//截取字符串的最后一位，即放置姿态
                    Console.WriteLine(str7);


                    int z8 = output_str0.IndexOf("*");
                    string str8 = output_str0.Substring(z8 - 5, 5);//截取字符串"*"之前5位的部分，即目标堆位
                    Console.WriteLine(str8);

                    string path_b1 = str5;
                    string path_b2 = str6;
                    string blockpo_b = str7;
                    string de_cell = str8;


                    //分析路径上哪个分段是额外的阻挡分段
                    string path5 = str5;

                    string path_in = str5;
                    Console.WriteLine(path_in);

                    //路径上的堆位集合
                    string[] vertex_set = path_in.Split('-');
                    for (int i1 = vertex_set.Length - 1; i1 > 0; i1--)
                    {
                        for (int o1 = 0; o1 < occupiedTable.Rows.Count; o1++)
                        {
                            if (occupiedTable.Rows[o1]["堆位ID"].ToString() == vertex_set[i1])//判断路径上的堆位集合中的堆位是否是占用信息表的堆位ID，如果是，则表示这个堆位上的分段是额外的阻挡分段
                            {
                                bk_second = new Block(occupiedTable.Rows[o1]["分段ID"].ToString(), occupiedTable.Rows[o1]["堆位ID"].ToString());
                                vx_second = new Vertex(occupiedTable.Rows[o1]["堆位ID"].ToString());
                                string xxxx = bk_second.id;
                                string xxxxx = vx_second.id;
                            }
                        }
                    }
                }
                //以上是采用场内直达的方式寻路，识别阻挡分段移动路径上额外的阻挡分段（可为0），并拆分output_str，传递给主程序

            }
            //以上是out_str不为空时，拆分output_str这个字符串为四个部分：出场路径，入场路径，放置姿态，目标堆位


            //conn.Close();
        }




        internal void getConn()
        {
            throw new NotImplementedException();
        }
    }
}





