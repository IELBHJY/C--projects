using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16_3Sum_Closest
{
    class Program
    {
        static void Main(string[] args)
        {
            int result = ThreeSumClosest(new int[] { 0, 6, 2,0,0,1 }, 1);
            Console.WriteLine(result);
            Console.ReadKey();
        }

        static public int ThreeSumClosest(int[] nums, int target)
        {
            if (nums.Length == 0)
                return 0;
            if (nums.Length == 3)
                return nums[0] + nums[1] + nums[2];
            Array.Sort(nums);
            int result = 0;
            int close = 1000;
            for(int i=0;i<nums.Length-2;i++)
            {
                if(i==0||(i>0&&nums[i]!=nums[i-1]))//此种情况已经包含在上一层中
                {
                    int lo = i + 1; int hi = nums.Length - 1; int first = i;
                    while(lo<hi)
                    {
                        int sum = nums[lo] + nums[hi] + nums[first];
                        if(sum<target)
                        {
                            if(target-sum<close)
                            {
                                close = target - sum;
                                result = sum;
                            }
                            lo++;
                        }
                        else if(sum>target)
                        {
                            if(sum-target<close)
                            {
                                close = sum-target;
                                result = sum;
                            }
                            hi--;
                        }
                        else
                        {
                            close = 0;
                            result = target;
                            do
                            {
                                lo++;
                            } while (lo < hi && nums[lo] == nums[lo + 1]);
                            do
                            {
                                hi--;
                            } while (lo < hi && nums[hi] == nums[hi - 1]);
                        }
                    }
                }
            }
            return result;
        }
    }
}
