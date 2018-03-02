using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_面向对象练习
{
    class Program
    {
        static void Main(string[] args)
        {
            Student student1 = new Student();
            student1.Name = "zhangsan";
            student1.Gener="Nan ";
            student1.Age = 17;
            student1.SayHello();
            Console.ReadKey();
        }
    }
}
