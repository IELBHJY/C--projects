using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_Container_With_Most_Water
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] height = { 2, 3, 4, 5, 18, 17, 6 };
            Console.WriteLine(MaxArea(height));
            Console.ReadKey();
        }
        #region//暴力循环
        //public static int MaxArea(int[] height)
        //{
        //    int area = 0;
        //    int max = 0;
        //    for(int i=0;i<height.Length;i++)
        //    {
        //        for(int j=i+1;j<height.Length;j++)
        //        {
        //            area = calC(height, i, j);
        //            if(area>max)
        //            {
        //                max = area;
        //            }
        //        }
        //    }
        //    return max;
        //}
        //public static int calC(int[] height,int i,int j)
        //{
        //    int y = height[i] > height[j] ? height[j] : height[i];
        //    int result = y * (Math.Abs(i - j));
        //    return result;
        //}
        #endregion


        // 求解思路：比较两边的高度大小，两边的盛水容积肯定大于一边和内部的容积。
        public static int MaxArea(int[] height)
        {
            int l = 0;
            int r = height.Length - 1;
            int ans = 0;
            while(l<r)
            {
                ans = Math.Max(ans, (r-l)*Math.Min(height[l],height[r]));
                if(height[l]<height[r])
                {
                    l++;
                }
                else { r--; }
            }
            return ans;
        }
    }
}
