using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _方法练习2
{
    class Program
    {
        //求一个整型数组的平均值，保留两位小数
        static void Main(string[] args)
        {
            int[] nums = {1,2,7};
           double numsavg= GetAvg(nums);
            //保留两位小数
          string s= numsavg.ToString("0.00");
          numsavg = Convert.ToDouble(s);
          Console.WriteLine(numsavg);
           //Console.WriteLine("这个数组的平均值是：{0:0.00}",numsavg);
           Console.ReadKey();
        }
        /// <summary>
        /// 求一个整型数组的平均值，并保留两位有效数字
        /// </summary>
        /// <param name="nums">数组</param>
        /// <returns>均值</returns>
        public static double GetAvg(int[] nums)
        {
            double sum = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                sum += nums[i];
            }
            double avg = sum / nums.Length;
            return avg;
        }
    }
}
