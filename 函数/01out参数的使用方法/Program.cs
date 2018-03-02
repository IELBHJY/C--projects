using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01out参数的使用方法
{
    class Program
    {
        public static int[] GetMaxMinSumAvg(int[] nums)
        {
            int[] res = new int[4];
            res[0] = nums[0];//max
            res[1] = nums[0];//min
            res[2] =0;//sum
            for (int i = 0; i < nums.Length;i++ )
            {
                if (res[0]<nums[i])
                {
                    res[0] = nums[i];
                }
                if (res[1] > nums[i])
                {
                    res[1] = nums[i];
                }
                res[2] += nums[i];
            }
            res[3] = res[2] / nums.Length;
            return res;

        }
        static void Main(string[] args)
        {
            //写一个方法，求一个数组的最大值，最小值，总和，平均值
            int[] nums = new int[] {0,1,2,3,4,5,6,7,8,9 };
            int[] a=GetMaxMinSumAvg(nums);
            Console.WriteLine("数组的最大值是{0}，最小值是在{1},总和是{2}，平均值是{3}",a[0],a[1],a[2],a[3]);
            Console.ReadKey();
        }
    }
}
