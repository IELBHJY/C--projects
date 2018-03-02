using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0712_reverse_integer
{
    class Program
    {
        public static int Reverse(int x)
        {
            int result = 0;
            while (x != 0)
            {
                int tail = x % 10;
                int newResult = result * 10 + tail;
                if ((newResult - tail) / 10 != result)
                { return 0; }
                result = newResult;
                x = x / 10;
            }
            return result;
        }
        static void Main(string[] args)
        {
            int result = Reverse(-2147483648);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
