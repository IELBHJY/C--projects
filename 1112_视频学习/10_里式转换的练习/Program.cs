using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_里式转换的练习
{
    class Program
    {
        static void Main(string[] args)
        {
            Person[] pers = new Person[10];
            Random r = new Random();
            for (int i = 0; i < pers.Length; i++)
            {
                int rnum = r.Next(1, 7);
                switch (rnum)
                {
                    case 1: pers[i] = new Student();
                        break;
                    case 2: pers[i] = new Teacter();
                        break;
                    case 3: pers[i] = new MeiLv();
                        break;
                    case 4: pers[i] = new ShuaiGuo();
                        break;
                    case 5: pers[i] = new YeShou();
                        break;
                    case 6: pers[i] = new Person();
                        break;
                }
            }
            for (int i = 0; i < pers.Length; i++)
            {
                if (pers[i] is Student)
                {
                    ((Student)pers[i]).StudentSay();
                }
                else if (pers[i] is Teacter)
                {
                    ((Teacter)pers[i]).TeacterSay();
                }
                else if (pers[i] is MeiLv)
                {
                    ((MeiLv)pers[i]).MeiLvSay();
                }
                else if (pers[i] is ShuaiGuo)
                {
                    ((ShuaiGuo)pers[i]).ShuaiGuoSay();
                }
                else if (pers[i] is YeShou)
                {
                    ((YeShou)pers[i]).YeShouSay();
                }
                else if (pers[i] is Person)
                {
                    pers[i].PersonSay();
                }
            }     
                    Console.ReadKey();
        }
    }
    public class Person
    {
        public void PersonSay()
        {
            Console.WriteLine("我是人类！");
        }
    }
    public class Student : Person
    {
        public void StudentSay()
        {
            Console.WriteLine("我是学生！");
        }
    }
    public class Teacter : Person
    {
        public void TeacterSay()
        {
            Console.WriteLine("我是老师！");
        }
    }
    public class MeiLv : Person
    {
        public void MeiLvSay()
        {
            Console.WriteLine("我是镁铝！");
        }
    }
    public class ShuaiGuo : Person
    {
        public void ShuaiGuoSay()
        {
            Console.WriteLine("我是帅锅！");
        }
    }
    public class YeShou : Person
    {
        public void YeShouSay()
        {
            Console.WriteLine("我是野兽！");
        }
    }
}
