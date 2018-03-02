using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _方法的重载
{
    class Program
    {
        //方法重载，就是指函数名称相同，但是参数类型不相同。
        public static int Sum(int n1, int n2)
        {
            return n1 + n2;
        }
        public static int Sum(int n1,int n2,int n3)
        {
            return n1 + n2 + n3;
        }
        static void Main(string[] args)
        {
            int result=Sum(1, 2);
            int a = Sum(1,2,3);
            Console.WriteLine(result);
            Console.WriteLine(a);
            Console.ReadKey();
        }
    }
}
