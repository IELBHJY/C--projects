using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
namespace _1_求解TSP
{
    public class Data
    {
        public int[] id = new int[28];
        public double[] X = new double[28];
        public double[] Y = new double[28];
        public Data()
        {

        }
        public  void savedExcel()
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = false;
            app.UserControl = true;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = app.Workbooks;
            Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(@"C:\Users\DELL\Desktop\tsp.xlsx");
            Microsoft.Office.Interop.Excel.Sheets sheets = workbook.Sheets;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = (Microsoft.Office.Interop.Excel._Worksheet)sheets.get_Item(1); //第一个工作薄。
            if (worksheet == null)
                return;  //工作薄中没有工作表.    
            for (int i = 2; i < 30; i++)
            {
                //worksheet.Cells[i, 1] = "";----写入单元格
                //id.Add(((Range)worksheet.Cells[i, 1]).Text.ToString());//读取单元格
                id[i - 2] = int.Parse(((Range)worksheet.Cells[i, 1]).Text.ToString());
                X[i - 2] = double.Parse(((Range)worksheet.Cells[i, 2]).Text.ToString());
                Y[i - 2] = double.Parse(((Range)worksheet.Cells[i, 3]).Text.ToString());
            }
            //Range rang = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[9, 1]];
            //rang.NumberFormat = @"yyyy - mm - dd hh:mm:ss";
            //string savaPath = @"C:\Users\GeLiang\Desktop" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".csv";
            //workbook.SaveAs(savaPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            ////4.关闭Excel对象
            workbook.Close(Missing.Value, Missing.Value, Missing.Value);
            app.Quit();
            Console.WriteLine("修改完成！");
        }
    }
}
