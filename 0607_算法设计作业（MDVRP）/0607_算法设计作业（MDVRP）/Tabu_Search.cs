using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0607_算法设计作业_MDVRP_
{
    class Tabu_Search
    {
        double[,] dis_cus_and_deport = new double[3,200];
        List<List<int>> deport_customers = new List<List<int>>();
        public List<int> first_sequence_customers = new List<int>();
        public List<int> second_sequence_customers = new List<int>();
        public List<int> third_sequence_customers = new List<int>();
        public List<int> first_depot_customers = new List<int>();
        public List<int> first_depot_route = new List<int>();
        public List<int> second_depot_customers = new List<int>();
        public List<int> second_depot_route = new List<int>();
        public List<int> third_depot_customers = new List<int>();
        public List<int> third_depot_route = new List<int>();
        Readdate readdate = new Readdate();
        /// <summary>
        /// 构造初始解
        /// </summary>
        public void initial()
        {
            readdate.read_Input();
            for(int j=0;j<3;j++)
            {
               for (int i = 0; i < 200; i++)
               {
                  dis_cus_and_deport[j,i] = Math.Pow(Math.Abs(readdate.deport_X[j] - readdate.customers_X[i]), 2) + Math.Pow(Math.Abs(readdate.deport_Y[j] - readdate.customers_Y[i]), 2);
               }
            }
            for (int i = 0; i < 3; i++)
            {
                deport_customers.Add(new List<int>());
            }
            for (int i = 0; i < 200; i++)
            {
                int j = 0;
                double min = 1000000;
                int deport_num = -1;
                for (j = 0; j < 3; j++)
                {
                    if (dis_cus_and_deport[j, i] < min)
                    {
                        min=dis_cus_and_deport[j, i];
                        deport_num = j;
                    }
                }
                deport_customers[deport_num].Add(i);
            }
            //针对第一个车场点构建初始解
            Random rand = new Random();
            int number = rand.Next(0, deport_customers[0].Count());
            int t0 = 0;
            int l_count = 0;
            for (int i = number; i < deport_customers[0].Count(); i++)
            {
                first_sequence_customers.Add(deport_customers[0][i]);
            }
            for (int i = 0; i < number; i++)
            {
                first_sequence_customers.Add(deport_customers[0][i]);
            }
            int sum_weight = readdate.demand[first_sequence_customers[l_count]];
            while (l_count < deport_customers[0].Count())
            {
                if (sum_weight>350)
                {
                    sum_weight = readdate.demand[first_sequence_customers[l_count]];
                    first_depot_customers.Add(first_sequence_customers[l_count]);
                    first_depot_route.Add(t0);
                    t0++;
                    l_count++;
                }
                else
                {
                    if ((sum_weight + readdate.demand[first_sequence_customers[l_count]]) > 350)
                    {
                        sum_weight = readdate.demand[first_sequence_customers[l_count]];
                        first_depot_customers.Add(first_sequence_customers[l_count]);
                        first_depot_route.Add(t0);
                        t0++;
                        l_count++;
                    }
                    else
                    {
                        sum_weight += readdate.demand[first_sequence_customers[l_count]];
                        first_depot_customers.Add(first_sequence_customers[l_count]);
                        first_depot_route.Add(t0);
                        l_count++;
                    }
                }
            }

            //针对第二个车场构建初始解
            Random rand1 = new Random();
            int number1 = rand.Next(0, deport_customers[1].Count());
            int t1 = 0;
            int l_count1 = 0;
            for (int i = number1; i < deport_customers[1].Count(); i++)
            {
                second_sequence_customers.Add(deport_customers[1][i]);
            }
            for (int i = 0; i < number1; i++)
            {
                second_sequence_customers.Add(deport_customers[1][i]);
            }
            sum_weight = readdate.demand[second_sequence_customers[l_count1]];
            while (l_count1 < deport_customers[1].Count())
            {
                if (sum_weight > 350)
                {
                    sum_weight = readdate.demand[second_sequence_customers[l_count1]];
                    second_depot_customers.Add(second_sequence_customers[l_count1]);
                    second_depot_route.Add(t1);
                    t1++;
                    l_count1++;
                }
                else
                {
                    if ((sum_weight + readdate.demand[second_sequence_customers[l_count1]]) > 350)
                    {
                        sum_weight = readdate.demand[second_sequence_customers[l_count1]];
                        second_depot_customers.Add(second_sequence_customers[l_count1]);
                        second_depot_route.Add(t1);
                        t1++;
                        l_count1++;
                    }
                    else
                    {
                        sum_weight += readdate.demand[second_sequence_customers[l_count1]];
                        second_depot_customers.Add(second_sequence_customers[l_count1]);
                        second_depot_route.Add(t1);
                        l_count1++;
                    }
                }
            }
            //针对第三个车场构建初始解
            Random rand2 = new Random();
            int number2 = rand.Next(0, deport_customers[2].Count());
            int t2 = 0;
            int l_count2 = 0;
            for (int i = number2; i < deport_customers[2].Count(); i++)
            {
                third_sequence_customers.Add(deport_customers[2][i]);
            }
            for (int i = 0; i < number2; i++)
            {
                third_sequence_customers.Add(deport_customers[2][i]);
            }
            sum_weight = readdate.demand[third_sequence_customers[l_count2]];
            while (l_count2 < deport_customers[2].Count())
            {
                if (sum_weight > 350)
                {
                    sum_weight = readdate.demand[third_sequence_customers[l_count2]];
                    third_depot_customers.Add(third_sequence_customers[l_count2]);
                    third_depot_route.Add(t2);
                    t2++;
                    l_count2++;
                }
                else
                {
                    if ((sum_weight + readdate.demand[third_sequence_customers[l_count2]]) > 350)
                    {
                        sum_weight = readdate.demand[third_sequence_customers[l_count2]];
                        third_depot_customers.Add(third_sequence_customers[l_count2]);
                        third_depot_route.Add(t2);
                        t2++;
                        l_count2++;
                    }
                    else
                    {
                        sum_weight += readdate.demand[third_sequence_customers[l_count2]];
                        third_depot_customers.Add(third_sequence_customers[l_count2]);
                        third_depot_route.Add(t2);
                        l_count2++;
                    }
                }
            }
        }

        /// <summary>
        /// 计算当前解的目标函数值
        /// </summary>
        /// <returns></returns>
        public double cal_object()
        {
            //计算第一个车场的客户信息的目标函数。
            #region
            double object_Value = 0;
            object_Value += cal_depot_distence(0, first_depot_customers[0]);
            for (int i = 0; i < first_depot_customers.Count(); i++)
            {
                if (first_depot_route[i] == 0 && first_depot_route[i + 1] == 0)
                {
                    object_Value += cal_customers_distence(first_depot_customers[i], first_depot_customers[i + 1]);
                }
                else if(first_depot_route[i]==0&&first_depot_route[i+1]!=0)
                {
                    object_Value += cal_depot_distence(0, first_depot_customers[i]);
                    object_Value += cal_depot_distence(0, first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 1 && first_depot_route[i + 1] == 1)
                {
                    object_Value += cal_customers_distence(first_depot_customers[i], first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 1 && first_depot_route[i + 1] != 1)
                {
                    object_Value += cal_depot_distence(0, first_depot_customers[i]);
                    object_Value += cal_depot_distence(0, first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 2 && first_depot_route[i + 1] == 2)
                {
                    object_Value += cal_customers_distence(first_depot_customers[i], first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 2 && first_depot_route[i + 1] != 2)
                {
                    object_Value += cal_depot_distence(0, first_depot_customers[i]);
                    object_Value += cal_depot_distence(0, first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 3 && first_depot_route[i + 1] == 3)
                {
                    object_Value += cal_customers_distence(first_depot_customers[i], first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 3 && first_depot_route[i + 1] != 3)
                {
                    object_Value += cal_depot_distence(0, first_depot_customers[i]);
                    object_Value += cal_depot_distence(0, first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 4 && i != (first_depot_route.Count() - 1))
                {
                    object_Value += cal_customers_distence(first_depot_customers[i], first_depot_customers[i + 1]);
                }
                else if (first_depot_route[i] == 4 && i == (first_depot_route.Count() - 1))
                {
                    object_Value += cal_depot_distence(0, first_depot_customers[i]);
                }
                else
                {
                    Console.WriteLine("第一个车场的目标函数时出现错误。");
                }
            }
            #endregion
            //计算第二个车场的客户信息的目标函数。
            #region
            object_Value += cal_depot_distence(1, second_depot_customers[0]);
            for (int i = 0; i < second_depot_customers.Count(); i++)
            {
                if (second_depot_route[i] == 0 && second_depot_route[i + 1] == 0)
                {
                    object_Value += cal_customers_distence(second_depot_customers[i], second_depot_customers[i + 1]);
                }
                else if (second_depot_route[i] == 0 && second_depot_route[i + 1] != 0)
                {
                    object_Value += cal_depot_distence(1, second_depot_customers[i]);
                    object_Value += cal_depot_distence(1, second_depot_customers[i + 1]);
                }
                else if (second_depot_route[i] == 1 && i!=(second_depot_route.Count()-1))
                {
                    object_Value += cal_customers_distence(second_depot_customers[i], second_depot_customers[i + 1]);
                }
                else if (second_depot_route[i] == 1 && i == (second_depot_route.Count() - 1))
                {
                    object_Value += cal_depot_distence(1, second_depot_customers[i]);
                }
                else 
                {
                    Console.WriteLine("第二个车场的目标函数值时出现错误！");
                }
            }
            #endregion
            //计算第三个车场的客户信息的目标函数。
            #region
            object_Value += cal_depot_distence(2, third_depot_customers[0]);
            for (int i = 0; i < third_depot_customers.Count(); i++)
            {
                if (third_depot_route[i] == 0 && third_depot_route[i + 1] == 0)
                {
                    object_Value += cal_customers_distence(third_depot_customers[i], third_depot_customers[i + 1]);
                }
                else if (third_depot_route[i] == 0 && third_depot_route[i + 1] != 0)
                {
                    object_Value += cal_depot_distence(2, third_depot_customers[i]);
                    object_Value += cal_depot_distence(2, third_depot_customers[i + 1]);
                }
                else if (third_depot_route[i] == 1 && third_depot_route[i + 1] == 1)
                {
                   object_Value += cal_customers_distence(third_depot_customers[i], third_depot_customers[i + 1]);
                }
                else if (third_depot_route[i] == 1 && third_depot_route[i + 1] != 1)
                {
                    object_Value += cal_depot_distence(2, third_depot_customers[i]);
                    object_Value += cal_depot_distence(2, third_depot_customers[i + 1]);
                }
                else if (third_depot_route[i] == 2 && i != third_depot_route.Count() - 1)
                {
                    object_Value += cal_customers_distence(third_depot_customers[i], third_depot_customers[i + 1]);
                }
                else if (third_depot_route[i] == 2 && i == (third_depot_route.Count() - 1))
                {
                    object_Value += cal_depot_distence(2, third_depot_customers[i]);
                }
                else
                {
                    Console.WriteLine("第三个车场的目标函数值计算出现错误！");
                }
            }
            #endregion
            return object_Value;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="depot">车场号</param>
        /// <param name="customer">客户号</param>
        /// <returns>距离值</returns>
        public double cal_depot_distence(int depot,int customer)
        {
            int temp1 = readdate.deport_X[depot] - readdate.customers_X[customer];
            int temp2 = readdate.deport_Y[depot] - readdate.customers_Y[customer];
            double tem1 =Math.Sqrt(Math.Pow(Convert.ToDouble(temp1), 2) + Math.Pow(Convert.ToDouble(temp2), 2));
            return tem1;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="customer1">客户1号</param>
        /// <param name="customer2">客户2号</param>
        /// <returns>距离值</returns>
        public double cal_customers_distence(int customer1,int customer2)
        {
            int temp1 = readdate.customers_X[customer1] - readdate.customers_X[customer2];
            int temp2 = readdate.customers_Y[customer1] - readdate.customers_Y[customer2];
            double tem1 =Math.Sqrt(Math.Pow(Convert.ToDouble(temp1), 2) + Math.Pow(Convert.ToDouble(temp2), 2));
            return tem1;
        }

        /// <summary>
        /// 同一车场内Move后的目标函数改变
        /// </summary>
        /// <param name="depot_number">车场号</param>
        /// <param name="customer_position">客户在depot_number车场上的位置</param>
        /// <param name="new_position">客户在depot_number车场上的新位置</param>
        /// <returns>目标函数值的变化</returns>
        public double cal_object_change(int depot_number,int customer_position,int new_position)
        {
            double change1 = 0;
            double change2 = 0;
            if (depot_number == 0)
            {
                int route0_number = -1;
                int route1_number = -1;
                int route2_number = -1;
                int route3_number = -1;
                int route4_number = -1;
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 3)
                    {
                        route3_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 4)
                    {
                        route4_number = i;
                        break;
                    }
                }
                //所选客户是路径上的第一个点，则需要计算下一个点与车场点的距离。
                if (customer_position == route0_number || customer_position == route1_number || customer_position == route2_number || customer_position == route3_number || customer_position == route4_number)
                {
                    change1 = cal_depot_distence(depot_number, first_depot_customers[customer_position + 1]) - cal_depot_distence(depot_number, first_depot_customers[customer_position]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position + 1]);
                }
                //所选客户是路径上的最后一个点
                else if (customer_position == route1_number - 1 || customer_position == route2_number - 1 || customer_position == route3_number - 1 || customer_position == route4_number - 1 || customer_position == first_depot_customers.Count() - 1)
                {
                    change1 = cal_depot_distence(depot_number, first_depot_customers[customer_position - 1]) - cal_depot_distence(depot_number, first_depot_customers[customer_position]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position - 1]);
                }
                else
                {
                    change1 = cal_customers_distence(first_depot_customers[customer_position + 1], first_depot_customers[customer_position - 1]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position - 1]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position + 1]);
                }
                if (new_position == route0_number || new_position == route1_number || new_position == route2_number || new_position == route3_number || new_position == route4_number)
                {
                    change2 = cal_depot_distence(depot_number, first_depot_customers[customer_position]) + cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[new_position]) - cal_depot_distence(depot_number, first_depot_customers[new_position]);
                }
                //else if (new_position == route1_number - 1 || new_position == route2_number - 1 || new_position == route3_number - 1 || new_position == route4_number - 1 || new_position == first_depot_customers.Count() - 1)
                //{
                //    change2 = cal_depot_distence(depot_number, first_depot_customers[customer_position]) + cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[new_position]) - cal_depot_distence(depot_number, new_position);
                //}
                else
                {
                    change2 = cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[new_position - 1]) + cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[new_position]) - cal_customers_distence(first_depot_customers[new_position-1], first_depot_customers[new_position]);
                }
              }
            else if (depot_number == 1)
            {
                int route0_number = 0;
                int route1_number = 0;
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                if (customer_position == route0_number || customer_position == route1_number)
                {
                    change1 = cal_depot_distence(depot_number, second_depot_customers[customer_position + 1]) - cal_depot_distence(depot_number, second_depot_customers[customer_position]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position + 1]);
                }
                else if (customer_position == route1_number - 1 || customer_position == second_depot_customers.Count() - 1)
                {
                    change1 = cal_depot_distence(depot_number, second_depot_customers[customer_position - 1]) - cal_depot_distence(depot_number, second_depot_customers[customer_position]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position - 1]);
                }
                else
                {
                    change1 = cal_customers_distence(second_depot_customers[customer_position - 1], second_depot_customers[customer_position + 1]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position - 1]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position + 1]);
                }
                if (new_position == route0_number || new_position == route1_number)
                {
                    change2 = cal_depot_distence(depot_number, second_depot_customers[customer_position]) + cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[new_position]) - cal_depot_distence(depot_number, second_depot_customers[new_position]);
                }
                //else if (new_position == route1_number - 1 || new_position == second_depot_customers.Count() - 1)
                //{
                //    change2 = cal_depot_distence(depot_number, second_depot_customers[customer_position]) + cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[new_position]) - cal_depot_distence(depot_number, second_depot_customers[new_position]);
                //}
                else
                {
                    change2 = cal_customers_distence(second_depot_customers[new_position - 1], second_depot_customers[customer_position]) + cal_customers_distence(second_depot_customers[new_position], second_depot_customers[customer_position]) - cal_customers_distence(second_depot_customers[new_position], second_depot_customers[new_position - 1]);
                }
            }
            else if (depot_number == 2)
            {
                int route0_number = 0;
                int route1_number = 0;
                int route2_number = 0;
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                if (customer_position == route0_number || customer_position == route1_number || customer_position == route2_number)
                {
                    change1 = cal_depot_distence(depot_number, third_depot_customers[customer_position + 1]) - cal_depot_distence(depot_number, third_depot_customers[customer_position]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position + 1]);
                }
                else if (customer_position == route1_number - 1 || customer_position == route2_number - 1 || customer_position == third_depot_customers.Count() - 1)
                {
                    change1 = cal_depot_distence(depot_number, third_depot_customers[customer_position - 1]) - cal_depot_distence(depot_number, third_depot_customers[customer_position]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position - 1]);
                }
                else
                {
                    change1 = cal_customers_distence(third_depot_customers[customer_position - 1], third_depot_customers[customer_position + 1]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position - 1]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position + 1]);
                }
                if (new_position == route0_number || new_position == route1_number || new_position == route2_number)
                {
                    change2 = cal_depot_distence(depot_number, third_depot_customers[customer_position]) + cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[new_position]) - cal_depot_distence(depot_number, third_depot_customers[new_position]);
                }
                //else if (new_position == route1_number - 1 || new_position == route2_number - 1 || new_position == third_depot_customers.Count() - 1)
                //{
                //    change2 = cal_depot_distence(depot_number, third_depot_customers[customer_position]) + cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[new_position]) - cal_depot_distence(depot_number, third_depot_customers[new_position]);
                //}
                else
                {
                    change2 = cal_customers_distence(third_depot_customers[new_position - 1], third_depot_customers[customer_position]) + cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[new_position]) - cal_customers_distence(third_depot_customers[new_position - 1], third_depot_customers[new_position]);
                }
              }
            return change1 + change2;
        }

        /// <summary>
        /// 不同车场之间Move后的目标函数改变
        /// </summary>
        /// <param name="depot1">车场号1</param>
        /// <param name="depot2">车场号2</param>
        /// <param name="customer_position">车场号1上客户的位置</param>
        /// <param name="new_position">车场号2上客户的新位置</param>
        /// <returns>目标函数值的变化</returns>
        public double cal_object_change(int depot1,int depot2,int customer_position,int new_position)
        {
            double change1 = 0;
            double change2 = 0;
            if (depot1 == 0)
            {
                int route0_number = -1;
                int route1_number = -1;
                int route2_number = -1;
                int route3_number = -1;
                int route4_number = -1;
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 3)
                    {
                        route3_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 4)
                    {
                        route4_number = i;
                        break;
                    }
                }
                if (customer_position == route0_number || customer_position == route1_number || customer_position == route2_number || customer_position == route3_number || customer_position == route4_number)
                {
                    change1 = cal_depot_distence(depot1, first_depot_customers[customer_position + 1]) - cal_depot_distence(depot1, first_depot_customers[customer_position]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position + 1]);
                }
                //所选客户是路径上的最后一个点
                else if (customer_position == route1_number - 1 || customer_position == route2_number - 1 || customer_position == route3_number - 1 || customer_position == route4_number - 1 || customer_position == first_depot_customers.Count() - 1)
                {
                    change1 = cal_depot_distence(depot1, first_depot_customers[customer_position - 1]) - cal_depot_distence(depot1, first_depot_customers[customer_position]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position - 1]);
                }
                else
                {
                    change1 = cal_customers_distence(first_depot_customers[customer_position + 1], first_depot_customers[customer_position - 1]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position - 1]) - cal_customers_distence(first_depot_customers[customer_position], first_depot_customers[customer_position + 1]);
                }
            }
            else if (depot1 == 1)
            {
                int route0_number = 0;
                int route1_number = 0;
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                if (customer_position == route0_number || customer_position == route1_number)
                {
                    change1 = cal_depot_distence(depot1, second_depot_customers[customer_position + 1]) - cal_depot_distence(depot1, second_depot_customers[customer_position]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position + 1]);
                }
                else if (customer_position == route1_number - 1 || customer_position == second_depot_customers.Count() - 1)
                {
                    change1 = cal_depot_distence(depot1, second_depot_customers[customer_position - 1]) - cal_depot_distence(depot1, second_depot_customers[customer_position]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position - 1]);
                }
                else
                {
                    change1 = cal_customers_distence(second_depot_customers[customer_position - 1], second_depot_customers[customer_position + 1]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position - 1]) - cal_customers_distence(second_depot_customers[customer_position], second_depot_customers[customer_position + 1]);
                }
            }
            else if (depot1 == 2)
            {
                int route0_number = 0;
                int route1_number = 0;
                int route2_number = 0;
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                if (customer_position == route0_number || customer_position == route1_number || customer_position == route2_number)
                {
                    change1 = cal_depot_distence(depot1, third_depot_customers[customer_position + 1]) - cal_depot_distence(depot1, third_depot_customers[customer_position]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position + 1]);
                }
                else if (customer_position == route1_number - 1 || customer_position == route2_number - 1 || customer_position == third_depot_customers.Count() - 1)
                {
                    change1 = cal_depot_distence(depot1, third_depot_customers[customer_position - 1]) - cal_depot_distence(depot1, third_depot_customers[customer_position]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position - 1]);
                }
                else
                {
                    change1 = cal_customers_distence(third_depot_customers[customer_position - 1], third_depot_customers[customer_position + 1]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position - 1]) - cal_customers_distence(third_depot_customers[customer_position], third_depot_customers[customer_position + 1]);
                }
            }
            int moved_customer_position = -1;
            if (depot1 == 0)
            {
                moved_customer_position = first_depot_customers[customer_position];
            }
            else if (depot1 == 1)
            {
                moved_customer_position = second_depot_customers[customer_position];
            }
            else
            {
                moved_customer_position = third_depot_customers[customer_position];
            }
            if (depot2 == 0)
            {
                int route0_number = -1;
                int route1_number = -1;
                int route2_number = -1;
                int route3_number = -1;
                int route4_number = -1;
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 3)
                    {
                        route3_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 4)
                    {
                        route4_number = i;
                        break;
                    }
                }
                if (new_position == route0_number || new_position == route1_number || new_position == route2_number || new_position == route3_number || new_position == route4_number)
                {
                    change2 = cal_depot_distence(depot2, moved_customer_position) + cal_customers_distence(moved_customer_position, first_depot_customers[new_position]) - cal_depot_distence(depot2, first_depot_customers[new_position]);
                }
                //所选客户是路径上的最后一个点
                //else if (new_position == route1_number - 1 || new_position == route2_number - 1 || new_position == route3_number - 1 || new_position == route4_number - 1 || new_position == first_depot_customers.Count() - 1)
                //{
                //    change2 = cal_depot_distence(depot2, moved_customer_position) + cal_customers_distence(moved_customer_position, first_depot_customers[new_position]) - cal_depot_distence(depot2, new_position);
                //}
                else
                {
                    change2 = cal_customers_distence(moved_customer_position, first_depot_customers[new_position - 1]) + cal_customers_distence(moved_customer_position, first_depot_customers[new_position]) - cal_customers_distence(first_depot_customers[new_position - 1], first_depot_customers[new_position]);
                }
            }
            else if (depot2 == 1)
            {
                int route0_number = 0;
                int route1_number = 0;
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                if (new_position == route0_number || new_position == route1_number)
                {
                    change2 = cal_depot_distence(depot2, moved_customer_position) + cal_customers_distence(moved_customer_position, second_depot_customers[new_position]) - cal_depot_distence(depot2, second_depot_customers[new_position]);
                }
                //else if (new_position == route1_number - 1 || new_position == second_depot_customers.Count() - 1)
                //{
                //    change2 = cal_depot_distence(depot2, moved_customer_position) + cal_customers_distence(moved_customer_position, second_depot_customers[new_position]) - cal_depot_distence(depot2, second_depot_customers[new_position]);
                //}
                else
                {
                    change2 = cal_customers_distence(second_depot_customers[new_position - 1], moved_customer_position) + cal_customers_distence(second_depot_customers[new_position], moved_customer_position) - cal_customers_distence(second_depot_customers[new_position], second_depot_customers[new_position - 1]);
                }
            }
            else if (depot2 == 2)
            {
                int route0_number = 0;
                int route1_number = 0;
                int route2_number = 0;
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                if (new_position == route0_number || new_position == route1_number || new_position == route2_number)
                {
                    change2 = cal_depot_distence(depot2, moved_customer_position) + cal_customers_distence(moved_customer_position, third_depot_customers[new_position]) - cal_depot_distence(depot2, third_depot_customers[new_position]);
                }
                //else if (new_position == route1_number - 1 || new_position == route2_number - 1 || new_position == third_depot_customers.Count() - 1)
                //{
                //    change2 = cal_depot_distence(depot2, moved_customer_position) + cal_customers_distence(moved_customer_position, third_depot_customers[new_position]) - cal_depot_distence(depot2, third_depot_customers[new_position]);
                //}
                else
                {
                    change2 = cal_customers_distence(third_depot_customers[new_position - 1], moved_customer_position) + cal_customers_distence(moved_customer_position, third_depot_customers[new_position]) - cal_customers_distence(third_depot_customers[new_position - 1], third_depot_customers[new_position]);
                }
            }
            return change1 + change2;
        }


        /// <summary>
        /// 构建同一车场内的邻域解
        /// </summary>
        /// <param name="depot">车场号</param>
        /// <param name="customer_position">车场上客户的位置</param>
        /// <param name="route1">客户对应的路径</param>
        /// <param name="route2">新位置的路径</param>
        /// <param name="penty_Weight">惩罚权重</param>
        /// <returns>一个数组，包括F,C,Q,新位置，车场，开始位置</returns>
        public double[] creat_Neighbour(int depot,int customer_position,int route1,int route2,double penty_Weight)
        {
            #region
            if (depot == 0)
            {
                int route0_number = 0;
                int route1_number = 0;
                int route2_number = 0;
                int route3_number = 0;
                int route4_number = 0;
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 3)
                    {
                        route3_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 4)
                    {
                        route4_number = i;
                        break;
                    }
                }
               double[] value = new double[6];
               value[0] = 1000000;
               int weight_Sum=0;
               int penty = 0;
               switch(route2)
               {                  
                   case 0:
                       for (int i = route0_number; i < route1_number;i++ )
                       {
                           weight_Sum += readdate.demand[first_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[first_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                       for (int i = route0_number; i < route1_number; i++)
                       {
                           if (value[0] >cal_object_change(depot, customer_position, i)+penty_Weight*penty)
                           {
                               value[0] = cal_object_change(depot, customer_position, i)+penty_Weight*penty;
                               value[1] = cal_object_change(depot, customer_position, i);
                               value[2] = penty;
                               value[3] = i;
                           }
                       }
                           break;
                   case 1:
                       for (int i = route1_number; i < route2_number;i++ )
                       {
                           weight_Sum += readdate.demand[first_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[first_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                       for (int i = route1_number; i < route2_number; i++)
                       {
                           if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                           {
                               value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                               value[1] = cal_object_change(depot, customer_position, i);
                               value[2] = penty;
                               value[3] = i;
                           }
                       }
                       break;
                   case 2:
                        for (int i = route2_number; i < route3_number;i++ )
                       {
                           weight_Sum += readdate.demand[first_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[first_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                       for (int i = route2_number; i < route3_number; i++)
                       {
                           if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                           {
                               value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                               value[1] = cal_object_change(depot, customer_position, i);
                               value[2] = penty;
                               value[3] = i;
                           }
                       }
                       break;
                   case 3:
                        for (int i = route3_number; i < route4_number;i++ )
                       {
                           weight_Sum += readdate.demand[first_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[first_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                       for (int i = route3_number; i < route4_number; i++)
                       {
                           if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                           {
                               value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                               value[1] = cal_object_change(depot, customer_position, i);
                               value[2] = penty;
                               value[3] = i;
                           }
                       }
                       break;
                   case 4:
                        for (int i = route4_number; i < first_depot_customers.Count();i++ )
                       {
                           weight_Sum += readdate.demand[first_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[first_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                       for (int i = route4_number; i < first_depot_customers.Count(); i++)
                       {
                           if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                           {
                               value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                               value[1] = cal_object_change(depot, customer_position, i);
                               value[2] = penty;
                               value[3] = i;
                           }
                       }
                       break;
               }
               value[4] = depot;
               //value[5] = depot;
               value[5] = customer_position;
               return value;
            }
            else if (depot == 1)
            {
                int route0_number = 0;
                int route1_number = 0;
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                double[] value = new double[6];
                value[0] = 1000000;
                int weight_Sum = 0;
                int penty = 0;
                switch (route2)
                {
                    case 0:
                       for (int i = route0_number; i < route1_number;i++ )
                       {
                           weight_Sum += readdate.demand[second_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[second_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                        for (int i = route0_number; i < route1_number; i++)
                        {
                            if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                            {
                                value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                                value[1] = cal_object_change(depot, customer_position, i);
                                value[2] = penty;
                                value[3] = i;
                            }
                        }
                        break;
                    case 1:
                       for (int i = route1_number; i <second_depot_customers.Count();i++ )
                       {
                           weight_Sum += readdate.demand[second_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[second_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                        for (int i = route1_number; i < second_depot_customers.Count(); i++)
                        {
                            if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                            {
                                value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                                value[1] = cal_object_change(depot, customer_position, i);
                                value[2] = penty;
                                value[3] = i;
                            }
                        }
                        break;
                }
                value[4] = depot;
                //value[5] = depot;
                value[5] = customer_position;
                return value;
            }
            else
            {
                int route0_number = 0;
                int route1_number = 0;
                int route2_number = 0;
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                double[] value = new double[6];
                value[0] = 1000000;
                int weight_Sum = 0;
                int penty = 0;
                switch (route2)
                {
                    case 0:
                       for (int i = route0_number; i < route1_number;i++ )
                       {
                           weight_Sum += readdate.demand[third_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[third_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                        for (int i = route0_number; i < route1_number; i++)
                        {
                            if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                            {
                                value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                                value[1] = cal_object_change(depot, customer_position, i);
                                value[2] = penty;
                                value[3] = i;
                            }
                        }
                        break;
                    case 1:
                        for (int i = route1_number; i < route2_number;i++ )
                       {
                           weight_Sum += readdate.demand[third_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[third_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                        for (int i = route1_number; i < route2_number; i++)
                        {
                            if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                            {
                                value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                                value[1] = cal_object_change(depot, customer_position, i);
                                value[2] = penty;
                                value[3] = i;
                            }
                        }
                        break;
                    case 2:
                        for (int i = route2_number; i < third_depot_customers.Count();i++ )
                       {
                           weight_Sum += readdate.demand[third_depot_customers[i]];
                       }
                       weight_Sum += readdate.demand[third_depot_customers[customer_position]];
                       if (weight_Sum > 350)
                       {
                           penty = weight_Sum - 350;
                       }
                       else
                       {
                           penty = 0;
                       }
                        for (int i = route2_number; i < third_depot_customers.Count(); i++)
                        {
                            if (value[0] > cal_object_change(depot, customer_position, i) + penty_Weight * penty)
                            {
                                value[0] = cal_object_change(depot, customer_position, i) + penty_Weight * penty;
                                value[1] = cal_object_change(depot, customer_position, i);
                                value[2] = penty;
                                value[3] = i;
                            }
                        }
                        break;
                }
                value[4] = depot;
                //value[5] = depot;
                value[5] = customer_position;
                return value;
            }
            #endregion
        }
        
        /// <summary>
        /// 车场间构建邻域解
        /// </summary>
        /// <param name="depot1">车场号1</param>
        /// <param name="depot2">车场号2</param>
        /// <param name="customer_position">对应车场号1的顾客位置</param>
        /// <param name="route1">顾客位置对应的路径号</param>
        /// <param name="route2">顾客Move到车场号2的路径号</param>
        /// <param name="penty_Weight">惩罚权重</param>
        /// <returns>一个数组,包括F,C,Q,新位置，新位置的车场号，开始车场号，开始位置</returns>
        public double[] creat_Neighbour(int depot1, int depot2, int customer_position, int route1, int route2,double penty_Weight)
        {
            #region
            int add_Weight = 0;
            if (depot1 == 0)
            {
                add_Weight = readdate.demand[first_depot_customers[customer_position]];
            }
            else if (depot1 == 1)
            {
                add_Weight = readdate.demand[second_depot_customers[customer_position]];
            }
            else
            {
                add_Weight = readdate.demand[third_depot_customers[customer_position]];
            }
            if (depot2 == 0)
            {
                int route0_number = 0;
                int route1_number = 0;
                int route2_number = 0;
                int route3_number = 0;
                int route4_number = 0;
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 3)
                    {
                        route3_number = i;
                        break;
                    }
                }
                for (int i = 0; i < first_depot_route.Count(); i++)
                {
                    if (first_depot_route[i] == 4)
                    {
                        route4_number = i;
                        break;
                    }
                }
                double[] value=new double[7];
                value[0] = 1000000;
                int weight_Sum = 0;
                int penty = 0;
                switch (route2)
                {
                    case 0:
                        for (int i = route0_number; i < route1_number; i++)
                        {
                            weight_Sum += readdate.demand[first_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route0_number; i < route1_number; i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i)+penty*penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i)+penty*penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                    case 1:
                        for (int i = route1_number; i < route2_number; i++)
                        {
                            weight_Sum += readdate.demand[first_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route1_number; i < route2_number; i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                    case 2:
                         for (int i = route2_number; i < route3_number; i++)
                        {
                            weight_Sum += readdate.demand[first_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route2_number; i < route3_number; i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                    case 3:
                        for (int i = route3_number; i < route4_number; i++)
                        {
                            weight_Sum += readdate.demand[first_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route3_number; i < route4_number; i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                    case 4:
                        for (int i = route4_number; i < first_depot_customers.Count(); i++)
                        {
                            weight_Sum += readdate.demand[first_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route4_number; i < first_depot_customers.Count(); i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                }
                value[4] = depot2;
                value[5] = depot1;
                value[6] = customer_position;
                return value;
            }
            else if (depot2 == 1)
            {
                int route0_number = 0;
                int route1_number = 0;
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < second_depot_route.Count(); i++)
                {
                    if (second_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                double[] value = new double[7];
                value[0] = 1000000;
                int weight_Sum = 0;
                int penty = 0;
                switch (route2)
                {
                    case 0:
                        for (int i = route0_number; i < route1_number; i++)
                        {
                            weight_Sum += readdate.demand[second_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route0_number; i < route1_number; i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                    case 1:
                        for (int i = route1_number; i <second_depot_customers.Count(); i++)
                        {
                            weight_Sum += readdate.demand[second_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route1_number; i < second_depot_customers.Count(); i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                }
                value[4] = depot2;
                value[5] = depot1;
                value[6] = customer_position;
                return value;
            }
            else
            {
                int route0_number = 0;
                int route1_number = 0;
                int route2_number = 0;
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 0)
                    {
                        route0_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 1)
                    {
                        route1_number = i;
                        break;
                    }
                }
                for (int i = 0; i < third_depot_route.Count(); i++)
                {
                    if (third_depot_route[i] == 2)
                    {
                        route2_number = i;
                        break;
                    }
                }
                double[] value = new double[7];
                value[0] = 1000000;
                int weight_Sum = 0;
                int penty = 0;
                switch (route2)
                {
                    case 0:
                        for (int i = route0_number; i <route1_number; i++)
                        {
                            weight_Sum += readdate.demand[third_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route0_number; i < route1_number; i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                    case 1:
                        for (int i = route1_number; i <route2_number; i++)
                        {
                            weight_Sum += readdate.demand[third_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route1_number; i < route2_number; i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                    case 2:
                        for (int i = route2_number; i <third_depot_customers.Count(); i++)
                        {
                            weight_Sum += readdate.demand[third_depot_customers[i]];
                        }
                        weight_Sum += add_Weight;
                        if (weight_Sum > 350)
                        {
                            penty = weight_Sum - 350;
                        }
                        else
                        {
                            penty = 0;
                        }
                        for (int i = route2_number; i < third_depot_customers.Count(); i++)
                        {
                            if (value[0] > cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight)
                            {
                                value[0] = cal_object_change(depot1, depot2, customer_position, i) + penty * penty_Weight;
                                value[1] = cal_object_change(depot1, depot2, customer_position, i);
                                value[2] = penty * penty_Weight;
                                value[3] = i;
                            }
                        }
                        break;
                }
                value[4] = depot2;
                value[5] = depot1;
                value[6] = customer_position;
                return value;
            }
            #endregion
        }
    }
}