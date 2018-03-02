using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _方法的练习1
{
    class Program
    {
        /// <summary>
        /// 求一个字符串数组中最大的元素
        /// </summary>
        /// <param name="names">数组</param>
        /// <returns>最大元素</returns>
        public static string GetLongest(params string[] names)
        {
            string max=names[0];
            for (int i = 0; i < names.Length;i++ )
            {
                if (names[i].Length > max.Length)
                {
                    max = names[i];
                }
            }
            return max;
        }
        static void Main(string[] args)
        {    
            //写一个函数求解下列数组中长度最长的字符串
            string[] names = {"马龙","迈克尔乔丹","雷吉米勒","科比布莱恩特","蒂姆邓肯"};
            string maxstr = GetLongest(names);
            Console.WriteLine("数组中字符串最长的是：{0}，长度是：{1}",maxstr,maxstr.Length);
            Console.ReadKey();
        }
    }
}
