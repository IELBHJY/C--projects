using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_3Sum
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IList<IList<int>> result = new List<IList<int>>();
            result = ThreeSum(new int[] {-1, -6, -3, -7, -4, -4, 0, 3, -2, -10, -10, 9 });
            for(int i=0;i<result.Count;i++)
            {
                Console.Write(result[i][0]+" "+result[i][1]+" "+result[i][2]);
                Console.WriteLine();
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
        public static IList<IList<int>> ThreeSum(int[] nums)
        {
            //先排序，按照从头到尾的顺序，先选中一个值，然后设定头尾指针两边扫描
            Array.Sort(nums);
            IList<IList<int>> result = new List<IList<int>>();
            for (int i = 0; i < nums.Length - 2; i++)
            {
                if (i == 0 || (i > 0 && nums[i] != nums[i - 1]))
                {
                    int lo = i + 1, hi = nums.Length - 1, sum = 0 - nums[i];
                    while (lo < hi)
                    {
                        if (nums[lo] + nums[hi] == sum)
                        {
                            IList<int> temp = new List<int>();
                            temp.Add(nums[i]);
                            temp.Add(nums[lo]);
                            temp.Add(nums[hi]);
                            result.Add(temp);
                            while (lo < hi && nums[lo] == nums[lo + 1]) lo++;
                            while (lo < hi && nums[hi] == nums[hi - 1]) hi--;
                            lo++; hi--;
                        }
                        else if (nums[lo] + nums[hi] < sum) lo++;
                        else hi--;
                    }
                }
            }
            return result;     
        }

        public static void Find_2Sum(int[] nums,int begin,int end,int target)
        {
            int l = begin, r = end;
            while (l < r)
            {
                if (nums[l] + nums[r]+target==0)
                {
                    List<int> ans = new List<int>();
                    ans.Add(target);
                    ans.Add(nums[l]);
                    ans.Add(nums[r]);            
                    //result.Add(ans); //放入结果集中  
                    while (l < r && nums[l] == nums[l + 1]) l++;
                    while (l < r && nums[r] == nums[r - 1]) r--;
                    l++;
                    r--;
                }
                else if (nums[l] + nums[r] + target < 0)
                {
                    l++;
                }
                else
                {
                    r--;
                }
            }
        }
    }
}
