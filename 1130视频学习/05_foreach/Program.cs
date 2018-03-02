using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_foreach
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] nums = {1,2,3,4,5,6,7,8,9 };
            for (int i = 0; i < nums.Length; i++)
            {
                Console.WriteLine(nums[i]);
            }
            Console.WriteLine("======================================================");
            foreach (var item in nums)
            {
                Console.WriteLine(item);
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //当循环很多时，foreach比for要耗时很多。
           for (int i = 0; i < 10; i++)
            {
                
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }
}
