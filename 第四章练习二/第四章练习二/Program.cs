using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第四章练习二
{
    class Program
    {
        static void Main(string[] args)
        {
            bool numbersOK =false;
            double var1, var2;
            var1= 0;
            var2= 0;
            while (!numbersOK)
            {
                Console.WriteLine("give me a number ");
                var1 = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("give me an another number");
                var2 = Convert.ToDouble(Console.ReadLine());
                if ((var1 > 10) ^ (var2 > 10))
                {
                    numbersOK =true;
                }
                else
                {
                    if ((var1 <= 10) && (var2 <= 10))
                    {
                        numbersOK = true;
                    }
                    else
                    {
                        Console.WriteLine("only one number maybe be greater than 10.");
                    }
                }
            }
            Console.WriteLine("you entered {0}and {1}.", var1, var2);
        }
    }
}