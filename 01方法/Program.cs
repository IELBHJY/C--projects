using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01方法
{
    class Program
    {
        //字段  属于类的字段  在声明前的括号 到声明的括号的那个括号结束
        public static int _number = 10;//达到全局变量的效果
        /// <summary>
        /// 取两个数的最大值并返回最大值
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static int GetMax(int n1, int n2)
        {
            return n1 > n2 ? n1 : n2;
        }
        public static bool isrun(int year)
        { 
        bool b=(year%400==0)||(year%4==0&&year%100!=0);
        return b;
        }
        static void Main(string[] args)
        {
            /*方法就是将一堆代码进行重用的机制
            [public] static 返回值类型 方法名([参数列表])
            {
                方法体;
            }*/
            int max = GetMax(1,3);
            int a=Convert.ToInt32("123456");
            Console.WriteLine(max);
            Console.ReadKey();
        }
    }
}
