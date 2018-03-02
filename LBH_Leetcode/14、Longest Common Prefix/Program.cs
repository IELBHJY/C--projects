using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14_Longest_Common_Prefix
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = LongestCommonPrefix(new string[] { "ca", "ab", "ab" });
            Console.WriteLine(result);
            Console.ReadKey();
        }

        public static string LongestCommonPrefix(string[] strs)
        {
            string result = "";
            string shortest = strs.Length == 0 ? "" : strs[0]; 
            int short_position = 0;
            for(int i=0;i<strs.Length;i++)
            {
                if(strs[i].Length<shortest.Length)
                {
                    shortest = strs[i];
                    short_position = i;
                }
            }
            int count = 0;
            while(count<strs.Length)
            {
                if(strs[count].Substring(0,shortest.Length)==shortest)
                {
                    count++;
                }
                else
                {
                    shortest = shortest.Substring(0, shortest.Length - 1);
                }
            }
            result = shortest;
            return result;
        }
    }
}
