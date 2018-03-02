using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_搜索排序
{
    class Program
    {
        static void Main(string[] args)
        {
            //默认第一个元素是最小的，然后逐个比较，若当前元素小于第一个，则交换位置。
            int[] a = { 13,14,2,1,57,2,1,3,6,3,45};
            selectionSort(a);
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i]+" ");
            }
                Console.ReadKey();
        }
        public static void selectionSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int min_index = i;
                for (int j =i+1; j < array.Length; j++)
                {
                    if (array[j] < array[min_index])
                    {
                        int temp = array[min_index];
                        array[min_index] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }
    }
}
