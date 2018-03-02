using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Dictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1,"a");
            dic.Add(2, "b");
            dic.Add(3, "c");
            foreach (KeyValuePair<int,string> kv in dic)
            {
                Console.WriteLine("{0}----{1}",kv.Key,kv.Value);
            }
            foreach (var item in dic.Keys)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
