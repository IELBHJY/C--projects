using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Windows.Forms; //命名空间引入；

namespace shipbuilding_yard
{
    class Program
    {
       // [STAThread]
        static void Main(string[] args)

        {

            //Form Form1 = new Form1();
            //Form1.Show();
            //Application.Run(Form1);


            path_road mypath_road = new path_road();
            string temppath = "";
            mypath_road.get_path(ref temppath);

/*
            string s0="R2";
            string s1="R3";
            string e0="T1102";
            string e1="P6302";
            string yid0="2";
            string yid1="3";
            Vertex vts0 = new Vertex(s0);
            Vertex vts1 = new Vertex(s1);
            Vertex vte0 = new Vertex(e0);
            Vertex vte1 = new Vertex(e1);
            Path myPath = new Path();
            myPath.getConn(vts0,vte0,yid0);
            myPath.getConn(vts1,vte1,yid1);

            string text0 = System.IO.File.ReadAllText(@"C:\\Users\\chenkai\\Desktop\\smart shipbuilding\\program\\shipbuilding yard\\"+yid0+"路径.txt");
           
            string pa0 = null;
            string[] sArray0=text0.Split('-');
            for(int i=sArray0.Length-2; i>0; i--)
            {
                pa0 = pa0 + sArray0[i]+"-";
            }

           //路径长度
            float plen0;
            string[] str0=sArray0[sArray0.Length-1].Split(':');
            string len0 =str0[str0.Length-1];
            plen0 = float.Parse(len0);
          
            string text1 = System.IO.File.ReadAllText(@"C:\\Users\\chenkai\\Desktop\\smart shipbuilding\\program\\shipbuilding yard\\"+yid1+"路径.txt");
            string pa1 = null;
            string[] sArray1=text1.Split('-');
            for(int i=1; i<sArray1.Length-2; i++)
            {
                pa1 = pa1 + sArray1[i]+"-";
            }
            pa1 = pa1 + sArray1[sArray1.Length-2];

            float plen1;
            string[] str1=sArray1[sArray1.Length-1].Split(':');
            string len1 =str1[str1.Length-1];
            plen1 = float.Parse(len1);    

            float len = plen0 + plen1;

            MessageBox.Show(pa0+"R-"+pa1+"\n" +"移动度为： "+len);
 */
           
        }
    }
}
