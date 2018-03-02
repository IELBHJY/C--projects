using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _方法练习3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入一个数字：");
            string number = Console.ReadLine();
            int n = GetNumber(number);
            bool result = GetPrimeNumber(n);
            if (result)
            {
                Console.WriteLine("输入数字是质数！");
            }
            else
            {
                Console.WriteLine("输入数字不是质数！");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// 判断一个整数是否是质数
        /// </summary>
        /// <param name="n">整数</param>
        /// <returns>是否是质数</returns>
        public static bool GetPrimeNumber(int n)
        {
            bool result=true;
            for (int i = 2; i < n; i++)
            {
                if (n % i == 0)
                {
                    result=false;
                    return result;
                }
                else
                {
                    result=true;
                }
            }
            return result;
        }

        public static int GetNumber(string number)
        {
            while (true)
            {
                try
                {
                    int n = Convert.ToInt32(number);
                    return n;
                }
                catch
                {
                    Console.WriteLine("输入数字不是整数，请重新输入！");
                    number = Console.ReadLine();
                }
            }
        }
    }
}
