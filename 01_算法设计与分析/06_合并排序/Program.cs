using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_合并排序
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = {6,5,4,3,2,1};
            mergeSort(a, 0, a.Length - 1);
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i]+" ");
            }
            Console.ReadKey();
        }
        public static void mergeSort(int[] a, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                mergeSort(a, left, mid);
                mergeSort(a, mid + 1, right);
                //合并
                merge(a, left, mid, right);
            }
        }
        public static void merge(int[] a, int left, int mid, int right)
        {
            int[] array = new int[a.Length];
            int i = left;
            int j = mid + 1;
            for (int k = left; k < right - 1; k++)
            {
                array[k] = a[k];
            }
            for (int k = left; k < right - 1; k++)
            {
                if (i > mid)
                {
                    a[k] = array[j++];
                }
                else if (j > right)
                {
                    a[k] = array[i++];
                }
                else if (array[i] <= array[j])
                {
                    a[k] = array[i++];
                }
                else
                {
                    a[k] = array[j++];
                }
            }
        }
    }
}
