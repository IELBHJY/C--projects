using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 数据交换2
{
    class Program
    {
        static int Sumvals(params int[] vals)
     {
        int sums=0;
    foreach(int val in vals)
    {
    sums+=val;
    }
    return sums;
    }
        static void Main(string[] args)
        {
            int sums = Sumvals(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Console.WriteLine("{0}", sums);
            Console.ReadKey();
        }
    }
}
