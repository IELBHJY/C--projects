using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Collections;
using System.Reflection;


namespace turnTest
{
    class suanfa
    {
        static int crossno = 10;
        static string exePath = "";
        static int[,] xy = new int[crossno, crossno];
        public  void readdate()
        {
            string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=input.accdb";
            using (OleDbConnection conn = new OleDbConnection(connstr))
            {
                conn.Open();
                OleDbCommand myCommand = conn.CreateCommand();
                for (int i = 1; i < crossno + 1; i++)//读取路口坐标信息
                {
                    try
                    {
                        myCommand.CommandText= "Select * from 路口坐标信息 where 路口ID='" + i.ToString() + "'";
                        OleDbDataReader reader = myCommand.ExecuteReader();                                      
                        while(reader.Read())
                        {
                            xy[0, i] = (int)reader["横坐标"];
                            xy[1, i] = (int)reader["纵坐标"];
                        }
                        reader.Close();
                        conn.Close();
                    }
                    catch (Exception e)
                    {
                        throw (new Exception("数据库出错:" + e.Message));
                    }
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            suanfa a = new suanfa();
            a.readdate();
            Console.ReadKey();
        }
    }
}
//static int maxnum=100;
//static int maxint=10000;
//static int[] dist=new int[maxnum];    // 表示当前点到源点的最短路径长度
//static int[] prev=new int[maxnum];     // 记录当前点的前一个结点
//static int[,] c=new int[maxnum,maxnum];   // 记录图的两点间路径长度
//static int n;
//static int line;             // 图的结点数和路径数
 
//// n -- n nodes
//// v -- the source node
//// dist[] -- the distance from the ith node to the source node
//// prev[] -- the previous node of the ith node
//// c[][] -- every two nodes' distance
//public static void Dijkstra(int n, int v, int[] dist, int[] prev,int[,] c)
//{
//    int[] s=new int[maxnum];    // 判断是否已存入该点到S集合中
//    for(int i=1; i<=n; ++i)
//    {
//        dist[i] = c[v,i];
//        s[i]=0;     // 初始都未用过该点
//        if(dist[i] == maxint)
//            prev[i] = 0;
//        else
//            prev[i] = v;
//    }
//    dist[v] = 0;
//    s[v] = 1;
 
//    // 依次将未放入S集合的结点中，取dist[]最小值的结点，放入结合S中
//    // 一旦S包含了所有V中顶点，dist就记录了从源点到所有其他顶点之间的最短路径长度
//         // 注意是从第二个节点开始，第一个为源点
//    for(int i=2; i<=n; ++i)
//    {
//        int tmp = maxint;
//        int u = v;
//        // 找出当前未使用的点j的dist[j]最小值
//        for(int j=1; j<=n; ++j)
//            if((s[j]==0) && dist[j]<tmp)
//            {
//                u = j;              // u保存当前邻接点中距离最小的点的号码
//                tmp = dist[j];
//            }
//        s[u] = 1;    // 表示u点已存入S集合中
 
//        // 更新dist
//        for(int j=1; j<=n; ++j)
//            if((s[j]==0) && c[u,j]<maxint)
//            {
//                int newdist = dist[u] + c[u,j];
//                if(newdist < dist[j])
//                {
//                    dist[j] = newdist;
//                    prev[j] = u;
//                }
//            }
//    }
//}
 
//// 查找从源点v到终点u的路径，并输出
//public static void searchPath(int[] prev,int v, int u)
//{
//    int[] que=new int[maxnum];
//    int tot = 1;
//    que[tot] = u;
//    tot++;
//    int tmp = prev[u];
//    while(tmp != v)
//    {
//        que[tot] = tmp;
//        tot++;
//        tmp = prev[tmp];
//    }
//    que[tot] = v;
//    for (int i = tot; i >= 1; --i)
//    {
//        if (i != 1)
//        {
//            //cout << que[i] << " -> ";
//            Console.Write(que[i] + "->");
//        }
//        else
//        {
//            //cout << que[i] << endl;
//            Console.Write(que[i]);
//        }
//    }
//}

//  static void Main(string[] args)
// {
//     readdate();
//      //freopen("input.txt", "r", stdin);
//    // 各数组都从下标1开始
//            Console.WriteLine("请输入节点数：");
//            string n = Console.ReadLine();
//            int N=Convert.ToInt32(n);
//            Console.WriteLine("请输入路径数：");
//            string l = Console.ReadLine();
//            int line = Convert.ToInt32(l);
//            int p, q, len;          // 输入p, q两点及其路径长度
 
//    // 初始化c[][]为maxint
//    for (int i = 1; i <= N; ++i)
//    {
//        for (int j = 1; j <= N; ++j)
//        {
//            c[i, j] = maxint;
//        }
//    }
//    for(int i=1; i<=line; ++i)  
//    {
//        //cin >> p >> q >> len;
//        Console.WriteLine("请输入一个点：");
//        string firstnum=Console.ReadLine();
//         p = Convert.ToInt32(firstnum);
//        Console.WriteLine("请输入另一个点：");
//        string secondnum = Console.ReadLine();
//         q = Convert.ToInt32(secondnum);
//        Console.WriteLine("请输入长度：");
//        string length=Console.ReadLine();
//         len=Convert.ToInt32(length);
//        if(len < c[p,q])       // 有重边
//        {
//            c[p,q] = len;      // p指向q
//            c[q,p] = len;      // q指向p，这样表示无向图
//        }
//    }

//    for (int i = 1; i <= N; ++i)
//    {
//        dist[i] = maxint;
//    }
//    for(int i=1; i<=N; ++i)
//    {
//        for (int j = 1; j <= N; ++j)
//        {
//            Console.Write(c[i,j]);
//        }
//         Console.WriteLine();
//    }
 
//    Dijkstra(N, 1,dist,prev,c);
//    Console.WriteLine(dist[N]);
//    searchPath(prev, 1, N);
//    Console.ReadKey();
////1 2 10
////1 4 30
////1 5 100
////2 3 50
////3 5 10
////4 3 20
////4 5 60
//    }
//  }
//}
