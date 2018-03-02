using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_随机偶数求和
{
    class Program
    {
        public static int GetNum(int[] a,int temp,int min,int max,Random r)
        {
            int i = 0;
            while (i < a.Length - 1)
            {
                if (a[i] ==temp)
                {
                    temp = r.Next(min, max + 1);
                    GetNum(a,temp,min,max,r);
                }
                i++;
            }
            return temp;
        }
        public static int[] GeTRandomArray(int num,int min,int max)
        {
            int[] a = new int[num];
            bool result = true;
            Random r =  new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < a.Length; i++)
            {
                while (result)
                {
                    int temp = r.Next(min, max);
                    if (temp % 2 == 0)
                    {
                        a[i] =GetNum(a,temp,min,max,r);
                        result = false;
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
           int[] array=GeTRandomArray(6, 2, 32);
           for (int i = 0; i < array.Length; i++)
           {
               Console.WriteLine(array[i]);
           }
           Console.ReadKey();
        }
    }
}
