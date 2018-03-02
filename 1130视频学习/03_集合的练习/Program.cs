using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_集合的练习
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建一个集合，里面存放一些数字，求平均值与和
           /* ArrayList list = new ArrayList();
            list.Add("哈哈哈");
            list.AddRange(new int[]{1,2,3,4,5,6,7,8,9});
            int sum=0;
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i] is int)
                {
                sum+=(int)list[i];
                }
            }
            Console.WriteLine("集合中的所有数字之和是{0}",sum);
            Console.ReadKey();
            */

        //长度为10的数字，随机放。并且不重复。
            ArrayList list = new ArrayList();
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                int rnum = r.Next(0,10);
                if (!list.Contains(rnum))
                {
                    list.Add(rnum);
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
            Console.ReadKey();





        }
    }
}
