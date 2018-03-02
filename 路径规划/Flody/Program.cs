using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flody
{
    class Program
    {
        private const int MaxSize = 6;
        private const int INF = 32767;   
        private const int MAXV = 4;

        struct VertexType
        {
            public int no;//顶点编号
            public int info;//顶点其他信息
        };

        struct MGraph//图的定义
        {
            public int[,] edges;//邻接矩阵
            public int n, e;//顶点数,弧数
            public VertexType[] vexs;//存放顶点信息
        };

        static void initdata()
        {
            int i, j;
            MGraph g;
            g.n = MAXV; g.e = 8;
            g.edges = new int[MAXV, MAXV];
            g.vexs = new VertexType[MAXV];
            //int [,] anArray = new int [2, 4] {{1,2,3,4},{5,6,7,8}};
            int[,] a = new int[MAXV, MAXV] {
            {0, 5,INF,7},
            {5, 0,  4,1},
            {INF, 4, 0,INF},
            {7,1,INF,0}
            };
            for (i = 0; i < g.n; i++)//建立图的邻接矩阵
                for (j = 0; j < g.n; j++)
                    g.edges[i, j] = a[i, j];
            Console.WriteLine("各顶点的最短路径:");
            Floyd(g);
        }
        static void Floyd(MGraph g)
        {
            int[,] A = new int[MAXV, MAXV];//A用于存放当前顶点之间的最短路径长度,分量A[i][j]表示当前顶点vi到顶点vj的最短路径长度。
            int[,] path = new int[MAXV, MAXV];//从顶点vi到顶点vj的路径上所经过的顶点编号不大于k的最短路径长度。
            int i, j, k;
            for (i = 0; i < g.n; i++)
            {
                for (j = 0; j < g.n; j++)//对各个节点初始已经知道的路径和距离
                {
                    A[i, j] = g.edges[i, j];
                    path[i, j] = -1;
                }
            }
            for (k = 0; k < g.n; k++)
            {
                for (i = 0; i < g.n; i++)
                    for (j = 0; j < g.n; j++)
                        if (A[i, j] > A[i, k] + A[k, j])//从i到j的路径比从i经过k到j的路径长
                        {
                            A[i, j] = A[i, k] + A[k, j];//更改路径长度
                            path[i, j] = k;//更改路径信息经过k
                        }
            }
            Dispath(A, path, g.n);   //输出最短路径
        }

        static void Dispath(int[,] A, int[,] path, int n)
        {

            int i, j;
            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                {
                    if (A[i, j] == INF)
                    {
                        if (i != j)
                        {

                            Console.WriteLine("从{0}到{1}没有路径\n", i, j);
                        }
                    }
                    else
                    {
                        Console.Write("从{0}到{1}=>路径长度:{2} 路径:", i, j, A[i, j]);
                        Console.Write("{0},", i);    //输出路径上的起点
                        Ppath(path, i, j);            //输出路径上的中间点
                        Console.WriteLine("{0}", j);    //输出路径上的终点
                    }
                }
        }

        static void Ppath(int[,] path, int i, int j)  //前向递归查找路径上的顶点
        {
            int k;
            k = path[i, j];
            if (k == -1) return;    //找到了起点则返回
            Ppath(path, i, k);    //找顶点i的前一个顶点k
            Console.Write("{0},", k);    //输出路径上的终点
            Ppath(path, k, j);    //找顶点k的前一个顶点j
        }

        static void Main(string[] args)
        {
            initdata();
            Console.WriteLine(MaxSize);
            Console.WriteLine(MAXV);
            Console.ReadKey();
        }
    }
}
