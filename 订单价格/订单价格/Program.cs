using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 订单价格
{
    class Program
    {
        struct order
        {
            public string name;
            public int count;
            public double cost;
            public double price()
            {
                return count * cost;
            }
            public string infor()
            {
                return "订单信息:" + count.ToString()+"kg" + name.ToString() + "items at $" + cost.ToString() + "each," + "total cost $" + price().ToString();
            }
        }
        static void Main(string[] args)
        {
            order myorder;
            myorder.name = "猪肉";
            myorder.count=20;
            myorder.cost=17.5;
            Console.WriteLine("订单的价格为:{0}", myorder.price());
            Console.WriteLine("订单的信息是:{0}", myorder.infor());
            Console.ReadKey();
        }
    }
}
