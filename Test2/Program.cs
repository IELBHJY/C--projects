using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] name = new string[8];
            int[] score = new int[8];
            for (int i = 0; i < name.Length; i++)
            {
                Console.Write("请输入第"+(i+1)+"个学生的姓名：");
                name[i] = Console.ReadLine();
                Console.Write("请输入第"+(i+1)+"个学生的成绩：");
                score[i]=int.Parse(Console.ReadLine());
            }
            int sum = 0, avg,k;
            for (int i = 0; i < score.Length; i++)
            {
                sum += score[i];
            }
            avg = sum / name.Length;
            Console.WriteLine("平均分是{0},高于平均分的有：",avg);
            for (int i = 0; i < name.Length; i++)
            {
                if (score[i] >= avg)
                {
                    k=i;
                    Console.Write(name[k]);
                }
            }
            Console.ReadLine();
        }
    }
}
