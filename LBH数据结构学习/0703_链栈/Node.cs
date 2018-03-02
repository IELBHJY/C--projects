using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0703_链栈
{
    class Node<T>
    {
        private T data;
        public T Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
        private Node<T> next;
        public Node<T> Next
        {
            get { return this.next; }
            set { this.next = value; }
        }
        public Node(T item,Node<T> p)
        {
            data = item;
            next = p;
        }
        public Node(T item)
        {
            data = item;
            next = null;
        }
        public Node(Node<T> p)
        {
            data = default(T);
            next = p;
        }
        public Node()
        {
            data = default(T);
            next = null;
        }
    }
}
