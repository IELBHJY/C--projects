using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0705_code3
{
    class Program
    {
        //public static int LengthOfLongestSubstring(string s)
        //{
        //    char[] temp = s.ToCharArray();
        //    int count = 0;
        //    List<char> temp1 = new List<char>();
        //    int first = 0;
        //    int end = 0;
        //    int i = 0;
        //    while(i<temp.Length)
        //    {
        //        if (!temp1.Contains(temp[i]))
        //        {
        //            temp1.Add(temp[i]);
        //            end = i;
        //            if (end - first + 1 > count)
        //            {
        //                count = end - first + 1;
        //            }
        //        }
        //        else
        //        {
        //            int length = temp1.Count();
        //            for (int j = 0; j < length; j++)
        //            {
        //                temp1.RemoveAt(0);
        //            }
        //            temp1.Add(temp[first+1]);
        //            first++;
        //            i = first;
        //        }
        //        i++;
        //    }
        //    return count;
        //}

        public static int LengthOfLongestSubstring(string s)
        {
            char[] temp = s.ToCharArray();
            int max = 0;
            Dictionary<char,int> collection=new Dictionary<char,int>();
            for (int i = 0, j = 0; i < temp.Length; i++)
            {
                if (collection.ContainsKey(temp[i]))
                {
                    j = Math.Max(j, collection[temp[i]] + 1);
                    collection.Remove(temp[i]);
                    collection.Add(temp[i], i);
                }
                else
                {
                    collection.Add(temp[i], i);
                }
                max = Math.Max(max, i - j + 1);
            }
            return max;
        }
        static void Main(string[] args)
        {

            int num = LengthOfLongestSubstring("abcadabcabc");
            Console.WriteLine(num);
            Console.ReadKey();
        }
    }
}
