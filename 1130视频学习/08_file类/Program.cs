using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_file类
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建文件
           // File.Create(@"C:\Users\lbh\Desktop\new.txt");
            //删除
           // File.Delete(@"C:\Users\lbh\Desktop\new.txt");
            //复制剪切
           //File.Copy(@"C:\Users\lbh\Desktop\new.txt", @"C:\Users\lbh\Desktop\new1.txt");
            //byte[] buffer = File.ReadAllBytes(@"C:\Users\lbh\Desktop\new.txt");
            //将字节数组中的每一个元素按照指定的编码格式解码成字符串
            //UTF-8 GB2312 GBK ASCII Unicode
           // string s=Encoding.GetEncoding("GB2312").GetString(buffer);
            string str="aaaaaaaaaaaaaaaaaaaaaaaa";
           byte[] buffer= Encoding.Default.GetBytes(str);
            File.WriteAllBytes(@"C:\Users\lbh\Desktop\new.txt",buffer);
            Console.WriteLine("写入成功！");
            Console.ReadKey();
        }
    }
}
