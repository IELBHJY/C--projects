using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01结构
{
    class Program
    {
        public enum Gener
        {
            男,
            女
        }
        public struct Person
        {
           public string name;
           public  int age;
           public Gener gener;
        }
        static void Main(string[] args)
        {
            Person person;
            person.name = "陈奕迅";
            person.age = 41;
            person.gener = Gener.男;
            Console.WriteLine("{0}是我的{1}偶像，今年{2}岁了", person.name, person.gener, person.age);
            Console.ReadKey();
        }
    }
}
