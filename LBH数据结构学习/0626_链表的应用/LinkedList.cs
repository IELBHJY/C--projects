using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0626_顺序表
{
    class LinkedList
    {
  //首结点、尾结点
  public Node First;
  public Node Last;
  public Node NextNode(Node n)//返回节点n的下一个节点的函数。
  { 
      return n.NextNode; 
  }
  public Node PreviousNode(Node n)//返回节点n的上一个节点的函数。
  { 
      return n.PreviousNode; 
  }
  //结点总数
  public int Count;
  //构造函数
  public LinkedList()
  {
    this.First = null;
    this.Last = null;
    Count = 0;
  }
  /// <summary>
  /// 在结点node1之后增加结点node2，如果没有该结点则在最后增加
  /// </summary>
  /// <param name="node1">结点1</param>
  /// <param name="node2">结点2</param>
  public void AddAfter(Node node1, Node node2)
  {
    //链表为空的情况
    if (First == null)
    {
      Console.WriteLine("Linked-list is null! Can not find node1(" + node1 + ")");
      return;
    }
    Node temp = First;
    do
    {
      if (temp.Data.Equals(node1.Data))
      {
        //如果node1是尾结点
        if (node1.NextNode == null)
        {
          node2.NextNode = null;
          node2.PreviousNode = node1;
          node1.NextNode = node2;
        }
        else //如果node1不是尾结点
        {
          node2.NextNode = node1.NextNode;
          node2.PreviousNode = node1;
          node2.NextNode.PreviousNode = node2;
          node1.NextNode = node2; ;
        }
        Count++;
        Console.WriteLine("Node(" + node2 + "): Add Complete!");
        return;
      }
      temp = temp.NextNode;
    } while (temp != null);
    Console.WriteLine("Can not find node(" + node1 + "), Misson defeat");
  }
  /// <summary>
  /// 在链表尾部增加结点
  /// </summary>
  public void AddLast(Node node)
  {
    //链表为空的情况
    if (this.First == null) 
    {
      node.NextNode = null;
      node.PreviousNode = null;
      this.First = node;
      this.Last = node;
    }
    else //链表不为空的情况
    {
      Node temp = First;
      while(temp.NextNode != null)
      {
        temp = temp.NextNode;
      }
      temp.NextNode = node;
      node.PreviousNode = temp;
      Last = node;
    }
    Count++;
    Console.WriteLine("Node(" + node + "): Add Complete!");
  }
  /// <summary>
  /// 删除指定结点
  /// </summary>
  /// <param name="node">被删除结点</param>
  public void Delete(Node node)
  {
    if (Count == 0)
    {
      Console.WriteLine("Can not find node(" + node + ")");
      return;
    }
    Node temp = First;
    do
    {
      //如果数据部分匹配，则删去该结点
      if (temp.Data.Equals(node.Data))
      {
        //temp是尾结点
        if (temp.NextNode == null)
        {
          temp.PreviousNode.NextNode = null;
          temp = null;
        }
        else //temp不是尾结点 
        {
          temp.PreviousNode.NextNode = temp.NextNode;
          temp.NextNode.PreviousNode = temp.PreviousNode;
          temp = null;
        }
        Count--;
        Console.WriteLine("Node(" + node + "): Delete Complete!");
        return;
      }
      temp = temp.NextNode;
    }
    while (temp != null);
    Console.WriteLine("Can not find node(" + node + "), Misson defeat");
  }
  /// <summary>
  /// 修改结点值
  /// </summary>
  /// <param name="node">被修改结点</param>
  /// <param name="value">结点值</param>
  public void Modify(Node node, object value)
  {
    if (Count == 0)
    {
      Console.WriteLine("Can not find node(" + node + ")");
      return;
    }
    Node temp = First;
    do
    {
      if (temp.Data.Equals(node.Data))
      {
        Console.WriteLine("Node: " + temp.Data + " → " + value.ToString());
        temp.Data = value;
        return;
      }
      temp = temp.NextNode;
    }
    while (temp != null);
  }
  /// <summary>
  /// 打印链表
  /// </summary>
  public void Print()
  {
    if (First == null)
    {
      Console.WriteLine("No nodes in this linked-list.");
      return;
    }
    else
    {
      Console.WriteLine("Print the linked-list...");
      Node temp = First;
      do
      {
        Console.WriteLine(temp.ToString());
        temp = temp.NextNode;
      }
      while (temp != null);
      Console.WriteLine("Mission Complete!");
    }
  }
    }
}
