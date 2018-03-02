using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_三种模式之间的区别
{
    class Program
    {
        static void Main(string[] args)
        {
            //三种模式总结
            //1，简单工厂，工厂方法，抽象工厂都属于设计模式中的创建型模式。其主要功能都是帮助我们把对象的实例化部分抽取出来，优化系统构架，并且增强了系统的可扩展性。
            //2，区别：
            //（1）简单工厂：工厂类一般是使用静态方法，通过接收的参数不同来返回不同的对象实例。
            //（2）工厂方法：是针对每一种产品提供一个工厂类。通过不同的工厂实例来创建不同的产品实例。
            //（3）抽象工厂：是对应产品族概念的。比如说，对于每个汽车公司可能都要同时产生轿车，货车，客车，那么每个工厂都要有创建轿车，货车，客车的方法。
        }
    }
}
