using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _params
{
    class Program
    {
        //params 将实参列表中 和 可变参数数组类型一致的元素都当做数组的元素去处理。
        //必须是形参列表中的最后一个，并且只能出现一次params
        #region//params用法介绍
        //static void Main(string[] args)
        //{
        //    Test("张三",96,95,88);
        //    Console.ReadKey();
        //}
        //public static void Test(string name,params int[] score)
        //{
        //    int sum = 0;
        //    for (int i = 0; i < score.Length;i++ )
        //    {
        //        sum += score[i];
        //    }
        //    Console.WriteLine("{0},{1}", name, sum);
        #endregion

        #region// params的联系
        public static void GetArrayMax(params int[] nums)
        {
            int max = nums[0];
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > max)
                {
                    max = nums[i];
                }
            }
            Console.WriteLine(max);
        }
        static void Main(string[] args)
        {
            int[] nums = new int[] { 1,2,3,4,5,6,7,8,9};
            GetArrayMax(nums);
            Console.ReadKey();
         #endregion 
        }
    }
}
