using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _05_基于彩色直方图相似度计算
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ColorAnalysis CA = new ColorAnalysis();
            Image tempimage1 = Image.FromFile(@"E:\复杂机电\单目标定图片\3.jpg");
            Image tempimage2 = Image.FromFile(@"E:\复杂机电\单目标定图片\4.jpg");
            double result = CA.ColorValue(tempimage1, tempimage2);
            textBox1.Text = result.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
