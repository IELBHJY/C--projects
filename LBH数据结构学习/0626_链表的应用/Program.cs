using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0626_顺序表
{
    class Program
    {
        static void Main(string[] args)
        {
            #region//顺序表测试
            LinkedList ll = new LinkedList();
            //添加三个结点 1 2（在1后） 3（在2后）
            Node n1 = new Node("A");
            Node n2 = new Node("C");
            Node n3 = new Node("E");
            ll.AddLast(n1);
            ll.AddLast(n2);
            ll.AddLast(n3);
            //添加三个结点 1.5（在1后） 2.5（在2后） 3.5（在3后）
            Node n1dot5 = new Node("B");
            Node n2dot5 = new Node("D");
            Node n3dot5 = new Node("F");
            ll.AddAfter(n1, n1dot5);
            ll.AddAfter(n2, n2dot5);
            ll.AddAfter(n3, n3dot5);
            Console.WriteLine("========================");
            //打印链表
            ll.Print();
            Console.WriteLine("========================");
            //删除结点 2 和 3，将结点 2.5 的值改为 "ThisNodeIsModified!"
            ll.Delete(n2);
            ll.Delete(n3);
            ll.Modify(n2dot5, "ThisNodeIsModified!");
            Console.WriteLine("========================");
            //打印链表
            ll.Print();
            #endregion

            Console.ReadKey();
        }
    }
}
