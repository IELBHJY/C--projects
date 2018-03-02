using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_构造函数
{
    public class Student
    {
        //字段，属性，方法，构造函数。

        //创建对象的时候调用构造函数。
        //构造函数可以重载。
        //创建类时，会有个默认的无参数构造函数。
        public Student(string name,int age, string gener)
        {
            this.Name = name;
            this.Age = age;
            this.Gener = gener;
        }
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
            Console.WriteLine("{0},{1},{2}", this.Name, this.Age, this.Gener);
            Console.ReadKey();
        }
    }
}
