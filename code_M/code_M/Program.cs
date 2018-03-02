
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace code_M
{
    class Program
    {
        static void Main(string[] args)
        {
            int n1 = int.Parse(Console.ReadLine().Trim());
            string input1 = Console.ReadLine().Trim();
            string[] input = input1.Split(' ');
            int[] y1 = new int[n1];
            for (int i = 0; i < n1;i++ )
            {
                y1[i] = Convert.ToInt32(input[i]);
            }
            int n2 = int.Parse(Console.ReadLine().Trim());
            input1 = Console.ReadLine().Trim();
            input = input1.Split(' ');
            int[] y2 = new int[n2];
            for (int i = 0; i < n2; i++)
            {
                y2[i] = Convert.ToInt32(input[i]);
            }
            double[] different = new double[n2 - n1+1];
            for (int i = 0; i < n2 - n1+1; i++)
            {
                int k=i;
                for (int j = 0; j < n1; j++)
                {
                    different[i] += Math.Pow(Convert.ToDouble(y1[j]-y2[k]), 2);
                    k++;
                }
            }
            double min = different[0];
            int index=0;
            for (int i = 1; i < n2 - n1+1; i++)
            {
                if (min > different[i])
                {
                    min = different[i];
                    index = i;
                }
            }
            Console.WriteLine(different[index]);
            Console.ReadKey();
        }
    }
}
