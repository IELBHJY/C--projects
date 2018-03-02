using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0626_顺序表
{
    class Node
    {
        //结点数据，前后结点
        public object Data;
        //public int Data;
        public Node PreviousNode;
        public Node NextNode;
        //构造函数
        public Node(object data = null)
        {
            Data = data;
            PreviousNode = null;
            NextNode = null;
        }
        //输出结点信息
        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
