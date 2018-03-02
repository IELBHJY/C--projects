using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_鸭脖鸭架实例
{
    class Program
    {
        static void Main(string[] args)
        {

            abstractfactory shanghaifactory = new shanghaifactory();
            yabo shanghaiyabo = shanghaifactory.creatyabo();
            shanghaiyabo.print();
            yajia shanghaiyajia = shanghaifactory.creatyajia();
            shanghaiyajia.print();

            abstractfactory nanchangfactory = new nanchangfactory();
            yabo nanchangyabo = nanchangfactory.creatyabo();
            nanchangyabo.print();
            yajia nanchangyajia = nanchangfactory.creatyajia();
            nanchangyajia.print();
            Console.ReadKey();
        }
    }

    public abstract class abstractfactory
    {
        public abstract yabo creatyabo();
        public abstract yajia creatyajia();
    }

    public class nanchangfactory : abstractfactory
    {
        public override yabo creatyabo()
        {
            return new nanchangyabo();
        }
        public override yajia creatyajia()
        {
            return new nanchangyajia();
        }
    }

    public class shanghaifactory : abstractfactory
    {
        public override yabo creatyabo()
        {
            return new shanghaiyabo();
        }
        public override yajia creatyajia()
        {
            return new shanghaiyajia();
        }
    }

    public abstract class yabo
    {
        public abstract void print();
    }
    public abstract class yajia
    {
        public abstract void print();
    }

    public class shanghaiyabo : yabo
    {
        public override void print()
        {
            Console.WriteLine("上海的鸭脖鸭脖，不辣有点甜。");
        }
    }
    public class shanghaiyajia : yajia
    {
        public override void print()
        {
            Console.WriteLine("上海的鸭架鸭架，不辣有点甜");
        }
    }

    public class nanchangyabo : yabo
    {
        public override void print()
        {
            Console.WriteLine("南昌的鸭脖鸭脖，狠辣狠辣");
        }
    }
    public class nanchangyajia : yajia
    {
        public override void print()
        {
            Console.WriteLine("南昌的鸭架鸭架，狠辣狠辣");
        }
    }

}
