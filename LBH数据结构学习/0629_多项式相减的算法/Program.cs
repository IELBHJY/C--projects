using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0629_多项式相减的算法
{
    class Program
    {
        static void Main(string[] args)
        {
            Polynominal p1 = new Polynominal();
            p1.HighPower = 4;
            p1.CoeffArray[0] = 3;
            p1.CoeffArray[1] = 2;
            p1.CoeffArray[2] = 4;
            p1.CoeffArray[3] = 6;
            p1.CoeffArray[4] = 10;
            Polynominal p2 = new Polynominal();
            p2.HighPower = 4;
            p2.CoeffArray[0] = 6;
            p2.CoeffArray[1] = 0;
            p2.CoeffArray[2] = 3;
            p2.CoeffArray[3] = 0;
            p2.CoeffArray[4] = 5;
            Polynominal p = new Polynominal();
            p.addPoly(p1, p2);
            p.printPoly();
            Console.ReadKey();
        }
    }
}
