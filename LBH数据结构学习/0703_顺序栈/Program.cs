using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0703_顺序栈
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> stack = new Stack<int>(10);
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.print();
            Console.ReadKey();
        }
    }
}
