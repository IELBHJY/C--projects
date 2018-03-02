using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing.Drawing2D;
using System.Collections;
using System.IO;
using System.Reflection;

namespace shipbuilding_yard
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        private static string exepath()
        {
            string exePath = System.Windows.Forms.Application.ExecutablePath;
            int index;
            for (int i = 0; i < 4; i++)
            {
                index = exePath.LastIndexOf("\\");
                exePath = exePath.Substring(0, index);
            }
            return exePath;
        }

        private static string exep = exepath();
        private OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exep + @"\input database.accdb");
        private OleDbDataAdapter da1;
        private OleDbDataAdapter da2;
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        DataTable dt4 = new DataTable();
        OleDbConnection connew = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + exep + @"\output.xlsx;" + "Extended Properties=\"Excel 12.0;HDR=False;IMEX=1;\"");
        private OleDbDataAdapter danew;
        DataTable dtnew = new DataTable();
        private int f, f2;
        private string zzpt = "总组平台-", tzcj = "涂装车间-", pzcj = "平直车间-", yxzcj = "预舾装车间-", qmcj = "曲面车间-", ehpt = "8号平台-";
        Bitmap bm5 = new Bitmap(232, 274);
        Bitmap bm3 = new Bitmap(211, 253);
        Bitmap bm2 = new Bitmap(64, 106);
        Bitmap bm1 = new Bitmap(1200, 900);
        Bitmap bm4 = new Bitmap(169, 64);
        Bitmap bm6 = new Bitmap(463, 64);
        Bitmap bm7 = new Bitmap(316, 64);
        Bitmap bm8 = new Bitmap(221, 40);
        Bitmap bm9 = new Bitmap(57, 253);
        Bitmap bm10 = new Bitmap(55, 176);
        Bitmap bm11 = new Bitmap(463, 64);
        Bitmap bm121 = new Bitmap(298, 167);
        Bitmap bm131 = new Bitmap(100, 100);
        Bitmap bm52 = new Bitmap(232, 274);
        Bitmap bm32 = new Bitmap(211, 253);
        Bitmap bm22 = new Bitmap(64, 106);
        Bitmap bm12 = new Bitmap(1200, 900);
        Bitmap bm42 = new Bitmap(169, 64);
        Bitmap bm62 = new Bitmap(463, 64);
        Bitmap bm72 = new Bitmap(316, 64);
        Bitmap bm82 = new Bitmap(221, 40);
        Bitmap bm92 = new Bitmap(57, 253);
        Bitmap bm102 = new Bitmap(55, 176);
        Bitmap bm112 = new Bitmap(463, 64);
        Bitmap bm53 = new Bitmap(232, 274);
        Bitmap bm33 = new Bitmap(211, 253);
        Bitmap bm23 = new Bitmap(64, 106);
        Bitmap bm13 = new Bitmap(1200, 900);
        Bitmap bm43 = new Bitmap(169, 64);
        Bitmap bm63 = new Bitmap(463, 64);
        Bitmap bm73 = new Bitmap(316, 64);
        Bitmap bm83 = new Bitmap(221, 40);
        Bitmap bm93 = new Bitmap(57, 253);
        Bitmap bm103 = new Bitmap(55, 176);
        Bitmap bm113 = new Bitmap(463, 64);

        private void button15_Click_1(object sender, EventArgs e)
        {
            //读数据库
            conn.Open();
            //da1 = new OleDbDataAdapter("select * from 当前全局堆场占用信息", conn);
            //da2 = new OleDbDataAdapter("select * from 堆位信息", conn);
            //DataTable dt1 = new DataTable();
            //DataTable dt2 = new DataTable();
            //da1.Fill(dt1);
            //da2.Fill(dt2);

            //输入初始信息
            //label5初始信息
            Bitmap bm52 = new Bitmap(bm5);
            Graphics g5 = Graphics.FromImage(bm5);
            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                for (int n = 0; n < dt1.Rows.Count; n++)
                {

                    if (dt1.Rows[n]["堆位ID"].Equals(dt2.Rows[m]["堆位ID"]))
                    {
                        if (Convert.ToInt32(dt2.Rows[m]["场地ID"]) == 3)
                        {
                            if (!Convert.IsDBNull(dt2.Rows[m]["堆场内位置序号"]))
                            {

                                int k = Convert.ToInt32(dt2.Rows[m]["堆场内位置序号"]);
                                int i = k / 11; int j = k % 11;
                                Brush b = new SolidBrush(Color.Green);
                                g5.FillRectangle(b, 21 * j + 1, 21 * i + 1, 20, 20);

                            }
                        }
                    }
                }
            }
            label5.Image = bm52;
            //label3的初始信息
            Bitmap bm32 = new Bitmap(bm3);
            Graphics g3 = Graphics.FromImage(bm3);
            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                for (int n = 0; n < dt1.Rows.Count; n++)
                {
                    if (Convert.ToInt32(dt2.Rows[m]["场地ID"]) == 2)
                    {
                        if (dt1.Rows[n]["堆位ID"].Equals(dt2.Rows[m]["堆位ID"]))
                        {
                            if (!Convert.IsDBNull(dt2.Rows[m]["堆场内位置序号"]))
                            {

                                int k = Convert.ToInt32(dt2.Rows[m]["堆场内位置序号"]);
                                int i = k / 10; int j = k % 10;

                                Brush b = new SolidBrush(Color.Green);
                                g3.FillRectangle(b, 21 * j + 1, 21 * i + 1, 20, 20);


                            }
                        }
                    }
                }
            }
            label3.Image = bm32;
            conn.Close();

        }


        public int ctpos(int x)
        {
            int z;
            z = x * 21 + 11;
            return z;
        }
        //路口点坐标
        public int[] lk(int x)
        {
            int[] arr = new int[2];
            switch (x)
            {
                case 1:
                    arr[0] = 221;
                    arr[1] = 22;
                    break;
                case 2:
                    arr[0] = 221;
                    arr[1] = 591;
                    break;
                case 3:
                    arr[0] = 316;
                    arr[1] = 22;
                    break;
                case 4:
                    arr[0] = 316;
                    arr[1] = 591;
                    break;
                case 5:
                    arr[0] = 316;
                    arr[1] = 870;
                    break;
                case 6:
                    arr[0] = 398;
                    arr[1] = 591;
                    break;
                case 7:
                    arr[0] = 398;
                    arr[1] = 870;
                    break;
                case 8:
                    arr[0] = 640;
                    arr[1] = 22;
                    break;
                case 9:
                    arr[0] = 640;
                    arr[1] = 321;
                    break;
                case 10:
                    arr[0] = 640;
                    arr[1] = 501;
                    break;
                case 11:
                    arr[0] = 640;
                    arr[1] = 591;
                    break;
                case 12:
                    arr[0] = 640;
                    arr[1] = 870;
                    break;
                case 13:
                    arr[0] = 1117;
                    arr[1] = 22;
                    break;
                case 14:
                    arr[0] = 1117;
                    arr[1] = 321;
                    break;
                case 15:
                    arr[0] = 1117;
                    arr[1] = 591;
                    break;
                case 16:
                    arr[0] = 1117;
                    arr[1] = 591;
                    break;

            }
            return arr;

        }
        private static int num = -1;
        private void button16_Click_1(object sender, EventArgs e)
        {
            num = num + 1;
            label14.Text = (num + 1).ToString();
            Bitmap bm12 = new Bitmap(bm1);
            Graphics g1 = Graphics.FromImage(bm12);
            Bitmap bm52 = new Bitmap(bm5);
            Graphics g5 = Graphics.FromImage(bm52);
            Bitmap bm32 = new Bitmap(bm3);
            Graphics g3 = Graphics.FromImage(bm32);
            Pen myp = new Pen(Color.Blue, 3);
            //读access数据库

            conn.Open();
            connew.Open();
            //路口


            string[] arrS2 = dtnew.Rows[num]["堆场间路径"].ToString().Split('>');
            int[] arrS2int = new int[arrS2.Length];
            string arr = dtnew.Rows[num]["起始堆场内路径"].ToString();//起始堆场内路径出场路径
            string arrr = dtnew.Rows[num]["目标堆场内路径"].ToString();//入场路径
            //强制类型转换
            if (Convert.IsDBNull(dtnew.Rows[num]["堆场间路径"]))
            {
                if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 500, 591, 500, 561);

                }
                if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 398, 700, 368, 700);

                }
                if (arr.IndexOf("-") == 5 && arrr.IndexOf("-") == 5)
                {
                    string[] arrS = arr.Split('-');
                    int[] xpos = new int[arrS.Length];
                    int[] ypos = new int[arrS.Length];
                    string[] arrS3 = arrr.Split('-');
                    int[] xfpos = new int[arrS3.Length];
                    int[] yfpos = new int[arrS3.Length];
                    for (int i = 0; i < arrS.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f = 2;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < arrS3.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS3[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f2 = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f2 = 2;
                                }
                            }
                        }
                    }
                    if (f == 3 && f2 == 3)
                    {
                        for (int k = 0; k < arrS.Length; k++)
                        {
                            if (k == arrS.Length - 2 || arrS.Length == 1)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));
                            if (k == arrS.Length - 3)
                            {
                                break;
                            }

                        }
                        for (int k = 0; k < arrS3.Length; k++)
                        {
                            if (k == arrS3.Length - 2 || arrS3.Length == 1)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }

                        }
                        if (Math.Abs(ctpos(xfpos[0]) - ctpos(xpos[arrS.Length - 2])) > 25)
                        {
                            if (ypos[arrS.Length - 2] == 0 || ypos[arrS.Length - 2] == 12)
                            {
                                if (ypos[arrS.Length - 2] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]) - 10);
                                    g5.DrawLine(myp, ctpos(xfpos[0]), ctpos(yfpos[0]), ctpos(xfpos[0]), ctpos(yfpos[0]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 34, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[0]) - 12, 34, 890 + ctpos(xfpos[0]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 22, 890 + ctpos(xfpos[0]) - 12, 22);

                                }
                                if (ypos[arrS.Length - 2] == 12)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]) + 10);
                                    g5.DrawLine(myp, ctpos(xfpos[0]), ctpos(yfpos[0]), ctpos(xfpos[0]), ctpos(yfpos[0]) + 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 300, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[0]) - 12, 300, 890 + ctpos(xfpos[0]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 321, 890 + ctpos(xfpos[0]) - 12, 321);

                                }

                            }
                        }

                        if (Math.Abs(ctpos(xfpos[0]) - ctpos(xpos[arrS.Length - 2])) < 25)
                        {
                            g5.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xfpos[0]), ctpos(yfpos[0]));
                        }

                    }
                    if (f == 2 && f2 == 2)
                    {
                        for (int k = 0; k < arrS.Length; k++)
                        {
                            if (k == arrS.Length - 2 || arrS.Length == 1)
                            {
                                break;
                            }
                            if (k == arrS.Length - 3)
                            {
                                break;
                            }

                            g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));

                        }
                        for (int k = 0; k < arrS3.Length; k++)
                        {
                            if (k == arrS3.Length - 2 || arrS3.Length == 1)
                            {
                                break;
                            }
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }

                            g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                        }
                        g3.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xfpos[0]), ctpos(yfpos[0]));

                    }
                    label5.Image = bm52;
                    label3.Image = bm32;
                }
                if (arr.IndexOf("-") == 5 && arrr.IndexOf("-") != 5)
                {
                    string[] arrS = arr.Split('-');
                    int[] xpos = new int[arrS.Length];
                    int[] ypos = new int[arrS.Length];

                    //出场
                    for (int i = 0; i < arrS.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f = 2;
                                }
                            }
                        }
                    }
                    //出场画图
                    for (int k = 0; k < arrS.Length; k++)
                    {
                        if (f == 3)
                        {
                            if (xpos[k] == 11 || ypos[k] == 0)
                            {
                                if (ypos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 34, 890 + ctpos(xpos[k]) - 12, 22);

                                }
                                else
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) + 10, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]), ctpos(ypos[k]), 1117, ctpos(ypos[k]));

                                }
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));
                        }
                        if (f == 2)
                        {
                            if (xpos[k] == 10 || ypos[k] == 0)
                            {
                                if (ypos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]) - 12, 591, 431 + ctpos(xpos[k]) - 12, 604);
                                    if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 500, 591, 431 + ctpos(xpos[k]) - 12, 591);

                                    }
                                    if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 398, 700, 604, ctpos(ypos[k]));

                                    }
                                }
                                //需要改
                                else
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) + 10, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]), ctpos(ypos[k]), 604, ctpos(ypos[k]));
                                    if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 500, 591, 431 + ctpos(xpos[k]) - 12, 591);

                                    }
                                    if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 398, 700, 604, ctpos(ypos[k]));

                                    }

                                }
                                break;
                            }
                            g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));

                        }
                    }
                }
                if (arrr.IndexOf("-") == 5 && arr.IndexOf("-") != 5)
                {

                    string[] arrS3 = arrr.Split('-');
                    int[] xfpos = new int[arrS3.Length];
                    int[] yfpos = new int[arrS3.Length];
                    for (int i = 0; i < arrS3.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS3[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f2 = 3;

                                }
                                if ((int)dt4.Rows[j]["场地ID"] == 2)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f2 = 2;
                                }
                            }
                        }
                    }
                    //入场画图
                    for (int k = 0; k < arrS3.Length; k++)
                    {
                        if (f2 == 3)
                        {
                            if (xfpos[k] == 11 || yfpos[k] == 0)
                            {
                                if (yfpos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 34, 890 + ctpos(xfpos[k]) - 12, 22);

                                }
                                else
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) + 10, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]), ctpos(yfpos[k]), 1117, ctpos(yfpos[k]));

                                }

                            }
                            if (arrS3[1] == "" || arrS3[arrS3.Length - 1] == "")
                            {
                                break;
                            }
                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                        }
                        if (f2 == 2)
                        {
                            if (xfpos[k] == 0 || yfpos[k] == 0)
                            {
                                if (yfpos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xfpos[k]) - 12, 591, 431 + ctpos(xfpos[k]) - 12, 604);
                                    if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 500, 591, 431 + ctpos(xfpos[k]) - 12, 591);

                                    }



                                }
                                //需要改
                                else
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) - 10, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 20, 591 + ctpos(yfpos[k]) + 11, 398, 591 + ctpos(yfpos[k]) + 11);
                                    if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 398, 700, 398, 591 + ctpos(yfpos[k]) + 11);

                                    }

                                }

                            }
                            if (arrS3[1] == "" || arrS3[arrS3.Length - 1] == "")
                            {
                                break;
                            }
                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }
                            g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));

                        }
                    }
                }
                label5.Image = bm52;
                label3.Image = bm32;
                label1.Image = bm12;
            }
            if (!Convert.IsDBNull(dtnew.Rows[num]["堆场间路径"]))
            {
                for (int i = 0; i < arrS2.Length; i++)
                {
                    arrS2int[i] = Convert.ToInt32(arrS2[i]);
                }
                //总组
                if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 500, 591, 500, 561);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == zzpt)
                    {
                        g1.DrawLine(myp, 500, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == zzpt)
                    {
                        g1.DrawLine(myp, 500, 591, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 398, 700, 368, 700);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == tzcj)
                    {
                        g1.DrawLine(myp, 398, 700, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == tzcj)
                    {
                        g1.DrawLine(myp, 398, 700, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (yxzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || yxzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 750, 281, 750, 321);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == yxzcj)
                    {
                        g1.DrawLine(myp, 750, 321, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == yxzcj)
                    {
                        g1.DrawLine(myp, 750, 321, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (pzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || pzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 271, 611, 271, 591);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == pzcj)
                    {
                        g1.DrawLine(myp, 271, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == pzcj)
                    {
                        g1.DrawLine(myp, 271, 591, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (qmcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || qmcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 261, 591, 261, 561);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == qmcj)
                    {
                        g1.DrawLine(myp, 261, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == qmcj)
                    {
                        g1.DrawLine(myp, 261, 591, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (ehpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || ehpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 640, 351, 660, 351);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == ehpt)
                    {
                        g1.DrawLine(myp, 640, 351, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == ehpt)
                    {
                        g1.DrawLine(myp, 640, 351, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                //涂装车间
                //堆位调度

                if (arr.IndexOf("-") == 5 && arr != yxzcj)
                {
                    string[] arrS = arr.Split('-');
                    int[] xpos = new int[arrS.Length];
                    int[] ypos = new int[arrS.Length];

                    //出场
                    for (int i = 0; i < arrS.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f = 2;
                                }
                            }
                        }
                    }
                    //出场画图
                    for (int k = 0; k < arrS.Length; k++)
                    {
                        if (f == 3)
                        {
                            if (xpos[k] == 10 || ypos[k] == 0 || ypos[k] == 12)
                            {
                                if (ypos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 34, 890 + ctpos(xpos[k]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 22, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                if (ypos[k] == 12)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) + 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 300, 890 + ctpos(xpos[k]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 321, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);

                                }
                                if (xpos[k] == 10)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) + 40, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 10, ctpos(ypos[k]) + 32, 1117, ctpos(ypos[k]) + 32);
                                    g1.DrawLine(myp, 1117, ctpos(ypos[k]) + 32, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                break;
                            }

                            g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));
                        }
                        if (f == 2)
                        {
                            if (xpos[k] == 0 || ypos[k] == 0)
                            {
                                if (ypos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]) - 12, 591, 431 + ctpos(xpos[k]) - 12, 604);
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]) - 12, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                //需要改
                                else
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) - 10, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 398 + ctpos(xpos[k]) + 20, 591 + ctpos(ypos[k]) + 11, 398, 591 + ctpos(ypos[k]) + 11);
                                    g1.DrawLine(myp, 398, 591 + ctpos(ypos[k]) + 11, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                break;
                            }
                            g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));

                        }
                    }
                }
                //入场图
                if (arrr.IndexOf("-") == 5 && arrr != yxzcj)
                {

                    string[] arrS3 = arrr.Split('-');
                    int[] xfpos = new int[arrS3.Length];
                    int[] yfpos = new int[arrS3.Length];
                    for (int i = 0; i < arrS3.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS3[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f2 = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f2 = 2;
                                }
                            }
                        }
                    }
                    //入场画图
                    for (int k = 0; k < arrS3.Length; k++)
                    {
                        if (f2 == 3)
                        {
                            if (xfpos[k] == 10 || yfpos[k] == 0 || yfpos[k] == 12)
                            {

                                if (yfpos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 34, 890 + ctpos(xfpos[k]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 22, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }
                                if (yfpos[k] == 12)
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) + 10);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 300, 890 + ctpos(xfpos[k]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 321, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);

                                }
                                if (xfpos[k] == 10)
                                {

                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) + 40, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 10, ctpos(yfpos[k]) + 32, 1117, ctpos(yfpos[k]) + 32);
                                    g1.DrawLine(myp, 1117, ctpos(yfpos[k]) + 32, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }

                            }
                            if (arrS3[1] == "" || k == arrS3.Length - 2)
                            {
                                break;
                            }

                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }
                        }
                        if (f2 == 2)
                        {
                            if (xfpos[k] == 0 || yfpos[k] == 0 || yfpos[k] == 11)
                            {
                                if (yfpos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xfpos[k]) - 12, 591, 431 + ctpos(xfpos[k]) - 12, 604);
                                    g1.DrawLine(myp, 431 + ctpos(xfpos[k]) - 12, 591, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }
                                //需要改
                                if (xfpos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) - 10, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 20, 591 + ctpos(yfpos[k]) + 11, 398, 591 + ctpos(yfpos[k]) + 11);
                                    g1.DrawLine(myp, 398, 591 + ctpos(yfpos[k]) + 11, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }
                                if (yfpos[k] == 11)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) + 10);
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 21, 850, 398 + ctpos(xfpos[k]) + 20, 870);
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 21, 870, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);

                                }


                            }
                            if (arrS3[1] == "" || k == arrS3.Length - 2)
                            {
                                break;
                            }

                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }


                            g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }

                        }
                    }
                }
                label5.Image = bm52;
                label3.Image = bm32;

                for (int i2 = 0; i2 < arrS2.Length; i2++)
                {
                    if (i2 == arrS2.Length - 1)
                    {
                        break;
                    }
                    g1.DrawLine(myp, lk(arrS2int[i2])[0], lk(arrS2int[i2])[1], lk(arrS2int[i2 + 1])[0], lk(arrS2int[i2 + 1])[1]);

                }



                label1.Image = bm12;
            }
            conn.Close();
            connew.Close();


        }


        private void Form2_Load(object sender, EventArgs e)
        {

            da1 = new OleDbDataAdapter("select * from 当前全局堆场占用信息", conn);
            da2 = new OleDbDataAdapter("select * from 堆位信息", conn);
            da1.Fill(dt1);
            da2.Fill(dt2);
            da1.Fill(dt3);
            da2.Fill(dt4);
            danew = new OleDbDataAdapter("select * from [Sheet1$]", connew);
            danew.Fill(dtnew);

            //label5的网格
            Graphics g5 = Graphics.FromImage(bm5);
            g5.Clear(Color.AliceBlue);
            Brush mybs = new SolidBrush(Color.Gray);
            Pen p = new Pen(Color.Black);
            for (int i = 0; i < 232; i = i + 21)
            {
                g5.DrawLine(p, i, 0, i, 273);
            }
            for (int i = 0; i < 274; i = i + 21)
            {
                g5.DrawLine(p, 0, i, 231, i);
            }
            g5.FillRectangle(mybs, 1, 1, 20, 272);
            label5.Image = bm5;
            //label3的网格
            Graphics g3 = Graphics.FromImage(bm3);
            g3.Clear(Color.AliceBlue);
            for (int i = 0; i < 232; i = i + 21)
            {
                g3.DrawLine(p, i, 0, i, 273);
            }
            for (int i = 0; i < 274; i = i + 21)
            {
                g3.DrawLine(p, 0, i, 231, i);
            }

            g3.FillRectangle(mybs, 43, 148, 167, 83);
            label3.Image = bm3;
            //label8的路径
            Graphics g1 = Graphics.FromImage(bm1);
            g1.Clear(Color.AliceBlue);

            g1.DrawLine(p, 221, 22, 316, 22);
            g1.DrawLine(p, 221, 22, 221, 591);
            g1.DrawLine(p, 221, 591, 316, 591);
            g1.DrawLine(p, 316, 591, 316, 22);
            g1.DrawLine(p, 316, 591, 316, 870);
            g1.DrawLine(p, 316, 591, 640, 591);
            g1.DrawLine(p, 316, 870, 640, 870);
            g1.DrawLine(p, 640, 591, 640, 870);
            g1.DrawLine(p, 640, 591, 640, 501);
            g1.DrawLine(p, 640, 501, 640, 321);
            g1.DrawLine(p, 640, 501, 1117, 501);
            g1.DrawLine(p, 640, 321, 640, 22);
            g1.DrawLine(p, 640, 22, 1117, 22);
            g1.DrawLine(p, 640, 591, 1117, 591);
            g1.DrawLine(p, 640, 321, 1117, 321);
            g1.DrawLine(p, 1117, 22, 1117, 321);
            g1.DrawLine(p, 1117, 321, 1117, 501);
            g1.DrawLine(p, 1117, 501, 1117, 591);
            g1.DrawLine(p, 398, 870, 398, 591);
            label1.Image = bm1;
            //label2的网格
            Graphics g2 = Graphics.FromImage(bm2);
            g2.Clear(Color.AliceBlue);
            for (int i = 0; i < 64; i = i + 21)
            {
                g2.DrawLine(p, i, 0, i, 105);
            }
            for (int i = 0; i < 106; i = i + 21)
            {
                g2.DrawLine(p, 0, i, 63, i);
            }
            label2.Image = bm2;
            //label4的网格
            Graphics g4 = Graphics.FromImage(bm4);
            g4.Clear(Color.AliceBlue);

            for (int i = 0; i < 169; i = i + 21)
            {
                g4.DrawLine(p, i, 0, i, 63);
            }
            for (int i = 0; i < 64; i = i + 21)
            {
                g4.DrawLine(p, 0, i, 168, i);
            }
            label4.Image = bm4;
            //label6的网格
            Graphics g6 = Graphics.FromImage(bm6);
            g6.Clear(Color.AliceBlue);

            for (int i = 0; i < 463; i = i + 21)
            {
                g6.DrawLine(p, i, 0, i, 63);
            }
            for (int i = 0; i < 64; i = i + 21)
            {
                g6.DrawLine(p, 0, i, 462, i);
            }
            label6.Image = bm6;
            //label7的网格
            Graphics g7 = Graphics.FromImage(bm7);
            g7.Clear(Color.AliceBlue);
            for (int i = 0; i < 316; i = i + 21)
            {
                g7.DrawLine(p, i, 0, i, 63);
            }
            for (int i = 0; i < 64; i = i + 21)
            {
                g7.DrawLine(p, 0, i, 315, i);
            }
            label7.Image = bm7;
            //label8
            Graphics g8 = Graphics.FromImage(bm8);
            g8.Clear(Color.Gray);
            label8.Image = bm8;
            //label9
            Graphics g9 = Graphics.FromImage(bm9);
            g9.Clear(Color.Gray);
            label9.Image = bm9;
            //label10
            Graphics g10 = Graphics.FromImage(bm10);
            g10.Clear(Color.Gray);
            label10.Image = bm10;
            //label11
            Graphics g11 = Graphics.FromImage(bm11);
            g11.Clear(Color.Gray);
            label11.Image = bm11;
            Graphics g12 = Graphics.FromImage(bm121);
            g12.Clear(Color.Gray);
            label12.Image = bm121;
            Graphics g13 = Graphics.FromImage(bm131);
            g13.Clear(Color.Gray);
            label13.Image = bm131;
        }
        string[] sav1 = new string[50];
        private void button17_Click_1(object sender, EventArgs e)
        {
            Bitmap bm12 = new Bitmap(bm1);
            Graphics g1 = Graphics.FromImage(bm12);
            Bitmap bm52 = new Bitmap(bm5);
            Graphics g5 = Graphics.FromImage(bm5);
            Bitmap bm32 = new Bitmap(bm3);
            Graphics g3 = Graphics.FromImage(bm3);
            Pen myp = new Pen(Color.Blue, 3);

            //读access数据库
            conn.Open();

            //读取要移动的分段（输出数据库）

            connew.Open();

            //处理数据
            for (int ik = 0; ik <= num; ik++)
            {
                string arr = dtnew.Rows[ik]["起始堆场内路径"].ToString();
                string[] arrS = arr.Split('-');
                string arrr = dtnew.Rows[ik]["目标堆场内路径"].ToString();
                string[] arrS3 = arrr.Split('-');
                sav1[ik] = arrS[0];

                if (arr.IndexOf("-") == 5)
                {
                    //for (int i = 0; i < dt3.Rows.Count; i++)
                    //{
                    //    if (dt3.Rows[i]["堆位ID"].Equals(arrS[0]))
                    //    {
                    //dt3.Rows[i]["堆位ID"] = arrS3[arrS3.Length - 1];



                    for (int j = 0; j < dt4.Rows.Count; j++)
                    {

                        int ti = 0;
                        if (arrS[0].Equals(dt4.Rows[j]["堆位ID"]))
                        {

                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                if (arrS[0].Equals(dt3.Rows[i]["堆位ID"]))
                                {
                                    ti = ti + 1;
                                }
                            }
                            if (ti == 1)
                            {

                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {
                                    if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                    {

                                        int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                        int n = k / 11; int m = k % 11;
                                        Brush b = new SolidBrush(Color.AliceBlue);
                                        g5.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, 20);


                                    }
                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {
                                    if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                    {

                                        int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                        int n = k / 10; int m = k % 10;
                                        Brush b = new SolidBrush(Color.AliceBlue);
                                        g3.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, 20);

                                    }
                                }
                            }
                            int fg = 10;
                            if (ti == 2)
                            {
                                for (int t = 0; t < ik; t++)
                                {
                                    if (sav1[t].Equals(arrS[0]))
                                    {
                                        fg = 20;
                                        break;
                                    }

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {
                                    if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                    {

                                        int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                        int n = k / 11; int m = k % 11;
                                        Brush b = new SolidBrush(Color.AliceBlue);
                                        g5.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, fg);


                                    }
                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {
                                    if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                    {

                                        int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                        int n = k / 10; int m = k % 10;
                                        Brush b = new SolidBrush(Color.AliceBlue);
                                        g3.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, fg);

                                    }
                                }


                            }
                        }

                    }
                    //update 当前全局堆场占用信息 set 堆位ID='"+arrS3[arrS3.Length - 1]+"' where 堆位ID='"+arrS[0]+"'", conn);
                    //    }
                    //}
                }
                if (arrr.IndexOf("-") == 5)
                {
                    for (int j = 0; j < dt4.Rows.Count; j++)
                    {

                        //空的情况

                        if (arrS3[arrS3.Length - 1] == "")
                        {
                            if (arrS3[arrS3.Length - 2].Equals(dt4.Rows[j]["堆位ID"]))
                            {
                                //添加堆场ID
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {
                                    if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                    {

                                        int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                        int n = k / 11; int m = k % 11;
                                        Brush b = new SolidBrush(Color.Black);
                                        g5.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, 20);


                                    }
                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {
                                    if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                    {

                                        int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                        int n = k / 10; int m = k % 10;
                                        Brush b = new SolidBrush(Color.Black);
                                        g3.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, 20);

                                    }
                                }
                            }

                        }
                        if (arrS3[arrS3.Length - 1].Equals(dt4.Rows[j]["堆位ID"]))
                        {
                            //添加堆场ID
                            if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                            {
                                if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                {

                                    int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                    int n = k / 11; int m = k % 11;
                                    Brush b = new SolidBrush(Color.Black);
                                    g5.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, 20);


                                }
                            }
                            if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                            {
                                if (!Convert.IsDBNull(dt4.Rows[j]["堆场内位置序号"]))
                                {

                                    int k = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]);
                                    int n = k / 10; int m = k % 10;
                                    Brush b = new SolidBrush(Color.Black);
                                    g3.FillRectangle(b, 21 * m + 1, 21 * n + 1, 20, 20);

                                }
                            }
                        }
                    }

                }


            }
            label5.Image = bm5;
            label3.Image = bm3;
            label1.Image = bm1;
            conn.Close();
            connew.Close();
        }





        private void button21_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            num = num - 1;
            label14.Text = (num + 1).ToString();
            Bitmap bm12 = new Bitmap(bm1);
            Graphics g1 = Graphics.FromImage(bm12);
            Bitmap bm52 = new Bitmap(bm5);
            Graphics g5 = Graphics.FromImage(bm52);
            Bitmap bm32 = new Bitmap(bm3);
            Graphics g3 = Graphics.FromImage(bm32);
            Pen myp = new Pen(Color.Blue, 3);
            //读access数据库

            conn.Open();
            connew.Open();
            //路口


            string[] arrS2 = dtnew.Rows[num]["堆场间路径"].ToString().Split('>');
            int[] arrS2int = new int[arrS2.Length];
            string arr = dtnew.Rows[num]["起始堆场内路径"].ToString();//起始堆场内路径出场路径
            string arrr = dtnew.Rows[num]["目标堆场内路径"].ToString();//入场路径
            //强制类型转换
            if (Convert.IsDBNull(dtnew.Rows[num]["堆场间路径"]))
            {
                if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 500, 591, 500, 561);

                }
                if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 398, 700, 368, 700);

                }
                if (arr.IndexOf("-") == 5 && arrr.IndexOf("-") == 5)
                {
                    string[] arrS = arr.Split('-');
                    int[] xpos = new int[arrS.Length];
                    int[] ypos = new int[arrS.Length];
                    string[] arrS3 = arrr.Split('-');
                    int[] xfpos = new int[arrS3.Length];
                    int[] yfpos = new int[arrS3.Length];
                    for (int i = 0; i < arrS.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f = 2;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < arrS3.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS3[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f2 = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f2 = 2;
                                }
                            }
                        }
                    }
                    if (f == 3 && f2 == 3)
                    {
                        for (int k = 0; k < arrS.Length; k++)
                        {
                            if (k == arrS.Length - 2 || arrS.Length == 1)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));
                            if (k == arrS.Length - 3)
                            {
                                break;
                            }

                        }
                        for (int k = 0; k < arrS3.Length; k++)
                        {
                            if (k == arrS3.Length - 2 || arrS3.Length == 1)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }

                        }
                        if (Math.Abs(ctpos(xfpos[0]) - ctpos(xpos[arrS.Length - 2])) > 25)
                        {
                            if (ypos[arrS.Length - 2] == 0 || ypos[arrS.Length - 2] == 12)
                            {
                                if (ypos[arrS.Length - 2] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]) - 10);
                                    g5.DrawLine(myp, ctpos(xfpos[0]), ctpos(yfpos[0]), ctpos(xfpos[0]), ctpos(yfpos[0]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 34, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[0]) - 12, 34, 890 + ctpos(xfpos[0]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 22, 890 + ctpos(xfpos[0]) - 12, 22);

                                }
                                if (ypos[arrS.Length - 2] == 12)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]) + 10);
                                    g5.DrawLine(myp, ctpos(xfpos[0]), ctpos(yfpos[0]), ctpos(xfpos[0]), ctpos(yfpos[0]) + 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 300, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[0]) - 12, 300, 890 + ctpos(xfpos[0]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[arrS.Length - 2]) - 12, 321, 890 + ctpos(xfpos[0]) - 12, 321);

                                }

                            }
                        }

                        if (Math.Abs(ctpos(xfpos[0]) - ctpos(xpos[arrS.Length - 2])) < 25)
                        {
                            g5.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xfpos[0]), ctpos(yfpos[0]));
                        }

                    }
                    if (f == 2 && f2 == 2)
                    {
                        for (int k = 0; k < arrS.Length; k++)
                        {
                            if (k == arrS.Length - 2 || arrS.Length == 1)
                            {
                                break;
                            }
                            if (k == arrS.Length - 3)
                            {
                                break;
                            }

                            g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));

                        }
                        for (int k = 0; k < arrS3.Length; k++)
                        {
                            if (k == arrS3.Length - 2 || arrS3.Length == 1)
                            {
                                break;
                            }
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }

                            g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                        }
                        g3.DrawLine(myp, ctpos(xpos[arrS.Length - 2]), ctpos(ypos[arrS.Length - 2]), ctpos(xfpos[0]), ctpos(yfpos[0]));

                    }
                    label5.Image = bm52;
                    label3.Image = bm32;
                }
                if (arr.IndexOf("-") == 5 && arrr.IndexOf("-") != 5)
                {
                    string[] arrS = arr.Split('-');
                    int[] xpos = new int[arrS.Length];
                    int[] ypos = new int[arrS.Length];

                    //出场
                    for (int i = 0; i < arrS.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f = 2;
                                }
                            }
                        }
                    }
                    //出场画图
                    for (int k = 0; k < arrS.Length; k++)
                    {
                        if (f == 3)
                        {
                            if (xpos[k] == 11 || ypos[k] == 0)
                            {
                                if (ypos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 34, 890 + ctpos(xpos[k]) - 12, 22);

                                }
                                else
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) + 10, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]), ctpos(ypos[k]), 1117, ctpos(ypos[k]));

                                }
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));
                        }
                        if (f == 2)
                        {
                            if (xpos[k] == 10 || ypos[k] == 0)
                            {
                                if (ypos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]) - 12, 591, 431 + ctpos(xpos[k]) - 12, 604);
                                    if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 500, 591, 431 + ctpos(xpos[k]) - 12, 591);

                                    }
                                    if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 398, 700, 604, ctpos(ypos[k]));

                                    }
                                }
                                //需要改
                                else
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) + 10, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]), ctpos(ypos[k]), 604, ctpos(ypos[k]));
                                    if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 500, 591, 431 + ctpos(xpos[k]) - 12, 591);

                                    }
                                    if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 398, 700, 604, ctpos(ypos[k]));

                                    }

                                }
                                break;
                            }
                            g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));

                        }
                    }
                }
                if (arrr.IndexOf("-") == 5 && arr.IndexOf("-") != 5)
                {

                    string[] arrS3 = arrr.Split('-');
                    int[] xfpos = new int[arrS3.Length];
                    int[] yfpos = new int[arrS3.Length];
                    for (int i = 0; i < arrS3.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS3[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f2 = 3;

                                }
                                if ((int)dt4.Rows[j]["场地ID"] == 2)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f2 = 2;
                                }
                            }
                        }
                    }
                    //入场画图
                    for (int k = 0; k < arrS3.Length; k++)
                    {
                        if (f2 == 3)
                        {
                            if (xfpos[k] == 11 || yfpos[k] == 0)
                            {
                                if (yfpos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 34, 890 + ctpos(xfpos[k]) - 12, 22);

                                }
                                else
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) + 10, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]), ctpos(yfpos[k]), 1117, ctpos(yfpos[k]));

                                }

                            }
                            if (arrS3[1] == "" || arrS3[arrS3.Length - 1] == "")
                            {
                                break;
                            }
                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                        }
                        if (f2 == 2)
                        {
                            if (xfpos[k] == 0 || yfpos[k] == 0)
                            {
                                if (yfpos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xfpos[k]) - 12, 591, 431 + ctpos(xfpos[k]) - 12, 604);
                                    if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 500, 591, 431 + ctpos(xfpos[k]) - 12, 591);

                                    }



                                }
                                //需要改
                                else
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) - 10, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 20, 591 + ctpos(yfpos[k]) + 11, 398, 591 + ctpos(yfpos[k]) + 11);
                                    if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                                    {
                                        g1.DrawLine(myp, 398, 700, 398, 591 + ctpos(yfpos[k]) + 11);

                                    }

                                }

                            }
                            if (arrS3[1] == "" || arrS3[arrS3.Length - 1] == "")
                            {
                                break;
                            }
                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }
                            g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));

                        }
                    }
                }
                label5.Image = bm52;
                label3.Image = bm32;
                label1.Image = bm12;
            }
            if (!Convert.IsDBNull(dtnew.Rows[num]["堆场间路径"]))
            {
                for (int i = 0; i < arrS2.Length; i++)
                {
                    arrS2int[i] = Convert.ToInt32(arrS2[i]);
                }
                //总组
                if (zzpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || zzpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 500, 591, 500, 561);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == zzpt)
                    {
                        g1.DrawLine(myp, 500, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == zzpt)
                    {
                        g1.DrawLine(myp, 500, 591, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (tzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || tzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 398, 700, 368, 700);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == tzcj)
                    {
                        g1.DrawLine(myp, 398, 700, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == tzcj)
                    {
                        g1.DrawLine(myp, 398, 700, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (yxzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || yxzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 750, 281, 750, 321);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == yxzcj)
                    {
                        g1.DrawLine(myp, 750, 321, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == yxzcj)
                    {
                        g1.DrawLine(myp, 750, 321, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (pzcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || pzcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 271, 611, 271, 591);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == pzcj)
                    {
                        g1.DrawLine(myp, 271, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == pzcj)
                    {
                        g1.DrawLine(myp, 271, 591, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (qmcj == dtnew.Rows[num]["起始堆场内路径"].ToString() || qmcj == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 261, 591, 261, 561);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == qmcj)
                    {
                        g1.DrawLine(myp, 261, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == qmcj)
                    {
                        g1.DrawLine(myp, 261, 591, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                if (ehpt == dtnew.Rows[num]["起始堆场内路径"].ToString() || ehpt == dtnew.Rows[num]["目标堆场内路径"].ToString())
                {
                    g1.DrawLine(myp, 640, 351, 660, 351);
                    if (dtnew.Rows[num]["起始堆场内路径"].ToString() == ehpt)
                    {
                        g1.DrawLine(myp, 640, 351, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                    }
                    if (dtnew.Rows[num]["目标堆场内路径"].ToString() == ehpt)
                    {
                        g1.DrawLine(myp, 640, 351, lk(arrS2int[arrS2.Length - 1])[0], lk(arrS2int[arrS2.Length - 1])[1]);
                    }
                }
                //涂装车间
                //堆位调度

                if (arr.IndexOf("-") == 5 && arr != yxzcj)
                {
                    string[] arrS = arr.Split('-');
                    int[] xpos = new int[arrS.Length];
                    int[] ypos = new int[arrS.Length];

                    //出场
                    for (int i = 0; i < arrS.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    ypos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f = 2;
                                }
                            }
                        }
                    }
                    //出场画图
                    for (int k = 0; k < arrS.Length; k++)
                    {
                        if (f == 3)
                        {
                            if (xpos[k] == 10 || ypos[k] == 0 || ypos[k] == 12)
                            {
                                if (ypos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 34, 890 + ctpos(xpos[k]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 22, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                if (ypos[k] == 12)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) + 10);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 300, 890 + ctpos(xpos[k]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 12, 321, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);

                                }
                                if (xpos[k] == 10)
                                {
                                    g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) + 40, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xpos[k]) - 10, ctpos(ypos[k]) + 32, 1117, ctpos(ypos[k]) + 32);
                                    g1.DrawLine(myp, 1117, ctpos(ypos[k]) + 32, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                break;
                            }

                            g5.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));
                        }
                        if (f == 2)
                        {
                            if (xpos[k] == 0 || ypos[k] == 0)
                            {
                                if (ypos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]), ctpos(ypos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]) - 12, 591, 431 + ctpos(xpos[k]) - 12, 604);
                                    g1.DrawLine(myp, 431 + ctpos(xpos[k]) - 12, 591, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                //需要改
                                else
                                {
                                    g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k]) - 10, ctpos(ypos[k]));
                                    g1.DrawLine(myp, 398 + ctpos(xpos[k]) + 20, 591 + ctpos(ypos[k]) + 11, 398, 591 + ctpos(ypos[k]) + 11);
                                    g1.DrawLine(myp, 398, 591 + ctpos(ypos[k]) + 11, lk(arrS2int[0])[0], lk(arrS2int[0])[1]);
                                }
                                break;
                            }
                            g3.DrawLine(myp, ctpos(xpos[k]), ctpos(ypos[k]), ctpos(xpos[k + 1]), ctpos(ypos[k + 1]));

                        }
                    }
                }
                //入场图
                if (arrr.IndexOf("-") == 5 && arrr != yxzcj)
                {

                    string[] arrS3 = arrr.Split('-');
                    int[] xfpos = new int[arrS3.Length];
                    int[] yfpos = new int[arrS3.Length];
                    for (int i = 0; i < arrS3.Length; i++)
                    {
                        for (int j = 0; j < dt4.Rows.Count; j++)
                        {

                            if (arrS3[i].Equals(dt4.Rows[j]["堆位ID"].ToString()))
                            {
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 3)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 11;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 11;
                                    f2 = 3;

                                }
                                if (Convert.ToInt32(dt4.Rows[j]["场地ID"]) == 2)
                                {

                                    yfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) / 10;
                                    xfpos[i] = Convert.ToInt32(dt4.Rows[j]["堆场内位置序号"]) % 10;
                                    f2 = 2;
                                }
                            }
                        }
                    }
                    //入场画图
                    for (int k = 0; k < arrS3.Length; k++)
                    {
                        if (f2 == 3)
                        {
                            if (xfpos[k] == 10 || yfpos[k] == 0 || yfpos[k] == 12)
                            {

                                if (yfpos[k] == 0)
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 34, 890 + ctpos(xfpos[k]) - 12, 22);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 22, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }
                                if (yfpos[k] == 12)
                                {
                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) + 10);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 300, 890 + ctpos(xfpos[k]) - 12, 321);
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 12, 321, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);

                                }
                                if (xfpos[k] == 10)
                                {

                                    g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) + 40, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 890 + ctpos(xfpos[k]) - 10, ctpos(yfpos[k]) + 32, 1117, ctpos(yfpos[k]) + 32);
                                    g1.DrawLine(myp, 1117, ctpos(yfpos[k]) + 32, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }

                            }
                            if (arrS3[1] == "" || k == arrS3.Length - 2)
                            {
                                break;
                            }

                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }
                            g5.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }
                        }
                        if (f2 == 2)
                        {
                            if (xfpos[k] == 0 || yfpos[k] == 0 || yfpos[k] == 11)
                            {
                                if (yfpos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) - 10);
                                    g1.DrawLine(myp, 431 + ctpos(xfpos[k]) - 12, 591, 431 + ctpos(xfpos[k]) - 12, 604);
                                    g1.DrawLine(myp, 431 + ctpos(xfpos[k]) - 12, 591, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }
                                //需要改
                                if (xfpos[k] == 0)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]) - 10, ctpos(yfpos[k]));
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 20, 591 + ctpos(yfpos[k]) + 11, 398, 591 + ctpos(yfpos[k]) + 11);
                                    g1.DrawLine(myp, 398, 591 + ctpos(yfpos[k]) + 11, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);
                                }
                                if (yfpos[k] == 11)
                                {
                                    g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k]), ctpos(yfpos[k]) + 10);
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 21, 850, 398 + ctpos(xfpos[k]) + 20, 870);
                                    g1.DrawLine(myp, 398 + ctpos(xfpos[k]) + 21, 870, lk(arrS2int[arrS2int.Length - 1])[0], lk(arrS2int[arrS2int.Length - 1])[1]);

                                }


                            }
                            if (arrS3[1] == "" || k == arrS3.Length - 2)
                            {
                                break;
                            }

                            if ((k + 1) == arrS3.Length)
                            {
                                break;
                            }


                            g3.DrawLine(myp, ctpos(xfpos[k]), ctpos(yfpos[k]), ctpos(xfpos[k + 1]), ctpos(yfpos[k + 1]));
                            if (k == arrS3.Length - 3)
                            {
                                break;
                            }

                        }
                    }
                }
                label5.Image = bm52;
                label3.Image = bm32;

                for (int i2 = 0; i2 < arrS2.Length; i2++)
                {
                    if (i2 == arrS2.Length - 1)
                    {
                        break;
                    }
                    g1.DrawLine(myp, lk(arrS2int[i2])[0], lk(arrS2int[i2])[1], lk(arrS2int[i2 + 1])[0], lk(arrS2int[i2 + 1])[1]);

                }



                label1.Image = bm12;
            }
            conn.Close();
            connew.Close();


        }

        private void button18_Click(object sender, EventArgs e)
        {
            num = -1;
        }



    }
}
