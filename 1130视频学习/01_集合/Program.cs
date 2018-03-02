using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_集合
{
    class Program
    {
        static void Main(string[] args)
         //ArrayList集合
         //可以创建集合对象，长度可变，类型随意
        {
                ArrayList list = new ArrayList();
                list.Add("集合");
                list.AddRange(new int[] {1,2,3,4,5,6,7,8,9});
                list.AddRange(list);
                //list.Clear();
                list.Remove("集合");
                list.RemoveAt(0);//根据下标删除元素
                list.RemoveRange(0,2);//根据下标移除一定范围的元素。
                list.Reverse();
                list.Sort();
                list.Insert(1, "haha");
                list.InsertRange(0,new int[] {11,22});
                list.Contains(1);//判断是否包含
                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine(list[i]);
                }
                Console.ReadKey();
        }
    }
}
