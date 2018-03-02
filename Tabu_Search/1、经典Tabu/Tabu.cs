using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
namespace _1_经典Tabu
{
     class Tabu
    {
        static int customer;
        public Tabu(int m)
        {
            customer = m;      
        }        
        public double[] customer_x = new double[200];
        public double[] customer_y = new double[200];
        public double[] customer_demand = new double[200];
        public double[] customer_start_time = new double[customer];
        public double[] customer_end_time = new double[customer];
        public double deport_x = 0;
        public double deport_y = 0;
        public double vechile_demand = 550;
        public List<int> customer_sequence = new List<int>();
        public List<int> route_sequence = new List<int>();
        Random rand = new Random();
        
        public void ReadData()
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = false;
            app.UserControl = true;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = app.Workbooks;
            Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(@"C:\Users\lbh\Desktop\MDVRP - 副本.xlsx");
            Microsoft.Office.Interop.Excel.Sheets sheets = workbook.Sheets;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = (Microsoft.Office.Interop.Excel._Worksheet)sheets.get_Item(1); //第一个工作薄。
            if (worksheet == null)
                return;  //工作薄中没有工作表.    
            for (int i = 2; i < 8; i++)
            {
                //worksheet.Cells[i, 1] = "";//写入单元格
                //((Range)worksheet.Cells[i, 1]).Text.ToString();//读取单元格 
                customer_x[i - 2] = double.Parse(((Range)worksheet.Cells[i, 2]).Text.ToString());
                customer_y[i - 2] = double.Parse(((Range)worksheet.Cells[i, 3]).Text.ToString());
                customer_demand[i - 2] = double.Parse(((Range)worksheet.Cells[i, 4]).Text.ToString());
            }
            worksheet = (Microsoft.Office.Interop.Excel._Worksheet)sheets.get_Item(2);
            if (worksheet == null)
                return;
            deport_x = double.Parse(((Range)worksheet.Cells[2, 2]).Text.ToString());
            deport_y = double.Parse(((Range)worksheet.Cells[2, 3]).Text.ToString());
            //Range rang = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[9, 1]];
            //rang.NumberFormat = @"yyyy - mm - dd hh:mm:ss";
            //string savaPath = @"C:\Users\GeLiang\Desktop" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".csv";
            //workbook.SaveAs(savaPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            ////4.关闭Excel对象
            workbook.Close(Missing.Value, Missing.Value, Missing.Value);
            app.Quit();
            Console.WriteLine("读取完成！");
        }

        public void initial()
        {
            int num = rand.Next(0, customer_x.Length);
            for(int i=num;i<customer_x.Length;i++)
            {
                customer_sequence.Add(i);
            }
            for(int i=0;i<num;i++)
            {
                customer_sequence.Add(i);
            }
            double sum_weight = 0;
            int route = 0;
            for(int i=0;i<customer_sequence.Count;i++)
            {
                if(sum_weight+customer_demand[customer_sequence[i]]<=550)
                {
                    route_sequence.Add(route);
                    sum_weight += customer_demand[customer_sequence[i]];
                }
                else
                {
                    sum_weight = 0;
                    sum_weight += customer_demand[customer_sequence[i]];
                    route_sequence.Add(++route);
                }
            }
            Console.WriteLine("初始化完成！");
        }
       
        public int cal_route()
        {
            int count = 0;
            for (int i = 0; i < route_sequence.Count - 1; i++)
            {
                if (route_sequence[i] != route_sequence[i + 1])
                {
                    count++;
                }
            }
            //if (route_sequence[route_sequence.Count - 1] != route_sequence[route_sequence.Count - 2])
            //    count++;
            return count;
        }

        public double cal_object()
        {
            double sum = 0;
            int count = 0;//求解路径个数
            for(int i=0;i<route_sequence.Count-1;i++)
            {
                if(route_sequence[i]!=route_sequence[i+1])
                {
                    count++;
                }
            }        
            for(int i=0;i<=count;i++)
            {
                int first = route_sequence.IndexOf(i);
                int j = first;
                sum += cal_depot_distance(customer_sequence[first]);
                while(route_sequence[j]==i&&j<199)
                {
                    if(route_sequence[j+1]==i)
                    {
                        sum += cal_customers_distance(customer_sequence[j], customer_sequence[j + 1]);
                    }
                    j++;
                }
                int end = route_sequence.LastIndexOf(i);
                sum += cal_depot_distance(customer_sequence[end]);
            }
            return sum;
        }

        public double cal_depot_distance(int customer)
        {
            double temp1 =deport_x - customer_x[customer];
            double temp2 =deport_y - customer_y[customer];
            double tem1 = Math.Sqrt(Math.Pow(Convert.ToDouble(temp1), 2) + Math.Pow(Convert.ToDouble(temp2), 2));
            return tem1;
        }

