using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_求解TSP
{
    class Program
    {
        static void Main(string[] args)
        {
            //int city = 28;
            int popsize = 40;
            GA ga = new GA();
            double cross_r = 0.4;
            double mutation_r = 0.2;
            double inherit_r = 0.4;
            //GA1 ga = new GA1();
            //double inherit_r = 0.2;         
            ga.creatfirstsolution();
            for (int itertion = 0; itertion < 1000; itertion++)
            {
                #region//GA进化方式的进化过程
                ga.calfitness();
                ga.Find_best((int)(popsize * inherit_r));
                ga.selected((int)(popsize * (cross_r + mutation_r)));
                ga.firsttofather();
                int i = 0;
                while (i < ((int)(popsize * cross_r)))
                {
                    ga.crossover(ga.select[i], ga.select[i + 1], i, i + 1);
                    i += 2;
                }
                i = (int)(popsize * (cross_r));
                while (i < (int)(popsize * (cross_r + mutation_r)))
                {
                    ga.mutation(ga.select[i], i);
                    i++;
                }
                i = (int)(popsize * (cross_r + mutation_r));
                int inherit_i = 0;
                while (i < popsize)
                {
                    ga.inheritbest(ga.label[inherit_i], i);
                    inherit_i++;
                    i++;
                }
                #endregion
                #region//GA1进化思路进化
                //ga.calfitness();
                //ga.Find_best((int)(popsize * inherit_r));
                //ga.selected((int)(popsize * (1-inherit_r)));
                //ga.firsttofather();
                //int i = 0;
                //int inherit_i = 0;
                //while (i< (int)(popsize * inherit_r))
                //{
                //    ga.inheritbest(ga.label[inherit_i], i);
                //    inherit_i++;
                //    i++;
                //}
                //i = 0;
                //while(i< (int)(popsize * (1 - inherit_r)))
                //{
                //    ga.crossover(ga.select[i], ga.select[i + 1], i+inherit_i, i +inherit_i+ 1);
                //    i += 2;
                //}
                //for(int j=inherit_i;j<popsize;j++)
                //{
                //    ga.mutation(j);
                //}
                #endregion
                ga.sontofather();
            }
            Console.WriteLine("迭代结束！");
            ga.calfitness();
            double min = 1000;
            int label = 0;
            for(int i=0;i<popsize;i++)
            {
                if(ga.fitness[i]<min)
                {
                   min = ga.fitness[i];
                   label = i;
                }
            }
            Console.WriteLine(ga.fitness[label]+"  "+min);
            Console.ReadKey();
        }

    }
}
