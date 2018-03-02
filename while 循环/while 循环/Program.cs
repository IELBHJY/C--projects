using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace while_循环
{
    class Program
    {
        static void Main(string[] args)
        {
            double balance, interestRate, targetBalance;
            Console.WriteLine("what is your current balance?");
            balance = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("what is your current annual interest rate(in %)?");
            interestRate = 1 + Convert.ToDouble(Console.ReadLine()) / 100;
            Console.WriteLine("what balance would you like to have?");
            targetBalance = Convert.ToDouble(Console.ReadLine());
            int totalyears = 0;
            while(balance<targetBalance)
            {
                balance *= interestRate;
                ++totalyears;
            };
            Console.WriteLine("in {0} year{1} you will have a balance of {2}.", totalyears, totalyears == 1 ? "" : "s", balance);
            if (totalyears == 0)
                Console.WriteLine("to be honesty,you really do not need this calculator!");
            Console.ReadKey();
        }
    }
}
