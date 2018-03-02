using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 遗传算法的简单例子
{
    class GA
    {
       int[] genticarray = new int[6];//基因数组。
       public int[,,] songeneration = new int[4,6,110];
       public int[,,] fathergeneration = new int[4,6,110];
       double fitness = 0;
       double[] fitnessarray = new double[4];
        //编码，采用二进制。
        public void creatfirst()
        {
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {     
                for (int j= 0; j< genticarray.Length; j++)
                {
                    int num = r.Next(0, 2);
                    fathergeneration[i,j,0] = num;
                   // Console.Write(num+"\t");
                }
                //Console.WriteLine();
            }
            //Console.ReadKey();
        }

        //计算适应度
        public double[] calfitness(int g)
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 4; i++)
            {
                x = fathergeneration[i,0,g] * 4 + fathergeneration[i,1,g] * 2 + fathergeneration[i,2,g];
                y = fathergeneration[i,3,g] * 4 + fathergeneration[i,4,g] * 2 + fathergeneration[i,5,g];
                fitness = x * x + y * y;
                fitnessarray[i] = fitness;
            }
            return fitnessarray;
        }

        //轮盘赌选择
        public int sign1;
        public int sign2;
        public int sign3;
        public int sign4;
        
        public void choosefather()
        {
            double max1 = 0;
            double max2 = 0;
           for (int i = 0; i < 4; i++)
            {
                if (max1 < fitnessarray[i])
                {
                    max1 = fitnessarray[i];
                    sign1= i;
                   //Console.WriteLine(max1);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (max2 < fitnessarray[i] && i!=sign1)
                {
                    max2 = fitnessarray[i];
                    sign2 = i;
                    //Console.WriteLine(max2);
                }
            }
        }

        /// <summary>
        /// 单点交叉
        /// </summary>
        /// <param name="f">代数</param>
        public void crossover(int f)
        {
            Random r = new Random();
            int num = r.Next(0, 5);
            for (int i = 0; i < num+1; i++)
            {
                fathergeneration[sign1, i, f + 1] = fathergeneration[sign1, i, f];
                fathergeneration[sign2, i, f + 1] = fathergeneration[sign2, i, f];
            }
            for (int i = num+1; i <6; i++)
            {
                fathergeneration[sign1, i, f + 1] = fathergeneration[sign2, i, f];
                fathergeneration[sign2, i, f + 1] = fathergeneration[sign1, i, f];
            }
        }

        /// <summary>
        /// 交换变异
        /// </summary>
        /// <param name="f">代数</param>
        public void mutation(int f)
        {
            Random r = new Random();
            int num1 = r.Next(0,6);
            int num2 = r.Next(0,6);
            int num3 = r.Next(0,4);
            //Console.WriteLine("变异的个体是："+num3+" "+"变异的位置是"+num1);
            for (int i = 0; i < 4; i++)
            {
               if(i!=sign1&&i!=sign2)
                {
                    sign3=i;
                    for (int j = 0; j <6; j++)
                    {
                        if (j != num1)
                        {
                            fathergeneration[sign3, j, f + 1] = fathergeneration[num3, j, f];
                        }
                        else
                        {
                            fathergeneration[sign3, j, f + 1] = 1-fathergeneration[num3, j, f];
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 保留精英个体
        /// </summary>
        /// <param name="f">当前代数</param>
        public void reproduce(int f)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i != sign1 && i != sign2 && i != sign3)
                {
                    sign4 = i;
                }
            }
            for (int j = 0; j < 6; j++)
            {
                fathergeneration[sign4, j, f + 1] = fathergeneration[sign1, j, f];
            }
        }
        public void creatnext()
        { 
        
        }
    }
}
