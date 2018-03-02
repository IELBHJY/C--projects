using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0628_循环链表
{
    class Program
    {
        static void Main(string[] args)
        {
            LoopLinkedList<int> lll = new LoopLinkedList<int>();
            lll.Add(1);
            lll.Add(2);
            lll.Add(3);
            lll.print();
            Console.ReadKey();
        }
    }
}
