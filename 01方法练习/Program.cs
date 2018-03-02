using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01方法练习
{
    class Program
    {
        /// <summary>
        ///判断用户输入是否是数字
        /// </summary>
        public static int  ReadInput(string s)
        {
            while (true)
            {
                try
                {
                    int number = Convert.ToInt32(s);
                    return number;
                }
                catch
                {
                    Console.WriteLine("输入有误，请重新输入！");
                    s = Console.ReadLine();
                }
            }

        }
        static void Main(string[] args)
        {
            Console.WriteLine("请输入一个数字！");
            string str = Console.ReadLine();
           int number= ReadInput(str);
           Console.WriteLine(number);
            Console.ReadKey();
        }
    }
}
