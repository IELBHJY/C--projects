using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0703_链栈
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedStack<int> ls = new LinkedStack<int>();
            ls.Push(10);
            ls.Push(20);
            ls.Push(30);
            ls.Push(40);
            ls.Push(50);
            ls.print();
            //ls.Pop();
            //ls.print();
            Console.ReadKey();
        }
    }
}
