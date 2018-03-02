using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    class Program
    {
        static int[,] Metro = new int[7, 7] {
              { 0, 3, 7, 5,2048,2048,2048},
              { 3, 0, 2,2048, 6,2048,2048},
              { 7, 2, 0, 3, 3,2048,2048},
              { 5,2048, 3, 0,2048, 2, 8},
              {2048, 6, 3,2048, 0,2048, 2},
              {2048,2048,2048, 2,2048, 0, 2},
              {2048,2048,2048, 8, 2, 2, 0}};
        static int row = 7;
        ArrayList S = new ArrayList(row);//S储存确定最短路径的顶点的下标
        ArrayList U = new ArrayList(row);//U中存储尚未确定路径的顶点的下标
        int[] distance = new int[7];//用以每次查询存放数据
        int[] prev = new int[7];//用以存储前一个最近顶点的下标
        bool[] Isfor = new bool[7] { false, false, false, false, false, false, false };

       void FindWay(int Start)
        {
            S.Add(Start);
            Isfor[Start] = true;
            for (int i = 0; i < row; i++)
            {
                if (i != Start)
                    U.Add(i);
            }
            for (int i = 0; i < row; i++)
            {
                distance[i] = Metro[Start, i];
                prev[i] = 0;
            }
            int Count = U.Count;
            while (Count > 0)
            {
                int min_index = (int)U[0];//假设U中第一个数存储的是最小的数的下标
                foreach (int r in U)
                {
                    if (distance[r] < distance[min_index] && !Isfor[r])
                        min_index = r;
                }
                S.Add(min_index);//S加入这个最近的点
                Isfor[min_index] = true;
                U.Remove(min_index);//U中移除这个点；
                foreach (int r in U)
                {
                    //查找下一行邻接矩阵，如何距离和上一个起点的距离和与数组存储的相比比较小，就更改新的距离和起始点,再比对新的起点到各边的距离
                    if (distance[r] > distance[min_index] + Metro[min_index, r])
                    {
                        distance[r] = distance[min_index] + Metro[min_index, r];
                        prev[r] = min_index;
                    }
                    else
                    {
                        distance[r] = distance[r];
                    }
                }
                Count = U.Count;
            }
        }

        void display(int start)
        {
            for (int i = 0; i < row; i++)
            {
                Console.Write("V{0}到V{1}的最短路径为→V{2}", start,i,start);
                int prePoint = prev[i];
                string s = "";
                StringBuilder sb = new StringBuilder(10);
                while (prePoint > 0)
                {
                    s = (prePoint) + s;
                    prePoint = prev[prePoint];
                }
                for (int j = 0; j < s.Length; j++)
                {
                    sb.Append("-V").Append(s[j]);
                }
                Console.Write(sb.ToString());
                Console.Write("-V{0}", i);
                Console.WriteLine(":{0}", distance[i]);
            }
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            p.FindWay(0);
            p.display(0);
            Console.ReadKey();
        }
    }
}
