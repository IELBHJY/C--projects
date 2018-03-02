using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code_M_0625
{
    class Program:TreeNode
    {
        public string Name { get; set; }
        static void Main(string[] args)
        {
//第一行一个整数n (1 ≤ n ≤ 10^5)
//接下来n-1行，每行一个整数，依次为2号点到n号点父亲的编号。
//最后一行n个整数为k[i] (1 ≤ k[i] ≤ 10^5)
//样例解释:
//对节点3操作，导致节点2与节点3变黑
//对节点4操作，导致节点4变黑
//对节点1操作，导致节点1变黑
//4
//1
//2
//1
//1 2 2 1
            Program tree1 = new Program() {Name="1" };
            Program tree2 = new Program() {Name="2" };
            Program tree3 = new Program() {Name="3" };
            tree1.AddChild(tree2, tree3);
        }
    }

    public class TreeNode : IEnumerable
    {
        public TreeNode()
        {
            Childs = new List<TreeNode>();
        }
        public TreeNode Parent { get; set; }
        public List<TreeNode> Childs { get; protected set; }

        public void AddChild(params TreeNode[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].Parent = this;
                Childs.Add(nodes[i]);
            }
        }
        public void RemoveChild(params TreeNode[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].Parent = null;
                Childs.Remove(nodes[i]);
            }
        }
        public List<TreeNode> GetBrothers()
        {
            if (this.Parent != null)
            {
                TreeNode[] childsOfPapa = new TreeNode[Parent.Childs.Count];
                this.Parent.Childs.CopyTo(childsOfPapa);
                List<TreeNode> childsOfPapaList = childsOfPapa.ToList();
                childsOfPapaList.Remove(this);
                return childsOfPapaList;
            }
            return null;
        }
        public IEnumerator GetEnumerator()
        {
            return new TreeEnum(this);
        }
    }
    class TreeEnum : IEnumerator
    {
        private TreeNode rootNode;
        private TreeNode curNode;
        Queue<TreeNode> collection;
        public TreeEnum(TreeNode _collection)
        {
            rootNode = _collection;

            collection = new Queue<TreeNode>();
            FillQueue(rootNode);
            curNode = rootNode;
        }

        private void FillQueue(TreeNode _collection)
        {
            //前序遍历
            collection.Enqueue(_collection);
            if (_collection.Childs != null && _collection.Childs.Count > 0)
                foreach (TreeNode child in _collection.Childs)
                {
                    FillQueue(child);
                }
        }

        public bool MoveNext()
        {
            if (collection.Count > 0)
            {
                curNode = collection.Dequeue();
                return true;
            }
            else
                return false;
        }


        public void Reset()
        {
            collection = new Queue<TreeNode>();
            FillQueue(rootNode);
            curNode = rootNode;
        }


        public TreeNode Current
        {
            get { return curNode; }
        }


        object IEnumerator.Current
        {
            get { return Current; }
        }

    }
}
