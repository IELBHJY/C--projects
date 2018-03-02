using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_二分法搜索
{
    class Program
    {
        static void Main(string[] args)
        {
            //二分法搜索的基本思想是：将n个元素分成个数大致相同的两半。
            int[] a = { 1,4,3,7,9,12,15,4,17,3,4,5,6,12,18,23,24,25};
            int popsition = binarySearch(a,25,a.Length);
            Console.WriteLine("17在数组中的位置是："+(popsition+1));
            Console.ReadKey();
        }
        public static int binarySearch(int[] a,int x,int n)
        {
            int left =0;
            int right =n-1;
            while (left <= right)
            {
                int middle = (left + right) / 2;
                if (x == a[middle])
                {
                    return middle;
                }
                if (x > a[middle])
                {
                    left = a[middle];
                }
                else
                {
                    right = a[middle];

                }
            }
            return -1;
        }
    }
}
