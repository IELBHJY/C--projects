using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13_Roman_to_Integer
{
    class Program
    {
        static void Main(string[] args)
        {
            int result = RomanToInt("MMXVII");
            Console.WriteLine(result);
            Console.ReadKey();
        }

        public static int RomanToInt(string s)
        {
            int result = 0;
            List<List<string>> Roman = new List<List<string>>();
            List<string> one= new List<string> { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            List<string> ten =new List<string> { "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            List<string> hunred =new List<string> { "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            List<string> thousand =new List<string> { "M", "MM", "MMM" };
            Roman.Add(one);
            Roman.Add(ten);
            Roman.Add(hunred);
            Roman.Add(thousand);
            char[] temp = s.ToCharArray();
            string t = "";
            for(int i=0;i<temp.Length;i++)
            {
                t = temp[i].ToString();
                while(Roman[0].Contains(t)|| Roman[1].Contains(t) || Roman[2].Contains(t) || Roman[3].Contains(t))
                {
                    if (i != temp.Length - 1)
                    {
                        if (Roman[0].Contains(t + temp[i+1].ToString()) || Roman[1].Contains(t + temp[i+1].ToString()) || Roman[2].Contains(t + temp[i+1].ToString()) || Roman[3].Contains(t + temp[i+1].ToString()))
                        {
                            t += temp[++i].ToString();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int index = 0;
                int position = 0;
                for(int j=0;j<Roman.Count();j++)
                {
                    if(Roman[j].Contains(t))
                    {
                        index = Roman[j].IndexOf(t);
                        position = j;
                    }
                }
                switch(position)
                {
                    case 3:
                        result += (index + 1) * 1000;
                        break;
                    case 2:
                        result += (index + 1) * 100;
                        break;
                    case 1:
                        result += (index + 1) * 10;
                        break;
                    case 0:
                        result += (index + 1) * 1;
                        break;
                }
            }
            return result;
        }
    }
}
