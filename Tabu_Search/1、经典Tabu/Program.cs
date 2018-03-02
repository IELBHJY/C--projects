using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_经典Tabu
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int iteration = 100;
            int N_count = 4;
            int customer_number = 6;
            double penty_weight=1;
            double[] value = new double[7];
            double[] object_value = new double[iteration];
            List<string> tabulist = new List<string>();//禁忌表
            List<int> tabulength = new List<int>();
            Tabu tabu = new Tabu(6);           
            tabu.ReadData();
            tabu.initial();
            double ini = tabu.cal_object();
            for (int i = 0; i < iteration; i++)
            {
                Console.WriteLine(i);
                //创建邻域解
                int j = 0;
                int min_label = 0;
                double min = 10000;
                double minc = 10000;
                int length = 1;
                List<int> N_tabulist = new List<int>();
                List<double[]> valuelist = new List<double[]>();  
                if (tabulist.Count != 0)
                {
                    for (int k = 0; k < tabulist.Count; k++)
                    {
                        tabulength[k] -= 1;
                        if (tabulength[k] == 0)
                        {
                            tabulist.RemoveAt(0);
                            tabulength.RemoveAt(0);
                        }
                    }
                }
                Console.WriteLine("A");
                while (j < N_count)
                {
                    Random rand = new Random();
                    int num = rand.Next(0,customer_number);
                    while(N_tabulist.Contains(num))
                    {
                        num = rand.Next(0, customer_number);
                    }
                    N_tabulist.Add(num);
                    int customer_position = tabu.customer_sequence.IndexOf(num);
                    int old_route = tabu.route_sequence[customer_position];
                    int route_number = tabu.cal_route();
                    int new_route = rand.Next(0, route_number + 1);
                    string attribute = num.ToString() + "-" + new_route.ToString();
                    while (new_route == old_route||tabulist.Contains(attribute))
                    {
                        new_route = rand.Next(0, route_number + 1);
                        attribute = num.ToString() + "-" + new_route.ToString();
                    }//如果测试发现有问题，则需更改，把任务也重新生成。
                    value = tabu.creat_N(customer_position, old_route, new_route, penty_weight);
                    valuelist.Add(value);
                    j++;
                }
                Console.WriteLine("B");
                for (int k = 0; k < N_count; k++)
                {
                    if (min > valuelist[k][0])
                    {
                        min = valuelist[k][0];
                        minc = valuelist[k][1];
                        min_label = k;
                    }
                }
                if (valuelist[min_label][2] > 0)
                {
                    penty_weight *= 1.2;
                }
                else
                {
                    penty_weight /= 1.2;
                }
                tabulist.Add(tabu.customer_sequence[(int)valuelist[min_label][3]].ToString() + "-" + tabu.customer_sequence[(int)valuelist[min_label][4]].ToString());
                tabulength.Add(length);
                tabu.modify((int)(valuelist[min_label][3]), (int)(valuelist[min_label][5]));
                object_value[i] = tabu.cal_object();
                Console.WriteLine(tabu.cal_object());
                Console.WriteLine(valuelist[min_label][2]);
            }
            Console.WriteLine("-----------------");
            Console.WriteLine(object_value.Min());
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }
}
