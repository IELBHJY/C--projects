using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
        
           string[] name = new string[]{"李柏峰","李柏阳","李天宇","齐琳","李天博","李柏鹤","李柏赞"};
            int[] score=new int[]{20,70,50,80,60,90,20};
            int k=0,max=0;
            for (int i = 0; i < score.Length; i++)
            {
                if (score[i] > max)
                {
                    max = score[i];
                    k = i;
                }
            }
          Console.WriteLine("分数最高的是{0},分数是{1}",name[k],max);
            



                Console.ReadLine();
        }
    }
}
