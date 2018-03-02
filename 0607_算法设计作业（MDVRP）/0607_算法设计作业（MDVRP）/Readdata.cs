using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
//using System.Text;
//using System.ComponentModel;
//using System.Data;
//using System.IO;
//using System.Collections;
//using System.Reflection;

namespace _0607_算法设计作业_MDVRP_
{
    public class Readdate
    {
       public int[] deport_number = new int[3];
       public int[] customers_number = new int[200];
       public int[] deport_X = new int[3];
       public int[] deport_Y = new int[3];
       public int[] customers_X = new int[200];
       public int[] customers_Y = new int[200];
       public int[] demand = new int[200];
       public void read_Input()
       {
           string AccessConnection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\lbh\Desktop\MDVRP.accdb";
           OleDbConnection odcConnection = new OleDbConnection(AccessConnection);
           odcConnection.Open();
           OleDbCommand odCommand = odcConnection.CreateCommand();
           odCommand.CommandText = "select NO,XCOORD,YCOORD from 车场信息";
           OleDbDataReader Reader = odCommand.ExecuteReader();
           int i = 0;
           while (Reader.Read())
           {
               deport_number[i] = i;
               deport_X[i] = Convert.ToInt32(Reader["XCOORD"]);
               deport_Y[i] = Convert.ToInt32(Reader["YCOORD"]);
               i++;
           }
           Reader.Close();
           odCommand.CommandText = "select NO,XCOORD,YCOORD,DEMAND from 客户信息";
           Reader = odCommand.ExecuteReader();
           int j = 0;
           while (Reader.Read())
           {
               customers_number[j] = j;
               customers_X[j] = Convert.ToInt32(Reader["XCOORD"]);
               customers_Y[j] = Convert.ToInt32(Reader["YCOORD"]);
               demand[j]=Convert.ToInt32(Reader["DEMAND"]);
               j++;
           }  
           //关闭连接
           Reader.Close();
           odcConnection.Close();
       }
    }
}
