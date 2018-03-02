using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0707_code4
{
    class Program
    {
        public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
        {
            int[] sumtemp = new int[nums1.Length + nums2.Length];
            List<int> temp = new List<int>();
            double media = 0;
            for(int i=0;i<nums1.Length;i++)
            {
                temp.Add(nums1[i]);
            }
            for(int i=0;i<nums2.Length;i++)
            {
                temp.Add(nums2[i]);
            }
            temp.Sort();
            if((nums1.Length+nums2.Length)%2==0)
            {
                media = Convert.ToDouble(temp[temp.Count/2 - 1] + temp[temp.Count/2])/2;
            }
            else
            {
                media = Convert.ToDouble(temp[temp.Count/2]);
            }
            return media;
        }
        static void Main(string[] args)
        {
            int[] nums1 = { 1,2};
            int[] nums2 = { 3,4 };
            double media = FindMedianSortedArrays(nums1, nums2);
            Console.WriteLine(media);
            Console.ReadKey();
        }
    }
}
