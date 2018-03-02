using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Array
{
    class Program
    {
        static void array1method(int x)
        {
           int[] array1=new int[x];
           Console.WriteLine("这是一个一维数组，包含{0}个元素",x);
           for (int i = 0; i < array1.Length; i++)
           {
               array1[i] = i + i;
           }
           for (int i = 0; i < array1.Length; i++)
           {
               Console.WriteLine("array1[{0}]is{1}",i,array1);
           }
        }
        static void array2method(int x, int y)
        {
            int[,] array2 = new int[x,y];//声明二维数组
            Console.WriteLine("这是一个{0}行，{1}列的二维数组",x,y);
            for (int i = 0; i < array2.GetLength(0); i++)
            {
                for (int j = 0; j < array2.GetLength(1); j++)
                {
                    array2[i,j] = i + j;
                }
            }
            for (int i = 0; i < array2.GetLength(0); i++)
            {
                for (int j = 0; j < array2.GetLength(1); j++)
                {
                    Console.WriteLine("array2[{0],{1}]is{2}",i,j,array2[i,j]);       
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("请输入一维数组的元素个数：");
            int x = int.Parse(Console.ReadLine());
            Console.WriteLine("hanghshu:");
            int x1=int.Parse(Console.ReadLine());
            Console.WriteLine("lieshu");
            int x2=int.Parse(Console.ReadLine());
            array1method(x);
            array2method(x1, x2);
            Console.ReadLine();
        }    
    }
}
