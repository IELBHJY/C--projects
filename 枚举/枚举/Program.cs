using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 枚举
{
    enum orientation:byte
    {
        north=1,
        south,
        east,
        west,
    }
    class Program
    {
        static void Main(string[] args)
        {
            byte directionbyte;
            string directionstring;
            orientation mydirection=orientation.north;
            Console.WriteLine("mydirection={0}",mydirection);
            directionbyte=(byte)mydirection;
            directionstring=Convert.ToString(mydirection);
            Console.WriteLine("byte equivalent={0}",directionbyte);
            Console.WriteLine("string equivalent={0}", directionstring);
            Console.ReadKey();
        }
    }
}
