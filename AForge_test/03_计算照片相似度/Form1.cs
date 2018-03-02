using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace _03_计算照片相似度
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Image tempimage = Image.FromFile(@"E:\复杂机电\待测试照片\4.bmp");           
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = tempimage;
        }
        public static double[] result = new double[31];
        public static int label = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            calSimilar cs = new calSimilar();
            Bitmap object_bm = cs.Resize(@"E:\复杂机电\待测试照片\4.bmp");
            int[] first = cs.GetHisogram(object_bm);          
            for (int i = 0; i < 29; i++)
            {
                Bitmap class_bm = cs.Resize(@"E:\复杂机电\模型检索图片\"+(i+1).ToString()+".bmp");             
                int[] second = cs.GetHisogram(class_bm);
                result[i] = cs.GetResult(first, second);
            }
            double max = result[0];      
            for (int i = 0; i < 29; i++)
            {
                if (max < result[i])
                {
                    max = result[i];
                    label = i;
                }
            }
            //SimilarPhoto sp1 = new SimilarPhoto(@"E:\复杂机电\单目标定图片\5.bmp");
            //SimilarPhoto sp2 = new SimilarPhoto(@"E:\复杂机电\单目标定图片\6.bmp");
            //Image image1 = sp1.ReduceSize(8, 8);
            //Image image2 = sp2.ReduceSize(8, 8);
            //Byte[] byte1 = sp1.ReduceColor(image1);
            //Byte byte3 = sp1.CalcAverage(byte1);
            //string str1 = sp1.ComputeBits(byte1, byte3);
            //Byte[] byte2 = sp2.ReduceColor(image2);
            //Byte byte4 = sp2.CalcAverage(byte2);
            //string str2 = sp2.ComputeBits(byte2, byte4);
            //double result = SimilarPhoto.CalcSimilarDegree(str1, str2);
            textBox1.Text = "两张图片的相似度：" + result[label].ToString();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {        
            Image tempimage = Image.FromFile(@"E:\复杂机电\模型检索图片\"+(label+1).ToString()+".bmp");
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = tempimage;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
