using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_exercise1
{
    public class Person
    {
        //字段
        public string _name;   
        public int _age;
        public char _gener;

        //属性（作用就是保护字段，对字段的赋值和取值进行限制）
        //属性的本质：get,set方法；
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //方法
        public void CHLSS()
        { 
        
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //this表示当前对象；类是不占内存的，对象是占内存的。
           //结构不具备面向对象的特征：封装，多态，继承。
        }
    }
}
