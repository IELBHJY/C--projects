using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_里式转换
{
    class Program
    {
        static void Main(string[] args)
        {
            //里式转换：子类可以赋值给父类；如果父类中装的是子类对象，那么可以将这个父类对象强转为子类对象。
            Person p = new Student();
            Student stu=(Student)p;
            //is：表示类型转换，如果能够转换成功，返回true，否则返回false。
            //as：表示类型转换，如果可以转换，则返回对应的对象，否则返回一个null。
            Student stu1 = p as Student;
        }
    }
    public class Person
    {
        public void sayhello()
        {
            Console.WriteLine("person hello");
        }
    }
    public class Student:Person
    {
        public void sayhello()
        {
            Console.WriteLine("student hello");
        }
    }
}
