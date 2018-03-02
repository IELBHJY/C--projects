using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0627_单链表
{
  //定义线性表的行为，可供顺序表类和单链表类继承
  public interface IListDS<T>
  {
      int GetLength();
      void Insert(T item, int i);
      void Add(T item);
      bool IsEmpty();
      T GetElement(int i);
      void Delete(int i);
      void Clear();
      int LocateElement(T item);
      void Reverse();
  }
  //链表节点类
  class Node<T>
  {
    private T tData;//数据
    public T Data
    {
        get { return this.tData; }
        set { this.tData = value; }
    }
    private Node<T> nNext;//指针
    public Node<T> Next
    {
      get { return this.nNext; }
      set { this.nNext = value; }
    }
    public Node()
    {
      this.tData = default(T);
      this.nNext = null;
    }
    public Node(T t)
    {
      this.tData = t;
      this.nNext = null;
    }
    public Node(T t,Node<T> node)
    {
      this.tData = t;
      this.nNext = node;
    }
    public override string ToString()
    {
        return this.Data.ToString();
    }
  }
  //该枚举表示单链表Add元素的位置，分头部和尾部两种
  enum AddPosition {Head,Tail};
  //单链表类
  class LinkedList<T>:IListDS<T>
  {
    private Node<T> tHead;//单链表的表头
    public Node<T> Head
    {
      get { return this.tHead; }
      set { this.tHead = value; }
    }
    public LinkedList()
    {
      this.tHead = null;
    }
    public LinkedList(Node<T> node)
    {
      this.tHead = node;
    }
    #region IListDS<T> 成员
    public int GetLength()
    {
        Node<T> node = new Node<T>();
        int count = 0;
        node = this.tHead;
        while (node != null)
        {
            count++;
            node = node.Next;
        }
        return count;
    }
    public void Insert(T item, int i)//i最小从1开始
    {
        Node<T> insertNode = new Node<T>(item, null);//实例化待添加的Node
        if (this.tHead == null && i == 1)
        {
            this.tHead = insertNode;
            return;
        }
        if (i < 1 || i > this.GetLength() || (this.tHead == null && i != 1))
        {
            Console.WriteLine("There are no elements in this linked list!");
            return;
        }
        int j = 1;
        Node<T> node = this.tHead;
        Node<T> nodeTmp;
        while (node != null && j < i)//循环结束时，保证node为第i个node
        {
            node = node.Next;
            j++;
        }
        nodeTmp = node.Next;//原来的单链表的第i+1个node
        node.Next = insertNode;//第i个node后的node修改为待插入的node
        insertNode.Next = nodeTmp;//待插入的node插入后，其后继node为原来链表的第i+1个node
    }
    public void Add(T item)//添加至尾部，时间复杂度为O(n)，如果添加至头部，则会节省循环的开销
    {
        Node<T> LastNode = new Node<T>(item, null);//实例化待添加的Node
        if (this.tHead == null)//当前表为空
        {
            this.tHead = LastNode;
        }
        else
        {
            Node<T> node = this.tHead;
            while (node.Next != null)
            {
                node = node.Next;
            }
            node.Next = LastNode;
        }
    }
    public void Add(T item, AddPosition p)//选择添加位置
    {
        if (p == AddPosition.Tail)
        {
            this.Add(item);//默认添加在末尾
        }
        else//从头部添加会节省查找的开销，时间复杂度为O(1)不必每次都循环到尾部，这恰好是顺序表的优点
        {
            Node<T> node = this.Head;
            Node<T> nodeTmp = new Node<T>(item);
            if (node == null)
            {
                this.Head = nodeTmp;
            }
            else
            {
                nodeTmp.Next = node;
                this.tHead = nodeTmp;
            }
        }
    }
    public bool IsEmpty()
    {
        return this.tHead == null;
    }
    public T GetElement(int i)//设i最小从1开始
    {
        if (i < 1 || i > this.GetLength())
        {
            Console.WriteLine("The location is not right!");
            return default(T);
        }
        else
        {
            if (i == 1)
            {
                return this.tHead.Data;
            }
            else
            {
                Node<T> node = this.tHead;
                int j = 1;
                while (j < i)
                {
                    node = node.Next;
                    j++;
                }
                return node.Data;
            }
        }
    }
    public void Delete(int i)//设i最小从1开始
    {
        if (i < 1 || i > this.GetLength())
        {
            Console.WriteLine("The location is not right!");
        }
        else
        {
            if (i == 1)
            {
                Node<T> node = this.tHead;
                this.tHead = node.Next;
            }
            else
            {
                Node<T> node = this.tHead;
                int j = 1;
                while (j < i - 1)
                {
                    node = node.Next;
                    j++;
                }
                node.Next = node.Next.Next;
            }
        }
    }
    public void Clear()
    {
        this.tHead = null;//讲thead设为null后，则所有后继结点由于失去引用，等待GC自动回收
    }
    public int LocateElement(T item)//返回值最小从1开始
    {
        if (this.tHead == null)
        {
            Console.WriteLine("There are no elements in this linked list!");
            return -1;
        }
        Node<T> node = this.tHead;
        int i = 0;
        while (node != null)
        {
            i++;
            if (node.Data.Equals(item))//如果Data是自定义类型，则其Equals函数必须override
            {
                return i;
            }
            node = node.Next;
        }
        Console.WriteLine("No found!");
        return -1;
    }
    public void Reverse()
    {
        if (this.tHead == null)
        {
            Console.WriteLine("There are no elements in this linked list!");
        }
        else
        {
            Node<T> node = this.tHead;
            if (node.Next == null)//如果只有头节点，则不作任何改动
            {
            }
            else
            {
                Node<T> node1 = node.Next;
                Node<T> node2;
                while (node1 != null)
                {
                    node2 = node.Next.Next;
                    node.Next = node2;//可以发现node始终未变，始终是原来的那个头节点
                    node1.Next = this.tHead;
                    this.tHead = node1;
                    node1 = node2;
                }
            }
        }
    }
    public void print()
    {
        if (this.Head == null)
        {
            Console.WriteLine("This linked list is empty!");
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
    public void show()
    {
        if (this.Head == null)
        {
            Console.WriteLine("This linked list is empty!");
        }
        else
        {
            Node<T> temp = this.Head;
            int i = 0;
            while (temp != null&&Convert.ToInt32(temp.Data)!=0)
            {
                if (i == 0)
                {
                    Console.Write((temp.Data).ToString() + " ", i);
                }
                else if (i == 1)
                {
                    Console.Write((temp.Data).ToString() + "*" + "x" + " ", i);
                }
                else
                {
                    Console.Write((temp.Data).ToString() + "*" + "x^{0}" + " ", i);
                }
                temp = temp.Next;
                i++;
            }
        }
    }
    public static LinkedList<int> addLinkedList(LinkedList<int> LL1, LinkedList<int> LL2)
    {
        LinkedList<int> result = new LinkedList<int>();
        Node<int> temp1 = LL1.Head;
        Node<int> temp2 = LL2.Head;
        while(temp1 != null && temp2 != null)
        {
            result.Add(Convert.ToInt32(temp1.Data) + Convert.ToInt32(temp2.Data));
            temp1 = temp1.Next;
            temp2 = temp2.Next;
        }
        while(temp1 != null)
        {
            result.Add(temp1.Data);
            temp1 = temp1.Next;
        }
        while(temp2!=null)
        {
            result.Add(temp2.Data);
            temp2 = temp2.Next;
        }
        return result;
    }
    #endregion
  }
  class Program
  {
    static void Main(string[] args)
    {
        //单链表实现多项式相加。
        LinkedList<int> poly1 = new LinkedList<int>();
        LinkedList<int> poly2 = new LinkedList<int>();
        LinkedList<int> poly3 = new LinkedList<int>();
        poly1.Add(3);
        poly1.Add(2);
        poly1.Add(4);
        poly1.Add(6);
        poly1.Add(10);
        poly2.Add(6);
        poly2.Add(0);
        poly2.Add(3);
        poly2.Add(0);
        poly2.Add(5);
        poly3 = LinkedList<int>.addLinkedList(poly1, poly2);
        poly3.show();
        Console.ReadKey();
        #region
        //测试单链表的清空
        //lList.Clear();
        //Node<int> n = new Node<int>();
        //n = lList.Head;
        //while (n != null)
        //{
        //    Console.WriteLine(n.Data);
        //    n = n.Next;
        //}
        //Console.ReadLine();
        //Console.ReadKey();
      /*测试单链表插入
      LinkedList<int> lList = new LinkedList<int>();
      lList.Insert(0,1);
      lList.Add(1);
      lList.Add(2);
      lList.Add(3);
      lList.Add(4);
      lList.Insert(99,3);
      Node<int> n = new Node<int>();
      n = lList.Head;
      while (n != null)
      {
        Console.WriteLine(n.Data);
        n = n.Next;
      }
      Console.ReadLine();
      */
      /*测试单链表获取某位置的值
      LinkedList<int> lList = new LinkedList<int>();
      lList.Add(1);
      lList.Add(2);
      lList.Add(3);
      lList.Add(4);
      Console.WriteLine(lList.GetElement(1));
      Console.ReadLine();
       */
      /*测试单链表删除某位置的值
      LinkedList<int> lList = new LinkedList<int>();
      lList.Add(1);
      lList.Add(2);
      lList.Add(3);
      lList.Add(4);
      Node<int> n = new Node<int>();
      n = lList.Head;
      while (n != null)
      {
        Console.WriteLine(n.Data);
        n = n.Next;
      }
      Console.ReadLine();
      lList.Delete(2);
      Node<int> m = new Node<int>();
      m = lList.Head;
      while (m != null)
      {
        Console.WriteLine(m.Data);
        m = m.Next;
      }
      Console.ReadLine();
      */
       /*测试单链表按值查找元素位置
      LinkedList<int> lList = new LinkedList<int>();
      lList.Add(1);
      lList.Add(2);
      lList.Add(3);
      lList.Add(4);
      Console.WriteLine(lList.LocateElement(3));
      Console.ReadLine();
      */
      /*测试单链表Reverse操作（逆序）
      LinkedList<int> lList = new LinkedList<int>();
      lList.Add(1);
      lList.Add(2);
      lList.Add(3);
      lList.Add(4);
      lList.Add(5);
      Node<int> m = new Node<int>();
      m = lList.Head;
      while (m != null)
      {
        Console.WriteLine(m.Data);
        m = m.Next;
      }
      Console.ReadLine();
      lList.Reverse();
      Node<int> n = new Node<int>();
      n = lList.Head;
      while (n != null)
      {
        Console.WriteLine(n.Data);
        n = n.Next;
      }
      Console.ReadLine();
      */
      /*测试单链表从头部添加元素
      LinkedList<int> lList = new LinkedList<int>();
      lList.Add(1,AddPosition.Head);
      lList.Add(2, AddPosition.Head);
      lList.Add(3, AddPosition.Head);
      lList.Add(4, AddPosition.Head);
      lList.Add(5, AddPosition.Head);
      Node<int> m = new Node<int>();
      m = lList.Head;
      while (m != null)
      {
        Console.WriteLine(m.Data);
        m = m.Next;
      }
      Console.ReadLine();
      */
        /*测试对单链表清除重复元素操作（返回另一链表）。这个例子中避免使用Add（）方法，因为每个Add（）都要从头到尾进行遍历，不适用Add（）方法
         就要求对目标链表的最后一个元素实时保存。另一种避免的方法在于从头部Add，但这样的最终结果为倒序
        LinkedList<int> lList = new LinkedList<int>();//原链表
        LinkedList<int> lList2 = new LinkedList<int>();//保存结果的链表
        lList.Add(1);
        lList.Add(2);
        lList.Add(1);
        lList.Add(3);
        lList.Add(3);
        lList.Add(4);
        lList.Add(5);
        Node<int> m = new Node<int>();
        m = lList.Head;
        while (m != null)
        {
          Console.WriteLine(m.Data);
          m = m.Next;
        }
        Console.ReadLine();
        Node<int> node1 = lList.Head;//标识原链表的当前要参与比较大小的元素，即可能放入链表2中的元素
        Node<int> node2;//标识结果单链表的最后一个元素，避免使用Add函数造成多次遍历
        Node<int> node3;//对node1的后继进行暂时保存，并最终付给node1
        node3 = node1.Next;
        lList2.Head = node1;//链表1的头结点肯定要加入链表2
        node2 = lList2.Head;//node2表示链表2的最后一个元素，此时最后一个元素为头结点
        node2.Next = null;//由于是把node1赋给了链表2的头结点，必须把它的后续结点设为null，否则会一起带过来
        node1 = node3;//如果没有node3暂存node1的后继，对lList2.Head后继赋为null的就会同时修改node1的后继，因为两者指向同一块内存
        while (node1 != null)
        {
          //在iList2中比较大小
          Node<int> tmp = lList2.Head;
          if (node1.Data.Equals(tmp.Data))
          {
            node1 = node1.Next;
            continue;//若相等，则node1向后移一位，重新计算
          }
          else
          {
            Node<int> tmp2 = tmp;
            tmp = tmp.Next;//tmp标识链表2的用于循环的节点，与node比较
            if (tmp == null)//当链表2中现有元素与node1都不相等时
            {
              node3 = node1.Next;
              node2.Next = node1;
              node2 = node1;
              node2.Next = null;
              node1 = node3;
              continue;
            }
            while (tmp != null)//tmp不为null时，一直循环到它为null
            {
              if (node1.Data.Equals(tmp.Data))
              {
                node1 = node1.Next;
              }
              else
              {
                tmp2 = tmp;
                tmp = tmp.Next;
                if (tmp == null)
                {
                  node3 = node1.Next;
                  node2.Next = node1;
                  node2 = node1;
                  node2.Next = null;
                  node1 = node3;
                }
              }
            }
          }
        }
        //输出清除重复处理后的数组
        Node<int> n = new Node<int>();
        n = lList2.Head;
        while (n!= null)
        {
          Console.WriteLine(n.Data);
          n = n.Next;
        }
        Console.ReadLine();
        */
        #endregion
    }
  }
}
