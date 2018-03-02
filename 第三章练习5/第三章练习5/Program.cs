using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第三章练习5
{
    class Program
    {
        static void Main(string[] args)
        {
            int var1, var2, var3, var4;
            var1 = 0;
            var2 = 0;
            var3 = 0;
            var4 = 0;
            Console.WriteLine("please input an int number");
            var1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("please input an int number");
            var2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("please input an int number");
            var3 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("please input an int number");
            var4 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("这四个数字 {0},{1},{2},{3}的乘积是 {4}.", var1, var2, var3, var4, var1 * var2 * var3 * var4);
            Console.ReadKey();
        }
    }
}
