using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_随机数
{
    class Program
    {
        public static int GetNum(int[] array,int num,int min,int max,Random r)
        {
            int i = 0;
            while (i < array.Length)
            {
                if (array[i] == num)
                {
                    num = r.Next(min, max);
                    GetNum(array,num,min,max,r);
                }
             i++;
            }
            return num;
        }
        public static int[] GetRandom()
        {
            int[] a = new int[6];
            int[] b = new int[6];
            Random r = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < a.Length; i++)
            {
                bool result = true;
                while (result)
                {
                    int num = r.Next(2, 33);
                    b[i] = GetNum(a, num, 2, 33, r);
                    if (b[i] % 2 == 0)
                    {
                        a[i] = b[i];
                        break;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return a;
        }
        static void Main(string[] args)
        {
            int[] b= GetRandom();
            int sum = 0;
            for (int i = 0; i < b.Length; i++)
            { 
             sum+=b[i];
            }
                for (int i = 0; i < b.Length; i++)
                {
                    Console.Write(b[i] + "  ");
                }
                Console.WriteLine();
                Console.WriteLine("产生不重复随机数的总和是："+sum);
                Console.ReadKey();
        }
    }
}
