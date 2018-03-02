using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_顾客餐厅实例
{
    class Customer
    {
        static void Main(string[] args)
        {
            Xionghongshifactory xf = new Xionghongshifactory();
            Tudouchaorousifactory tf = new Tudouchaorousifactory();
            Guobaoroufactory gb = new Guobaoroufactory();
            Food food1 = xf.creatfoodfactory();
            food1.print();
            Food food2 = tf.creatfoodfactory();
            food2.print();
            Food food3 = gb.creatfoodfactory();
            food3.print();
            Console.ReadKey();
        }
    }
    public abstract class Food
    {
        public abstract void print();
    }
    public class Xionghongshichaojidan:Food
    {
        public override void print()
        {
            Console.WriteLine("西红柿炒鸡蛋");
        }
    }
    public class Tudouchaorousi:Food
    {
        public override void print()
        {
            Console.WriteLine("土豆炒肉丝");
        }
    }

    public class Guobaorou : Food
    {
        public override void print()
        {
            Console.WriteLine("锅包肉");
        }
    }

    public abstract class Creat
    {
        public abstract Food creatfoodfactory();
    }

    public class Xionghongshifactory:Creat
    {
        public override Food creatfoodfactory()
        {
            return new Xionghongshichaojidan();
        }
    }
    public class Tudouchaorousifactory:Creat
    {
        public override Food creatfoodfactory()
        {
            return new Tudouchaorousi();
        }
    }
    public class Guobaoroufactory : Creat
    {
        public override Food creatfoodfactory()
        {
            return new Guobaorou();
        }
    }
}
