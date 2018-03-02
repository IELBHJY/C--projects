using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_装箱拆箱
{
    class Program
    {
        static void Main(string[] args)
        {
            //装箱：就是将值类型转换为引用类型。
            //拆箱：将引用类型转换为值类型。
            //看两种类型是否发生了装箱和拆箱，要看，这两种类型是否存在继承关系。
            int n = 10;
            object o = n;
            int nn = (int)o;
        }
    }
}
