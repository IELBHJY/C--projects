using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0628_循环链表
{
    class Node<T>
    {
        private T tdata;
        public T Data
        {
            get { return this.tdata; }
            set { this.tdata = value; }
        }
        private Node<T> tNext;
        public Node<T> Next
        {
            get { return this.tNext; }
            set { this.tNext = value; }
        }
        public Node()
        {
            tdata = default(T);
            tNext = null;
        }
        public Node(T t)
        {
            this.tdata = t;
            this.Next = null;
        }
        public Node(T t, Node<T> tNext)
        {
            this.tdata = t;
            this.tNext = tNext;
        }
        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
