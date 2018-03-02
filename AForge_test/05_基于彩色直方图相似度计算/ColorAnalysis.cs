using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;

namespace _05_基于彩色直方图相似度计算
{
    class ColorAnalysis
    {
        int i, j;
        string c11, c22;
        int r = 16000, n1, m11, m1, n2, m22, m2;
        Color c1 = new Color();
        Color c2 = new Color();
        public double ColorValue(Image p,Image pS1)//通过与标准图像颜色分布进行对比，并返回颜色分布相似值
        {
            //变量声明以及初始化
            double[,] s1 = new double[20000, 2];
            double[,] s2 = new double[20000, 2];
            double seaBlue=0;//海蓝色像素点数
            double percent;//海蓝色占图像比率
            double u = 0, d;
            string D = " ";
            u = 0;
            n1 = 0;
            m11 = 0;
            m1 = 0;
            n2 = 0;
            m22 = 0;
            m2 = 0;
           Form1 f=new Form1();
            if (p != null)
            {
                Bitmap p1 = new Bitmap(p);
                for (i = 0; i < p1.Width - 1; i++)
                {
                    for (j = 0; j < p1.Height - 1; j++)
                    {
                        c1 = p1.GetPixel(i, j); //获得图像每点的颜色基本属性 
                        if (c1.R >= 0 && c1.R <= 100 && c1.G >= 100 && c1.G <= 255 && c1.B >= 150 && c1.B <= 255)//判断颜色是否为海蓝色区域
                        {
                            seaBlue++;
                        }
                        n1 = n1 + 1; //统计像素数
                        c11 = c1.Name; //返回RGB 值
                        m1 = Convert.ToInt32(Math.Pow(256, 3) / r); //每组颜色值的范围，这里设置组数为16000，可以扩大组数以提高精度，但会降低识别速度。
                        string str = c11;
                        //将十六进制装换成十进制
                        int val = (-1) * Int32.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        m11 = Convert.ToInt32(Math.Floor(Convert.ToDouble(val / m1))); //提取颜色归属于哪一组
                        s1[m11, 0]++; //统计每组颜色的像素数

                    }
                }
                Bitmap p2 = new Bitmap(pS1);
                for (i = 0; i < p2.Width - 1; i++)
                {
                    for (j = 0; j < p2.Height - 1; j++)
                    {
                        c2 = p2.GetPixel(i, j); //获得图像每点的颜色基本属性
                        n2 = n2 + 1; //统计像素数
                        c22 = c2.Name; //返回RGB 值
                        m2 = Convert.ToInt32(Math.Pow(256, 3) / r); //每组颜色值的范围
                        string str = c22;
                        //将十六进制装换成十进制
                        int val = (-1) * Int32.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        m22 = Convert.ToInt32(Math.Floor(Convert.ToDouble(val / m2))); //提取颜色归属于哪一组
                        s2[m22, 0]++; //统计每组颜色的像素数
                    }
                }
                for (i = 0; i < 16000; i++)
                {
                    u += (Convert.ToDouble((s1[i, 0] - s2[i, 0]) / n1)) * (Convert.ToDouble((s1[i, 0] - s2[i, 0]) / n1));
                }
                percent = seaBlue / n1;
                if (percent > 0.03)//海蓝色比率大于0.03再进行d值的判断，否则直接跳过d值的计算
                {
                    d = Math.Pow(u, 0.5);
                    D = String.Format("{0:F6}", d);
                    return d;
                }
                else
                    return d = 1;
            }
              else
                  return d = 1;
        }
    }
}
