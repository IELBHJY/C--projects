using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_顾客餐厅实例
{
    class Program
    {
        static void Main(string[] args)
        {
            Food food1 = FoodSimpleMode.Creatfood("西红柿炒鸡蛋");
            food1.print();
            Food food2 = FoodSimpleMode.Creatfood("土豆炒肉丝");
            food2.print();
            Console.ReadKey();
        }
    }

    public abstract class Food
    {
        public abstract void print();
    }

    public class Xihongshijidan : Food
    {
        public override void print()
        {
            Console.WriteLine("西红柿炒鸡蛋");
        }
    }

    public class Tudouchaorousi : Food
    {
        public override void print()
        {
            Console.WriteLine("土豆炒肉丝");
        }
    }

    public class FoodSimpleMode
    {
        public static Food Creatfood(string type)
        {
            Food food = null;
            if (type.Equals("西红柿炒鸡蛋"))
            {
                food = new Xihongshijidan();
            }
            else if (type.Equals("土豆炒肉丝"))
            {
                food = new Tudouchaorousi();
            }
            return food;
        }
    }
}
