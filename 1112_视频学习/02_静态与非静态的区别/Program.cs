using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_静态与非静态的区别
{
    class Program
    {
        static void Main(string[] args)
        {
            //在非静态类（实例类）中，既可以有静态成员，也可以有非静态成员。调用实例成员时，使用对象名.实例成员。
            //在静态类中，只能有静态成员。调用时使用类名.静态成员
            //静态类在整个项目中资源共享。  堆  栈  静态存储区域
        }
    }
}
