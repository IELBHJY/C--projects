using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 厦航大数据
{
    class Program
    {
        static void Main(string[] args)
        {
            bad_airport_count bac = new bad_airport_count(@"C:\Users\DELL\Desktop\厦航大数据.accdb");
            bac.show_badairport();
            bac.show_lianchenglazhi();
            bac.show_yanchi_10hours();
            bac.cal_yanchi_10hours();
            bac.change();
            Console.ReadKey();
        }
    }
}
