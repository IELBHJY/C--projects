using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
namespace Kinect_photo
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        static void Main(string[] args)
        {
            Console.Title = "我被隐藏"; //为控制台窗体指定一个标题，便于定位和区分
            IntPtr a = FindWindow("ConsoleWindowClass", "我被隐藏");
            if (a != IntPtr.Zero)
            {
                ShowWindow(a, 0); //隐藏这个窗口
            }
            double[] result = new double[31];
            int label = 0;
            calSimilar cs = new calSimilar();
            Bitmap object_bm = cs.Resize(@"C:\Users\DELL\Desktop\拍摄照片\0.bmp");
            int[] first = cs.GetHisogram(object_bm);
            for (int i = 0; i < 28; i++)
            {
                Bitmap class_bm = cs.Resize(@"E:\复杂机电\模型检索图片\" + (i + 1).ToString() + ".bmp");
                int[] second = cs.GetHisogram(class_bm);
                result[i] = cs.GetResult(first, second);
            }
            double max = result[0];
            for (int i = 0; i < 28; i++)
            {
                if (max < result[i])
                {
                    max = result[i];
                    label = i;
                }
            }
            System.IO.File.WriteAllText(@"C:\Users\DELL\Desktop\result.txt", (label + 1).ToString());
            Thread.Sleep(2000);
        }
    }
}
