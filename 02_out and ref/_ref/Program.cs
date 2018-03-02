using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ref
{
    class Program
    {
        //ref 参数能够将一个变量带入到方法中进行改变，改变完成后，再将改变后的值带出方法
        //ref关键字 要求在方法外部赋值，而方法内无法赋值
        public static void Test(ref int x1, ref int x2)
        {
            int temp;
            temp = x1;
            x1 = x2;
            x2 = temp;
        }
        static void Main(string[] args)
        {
            //不用中间变量交换
            //int n1 = 10;
            //int n2 = 20;
            //n1 = n1 - n2;//n1=-10 n2=20
            //n2 = n1 + n2;//n1=-10  n2=10
            //n1 = n2 - n1;
            int n1 = 10;
            int n2 = 20;
            Test(ref n1,ref n2);
            Console.WriteLine(n1);
            Console.WriteLine(n2);
            Console.ReadKey();

        }
    }
}
