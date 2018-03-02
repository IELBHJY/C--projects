using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace _04_hashtable
{
    class Program
    {
        static void Main(string[] args)
        {
            //hashtable:键值对集合
            Hashtable ht = new Hashtable();
            ht.Add(1,"张三");
            ht.Add(2, true);
            ht.Add(false, "错误的");
            Console.WriteLine(ht[1]);
            Console.WriteLine(ht[false]);
            //var:推断类型.根据值能推断类型。
            foreach (var item in ht.Keys)
            {
                Console.WriteLine(ht[item]);
            }
           
            
            Console.ReadKey();
        }
    }
}
