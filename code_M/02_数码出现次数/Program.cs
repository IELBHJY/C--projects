using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_数码出现次数
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //string str = Console.ReadLine();
            string str = "1,100000";
            string[] str1 = str.Split(',');
            int num1 = int.Parse(str1[0]);
            int num2 = int.Parse(str1[1]);
            List<char> list = new List<char>();
            for (int i = num1; i < num2 + 1;i++ )
            {
                for (int j = 1; j <num2 + 1; j++)
                {
                    if (i % j == 0)
                    {
                        char[] tempchar = j.ToString().ToCharArray();
                        list.Add(tempchar[0]);
                    }
                }
            }
            foreach (var v in list.GroupBy(x => x).Select(x => new { k = x.Key, c = x.Count() }))           
            Console.WriteLine(v.c);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }
}
