using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 函数交换数据1
{
    class Program
    {
        static int Maxvaule(int[] intarray)
        {
            int Maxval = intarray[0];
            for (int i = 1; i < intarray.Length; i++)
            {
                if (Maxval < intarray[i])
                Maxval = intarray[i];
            }
            return Maxval;
        }
        static void Main(string[] args)
        {
            int[] myarray = { 0, 3, 5, 7, 2, 7, 9, 1, 4, 6, 8 };
            int Maxval = Maxvaule(myarray);
            Console.WriteLine("数组中的最大值为 {0}", Maxval);
            Console.ReadKey();
        }
    }
}
