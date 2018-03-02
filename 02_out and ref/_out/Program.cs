using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _out
{
    class Program
    {
        //在一个方法中，如果返回多个类型相同的值得时候，可以考虑返回数组
        //但如果返回值类型不一样，可以考虑用 out 
        //out在方法离开前 必须在其内部赋值
        public static bool MyTryParse(string s,out int number)
        {
            number = 0;
            try
            {
                number = Convert.ToInt32(s);
                return true;
            }
            catch 
            {
                return false;
            }
        }
        static void Main(string[] args)
        {
          int a;
          bool b= MyTryParse("123456",out a);
          Console.WriteLine(a);
          Console.WriteLine(b);
          Console.ReadKey();

        }
    }
}
