using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0629_双向链表
{
  class Node<T>
  {
    private T data;
    private Node<T> prev;
    private Node<T> next;
    public T Data 
    {
      set { this.data = value; }
      get { return this.data; }
    }
    public Node<T> Prev 
    {
      get { return this.prev; }
      set { this.prev = value; }
    }
    public Node<T> Next 
    {
      get { return this.next; }
      set { this.next = value; }
    }
    public Node(T data, Node<T> next,Node<T> prev)
    {
      this.data = data;
      this.next = next;
      this.prev = prev;
    }
    public Node(T data, Node<T> next) 
    {
      this.data = data;
      this.next = next;
      this.prev = null;
    }
    public Node(Node<T> next) 
    {
      this.data = default(T);
      this.next = next;
      this.prev = null;
    }
    public Node(T data) 
    {
      this.data = data;
      this.next = null;
      this.prev = null;
    }
    public Node() 
    {
      data = default(T);
      next = null;
      prev = null;
    }
  }
}
