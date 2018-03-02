using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 遗传算法的简单例子
{
    class Program
    {
        static void Main(string[] args)
        {
            GA myga = new GA();
            myga.creatfirst();
            Console.WriteLine("初始种群：");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Console.Write(myga.fathergeneration[i, j, 0] + "\t");
                }
                Console.WriteLine();
            }
            double[] fit=myga.calfitness(0);
            Console.WriteLine("适应度为：");
            for (int i = 0; i < fit.Length; i++)
            {
                Console.Write(fit[i]+"\t");
            }
            Console.WriteLine();
            for (int g = 0; g < 100; g++)
            {
                myga.choosefather();
                myga.crossover(g);
                //Console.WriteLine("交叉结果为：" + myga.sign1 + " " + myga.sign2);

                myga.mutation(g);
                //Console.WriteLine("交叉变异的结果如下：" + "(" + myga.sign3 + ")");

                myga.reproduce(g);
                //Console.WriteLine("保留的结果如下：" + "(" + myga.sign4 + ")");
                Console.WriteLine("第{0}代种群：",g+1);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        Console.Write(myga.fathergeneration[i, j, g+1] + "\t");
                    }
                    Console.WriteLine();
                }
                double[] fit1 = myga.calfitness(g);
                Console.WriteLine("适应度为：");
                for (int i = 0; i < fit1.Length; i++)
                {
                    Console.Write(fit1[i] + "\t");
                }
                Console.WriteLine();
            }
                Console.ReadKey();
        }
    }
}
