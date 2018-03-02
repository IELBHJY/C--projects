using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_path类
{
    class Program
    {
        static void Main(string[] args)
        {
            //path:静态类，调用方法直接.方法名
            //path,专门操作路径。
            string str = @"E:\李柏鹤研一\C#\c#遗传算法\使用手册.wav";
            //int index=str.LastIndexOf("\\");
            //    str=str.Substring(index+1);
            //    Console.WriteLine(str);
            //    Console.ReadKey();
            //获得文件名
            Console.WriteLine(Path.GetFileName(str));//文件和拓展名
            Console.WriteLine(Path.GetFileNameWithoutExtension(str));//文件去除拓展名
            Path.GetExtension(str);//文件拓展名
            Console.WriteLine(Path.GetDirectoryName(str));//文件所在文件夹。全目录
            Console.WriteLine(Path.GetFullPath(str));
            Console.WriteLine(Path.Combine(@"c\a\","b.txt"));

            Console.ReadKey();
        }
    }
}