        public double cal_customers_distance(int customer1, int customer2)
        {
            double temp1 = customer_x[customer1] - customer_x[customer2];
            double temp2 = customer_y[customer1] - customer_y[customer2];
            double tem1 = Math.Sqrt(Math.Pow(Convert.ToDouble(temp1), 2) + Math.Pow(Convert.ToDouble(temp2), 2));
            return tem1;
        }
        
        public double cal_change(int old_position,int new_position)
        {
            double change1 = 0;
            double change2 = 0;
            //先判断是否在端点
            List<int> start = new List<int>();
            List<int> end = new List<int>();
            int count = 0;//求解路径个数
            for (int i = 0; i < route_sequence.Count - 1; i++)
            {
                if (route_sequence[i] != route_sequence[i + 1])
                {
                    count++;
                }
            }
            for(int i=0;i<=count;i++)
            {
                start.Add(route_sequence.IndexOf(i));
                end.Add(route_sequence.LastIndexOf(i));
            }
            if (start.Contains(old_position) && !end.Contains(old_position))
            {
                change1 += cal_depot_distance(customer_sequence[old_position + 1]) - (cal_depot_distance(customer_sequence[old_position]) + cal_customers_distance(customer_sequence[old_position], customer_sequence[old_position + 1]));
            }
            else if (end.Contains(old_position) && !start.Contains(old_position))
            {
                change1 += cal_depot_distance(customer_sequence[old_position - 1]) - (cal_depot_distance(customer_sequence[old_position]) + cal_customers_distance(customer_sequence[old_position], customer_sequence[old_position - 1]));
            }
            else if (start.Contains(old_position) && end.Contains(old_position))
                change1 += 0;//wrong
            else
            {
                change1 += cal_customers_distance(customer_sequence[old_position - 1], customer_sequence[old_position + 1]) - cal_customers_distance(customer_sequence[old_position], customer_sequence[old_position - 1]) - cal_customers_distance(customer_sequence[old_position], customer_sequence[old_position + 1]);
            }
            if (start.Contains(new_position))
            {
                change2 += cal_depot_distance(customer_sequence[old_position])+cal_customers_distance(customer_sequence[old_position], customer_sequence[new_position])-cal_depot_distance(customer_sequence[new_position]);
            }
            else
            {
                change2 += cal_customers_distance(customer_sequence[old_position], customer_sequence[new_position-1])+cal_customers_distance(customer_sequence[old_position], customer_sequence[new_position]) - cal_customers_distance(customer_sequence[new_position], customer_sequence[new_position-1]);
            }
            return change1+change2;
        }

         /// <summary>
         /// 构造邻域解
         /// </summary>
         /// <param name="customer_position">所选客户点位置</param>
         /// <param name="old_route">所选客户点所在路径</param>
         /// <param name="new_route">移动后的路径</param>
         /// <param name="penty_weight">惩罚权重值</param>
         /// <returns>返回一个价值数组，包括F,C,惩罚值,customer_position,old_route,移动后的位置,new_route</returns>
        public double[] creat_N(int customer_position,int old_route,int new_route, double penty_weight)
        {
            List<int> start = new List<int>();
            List<int> end = new List<int>();
            int count = 0;//求解路径个数
            for (int i = 0; i < route_sequence.Count - 1; i++)
            {
                if (route_sequence[i] != route_sequence[i + 1])
                {
                    count++;
                }
            }
            for (int i = 0; i <= count; i++)
            {
                start.Add(route_sequence.IndexOf(i));
                end.Add(route_sequence.LastIndexOf(i));
            }
            double[] value = new double[7];
            value[0] = 10000;//初始化F值的非常大
            double sum_weight = 0;
            double penty = 0;
            for (int i = start[new_route]; i <= end[new_route]; i++)
            {
                sum_weight += customer_demand[customer_sequence[i]];
            }
            sum_weight += customer_demand[customer_sequence[customer_position]];
            if (sum_weight > 550)
                penty = sum_weight - 550;
            for (int i = start[new_route]; i <= end[new_route]; i++)
            {
                if (value[0] > cal_change(customer_position, i)+penty_weight*penty)
                {
                    value[0] = cal_change(customer_position, i) + penty_weight * penty;
                    value[1] = cal_change(customer_position, i);
                    value[2] = penty;
                    value[3] = customer_position;
                    value[4] = old_route;
                    value[5] = i;
                    value[6] = new_route;
                }
            }
            return value;
        }
        public void modify(int old_position,int new_position)
        {
            customer_sequence.Insert(new_position, customer_sequence[old_position]);
            route_sequence.Insert(new_position, route_sequence[new_position]);
            if (old_position < new_position)
            {
                customer_sequence.RemoveAt(old_position);
                route_sequence.RemoveAt(old_position);
            }
            else if (old_position > new_position)
            {
                customer_sequence.RemoveAt(old_position + 1);
                route_sequence.RemoveAt(old_position + 1);
            }
            else { Console.WriteLine("前后变换位置相同，有错误!"); }
        }
    }
}
