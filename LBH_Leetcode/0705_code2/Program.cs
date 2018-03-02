using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0705_code2
{
    public class ListNode
    { 
       public int val;
       public ListNode next;
       public ListNode(int x) { val = x; }
    }
    class Program
    {
        public static ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            ListNode result = new ListNode(0);
            ListNode d = result;
            int sum = 0;
            while (l1 != null || l2 != null)
            {
                sum /= 10;
                if (l1 != null)
                {
                    sum += l1.val;
                    l1 = l1.next;
                }
                if (l2 != null)
                {
                    sum += l2.val;
                    l2 = l2.next;
                }
                d.next = new ListNode(sum % 10);
                d = d.next;
            }
            if (sum / 10 == 1)
            {
                d.next = new ListNode(1);
            }
            return result.next;
        }
        static void Main(string[] args)
        {
            ListNode l1 = new ListNode(2);
            ListNode l2 = new ListNode(4);
            ListNode l3 = new ListNode(3);
            ListNode l4 = new ListNode(5);
            ListNode l5 = new ListNode(6);
            ListNode l6 = new ListNode(4);
            ListNode result = new ListNode(0);
            l1.next = l2;
            l2.next = l3;
            l4.next = l5;
            l5.next = l6;
            result = AddTwoNumbers(l1, l4);
            Console.WriteLine(result.val+" "+result.next.val+" "+result.next.next.val);
            Console.ReadKey();
        }
    }
}
