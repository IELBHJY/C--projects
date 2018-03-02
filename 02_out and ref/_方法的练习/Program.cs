using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _方法的练习
{
    class Program
    {
        public static int GetBetweenSum(int n1, int n2)
        { 
           int sum=0;
           for(int i=n1;i<=n2;i++)
           {
             sum+=i;
           }
            return sum;
        }
        public static int GetNumber(string s)
        { 
       
           while(true)
           {
           
                try
               {
                int num= Convert.ToInt32(s);
                return num;
               }
               catch
               {
                Console.WriteLine("输入有误，请重新输入！");
                s = Console.ReadLine();          
               }
            }
        }
        public static void ComTwonumber(ref int n1, ref int n2)
        {
            while (true)
            {
                if (n1 < n2)
                {
                     return;
                }
                else
                {
                    Console.WriteLine("第一个数字要小于第二个数字，请重新输入第一个数字");
                    string s1 = Console.ReadLine();
                    n1= GetNumber(s1);
                   Console.WriteLine("请输入第二个数字：");
                   string s2 = Console.ReadLine();
                    n2 = GetNumber(s2);
                   ComTwonumber(ref n1, ref n2);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("请输入第一个整数：");
            string num1 = Console.ReadLine();
            int s1 = GetNumber(num1);
            Console.WriteLine("请输入第二个整数：");
            string num2 = Console.ReadLine();
            int s2 = GetNumber(num2);
            ComTwonumber(ref s1,ref s2);
            int sum = GetBetweenSum(s1,s2);
            Console.WriteLine("输入的两个数字之间的和是：{0}",sum);
            Console.ReadKey();
        }
    }
}
