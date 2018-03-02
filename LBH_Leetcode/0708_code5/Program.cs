using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0708_code5
{
    class Program
    {
        public static string LongestPalindrome(string s)
        {
            bool[,] DP = new bool[s.Length, s.Length];
            for(int i=0;i<s.Length;i++)
            {
                for(int j=0;j<s.Length;j++)
                {
                    if(i>=j)//这很精髓
                    {
                        DP[i, j] = true;
                    }
                    else
                    {
                        DP[i, j] = false;
                    }
                }
            }
            int maxlength = 1;
            int fp = 0;
            int lp = 0;
            char[] temp = s.ToCharArray();
            for(int k=1;k<s.Length;k++)
            {
                for(int i=0;i+k<s.Length;i++)
                {
                    int j = i + k;
                    if(temp[i]!=temp[j])
                    {
                        DP[i, j] = false;
                    }
                    else
                    {
                        DP[i, j] = DP[i + 1, j - 1];
                    }
                    if(DP[i,j])
                    {
                        if(k+1>maxlength)
                        {
                            maxlength = k + 1;
                            fp = i;
                            lp = j;
                        }
                    }
                }
            }
            return s.Substring(fp, lp-fp+1);
        }
        static void Main(string[] args)
        {
            string result = LongestPalindrome("eabcb");
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
