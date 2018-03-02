using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_最小值最大值
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] num_array = { 1,2,3,4,5,6,7,8,9,10};
            int[] maxmin = search_maxmin(num_array);
            Console.WriteLine("数组中的最大元素是：{0}；最小元素是：{1}",maxmin[0],maxmin[1]);
            Console.ReadKey();
        }
        public static int[] search_maxmin(int[] num_array)
        {
            int x = num_array[0];
            int y = num_array[0];
            int[] maxmin = new int[2];
            for (int i = 1; i < num_array.Length; i++)
            {
                if (x < num_array[i])
                {
                    x = num_array[i];
                }
                if (y > num_array[i])
                {
                    y = num_array[i];
                }
            }
            maxmin[0] = x;
            maxmin[1] = y;
            return maxmin;
        }
    }
}
