using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            int s = 1;
            int e = 4;
            int n = 6;
            int[] dist = new int[n];
            int[] pre = new int[n];
            int[,] map = new int[6, 6];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    map[i, j] = 100000;
                }
            }
            for (int i = 0; i < n; i++)
            {
                map[i, i] = 0;
            }
            map[0, 1] = 10;
            map[1, 2] = 50;
            map[1, 3] = 10;
            map[1, 5] = 45;
            map[2, 3] = 15;
            map[2, 4] = 50;
            map[2, 5] = 10;
            map[3, 1] = 20;
            map[3, 4] = 15;
            map[4, 2] = 20;
            map[4, 5] = 35;
            map[5, 4] = 30;
            dijkstra(6, 1, 5, map, dist, pre);
            for (int i = 0; i < n; i++)
            {
                Console.Write(pre[i] + "  ");
            }
            Console.ReadKey();
        }
        public static void dijkstra(int n,int s,int e,int[,] map,int[] dist,int[] pre)
        {
            int[] p = new int[n];
          //初始化地图
            for (int i=0;i <n; i++)
            {
                p[i] = 0;
                if (i != s)
                {
                    dist[i] = map[s,i];
                }
            }
            dist[s] = 0;
            p[s] = 1;
            for (int i =0; i <n; i++)
            {
                int min = 100000;
                int k = 0;
                for (int j = 0; j <n; j++)
                {
                    if (p[j] != 1 && min > dist[j])
                    {
                        min = dist[j];
                        k = j;
                    }
                }
                p[k] = 1;
                pre[k] = s;
                for (int j = 0; j < n; j++)
                {
                    if (p[j] != 1 && map[k,j] < 100000)
                    {
                        if (dist[k] + map[k, j] < dist[j])
                        {
                            dist[j] = dist[k] + map[k, j];
                            pre[j] = k;
                        }
                    }
                }
            }
        }
    }
}
