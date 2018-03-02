using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0629_多项式相减的算法
{
    class Polynominal
    {
        private  int highPower;
        public  int HighPower
        {
            get { return this.highPower; }
            set { this.highPower = value; }
        }
        private double[] coeffArray=new double[10000];
        public double[] CoeffArray
        {
            get { return this.coeffArray; }
            set { this.coeffArray = value; }
        }
        public Polynominal()
        {
            highPower = 0;
            for (int i = 0; i < coeffArray.Length; i++)
            {
                coeffArray[i] = 0;
            }
        }
        private int Max(int t1, int t2)
        {
            return (t1 > t2 ? t1 : t2);
        }
        public void addPoly(Polynominal p1, Polynominal p2)
        {
            Polynominal p = new Polynominal();
            this.highPower = Max(p1.HighPower, p2.HighPower);
            for (int i = 0; i <= this.highPower; i++)
            {
                this.coeffArray[i] = p1.coeffArray[i] + p2.coeffArray[i];
            }
        }
        public void printPoly()
        {
            for (int i = 0; i <= highPower; i++)
            {
                Console.Write(CoeffArray[i]+"*"+"x^{0}"+" ",i);
            }
        }
        public static Polynominal operator +(Polynominal p1, Polynominal p2)
        {
            Polynominal p = new Polynominal();
            p.addPoly(p1, p2);
            return p;
        }
    }
}
