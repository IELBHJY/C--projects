using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0705_code1
{
    class Program
    {
        public static int[] TwoSum(int[] nums, int target)
        {
            int[] twosum = new int[2];
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[i] + nums[j] == target)
                    {
                        twosum[0] = i;
                        twosum[1] = j;
                        return twosum;
                    }
                }
            }
            return twosum;
        }
        static void Main(string[] args)
        {
            int[] nums = {2, 7, 11, 15};
            int target = 9;
            int[] twosum = TwoSum(nums, target);
            Console.WriteLine(twosum[0] + "  " + twosum[1]);
            Console.ReadKey();
        }
    }
}
