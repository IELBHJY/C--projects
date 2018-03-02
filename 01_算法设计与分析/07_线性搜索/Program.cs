using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_线性搜索
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] num = { 1,2,3,4,5,6,7,8,9,10};
            int position = linersearch(num, 6);
            Console.WriteLine(position);
            Console.ReadKey();
        }
        public static int linersearch(int[] a, int x)
        {
            int j = 0;
            while (j < a.Length && a[j] != x)
            {
                j++;
            }
            if (a[j] == x)
            {
                return j;
            }
            else
            {
                return -1;
            }
        }


    }
}
