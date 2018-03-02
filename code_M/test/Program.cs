using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<int> list=new List<int>();
            //int[] nums = { 1, 9, 3, 5, 4, 1, 1, 9, 7, 9, 4, 7, 1, 3, 2, 2, 8, 9, 6, 8 };
            //list = nums.ToList<int>();
            //Console.WriteLine(list[0]);
            foreach (var v in list.GroupBy(x => x).Select(x => new { k = x.Key, c = x.Count() }))
                Console.WriteLine("{0}出现了{1}次", v.k, v.c);

            int num = 13;
            char[] str = num.ToString().ToCharArray();
            Console.WriteLine(str[0]);
            Console.ReadLine();
        }
    }
}
