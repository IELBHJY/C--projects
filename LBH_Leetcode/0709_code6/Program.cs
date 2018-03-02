using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0709_code6
{
    class Program
    {
        //public static string Convert(string s, int numRows)
        //{
        //    char[] temp = s.ToCharArray();
        //    List<List<char>> list = new List<List<char>>();
        //    for(int i=0;i<numRows;i++)
        //    {
        //        list.Add(new List<char>());
        //    }      
        //    for(int i=0;i<numRows;i++)
        //    {
        //        int j = i;
        //        list[i].Add(temp[i]);
        //        if (i < numRows - 1)
        //        {
        //            while (j + numRows - i + numRows - i - 2 <temp.Length)
        //            {
        //                list[i].Add(temp[j + numRows - i + numRows - i - 2]);
        //                j = j + numRows - i + numRows - i - 2;
        //            }
        //        }
        //        else
        //        {
        //            while(j+numRows-2+numRows<temp.Length&&j+numRows*2-2>0)
        //            {
        //                list[i].Add(temp[j + numRows - 2 + numRows]);
        //                j = j + numRows - 2 + numRows;
        //            }
        //        }            
        //    }
        //    string result = "";
        //    for (int i = 0; i < numRows; i++)
        //    {
        //        for (int j = 0; j < list[i].Count; j++)
        //        {
        //            result += list[i][j];
        //        }
        //    }
        //    return result;
        //}

        public static string Convert(string s, int numRows)
        {
            char[] temp = s.ToCharArray();
            StringBuilder[] sb = new StringBuilder[numRows];
            for(int j = 0; j < sb.Length; j++) { sb[j] = new StringBuilder(); }
            int i = 0;
            int len = temp.Length;
            while(i<len)
            {
                for(int j=0;j<numRows&&i<len;j++)
                {
                    sb[j].Append(temp[i++]);
                }
                for(int j=numRows-2;j>=1&&i<len;j--)
                {
                    sb[j].Append(temp[i++]);
                }
            }
            string result = "";
            for(int k=0;k<numRows;k++)
            {
                result += sb[k];
            }
            return result;
        }
        static void Main(string[] args)
        {
            string result = Convert("PAYPALISHIRING", 3);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
