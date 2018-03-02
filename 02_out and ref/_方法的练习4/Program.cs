using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _方法的练习4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入你的分数：");
            string score=Console.ReadLine();
            int num = Convert.ToInt32(score);
            string level = JudgeRank(num);
            Console.WriteLine(level);
            Console.ReadKey();
        }
        public static string JudgeRank(int score)
        {
            string level = "";
            switch(score/10)
            {
                case 10:
                case 9: level = "优"; break;
                case 8: level = "良"; break;
                case 7: level = "及格"; break;
                default: level = "不及格"; break;
            }
            return level;
        }
    }
}
