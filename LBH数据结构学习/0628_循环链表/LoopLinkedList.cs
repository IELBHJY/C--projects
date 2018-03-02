using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0628_循环链表
{
    public interface IListDS<T>
    {
        int GetLength();
        void Insert(T item, int i);
        void Add(T item);
        bool IsEmpty();
        bool IsFull();
        T GetElement(int i);
        void Delete(int i);
        void Clear();
        int LocateElement(T item);
        void print();
        //void Reverse();
    }
    class LoopLinkedList<T>:IListDS<T>
    {
        private Node<T> head;
        public Node<T> Head
        {
            get { return this.head; }
            set { this.head = value; head.Next = head; }
        }
        public LoopLinkedList()
        {
            this.head = null;
        }
        public LoopLinkedList(Node<T> node)
        {
            this.head = node;
        }
        public Node<T> this[int index]
        {
            set
            {
                if (IsEmpty())
                    throw new Exception("链表为空");
                if (index < 0 || index > this.GetLength() - 1)
                    throw new Exception("索引超出链表长度");
                Node<T> node = head;
                for (int i = 0; i < index; i++)
                {
                    node = node.Next;
                }
                node.Data = value.Data;
                node.Next = value.Next;
            }
            get
            {
                if (IsEmpty())
                    throw new Exception("链表为空");
                if (index < 0 || index > this.GetLength() - 1)
                    throw new Exception("索引超出链表长度");
                Node<T> node = head;
                for (int i = 0; i < index; i++)
                {
                    node = node.Next;
                }
                return node;
            }
        }
        public int GetLength()
        {
            if (IsEmpty())
                return 0;
            int length = 1;
            Node<T> temp = head;
            while (temp.Next != head)
            {
                temp = temp.Next;
                length++;
            }
            return length;
        }
        public void Clear()
        {
            this.head = null;
        }
        public bool IsEmpty()
        {
            if (head == null)
                return true;
            return false;
        }
        public bool IsFull()
        {
            return false;
        }  
        public void Add(T item)  
        {  
            if (IsEmpty())  
            {  
                this.Head = new Node<T>(item);  
                return;  
            }  
            Node<T> node = new Node<T>(item);  
            Node<T> temp = head;  
            while (temp.Next != head)  
            {  
                temp = temp.Next;  
            }  
            temp.Next = node;  
            node.Next = head;  
        }  
        public void Insert(T item, int index)  
        {  
            if (IsEmpty())  
                throw new Exception("数据链表为空");  
            if (index < 0 || index > this.GetLength())  
                throw new Exception("给定索引超出链表长度");  
            Node<T> temp = new Node<T>(item);  
            Node<T> node = head;  
            if (index == 0)  
            {  
                while (node.Next != head)  
                {  
                    node = node.Next;  
                }  
                node.Next = temp;  
                temp.Next = this.head;  
                return;  
            }  
            for (int i = 0; i < index - 1; i++)  
            {  
                node = node.Next;  
            }  
            Node<T> temp2 = node.Next;  
            node.Next = temp;  
            temp.Next = temp2;  
        }   
        public void Delete(int index)  
        {  
            if (IsEmpty())  
                throw new Exception("链表为空，没有可清除的项");  
            if (index < 0 || index > this.GetLength() - 1)  
                throw new Exception("给定索引超出链表长度");  
            Node<T> node = head;  
            if (index == 0)  
            {  
                while (node.Next != head)  
                {  
                    node = node.Next;  
                }  
                this.head = head.Next;  
                node.Next = this.head;  
                return;  
            }  
            for (int i = 0; i < index - 1; i++)  
            {  
                node = node.Next;  
            }  
            node.Next = node.Next.Next;  
        }   
        public T GetElement(int index)  
        {  
            if (index < 0 || index > this.GetLength() - 1)  
                throw new Exception("索引超出链表长度");  
            Node<T> node = head;  
            for (int i = 0; i < index; i++)  
            {  
                node = node.Next;  
            }  
            return node.Data;  
        }  
        public int LocateElement(T value)  
        {  
            if (IsEmpty())  
                throw new Exception("链表为空");  
            Node<T> node = head;  
            int index = 0;  
            while (node.Next != head)  
            {  
                if (node.Data.Equals(value))  
                    return index;  
                else  
                    index++;  
                node = node.Next;  
            }  
            if (node.Data.Equals(value))  
                return index;  
            else  
                return -1;  
        }
        public void print()
        {
            if (head == null)
            {
                Console.WriteLine("this looplinkedlist is empty！");
            }
            else
            {
                Node<T> temp = this.Head;
                while (temp != null)
                {
                    Console.WriteLine(temp.ToString());
                    temp = temp.Next;
                }
                Console.WriteLine("print is completed!");
            }
        }
    }  
}
