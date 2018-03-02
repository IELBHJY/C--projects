using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0703_链栈
{
    public interface IListStack<T>
    {
        int getLength();
        void Clear();
        bool isEmpty();
        bool isFull();
        void Push(T item);
        T Pop();
        T getPop();
        void print();
    }
    class LinkedStack<T>:IListStack<T>
    {
        private Node<T> top;
        public Node<T> Top
        {
            get { return this.top; }
            set { this.top = value; }
        }
        private int num;
        public int Num
        {
            get { return this.num; }
            set { this.num = value; }
        }
        public LinkedStack()
        {
            top = null;
            num = 0;
        }
        public int getLength()
        {
            return num;
        }
        public void Clear()
        {
            top = null;
            num = 0;
        }
        public bool isEmpty()
        {
            if (top == null && num == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isFull()
        {
            return true;
        }
        public void Push(T item)
        {
            Node<T> q=new Node<T>(item);
            if (isEmpty())
            {
                top = q;
            }
            else
            {
                q.Next = top;
                top = q;
            }
            num++;
        }
        public T Pop()
        {
            if (isEmpty())
            {
                Console.WriteLine("This stack is empty!");
                return default(T);
            }
            else
            {
                Node<T> p = top;
                top = top.Next;
                num--;
                return p.Data;
            }
        }
        public T getPop()
        {
            if (isEmpty())
            {
                Console.WriteLine("This linkedstack is empty!");
                return default(T);
            }
            else
            {
                Node<T> p = top;
                return p.Data;
            }
        }
        public void print()
        {
            if (isEmpty())
            {
                Console.WriteLine("this stack is empty!");
            }
            else
            {
                Node<T> p = top;
                while(p!=null)
                {                
                  Console.WriteLine(p.Data);
                  p = p.Next;
                }
            }
        }
    }
}
