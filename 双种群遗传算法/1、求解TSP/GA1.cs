using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_求解TSP
{
    class GA1
    {
        //采用正常的进化策略，先交叉再变异
        static int city = 28;
        static int popsize = 40;
        double mutation_rate = 0.8;
        public List<int> select = new List<int>();
        public List<int> label = new List<int>();
        public double[] fitness = new double[popsize];
        public int[,] firstsolution = new int[popsize, city];
        public int[,] fathersolution = new int[popsize, city];
        public int[,] sonsolution = new int[popsize, city];
        Random rand = new Random();
        Data d = new Data();
        public void creatfirstsolution()
        {
            //for(int i=0;i<popsize;i++)
            //{
            //    int num = rand.Next(0, 28);
            //    for (int j = 0; j < city; j++)
            //    {
            //        if (num < city)
            //        {
            //            firstsolution[i, j] = num++;
            //        }
            //        else
            //        {
            //            num -= 28;
            //            firstsolution[i, j] = num++;
            //        }
            //    }
            //}
            d.savedExcel();
            List<int> temp = new List<int>();
            for (int i = 0; i < popsize; i++)
            {
                int num = 0;
                for (int j = 0; j < city; j++)
                {
                    num = rand.Next(0, 28);
                    while (temp.Contains(num))
                    {
                        num = rand.Next(0, 28);
                    }
                    firstsolution[i, j] = num;
                    temp.Add(num);
                }
                temp.Clear();
            }
        }

        public void calfitness()
        {

            for (int i = 0; i < popsize; i++)
            {
                double distance = 0;
                for (int j = 0; j < city - 1; j++)
                {
                    distance += Math.Sqrt(Math.Pow((d.X[firstsolution[i, j]] - d.X[firstsolution[i, j + 1]]), 2) + Math.Pow((d.Y[firstsolution[i, j]] - d.Y[firstsolution[i, j + 1]]), 2));
                }
                distance += Math.Sqrt(Math.Pow((d.X[firstsolution[i, 0]] - d.X[firstsolution[i, city - 1]]), 2) + Math.Pow((d.Y[firstsolution[i, 0]] - d.Y[firstsolution[i, city - 1]]), 2));
                fitness[i] = distance;
            }
        }

        public void selected(int n)
        {
            select.Clear();
            double sum = 0;
            for (int i = 0; i < popsize; i++)
            {
                sum += 2000 - fitness[i];
            }
            for (int i = 0; i < n; i++)
            {
                int target = rand.Next(0, (int)sum);
                int label = 0;
                double temp = 2000 - fitness[label];
                while (temp < target)
                {
                    temp += 2000 - fitness[++label];
                }
                select.Add(label);
            }
        }

        public void Find_best(int n)
        {
            label.Clear();
            for (int j = 0; j < n; j++)
            {
                double min = 10000;
                int min_label = -2;
                for (int i = 0; i < city; i++)
                {
                    if (fitness[i] <= min && !label.Contains(i))
                    {
                        min = fitness[i];
                        min_label = i;
                    }
                }
                label.Add(min_label);
            }
        }
        public void firsttofather()
        {
            for (int i = 0; i < popsize; i++)
            {
                for (int j = 0; j < city; j++)
                {
                    fathersolution[i, j] = firstsolution[i, j];
                }
            }
        }
        public void crossover(int f1, int f2, int s1, int s2)
        {
            //选择交叉点,除去首末两点
            int cross_point = rand.Next(1, city - 1);
            int[] result1 = new int[city - 1 - cross_point];
            int[] result2 = new int[city - 1 - cross_point];
            List<int> crosslist1 = new List<int>();
            List<int> crosslist2 = new List<int>();
            for (int i = cross_point + 1; i < city; i++)
            {
                crosslist1.Add(fathersolution[f1, i]);
                crosslist2.Add(fathersolution[f2, i]);
            }
            int count = 0;
            for (int i = 0; i < city; i++)
            {
                if (crosslist1.Contains(fathersolution[f2, i]))
                {
                    result1[count] = fathersolution[f2, i];
                    crosslist1.Remove(fathersolution[f2, i]);
                    count++;
                }
            }
            count = 0;
            for (int i = 0; i < city; i++)
            {
                if (crosslist2.Contains(fathersolution[f1, i]))
                {
                    result2[count] = fathersolution[f1, i];
                    crosslist2.Remove(fathersolution[f1, i]);
                    count++;
                }
            }
            for (int i = 0; i <= cross_point; i++)
            {
                sonsolution[s1, i] = fathersolution[f1, i];
                sonsolution[s2, i] = fathersolution[f2, i];
            }
            for (int i = cross_point + 1; i < city; i++)
            {
                sonsolution[s1, i] = result1[i - cross_point - 1];
                sonsolution[s2, i] = result2[i - cross_point - 1];
            }
        }

        public void mutation(int s)
        {
            int num = rand.Next(0, 11);
            if (num / 10.0 > mutation_rate)
            {
                //选择变异交叉点
                int num1 = rand.Next(0, city);
                int num2 = rand.Next(0, city);
                while (num2 == num1)
                {
                    num2 = rand.Next(0, city);
                }
                int temp1 = sonsolution[s, num2];
                int temp2 = sonsolution[s, num1];
                sonsolution[s, num1] = temp1;
                sonsolution[s, num2] = temp2;
            }
        }

        public void inheritbest(int f1, int s1)
        {
            for (int i = 0; i < city; i++)
            {
                sonsolution[s1, i] = fathersolution[f1, i];
            }
        }
        public void sontofather()
        {
            for (int i = 0; i < popsize; i++)
            {
                for (int j = 0; j < city; j++)
                {
                    firstsolution[i, j] = sonsolution[i, j];
                }
            }
        }
    }
}
