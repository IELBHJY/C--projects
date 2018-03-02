using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lbh
{
    class Program
    {
        static int addition(int val1,int val2)
        {
            return val1 + val2;
        }
        static void Main(string[] args)
        {
            int value=addition(2, 3);
            Console.WriteLine("{0}", value);
            Console.ReadKey();
        }
    }
}
