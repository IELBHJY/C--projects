using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12_Integer_to_Roman
{
    class Program
    {
//1~9: {"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"};
//10~90: {"X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"};
//100~900: {"C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"};
//1000~3000: {"M", "MM", "MMM"};
        static void Main(string[] args)
        {
            string result = IntToRoman(2017);
            Console.WriteLine(result);
            Console.ReadKey();
        }
        public static string IntToRoman(int num)
        {
            string result = "";
            int[] number = new int[4];
            int i = 0;
            while(num!=0)
            {
                int tail = num % 10;
                number[3 - i] = tail;
                num /= 10;
                i++;
            }
            switch(number[0])
            {
                case 0:
                    break;
                case 1:
                    result = "M" + result;
                    break;
                case 2:
                    result = "MM" + result;
                    break;
                case 3:
                    result = "MMM" + result;
                    break;
            }
            switch (number[1])
            {
                case 0:
                    break;
                case 1:
                    result+= "C";
                    break;
                case 2:
                    result += "CC";
                    break;
                case 3:
                    result += "CCC";
                    break;
                case 4:
                    result += "CD";
                    break;
                case 5:
                    result += "D";
                    break;
                case 6:
                    result += "DC";
                    break;
                case 7:
                    result += "DCC";
                    break;
                case 8:
                    result += "DCCC";
                    break;
                case 9:
                    result += "CM";
                    break;
            }
            switch (number[2])
            {
                case 0:
                    break;
                case 1:
                    result += "X";
                    break;
                case 2:
                    result += "XX";
                    break;
                case 3:
                    result += "XXX" ;
                    break;
                case 4:
                    result += "XL" ;
                    break;
                case 5:
                    result += "L" ;
                    break;
                case 6:
                    result += "LX" ;
                    break;
                case 7:
                    result += "LXX";
                    break;
                case 8:
                    result += "LXXX";
                    break;
                case 9:
                    result += "XC" ;
                    break;
            }
            switch (number[3])
            {
                case 0:
                    break;
                case 1:
                    result += "I" ;
                    break;
                case 2:
                    result += "II" ;
                    break;
                case 3:
                    result += "III" ;
                    break;
                case 4:
                    result += "IV" ;
                    break;
                case 5:
                    result += "V";
                    break;
                case 6:
                    result += "VI";
                    break;
                case 7:
                    result += "VII";
                    break;
                case 8:
                    result += "VIII";
                    break;
                case 9:
                    result += "IX";
                    break;
            }
            return result;
        }
    }
}
