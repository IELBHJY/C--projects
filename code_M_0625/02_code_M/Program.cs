using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_code_M
{
    class Program
    {
        static void Main(string[] args)
        {
//            美团外卖日订单数已经超过1200万，实时调度系统是背后的重要技术支撑，其中涉及很多复杂的算法。下面的题目是某类场景的抽象。

//一张 n 个点 m 条有向边的图上，有 q 个配送需求，需求的描述形式为( s_i , t_i , l_i , r_i )，即需要从点 s_i 送到 t_i， 在时刻 l_i 之后（包括 l_i）可以在 s_i 领取货物，需要在时刻 r_i 之前（包括 r_i）送达 t_i ，每个任务只需完成一次。 图上的每一条边均有边权，权值代表外卖配送员通过这条边消耗的时间。在时刻 0 有一个配送员在 点 1 上，求他最多能完成多少个配送任务。
//在整个过程中，我们忽略了取餐与最后给用户递餐的时间（实际场景中这两个时间是无法省略的），只考虑花费在路程上的时间。另外，允许在一个点逗留。
//        
        
//        //input：第一行，三个正整数 n , m , q (2 ≤ n ≤ 20, 1 ≤ m ≤ 400, 1 ≤ q ≤ 10)。
//接下来 m 行，每行三个正整数 u_i , v_i , c_i (1 ≤ u_i,v_i ≤ n, 1 ≤ c_i ≤ 20000)，表示有一条从 u_i 到 v_i 耗时为 c_i 的有向边。
//接下来 q 行，每行四个正整数 s_i , t_i , l_i , r_i (1 ≤ s_i,t_i ≤ n, 1 ≤ l_i ≤ r_i ≤ 10^6)，描述一个配送任务。
//            5 4 3
//1 2 1
//2 3 1
//3 4 1
//4 5 1
//1 2 3 4
//2 3 1 2
//3 4 3 4
            // 2
        }
    }
}
