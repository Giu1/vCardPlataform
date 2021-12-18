using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excel_Lib
{
    public class ExcelHandler
    {

        public static void CreateNewExcelFile(string filename)
        {
            //Creates and excel Application instance
            var excelAplication = new Excel.Application();
            excelAplication.Visible = true;

            //Creates an Excel Workbook with a default number of sheets.
            var excelWorkbook = excelAplication.Workbooks.Add();
            excelWorkbook.SaveAs(filename, AccessMode: Excel.XlSaveAsAccessMode.xlNoChange);

            //eliminates the instances
            //excelWorkbook.Close();
            //excelApplication.Quit();
            ReleaseCOMObjects(excelWorkbook);
            ReleaseCOMObjects(excelAplication);

            //Its necessary to free all the memory used by the excel objects e.g.
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook)
            //excelWorbook = null;
            //...
            //GC.Collect();
        }

        /*public static void WriteToExcelFile(string filename)
        {
            //Creates and excel Application instance
            var excelAplication = new Excel.Application();
            excelAplication.Visible = true;

            //opens the excel file.
            var excelWorkbook = excelAplication.Workbooks.Open(filename);
            Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;
            excelWorksheet.Cells[1, 2].Value = "Hello";
            excelWorksheet.Cells[1, 2].Value = "World!";

            //you may also use the get_Item(1) method where '1' is the worksheet number
            Excel.Worksheet excelWorksheet2 = excelWorkbook.Worksheets.Add();
            excelWorksheet2.Cells[1, 1].Value = "Goodbye";
            excelWorksheet2.Cells[1, 2].Value = "World!";


            //eliminates the instances
            //excelWorkbook.Close();
            //excelApplication.Quit();

            excelWorkbook.Save();
            ReleaseCOMObjects(excelWorksheet2);
            ReleaseCOMObjects(excelWorksheet);
            ReleaseCOMObjects(excelWorkbook);
            ReleaseCOMObjects(excelAplication);

            //Its necessary to free all the memory used by the excel objects e.g.
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook)
            //excelWorbook = null;
            //...
            //GC.Collect();
        }*/

        public static string ReadFromExcelFile(string filename)
        {
            //Creates and excel Application instance
            var excelAplication = new Excel.Application();
            excelAplication.Visible = false;

            //opens the excel file.
            var excelWorkbook = excelAplication.Workbooks.Open(filename);
            var excelWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            string content = excelWorksheet.Cells[1, 1].Value;
            content += (excelWorksheet.Cells[1, 2] as Excel.Range).Text;

            //eliminates the instances
            //excelWorkbook.Close();
            //excelApplication.Quit();


            ReleaseCOMObjects(excelWorksheet);
            ReleaseCOMObjects(excelWorkbook);
            ReleaseCOMObjects(excelAplication);

            return content;
            //Its necessary to free all the memory used by the excel objects e.g.
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook)
            //excelWorbook = null;
            //...
            //GC.Collect();
        }

        public static void GenerateChart(string filename)
        {
            var excelApplication = new Excel.Application();
            excelApplication.Visible = true;

            var excelWorkbook = excelApplication.Workbooks.Open(filename);
            var excelWorksheet = excelWorkbook.ActiveSheet;

            //Add a chart object
            Excel.Chart myChart = null;
            Excel.ChartObjects charts = excelWorksheet.ChartObjects();
            Excel.ChartObject chartObj = charts.Add(50, 50, 300, 300);//LEFT; TOP; WIDTH; HEIGHT
            myChart = chartObj.Chart;

            //set chart range -- cell values to be used in the graph
            Excel.Range myRange = excelWorksheet.get_Range("C2:C8");
            myChart.SetSourceData(myRange);

            //chart properties using the named properties and default parameters functionality in
            //the .NET framework
            myChart.ChartType = Excel.XlChartType.xlLine;
            myChart.ChartWizard(Source: myRange,
                Title: "Graph Title",
                CategoryTitle: "Title of x axis",
                ValueTitle: "Title of y axis");


            excelWorkbook.SaveAs(filename);
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(myRange);
            ReleaseCOMObjects(myChart);
            ReleaseCOMObjects(chartObj);
            ReleaseCOMObjects(charts);
            ReleaseCOMObjects(excelWorksheet);

        }


        private static void ReleaseCOMObjects(Object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception releasing COM object." + ex.ToString());
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

    }

}
