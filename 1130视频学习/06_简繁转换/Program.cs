
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace _06_简繁转换
{
    class Program
    {
        private const string jian = "我是好人";//存放简体字
        private const string fan = "iabc";//存放繁体字
        static void Main(string[] args)
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < jian.Length; i++)
            {
                ht.Add(jian[i], fan[i]);
            }
            Console.WriteLine("请输入：");
            string input = Console.ReadLine();
            for (int i = 0; i < input.Length; i++)
            {
                if (ht.ContainsKey(input[i]))
                {
                    Console.Write(ht[input[i]]);
                }
                else
                {
                    Console.Write(input[i]);
                }
            }
            Console.ReadKey();
        }
    }
}
