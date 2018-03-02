using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //声明整型数组，保存一组整数
            int[] num = new int[] { 3, 34, 42, 2, 11, 19, 30, 55, 20 };
            //请完善代码，循环打印数组中的偶数
            for (int i = 0; i < num.Length;i++ )
            {
                if (num[i] % 2 == 0)
                {
                    Console.Write(num[i]+",");
                }
            }
                Console.ReadLine();
        }
    }
}
