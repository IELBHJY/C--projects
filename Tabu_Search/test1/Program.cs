using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            double result = 0;
            result = cal(new int[] { 63, 65 }, new int[] { 20, 20 }) - cal(new int[] { 63, 65 }, new int[] { 2, 60 }) - cal(new int[] { 20, 20 }, new int[] { 2, 60 });
            result += cal(new int[] { 13, 52 }, new int[] { 2, 60 }) + cal(new int[] { 2, 60 }, new int[] { 6, 68 }) - cal(new int[] { 13, 52 }, new int[] { 6, 68 });
            Console.WriteLine(result);
            Console.ReadKey();
        }

        static double cal(int[] p1, int[] p2)
        {
            double result = 0;
            result = Math.Pow((Double)(p1[0] - p2[0]), 2) + Math.Pow((Double)(p1[1] - p2[1]), 2);
            result = Math.Sqrt(result);
            return result;
        }
    }
}
