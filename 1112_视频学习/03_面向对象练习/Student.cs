using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_面向对象练习
{
    public class Student
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        private string gener;
        public string Gener
        {
            get { return gener; }
            set { gener = value; }
        }
        public void SayHello()
        {
            Console.WriteLine("{0},{1},{2}",this.Name,this.Age,this.Gener);
            Console.ReadKey();
        }
    }
}
