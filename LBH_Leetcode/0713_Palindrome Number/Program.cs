using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0713_Palindrome_Number
{
    class Program
    {
        public static bool IsPalindrome(int x)
        {
            if(x<0)
            {
                return false;
            }
            int n = 0, temp = x, first = 1, last = 1;
            while(temp!=0)
            {
                n++;
                temp /= 10;
                if(temp!=0) first *= 10;
            }
            for(int i=0;i<n/2;i++)
            {
                if ((x / first) % 10 != (x / last) % 10) return false;
                first /= 10;
                last *= 10;
            }
            return true;
        }
        static void Main(string[] args)
        {
            bool result = IsPalindrome(10);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
