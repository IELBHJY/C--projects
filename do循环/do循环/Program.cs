using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do循环
{
    class Program
    {
        static void Main(string[] args)
        {
            double balance, interestRate, targetBalance;
            Console.WriteLine("what is your current balance?");
            balance = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("what is your current annual interest rate(in %)?");
            interestRate =1+ Convert.ToDouble(Console.ReadLine())/100;
            Console.WriteLine("what balance would you like to have?");
            targetBalance = Convert.ToDouble(Console.ReadLine());
            int totalyears = 0;
            do
            {
                balance *= interestRate;
                ++totalyears;
            } while (balance < targetBalance);
            Console.WriteLine("in {0} year{1} you will have a balance of {2}.", totalyears, totalyears == 1 ? "" : "s", balance);
            Console.ReadKey();
        }
    }
}
