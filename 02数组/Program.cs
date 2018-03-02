using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02数组
{
    class Program
    {
        static void Main(string[] args)
        {
            #region //数组定义 求最大值 最小值 平均值 总和
            /* int[] num=new int[10]{1,2,3,4,5,6,7,8,9,0};
             int max=num[0];
             int min=num[0];
             int sum=num[0];
             double avg;
             for (int i = 0; i < num.Length;i++ )
             {
                 if (num[i] >= max)
                 { 
                 max=num[i];
                 }
                 if (num[i] <= min)
                 { 
                 min=num[i];
                 }
                 sum+=num[i];
             }
            avg = sum / num.Length;
             Console.WriteLine("数组的最大值是{0}，最小值是{1}，总和是{2}，均值是{3}。",max,min,sum,avg);
             Console.ReadKey(); */
            #endregion
            #region//数组练习
            string[] names = new string[] {"老李","老龙","老裴"};
           //第一种解法 Console.WriteLine(names[0]+"|"+names[1]+"|"+names[2]);
           // Console.ReadKey();
            /*第二种解法  string str = null;
              for (int i = 0; i < names.Length;i++ )
              {
                  if (i != names.Length-1)
                  {
                      str += names[i] + "|";
                  }
                  else
                  {
                      str += names[i];
                  }
              }
              Console.WriteLine(str);
              Console.ReadKey(); */
            #endregion
            #region// 数组练习2
            /* int[] nums = new int[] {1,-2,3,-4,5,6,0};
            for (int i = 0; i < nums.Length;i++ )
            {
                if (nums[i]>0)
                {
                    nums[i] += 1;
                }
                if (nums[i] < 0)
                {
                    nums[i] -= 1;
                }
                if (i != nums.Length - 1)
                { Console.Write(nums[i] + ","); }
                else
                {
                    Console.Write(nums[i]);
                }
            }
            Console.ReadKey();  */
            #endregion
            #region//数组倒序
           /*  string[] a= new string[] {"我","是","好人"};
            string[] temp=new string[a.Length];
            for (int i = 0; i < a.Length; i++)
            { 
            temp[a.Length-1-i]=a[i];
            }
            for (int i = 0; i < temp.Length; i++)
            {
                Console.Write(temp[i]);
            }
             Console.ReadKey(); */
            #endregion
        }
    }
}
