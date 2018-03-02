using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 反向字符
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("please input a string");
            string mystring = Console.ReadLine();
            string reservestring = "";
            for (int i = mystring.Length - 1; i >= 0; i--)
            {
                reservestring += mystring[i];
            }
            Console.WriteLine("输入字符的反向字符是 {0}", reservestring);
            Console.ReadKey();
        }
    }
}
