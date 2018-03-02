using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_字符串
{
    class Program
    {
        static void Main(string[] args)
        {
            //字符串的不可变性：当你给一个字符串从新赋值后，老值并没有销毁，而是从新开辟地址。
            //当程序结束后，GC扫描整个内存，发现有的地方没有被任何指向，则消除它。
            //字符串的各种方法：
            // length：字符串的长度
            //toUpper(),toLower():转换成大写或者小写。Equal();
            Console.WriteLine("请输入一个人的名字：");
            string name=Console.ReadLine();
            name = name.ToUpper();
            name = name.ToLower();
            Console.WriteLine(name.Length);
            Console.ReadKey();
        }
    }
}
