using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using System.Drawing.Imaging;
namespace _01_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bt = new Bitmap(@"E:\复杂机电\单目标定图片\000002.bmp");
            Size size = new System.Drawing.Size();
            //将长宽处理为2的幂
            double n1, n2;
            n1 = Math.Log(bt.Width, 2);
            n2 = Math.Log(bt.Height, 2);
            size.Width = (Int32)Math.Pow(2, Convert.ToInt32(n1));
            size.Height = (Int32)Math.Pow(2, Convert.ToInt32(n2));
            Bitmap bt0 = new Bitmap(bt, size.Width, size.Height);
            Rectangle rec = new Rectangle(pictureBox1.Location, bt0.Size);
            //将彩色图转为灰度图
            Bitmap bt1 = bt0.Clone(rec, PixelFormat.Format8bppIndexed);
            pictureBox1.Image = bt1;
            //复杂图像的原图像需要满足两个条件：灰度图（像素深度：8bbp），长宽必须是2的幂
            //傅立叶正变换
            ComplexImage img1 = ComplexImage.FromBitmap(bt1);
            img1.ForwardFourierTransform();
            Bitmap bt2 = img1.ToBitmap();
            pictureBox2.Image = img1.ToBitmap();
            //傅立叶逆变换
            ComplexImage img2 = ComplexImage.FromBitmap(bt1);
            img2.BackwardFourierTransform();
            pictureBox3.Image = img2.ToBitmap();
        }

    }
}
