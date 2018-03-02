using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_构造函数
{
    class Program
    {
        static void Main(string[] args)
        {
            //构造函数，作用：帮助我们初始化对象（给对象的每个属性依次赋值）
            //构造函数是一个特殊的方法：
            //(1) 构造函数没有返回值，连void也不能写。
            //(2) 构造函数的名称必须和类名一样。
            Student s1 = new Student("张三", 18, "男");
            Student s2 = new Student("李楠", 17, "女");
            //new关键字作用
            //(1) 在内存中开辟一块空间。
            //(2) 在开辟空间中创建对象。
            //(3) 调用对象的构造函数进行初始化对象。
        }
    }
}
