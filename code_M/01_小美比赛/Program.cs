using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_小美比赛
{
    class Program
    {
        static void Main(string[] args)
        {
            int candidate_number = int.Parse(Console.ReadLine());
            string get_Score = Console.ReadLine().Trim();
            string[] score = get_Score.Split(' ');
            List<int> s = new List<int>();
            int xiaomei_score=int.Parse(score[0]);
            for (int i = 1; i < score.Length; i++)
            {
                s.Add(int.Parse(score[i]));
            }
            int level = 0;
            for (int i = 0; i < score.Length/2; i++)
            {
                s.Sort();
                int low_score = s[0];
                if (low_score > xiaomei_score)
                {
                    level = i;
                    break;
                }
                else
                {
                    s.RemoveAt(0);
                    if(s.Count()==0)
                    {
                        level = i+1;
                        break;
                    }
                    int delete_count = s.Count() / 2;
                    for (int j = 0; j <delete_count; j++)
                    {
                        s.RemoveAt(j);
                    }
                }
            }
            Console.WriteLine(level);
            Console.ReadKey();
        }
    }
}
