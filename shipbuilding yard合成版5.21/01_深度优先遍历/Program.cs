using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_深度优先遍历
{
    public class Vertex
    {
        public string Data;
        public bool IsVisited;
        public Vertex(string Vertexdata)
        {
            Data = Vertexdata;
        }
    }
    public class Graph
    {
        public int Number=5;//图中所能包含的点上限
        private Vertex[] vertiexes;//顶点数组
        public int[,] adjmatrix;//邻接矩阵
        public int numVerts = 0;//统计当前图中有几个点
        public Graph()
        {
            //初始化邻接矩阵和顶点数组
            adjmatrix = new int[Number, Number];
            vertiexes = new Vertex[Number];
            //将代表邻接矩阵的表全初始化为0
            for (int i = 0; i < Number; i++)
            {
                for (int j = 0; j < Number; j++)
                {
                    adjmatrix[i, j] = 0;
                }
            }
        }
      //向图中添加节点
        public void AddVertex(string v)
        {
            vertiexes[numVerts] =new Vertex(v);
            //Console.WriteLine(vertiexes[numVerts]);
            numVerts++;
        }
        //向图中添加无向边
        public void AddEdge(int vertex1, int vertex2)
        {
            adjmatrix[vertex1-1, vertex2-1] = 1;
            adjmatrix[vertex2-1, vertex1-1] = 1;
        }
        public void DisplayVert(int vertexPosition)
        {
            Console.WriteLine(vertiexes[vertexPosition]+" ");
        }
        private int GetAdjUnvisitedVertex(int v)
        {
            for (int j = 0; j <numVerts; j++)
            {
                if (adjmatrix[v, j] == 1 && vertiexes[j].IsVisited == false)
                {
                    return j;
                }
            }
            return -1;
        }
        public void DepthFirstSearch()
        {
            //声明一个存储临时结果的栈
            Stack s = new Stack();
            //先访问第一个节点
            vertiexes[0].IsVisited = true;
            DisplayVert(0);
            s.Push(0);
            int v;
            while (s.Count > 0)
            {
                //获得和当前节点连接的未访问过节点的序号
                v = GetAdjUnvisitedVertex((int)s.Peek());
                if (v == -1)
                {
                    s.Pop();
                }
                else
                {
                    //标记为已经被访问过
                    vertiexes[v].IsVisited = true;
                    DisplayVert(v);
                    s.Push(v);
                }
            }
            //重置所有节点为未访问过
            for (int u = 0; u < numVerts; u++)
            {
                vertiexes[u].IsVisited = false;
            }
        }
}
    class Program
    {
        static void Main(string[] args)
        {
            #region//创建邻接图 myGraph
            Graph myGraph = new Graph();
            for (int i = 1; i <= myGraph.Number; i++)
            {
                string j = Convert.ToString(i);
                Console.WriteLine(j);
                myGraph.AddVertex(j);
            }
            for (int i = 0; i < 7; i++)
            {
                Console.Write("请输入邻接的第一个点：");
                string str1 = Console.ReadLine();
                int num1 = Convert.ToInt32(str1);
                Console.Write("请输入临接的第二个点：");
                string str2 = Console.ReadLine();
                int num2 = Convert.ToInt32(str2);
                myGraph.AddEdge(num1, num2);
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(myGraph.adjmatrix[i, j] + "  ");
                }
                Console.WriteLine();
            }
            #endregion
            myGraph.DepthFirstSearch();
            Console.ReadKey();
        }
    }
}
