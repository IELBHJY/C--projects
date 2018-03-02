using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_选择排序
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] num = { 3,1,5,3,7,2,5};
            int k = 0;
            for (int i = 0; i < num.Length; i++)
            {
                k = i;
                for (int j = i + 1; j < num.Length; j++)
                {
                    if (num[j] <= num[k])
                    {
                        k = j;
                    }
                }
                if (k != i)
                {
                    int temp = -1;
                    temp = num[i];
                    num[i] = num[k];
                    num[k] = temp;
                }
            }
            for (int i = 0; i < num.Length; i++)
            {
                Console.Write(num[i]+" ");
            }
            Console.ReadKey();
        }

    }
}
