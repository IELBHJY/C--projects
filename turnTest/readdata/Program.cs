using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace readdata
{
    class Program
    {
        static void Main(string[] args)
        {
              //新建一个数据库连接  
            using(SqlConnection conn = new SqlConnection(GetConnectString()))  
            {  
                conn.Open();//打开数据库  
                //Console.WriteLine("数据库打开成功!");  
                //创建数据库命令  
                SqlCommand cmd = conn.CreateCommand();  
                //创建查询语句  
                cmd.CommandText = "SELECT * FROM input";  
                //从数据库中读取数据流存入reader中  
                SqlDataReader reader = cmd.ExecuteReader();                 
                  
                //从reader中读取下一行数据,如果没有数据,reader.Read()返回flase  
                while (reader.Read())  
                {  
                    //reader.GetOrdinal("id")是得到ID所在列的index,  
                    //reader.GetInt32(int n)这是将第n列的数据以Int32的格式返回  
                    //reader.GetString(int n)这是将第n列的数据以string 格式返回
                    
                    int id = reader.GetInt32(reader.GetOrdinal("id"));  
                    string name = reader.GetString(reader.GetOrdinal("name"));  
                    string pwd = reader.GetString(reader.GetOrdinal("password"));  
                    int age = reader.GetInt32(reader.GetOrdinal("age"));  
                    string sex = reader.GetString(reader.GetOrdinal("sex"));  
                    string phone = reader.GetString(reader.GetOrdinal("phone"));  
                    string address = reader.GetString(reader.GetOrdinal("Address"));  
  
                    //格式输出数据  
                    Console.Write("ID:{0},Name:{1},PWD:{2},Age:{3},Sex:{4},Phone{5},Address:{6}\n", id, name, pwd, age, sex, phone, address);  
                }  
            }  
            Console.ReadKey();  
        }  
        //得到一个数据库连接字符串  
        static string GetConnectString()  
        {  
            return "Data Source=(local);Initial Catalog=input;Integrated Security=SSPI;";  
        }  
    }  
}  