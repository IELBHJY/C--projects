using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0629_双向链表
{
    public interface IListDS<T>
    {
        int Count();
        void Clear();
    }
    class DoubleLinkedList<T>:IListDS<T>
    {
    private Node<T> head;
    public Node<T> Head
    {
      get { return head; }
      set { head = value; }
    }
    public DoubleLinkedList()
    {
      head = null;
    }
    public T this[int index] 
    {
      get
      {
        return this.GetItemAt(index);
      }
    }
    public int Count()
    {
      Node<T> p = head;
      int len = 0;
      while (p != null)
      {
        len++;
        p = p.Next;
      }
      return len;
    }
    public void Clear()
    {
      head = null;
    }
    public bool IsEmpty()
    {
      return head == null;
    }
    public void Append(T item)
    {
      Node<T> d = new Node<T>(item);
      Node<T> n = new Node<T>();
      if (head == null)
      {
        head = d;
        return;
      }
      n = head;
      while (n.Next != null)
      {
        n = n.Next;
      }
      n.Next = d;
      d.Prev = n;
    }
    public void InsertBefore(T item, int i)
    {
      if (IsEmpty() || i < 0)
      {
        Console.WriteLine("List is empty or Position is error!");
        return;
      }
      //在最开头插入
      if (i == 0)
      {
        Node<T> q = new Node<T>(item);
        q.Next = head;//把"头"改成第二个元素
        head.Prev = q;
        head = q;//把自己设置为"头"
        return;
      }
      Node<T> n = head;
      Node<T> d = new Node<T>();
      int j = 0;
      //找到位置i的前一个元素d
      while (n.Next != null && j < i)
      {
        d = n;
        n = n.Next;
        j++;
      }
      if (n.Next == null) //说明是在最后节点插入(即追加)
      {
        Node<T> q = new Node<T>(item);
        n.Next = q;
        q.Prev = n;
        q.Next = null;
      }
      else
      {
        if (j == i)
        {
          Node<T> q = new Node<T>(item);
          d.Next = q;
          q.Prev = d;
          q.Next = n;
          n.Prev = q;
        }
      }
    }
    public void InsertAfter(T item, int i)
    {
      if (IsEmpty() || i < 0)
      {
        Console.WriteLine("List is empty or Position is error!");
        return;
      }
      if (i == 0)
      {
        Node<T> q = new Node<T>(item);
        q.Next = head.Next;
        head.Next.Prev = q;
        head.Next = q;
        q.Prev = head;
        return;
      }
      Node<T> p = head;
      int j = 0;
      while (p != null && j < i)
      {
        p = p.Next;
        j++;
      }
      if (j == i)
      {
        Node<T> q = new Node<T>(item);
        q.Next = p.Next;
        if (p.Next != null)
        {
          p.Next.Prev = q;
        }
        p.Next = q;
        q.Prev = p;
      }
      else     
      {
        Console.WriteLine("Position is error!");
      }
    }
    public T RemoveAt(int i)
    {
      if (IsEmpty() || i < 0)
      {
        Console.WriteLine("Link is empty or Position is error!");
        return default(T);
      }
      Node<T> q = new Node<T>();
      if (i == 0)
      {
        q = head;
        head = head.Next;
        head.Prev = null;
        return q.Data;
      }
      Node<T> p = head;
      int j = 0;
      while (p.Next != null && j < i)
      {
        j++;
        q = p;
        p = p.Next;
      }
      if (j == i)
      {
        p.Next.Prev = q;
        q.Next = p.Next;        
        return p.Data;
      }
      else
      {
        Console.WriteLine("The node is not exist!");
        return default(T);
      }
    }
    public T GetItemAt(int i)
    {
      if (IsEmpty())
      {
        Console.WriteLine("List is empty!");
        return default(T);
      }
      Node<T> p = new Node<T>();
      p = head;
      if (i == 0) 
      { 
        return p.Data; 
      }
      int j = 0;
      while (p.Next != null && j < i)
      {
        j++;
        p = p.Next;
      }
      if (j == i)
      {
        return p.Data;
      }
      else
      {
        Console.WriteLine("The node is not exist!");
        return default(T);
      }
    }
    public int IndexOf(T value)
    {
      if (IsEmpty())
      {
        Console.WriteLine("List is Empty!");
        return -1;
      }
      Node<T> p = new Node<T>();
      p = head;
      int i = 0;
      while (!p.Data.Equals(value) && p.Next != null)
      {
        p = p.Next;
        i++;
      }
      return i;
    }
    public void Reverse()
    {
      DoubleLinkedList<T> result = new DoubleLinkedList<T>();
      Node<T> t = this.head;
      result.Head = new Node<T>(t.Data);
      t = t.Next;
      //(把当前链接的元素从head开始遍历，逐个插入到另一个空链表中，这样得到的新链表正好元素顺序跟原链表是相反的)
      while (t!=null)
      {        
        result.InsertBefore(t.Data, 0);
        t = t.Next;
      }
      this.head = result.head;//将原链表直接挂到"反转后的链表"上
      result = null;//显式清空原链表的引用，以便让GC能直接回收
    }
    private Node<T> GetNodeAt(int i){
      if (IsEmpty())
      {
        Console.WriteLine("List is empty!");
        return null;
      }
      Node<T> p = new Node<T>();
      p = head;
      if (i == 0)
      {
        return p;
      }
      int j = 0;
      while (p.Next != null && j < i)
      {
        j++;
        p = p.Next;
      }
      if (j == i)
      {
        return p;
      }
      else
      {
        Console.WriteLine("The node is not exist!");
        return null;
      }
    }
    public string TestPrevErgodic() 
    {
      Node<T> tail = GetNodeAt(Count() - 1);
      StringBuilder sb = new StringBuilder();
      sb.Append(tail.Data.ToString() + ",");
      while (tail.Prev != null)
      {
        sb.Append(tail.Prev.Data.ToString() + ",");
        tail = tail.Prev;
      }
      return sb.ToString().TrimEnd(',');      
    }
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      Node<T> n = this.head;
      sb.Append(n.Data.ToString() + ",");
      while (n.Next != null)
      {
        sb.Append(n.Next.Data.ToString() + ",");
        n = n.Next;
      }
      return sb.ToString().TrimEnd(',');
    }
  }
}