using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0607_算法设计作业_MDVRP_
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int tabu_iteration=1000;//迭代次数
            int candidate_length=100;//候选集大小
            int tabu_length = 18;//禁忌长度=[7.5*log(200)]
            List<string> tabuTable = new List<string>();
            List<int> tabuLength = new List<int>();
            List<int> tabu_mark = new List<int>();
            double[] object_Value = new double[tabu_iteration+1];
            double[] F = new double[candidate_length];
            double[] C = new double[candidate_length];
            double[] Q = new double[candidate_length];
            double[] Position = new double[candidate_length];
            double[] newPosition = new double[candidate_length];
            double[] original_depot = new double[candidate_length];
            double[] new_depot = new double[candidate_length];
            double[] customers_number=new double[candidate_length];
            double[] customers_routes=new double[candidate_length];
            double[] teshe_F = new double[tabu_length];
            double[] teshe_C = new double[tabu_length];
            double[] teshe_Q=new double[tabu_length];
            double[] teshe_Position=new double[tabu_length];
            double[] teshe_newPosition=new double[tabu_length];
            double[] teshe_original_depot=new double[tabu_length];
            double[] teshe_new_depot=new double[tabu_length];            
            Tabu_Search ts = new Tabu_Search();
            double[] value_array = new double[7];
            double[] teshe_value_array=new double[7];
            Random rand = new Random();
            double a = 1;
            ts.initial();
            object_Value[0]=ts.cal_object();
            for (int i = 1; i < tabu_iteration; i++)
            {
                List<int> candidate_tabuTable = new List<int>();
                #region//构造邻域解
                int j = 0;
                while (j < candidate_length)
                {
                    if (j < candidate_length / 2)
                    {
                        int customer_number = rand.Next(0, 200);
                        while (candidate_tabuTable.Contains(customer_number))//记录每次迭代中已经选择过的解，在本次迭代中不再选择。
                        {
                            customer_number = rand.Next(0, 200);
                        }
                        int customer_position = 0;
                        int depot_number = 0;
                        int route_number = 0;
                        int new_route = 0;
                        if (ts.first_depot_customers.Contains(customer_number))
                        {
                            depot_number = 0;
                            customer_position = ts.first_depot_customers.IndexOf(customer_number);
                            route_number = ts.first_depot_route[customer_position];
                        }
                        else if (ts.second_depot_customers.Contains(customer_number))
                        {
                            depot_number = 1;
                            customer_position = ts.second_depot_customers.IndexOf(customer_number);
                            route_number = ts.second_depot_route[customer_position];
                        }
                        else
                        {
                            depot_number = 2;
                            customer_position = ts.third_depot_customers.IndexOf(customer_number);
                            route_number = ts.third_depot_route[customer_position];
                        }
                        if (depot_number == 0)
                        {
                            new_route = rand.Next(0, 5);
                            while (new_route == route_number)
                            {
                                new_route = rand.Next(0, 5);
                            }
                        }
                        else if (depot_number == 1)
                        {
                            new_route = rand.Next(0, 2);
                            while (new_route == route_number)
                            {
                                new_route = rand.Next(0, 2);
                            }
                        }
                        else
                        {
                            new_route = rand.Next(0, 3);
                            while (new_route == route_number)
                            {
                                new_route = rand.Next(0, 3);
                            }
                        }
                        string tabu_element = customer_number.ToString() + "-" + new_route.ToString() + "-" + depot_number.ToString();
                        while (tabuTable.Contains(tabu_element))
                        {
                            customer_number = rand.Next(0, 200);
                            while (candidate_tabuTable.Contains(customer_number))
                            {
                                customer_number = rand.Next(0, 200);
                            }
                            customer_position = 0;
                            depot_number = 0;
                            route_number = 0;
                            new_route = 0;
                            if (ts.first_depot_customers.Contains(customer_number))
                            {
                                depot_number = 0;
                                customer_position = ts.first_depot_customers.IndexOf(customer_number);
                                route_number = ts.first_depot_route[customer_position];
                            }
                            else if (ts.second_depot_customers.Contains(customer_number))
                            {
                                depot_number = 1;
                                customer_position = ts.second_depot_customers.IndexOf(customer_number);
                                route_number = ts.second_depot_route[customer_position];
                            }
                            else
                            {
                                depot_number = 2;
                                customer_position = ts.third_depot_customers.IndexOf(customer_number);
                                route_number = ts.third_depot_route[customer_position];
                            }
                            if (depot_number == 0)
                            {
                                new_route = rand.Next(0, 5);
                                while (new_route == route_number)
                                {
                                    new_route = rand.Next(0, 5);
                                }
                            }
                            else if (depot_number == 1)
                            {
                                new_route = rand.Next(0, 2);
                                while (new_route == route_number)
                                {
                                    new_route = rand.Next(0, 2);
                                }
                            }
                            else
                            {
                                new_route = rand.Next(0, 3);
                                while (new_route == route_number)
                                {
                                    new_route = rand.Next(0, 3);
                                }
                            }
                            tabu_element = customer_number.ToString() + "-" + new_route.ToString() + "-" + depot_number.ToString();
                        }
                        value_array = ts.creat_Neighbour(depot_number, customer_position, route_number, new_route, a);
                        F[j] = value_array[0];
                        C[j] = value_array[1];
                        Q[j] = value_array[2];
                        newPosition[j] = value_array[3];
                        original_depot[j] = value_array[4];
                        Position[j] = value_array[5];
                        customers_number[j] = customer_number;
                        customers_routes[j] = route_number;
                        candidate_tabuTable.Add(customer_number);
                    }
                    else
                    {
                        int customer_number = rand.Next(0, 200);
                        while (candidate_tabuTable.Contains(customer_number))
                        {
                            customer_number = rand.Next(0, 200);
                        }
                        int depot_number1 = 0;
                        int depot_number2 = 0;
                        int customer_position = 0;
                        int route_number = 0;
                        int new_route = 0;
                        if (ts.first_depot_customers.Contains(customer_number))
                        {
                            depot_number1 = 0;
                            customer_position = ts.first_depot_customers.IndexOf(customer_number);
                            route_number = ts.first_depot_route[customer_position];
                        }
                        else if (ts.second_depot_customers.Contains(customer_number))
                        {
                            depot_number1 = 1;
                            customer_position = ts.second_depot_customers.IndexOf(customer_number);
                            route_number = ts.second_depot_route[customer_position];
                        }
                        else if (ts.third_depot_customers.Contains(customer_number))
                        {
                            depot_number1 = 2;
                            customer_position = ts.third_depot_customers.IndexOf(customer_number);
                            route_number = ts.third_depot_route[customer_position];
                        }
                        else
                        {
                            Console.WriteLine("任务丢失，出现错误");
                        }
                        if (depot_number1 == 0)
                        {
                            depot_number2 = rand.Next(1, 3);
                            if (depot_number2 == 1)
                            {
                                new_route = rand.Next(0, 2);
                            }
                            else
                            {
                                new_route = rand.Next(0, 3);
                            }
                        }
                        else if (depot_number1 == 1)
                        {
                            int num = rand.Next(0, 2);
                            if (num == 0)
                            {
                                depot_number2 = 0;
                                new_route = rand.Next(0, 5);
                            }
                            else
                            {
                                depot_number2 = 2;
                                new_route = rand.Next(0, 3);
                            }
                        }
                        else if (depot_number1 == 2)
                        {
                            depot_number2 = rand.Next(0, 2);
                            if (depot_number2 == 0)
                            {
                                new_route = rand.Next(0, 5);
                            }
                            else
                            {
                                new_route = rand.Next(0, 2);
                            }
                        }
                        else
                        {
                            Console.WriteLine("错误");
                        }
                        string tabu_element = customer_number.ToString() + "-" + new_route.ToString() + "-" + new_depot.ToString();
                        while (tabuTable.Contains(tabu_element))
                        {
                            //Console.WriteLine("该元素被禁忌！");
                            customer_number = rand.Next(0, 200);
                            while (candidate_tabuTable.Contains(customer_number))
                            {
                                customer_number = rand.Next(0, 200);
                            }
                            depot_number1 = 0;
                            depot_number2 = 0;
                            customer_position = 0;
                            route_number = 0;
                            new_route = 0;
                            if (ts.first_depot_customers.Contains(customer_number))
                            {
                                depot_number1 = 0;
                                customer_position = ts.first_depot_customers.IndexOf(customer_number);
                                route_number = ts.first_depot_route[customer_position];
                            }
                            else if (ts.second_depot_customers.Contains(customer_number))
                            {
                                depot_number1 = 1;
                                customer_position = ts.second_depot_customers.IndexOf(customer_number);
                                route_number = ts.second_depot_route[customer_position];
                            }
                            else if (ts.third_depot_customers.Contains(customer_number))
                            {
                                depot_number1 = 2;
                                customer_position = ts.third_depot_customers.IndexOf(customer_number);
                                route_number = ts.third_depot_route[customer_position];
                            }
                            else
                            {
                                Console.WriteLine("任务丢失，出现错误");
                            }
                            if (depot_number1 == 0)
                            {
                                depot_number2 = rand.Next(1, 3);
                                if (depot_number2 == 1)
                                {
                                    new_route = rand.Next(0, 2);
                                }
                                else
                                {
                                    new_route = rand.Next(0, 3);
                                }
                            }
                            else if (depot_number1 == 1)
                            {
                                int num = rand.Next(0, 2);
                                if (num == 0)
                                {
                                    depot_number2 = 0;
                                    new_route = rand.Next(0, 5);
                                }
                                else
                                {
                                    depot_number2 = 2;
                                    new_route = rand.Next(0, 3);
                                }
                            }
                            else if (depot_number1 == 2)
                            {
                                depot_number2 = rand.Next(0, 2);
                                if (depot_number2 == 0)
                                {
                                    new_route = rand.Next(0, 5);
                                }
                                else
                                {
                                    new_route = rand.Next(0, 2);
                                }
                            }
                            else
                            {
                                Console.WriteLine("错误");
                            }
                        }
                        value_array = ts.creat_Neighbour(depot_number1, depot_number2, customer_position, route_number, new_route, a);
                        F[j] = value_array[0];
                        C[j] = value_array[1];
                        Q[j] = value_array[2];
                        newPosition[j] = value_array[3];
                        new_depot[j] = value_array[4];
                        original_depot[j] = value_array[5];
                        Position[j] = value_array[6];
                        customers_number[j] = customer_number;
                        customers_routes[j] = route_number;
                        candidate_tabuTable.Add(customer_number);
                    }
                    j++;
                }
                #endregion
                double min = F[0];
                int index = 0;
                for (int n = 1; n < candidate_length; n++)
                {
                    if (min > F[n])
                    {
                        min = F[n];
                        index = n;
                    }
                }
                #region//判断特赦规则是否成立，遍历禁忌表中的所有元素，如果禁忌表中元素表现的较好，则将该元素作为本次最优解。
                for (int teshe_count = 0; teshe_count < tabuTable.Count(); teshe_count++)
                {
                    if (tabu_mark[teshe_count] == 1)
                    {
                        string[] temp = new string[3];
                        temp = tabuTable[teshe_count].Split('-');
                        int customer_number = 0;
                        int customer_position = 0;
                        int depot_number = 0;
                        int route_number = 0;
                        int new_route = 0;
                        customer_number = Convert.ToInt32(temp[0]);
                        new_route = Convert.ToInt32(temp[1]);
                        depot_number = Convert.ToInt32(temp[2]);
                        if (ts.first_depot_customers.Contains(customer_number))
                        {
                            depot_number = 0;
                            customer_position = ts.first_depot_customers.IndexOf(customer_number);
                            route_number = ts.first_depot_route[customer_position];
                        }
                        else if (ts.second_depot_customers.Contains(customer_number))
                        {
                            depot_number = 1;
                            customer_position = ts.second_depot_customers.IndexOf(customer_number);
                            route_number = ts.second_depot_route[customer_position];
                        }
                        else
                        {
                            depot_number = 2;
                            customer_position = ts.third_depot_customers.IndexOf(customer_number);
                            route_number = ts.third_depot_route[customer_position];
                        }
                        if (route_number != new_route)
                        {
                            teshe_value_array = ts.creat_Neighbour(depot_number, customer_position, route_number, new_route, a);
                            teshe_F[teshe_count] = teshe_value_array[0];
                            teshe_C[teshe_count] = teshe_value_array[1];
                            teshe_Q[teshe_count] = teshe_value_array[2];
                            teshe_newPosition[teshe_count] = teshe_value_array[3];
                            teshe_original_depot[teshe_count] = teshe_value_array[4];
                            teshe_Position[teshe_count] = teshe_value_array[5];
                        }
                        else
                        {
                            teshe_F[teshe_count] = 10000;
                        }
                    }
                    else
                    {
                        string[] temp = new string[3];
                        temp = tabuTable[teshe_count].Split('-');
                        int customer_number = 0;
                        int customer_position = 0;
                        int depot_number1 = 0;
                        int depot_number2 = 0;
                        int route_number = 0;
                        int new_route = 0;
                        customer_number = Convert.ToInt32(temp[0]);
                        new_route = Convert.ToInt32(temp[1]);
                        depot_number2 = Convert.ToInt32(temp[2]);
                        if (ts.first_depot_customers.Contains(customer_number))
                        {
                            depot_number1 = 0;
                            customer_position = ts.first_depot_customers.IndexOf(customer_number);
                            route_number = ts.first_depot_route[customer_position];
                        }
                        else if (ts.second_depot_customers.Contains(customer_number))
                        {
                            depot_number1 = 1;
                            customer_position = ts.second_depot_customers.IndexOf(customer_number);
                            route_number = ts.second_depot_route[customer_position];
                        }
                        else
                        {
                            depot_number1 = 2;
                            customer_position = ts.third_depot_customers.IndexOf(customer_number);
                            route_number = ts.third_depot_route[customer_position];
                        }
                        if (depot_number1 != depot_number2)
                        {
                            teshe_value_array = ts.creat_Neighbour(depot_number1, depot_number2, customer_position, route_number, new_route, a);
                            teshe_F[teshe_count] = teshe_value_array[0];
                            teshe_C[teshe_count] = teshe_value_array[1];
                            teshe_Q[teshe_count] = teshe_value_array[2];
                            teshe_newPosition[teshe_count] = teshe_value_array[3];
                            teshe_new_depot[teshe_count] = teshe_value_array[4];
                            teshe_original_depot[teshe_count] = teshe_value_array[5];
                            teshe_Position[teshe_count] = teshe_value_array[6];
                        }
                        else
                        {
                            teshe_F[teshe_count] = 10000;
                        }
                    }
                }
                double teshe_min = F[index];
                int teshe_index = -1;
                for (int n = 0; n < tabuTable.Count(); n++)
                {
                    if (teshe_min>teshe_F[n])
                    {
                        teshe_min = teshe_F[n];
                        teshe_index = n;
                    }
                }
                #endregion   
                if (Q[index] > 0)
                {
                    //不是可行解。
                    a *= 1.05;
                    object_Value[i] = object_Value[i - 1];
                }
                else
                {
                    Console.WriteLine("可行解！");//可行解。
                    a /= 1.05;
                    if (teshe_index == -1)//没有特赦解存在
                    {
                        if (C[index] < 0)
                        {
                            object_Value[i] = object_Value[i - 1] + C[index];
                            #region
                            if (index < candidate_length / 2)
                            {
                                if (original_depot[index] == 0)
                                {
                                    int customer = ts.first_depot_customers[Convert.ToInt32(Position[index])];
                                    int route = ts.first_depot_route[Convert.ToInt32(newPosition[index])];
                                    ts.first_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                    ts.first_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    if (Position[index] < newPosition[index])
                                    {
                                        ts.first_depot_customers.RemoveAt(Convert.ToInt32(Position[index]));
                                        ts.first_depot_route.RemoveAt(Convert.ToInt32(Position[index]));
                                    }
                                    else
                                    {
                                        ts.first_depot_customers.RemoveAt(Convert.ToInt32(Position[index] + 1));
                                        ts.first_depot_route.RemoveAt(Convert.ToInt32(Position[index] + 1));
                                    }
                                }
                                else if (original_depot[index] == 1)
                                {
                                    int customer = ts.second_depot_customers[Convert.ToInt32(Position[index])];
                                    int route = ts.second_depot_route[Convert.ToInt32(newPosition[index])];
                                    ts.second_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                    ts.second_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    if (Position[index] < newPosition[index])
                                    {
                                        ts.second_depot_customers.RemoveAt(Convert.ToInt32(Position[index]));
                                        ts.second_depot_route.RemoveAt(Convert.ToInt32(Position[index]));
                                    }
                                    else
                                    {
                                        ts.second_depot_customers.RemoveAt(Convert.ToInt32(Position[index] + 1));
                                        ts.second_depot_route.RemoveAt(Convert.ToInt32(Position[index] + 1));
                                    }
                                }
                                else if (original_depot[index] == 2)
                                {
                                    int customer = ts.third_depot_customers[Convert.ToInt32(Position[index])];
                                    int route = ts.third_depot_route[Convert.ToInt32(newPosition[index])];
                                    ts.third_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                    ts.third_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    if (Position[index] < newPosition[index])
                                    {
                                        ts.third_depot_customers.RemoveAt(Convert.ToInt32(Position[index]));
                                        ts.third_depot_route.RemoveAt(Convert.ToInt32(Position[index]));
                                    }
                                    else
                                    {
                                        ts.third_depot_customers.RemoveAt(Convert.ToInt32(Position[index] + 1));
                                        ts.third_depot_route.RemoveAt(Convert.ToInt32(Position[index] + 1));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("车场号错误");
                                }
                            }
                            else
                            {
                                if (original_depot[index] == 0)
                                {
                                    int customer = ts.first_depot_customers[Convert.ToInt32(Position[index])];
                                    if (new_depot[index] == 1)
                                    {
                                        int route = ts.second_depot_route[Convert.ToInt32(newPosition[index])];
                                        ts.second_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                        ts.second_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    }
                                    else if (new_depot[index] == 2)
                                    {
                                        int route = ts.third_depot_route[Convert.ToInt32(newPosition[index])];
                                        ts.third_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                        ts.third_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    }
                                    else
                                    {
                                        Console.WriteLine("错误1");
                                    }
                                    ts.first_depot_customers.RemoveAt(Convert.ToInt32(Position[index]));
                                    ts.first_depot_route.RemoveAt(Convert.ToInt32(Position[index]));
                                }
                                else if (original_depot[index] == 1)
                                {
                                    int customer = ts.second_depot_customers[Convert.ToInt32(Position[index])];
                                    if (new_depot[index] == 0)
                                    {
                                        int route = ts.first_depot_route[Convert.ToInt32(newPosition[index])];
                                        ts.first_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                        ts.first_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    }
                                    else if (new_depot[index] == 2)
                                    {
                                        int route = ts.third_depot_route[Convert.ToInt32(newPosition[index])];
                                        ts.third_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                        ts.third_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    }
                                    else
                                    {
                                        Console.WriteLine("错误2");
                                    }
                                    ts.second_depot_customers.RemoveAt(Convert.ToInt32(Position[index]));
                                    ts.second_depot_route.RemoveAt(Convert.ToInt32(Position[index]));
                                }
                                else if (original_depot[index] == 2)
                                {
                                    int customer = ts.third_depot_customers[Convert.ToInt32(Position[index])];
                                    if (new_depot[index] == 1)
                                    {
                                        int route = ts.second_depot_route[Convert.ToInt32(newPosition[index])];
                                        ts.second_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                        ts.second_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    }
                                    else if (new_depot[index] == 0)
                                    {
                                        int route = ts.first_depot_route[Convert.ToInt32(newPosition[index])];
                                        ts.first_depot_customers.Insert(Convert.ToInt32(newPosition[index]), customer);
                                        ts.first_depot_route.Insert(Convert.ToInt32(newPosition[index]), route);
                                    }
                                    else
                                    {
                                        Console.WriteLine("错误3");
                                    }
                                    ts.third_depot_customers.RemoveAt(Convert.ToInt32(Position[index]));
                                    ts.third_depot_route.RemoveAt(Convert.ToInt32(Position[index]));
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            object_Value[i] = object_Value[i - 1];
                        }
                    }
                    else//存在特赦解
                    {
                        Console.WriteLine("特赦成立！");
                        if (teshe_C[teshe_index] < 0)
                        {
                            object_Value[i] = object_Value[i - 1] + teshe_C[teshe_index];
                            #region
                            if (tabu_mark[teshe_index]==1)
                            {
                                if (teshe_original_depot[teshe_index] == 0)
                                {
                                    int customer = ts.first_depot_customers[Convert.ToInt32(teshe_Position[teshe_index])];
                                    int route = ts.first_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                    ts.first_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                    ts.first_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    if (teshe_Position[teshe_index] < teshe_newPosition[teshe_index])
                                    {
                                        ts.first_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                        ts.first_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                    }
                                    else
                                    {
                                        ts.first_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index] + 1));
                                        ts.first_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index] + 1));
                                    }
                                }
                                else if (teshe_original_depot[teshe_index] == 1)
                                {
                                    int customer = ts.second_depot_customers[Convert.ToInt32(teshe_Position[teshe_index])];
                                    int route = ts.second_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                    ts.second_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                    ts.second_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    if (teshe_Position[teshe_index] < teshe_newPosition[teshe_index])
                                    {
                                        ts.second_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                        ts.second_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                    }
                                    else
                                    {
                                        ts.second_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index] + 1));
                                        ts.second_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index] + 1));
                                    }
                                }
                                else if (teshe_original_depot[teshe_index] == 2)
                                {
                                    int customer = ts.third_depot_customers[Convert.ToInt32(teshe_Position[teshe_index])];
                                    int route = ts.third_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                    ts.third_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                    ts.third_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    if (teshe_Position[teshe_index] < teshe_newPosition[teshe_index])
                                    {
                                        ts.third_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                        ts.third_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                    }
                                    else
                                    {
                                        ts.third_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index] + 1));
                                        ts.third_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index] + 1));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("车场号错误");
                                }
                            }
                            else
                            {
                                if (teshe_original_depot[teshe_index] == 0)
                                {
                                    int customer = ts.first_depot_customers[Convert.ToInt32(teshe_Position[teshe_index])];
                                    if (teshe_new_depot[teshe_index] == 1)
                                    {
                                        int route = ts.second_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                        ts.second_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                        ts.second_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    }
                                    else if (teshe_new_depot[teshe_index] == 2)
                                    {
                                        int route = ts.third_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                        ts.third_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                        ts.third_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    }
                                    else
                                    {
                                        Console.WriteLine("错误1");
                                    }
                                    ts.first_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                    ts.first_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                }
                                else if (teshe_original_depot[teshe_index] == 1)
                                {
                                    int customer = ts.second_depot_customers[Convert.ToInt32(teshe_Position[teshe_index])];
                                    if (teshe_new_depot[teshe_index] == 0)
                                    {
                                        int route = ts.first_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                        ts.first_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                        ts.first_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    }
                                    else if (teshe_new_depot[teshe_index] == 2)
                                    {
                                        int route = ts.third_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                        ts.third_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                        ts.third_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    }
                                    else
                                    {
                                        Console.WriteLine("错误2");
                                    }
                                    ts.second_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                    ts.second_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                }
                                else if (teshe_original_depot[teshe_index] == 2)
                                {
                                    int customer = ts.third_depot_customers[Convert.ToInt32(teshe_Position[teshe_index])];
                                    if (teshe_new_depot[teshe_index] == 1)
                                    {
                                        int route = ts.second_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                        ts.second_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                        ts.second_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    }
                                    else if (teshe_new_depot[teshe_index] == 0)
                                    {
                                        int route = ts.first_depot_route[Convert.ToInt32(teshe_newPosition[teshe_index])];
                                        ts.first_depot_customers.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), customer);
                                        ts.first_depot_route.Insert(Convert.ToInt32(teshe_newPosition[teshe_index]), route);
                                    }
                                    else
                                    {
                                        Console.WriteLine("错误3");
                                    }
                                    ts.third_depot_customers.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                    ts.third_depot_route.RemoveAt(Convert.ToInt32(teshe_Position[teshe_index]));
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            object_Value[i] = object_Value[i - 1];
                        }
                    }
                    Console.WriteLine("当前解：{0}", object_Value[i]);
                    //将禁忌表中的元素的禁忌次数减少一次。并删去禁忌次数为0的元素。
                    if (tabuTable.Count() > 0)
                    {
                        for (int tabu = 0; tabu < tabuTable.Count(); tabu++)
                        {
                            tabuLength[tabu] -= 1;
                        }
                        if (tabuLength[0] == 0)
                        {
                            tabuTable.RemoveAt(0);
                            tabuLength.RemoveAt(0);
                            tabu_mark.RemoveAt(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("禁忌表为空！");
                    }
                    //将本次最优解添加进禁忌表，添加禁忌长度，并标记是车场内迁移还是车场间迁移，若车场内迁移计为1，车场间迁移计为2。
                    if (teshe_index == -1)
                    {
                        tabuTable.Add(customers_number[index].ToString() + "-" + customers_routes[index].ToString() + "-" + original_depot[index].ToString());
                        tabuLength.Add(tabu_length);
                        if (index < candidate_length / 2)
                        {
                            tabu_mark.Add(1);
                        }
                        else
                        {
                            tabu_mark.Add(2);
                        }
                    }
                }
            }
            #region
            for (int count = 0; count < ts.first_depot_customers.Count(); count++)
            {
                Console.Write(ts.first_depot_customers[count]+1 + "  ");
            }
            Console.WriteLine();
            for (int count = 0; count < ts.first_depot_route.Count(); count++)
            {
                Console.Write(ts.first_depot_route[count]+1 + " ");
            }
            Console.WriteLine();
            for (int count = 0; count < ts.second_depot_customers.Count(); count++)
            {
                Console.Write(ts.second_depot_customers[count]+1 + "  ");
            }
            Console.WriteLine();
            for (int count = 0; count < ts.second_depot_route.Count(); count++)
            {
                Console.Write(ts.second_depot_route[count]+1 + " ");
            }
            Console.WriteLine();
            for (int count = 0; count < ts.third_depot_customers.Count(); count++)
            {
                Console.Write(ts.third_depot_customers[count]+1 + "  ");
            }
            Console.WriteLine();
            for (int count = 0; count < ts.third_depot_route.Count(); count++)
            {
                Console.Write(ts.third_depot_route[count]+1 + " ");
            }
            Console.WriteLine();
            #endregion
            Console.WriteLine(ts.cal_object());
            Console.WriteLine("迭代结束");
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();       
       }
    }
}
