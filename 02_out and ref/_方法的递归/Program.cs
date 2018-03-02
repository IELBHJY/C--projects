using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _方法的递归
{
    class Program
    {
        //方法的递归：就是方法自己调用自己。
        public static int i = 0;//静态变量实现全局变量的作用

        public static void TellStory()
        {
            Console.WriteLine("从前有个山");
            i++;
            if (i >= 10)
            {
                return;
            }
            TellStory();
        }
        static void Main(string[] args)
        {
            TellStory();
            Console.ReadKey();
        }
    }
}
