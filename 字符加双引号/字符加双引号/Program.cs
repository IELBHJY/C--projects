using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 字符加双引号
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("input a string");
            string mystring = Console.ReadLine();
            mystring = "\"" + mystring.Replace(" ", "\" \"") + "\"";
            Console.WriteLine("{0}", mystring);
            Console.ReadKey();
        }
    }
}
