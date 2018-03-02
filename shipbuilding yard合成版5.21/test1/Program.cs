using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Collections;
using System.Reflection;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "12-13-14";
            string[] num=str.Split('-');
            Console.WriteLine(num[2]);
            Console.ReadKey();
        }
    }
}
