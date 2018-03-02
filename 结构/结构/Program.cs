using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 结构
{
    enum orientation : byte
    {
        north = 1,
        south = 2,
        east = 3,
        west = 4,
    }
    struct route
    {
        public orientation direction;
        public double distance;
    }
    class Program
    {
        static void Main(string[] args)
        {
            route myroute;
            int mydirection = 0;
            double mydistance;
            Console.WriteLine("1,north\n2,south\n3,east\n4,west");
            do
            {
                Console.WriteLine("please slect a direction");
                mydirection = Convert.ToInt32(Console.ReadLine());
            } while (mydirection < 1 || mydirection > 4);
            Console.WriteLine("please input a distance");
            mydistance = Convert.ToDouble(Console.ReadLine());
            myroute.direction = (orientation)mydirection;
            myroute.distance = (double)mydistance;
            Console.WriteLine("myroute is a direction of {0} and a distance of {1}", myroute.direction, myroute.distance);
            Console.ReadKey();
        }
    }
}
