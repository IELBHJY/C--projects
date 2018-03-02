using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_图节点着色问题
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] array = new int[5, 5];
            array[0, 1] = array[1, 0] = 1;
            array[0, 2] = array[2, 0] = 1;
            array[0, 3] = array[3, 0] = 1;
            array[1, 4] = array[4, 1] = 1;
            List<List<int>> solution = new List<List<int>>();
            List<List<int>> Tabu = new List<List<int>>();
            Random rand = new Random();
            List<int> num = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int number = rand.Next(0, 5);
                if (!num.Contains(number))
                {
                    num.Add(number);
                }
                else
                {
                    i--;
                }
            }
            List<int> list1= new List<int>();
            List<int> list2 = new List<int>();
            List<int> list3 = new List<int>();
            solution.Add(list1);
            solution.Add(list2);
            solution.Add(list3);
            solution[0].Add(num[0]);
            solution[0].Add(num[1]);
            solution[1].Add(num[2]);
            solution[1].Add(num[3]);
            solution[2].Add(num[4]);
            //计算目标函数
            int f = 0;
            for (int i = 0; i < 3; i++)
            {
                if (solution[i].Count == 1)
                {
                    f += 0;
                }
                else
                {
                    if (array[solution[i][0], solution[i][1]] == 1)
                    {
                        f += 1;
                    }
                }
            }
            while (f != 0)
            { 
            
            }
                Console.ReadKey();
        }
    }
}
