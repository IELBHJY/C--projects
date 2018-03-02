using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1216_简单工厂模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入一个名字：1，名字 姓名    2，姓名，名字");
            string inputname = Console.ReadLine();
            NameFactory nf = new NameFactory();
             Namer nm = nf.getName(inputname);
             string firstname = nm.getfirstName();
             string lastname = nm.getlastName();
             Console.WriteLine("输入名字的姓是：{0}，名是：{1}",firstname,lastname);
             Console.ReadKey();
        }
    }
    public class Namer
    { 
    protected string firstName;
    protected string lastName;
    public string getfirstName()
    {
        return firstName;
    }
    public string getlastName()
    {
        return lastName;
    }
    }

    public class spaceinput : Namer
    {
        public spaceinput(string name)
        {
            int i = name.Trim().IndexOf(" ");
            if (i > 0)
            {
                lastName = name.Substring(0, i);
                firstName = name.Substring(i + 1).Trim();
            }
            else
            {
                lastName = name.Trim();
                firstName = "";
            }
        }
    }

    public class pointinput: Namer
    {
        public pointinput(string name)
        {
            int i = name.Trim().IndexOf(',');
            if (i > 0)
            {
                firstName = name.Substring(0, i);
                lastName = name.Substring(i + 1);
            }
            else
            {
                firstName = name.Trim();
                lastName = "";
            }
        }
  
    }

    public class NameFactory
    {
        public NameFactory()
        { }
        public Namer getName(string name)
        {
            int i = name.Trim().IndexOf(',');
            if (i > 0)
            {
                return new pointinput(name);
            }
            else
            {
                return new spaceinput(name);
            }
        }
    }
}
