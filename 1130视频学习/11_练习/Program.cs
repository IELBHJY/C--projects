using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_练习
{
    class Program
    {
        //1，将一个数组中的奇数放到一个集合，偶数放到另一个集合，最终将奇数显示在左边，偶数显示在右边。
       //2,提示用户输入一个字符串，通过循环将输入赋值给字符数组
        //3，统计Welcome to china 中每个字符出现的次数，不考虑大小写
        static void Main(string[] args)
        {
            #region//第一道题
            /*
            int[] nums = new int[] { 0,1,2,3,4,5,6,7,8,9,10};
            List<int> jishu = new List<int>();
            List<int> oushu = new List<int>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] % 2 == 0)
                {
                    oushu.Add(nums[i]);
                }
                else
                {
                    jishu.Add(nums[i]);
                }
            }
            foreach (var item in jishu)
            {
                Console.Write(item+" ");
            }
            Console.Write("--------------");
            foreach (var item in oushu)
            {
                Console.Write(item+" ");
            }
             */
            #endregion
            #region//第二道题
            /*Console.WriteLine("Please input a string:");
            string str = Console.ReadLine();
            List<char> newstr = new List<char>();
            foreach (var item in str)
            {
                newstr.Add(item);
            }
            foreach (var item in newstr)
            {
                Console.Write(item);
            }*/
            #endregion
            //找到字符就出现出现次数，把字符作为建，出现次数作为值。
            string str = "Welcome to China";
            string newstr = str.ToLower();
            Dictionary<char, int> dic = new Dictionary<char, int>();
            for (int i = 0; i < str.Length; i++)
            {
                if (newstr[i] ==' ')
                {
                    continue;
                }
                if (dic.ContainsKey(newstr[i]))
                {
                    dic[newstr[i]]++;
                }
                else
                {
                    dic[newstr[i]] = 1;
                }
            }
            foreach(KeyValuePair<char,int> kv in dic)
            {
                Console.WriteLine("{0}出现了{1}次",kv.Key,kv.Value);
            }
            Console.ReadKey();
        }
    }
}
