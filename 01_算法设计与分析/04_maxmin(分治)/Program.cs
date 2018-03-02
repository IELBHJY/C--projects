using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_maxmin_分治_
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] num_array = { 1,2,3,4,5,6,7,8,9,10};
            int[] minmax = Search_minmax(num_array, 0, num_array.Length - 1);
            Console.WriteLine("数组最小值是:{0},数组最大值是：{1}",minmax[0],minmax[1]);
            Console.ReadKey();
        }

        public static int[] Search_minmax(int[] num_array, int low, int high)
        {
            int[] minmax = new int[2];
            if (high - low == 1)
            {
                if (num_array[low] < num_array[high])
                {
                    minmax[0] = num_array[low];
                    minmax[1] = num_array[high];
                }
                else
                {
                    minmax[0] = num_array[high];
                    minmax[1] = num_array[low];
                }
            }
            else
            {
                int mid = (int)(low + high) / 2;
                int[] minmax1 = new int[2];
                int[] minmax2 = new int[2];
                minmax1 = Search_minmax(num_array, low, mid);
                minmax2 = Search_minmax(num_array, mid, high);
                if (minmax1[0] < minmax2[0])
                {
                    minmax[0] = minmax1[0];
                }
                else
                {
                    minmax[0] = minmax2[0];
                }
                if (minmax1[1] < minmax2[1])
                {
                    minmax[1] = minmax2[1];
                }
                else
                {
                    minmax[1] = minmax1[1];
                }

            }
            return minmax;
        }
    }
}
