using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Test_MultiChoice
{
    public class ExcelTool
    {
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }

            catch (Exception ex)
            {
                obj = null;
            }

            finally
            {
                GC.Collect();
            }
        }

        public void CreateExcelGraph_AnimalsCountPerGroup(List<Model.AnimalCountPerGroup> Data)
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;

                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //add data 
                // TODO
                xlWorkSheet.Cells[1, 1] = "";
                xlWorkSheet.Cells[2, 1] = "Count";
                int col = 2, row = 2;
                foreach (Model.AnimalCountPerGroup animalGroup in Data)
                {
                    xlWorkSheet.Cells[1, col] = "Group " + animalGroup.Group_ID;
                    xlWorkSheet.Cells[row, col++] = "" + animalGroup.Count;
                }
                Excel.Range chartRange;
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
                Excel.Chart chartPage = myChart.Chart;

                chartRange = xlWorkSheet.get_Range("A1", (char)('A' + (col - 2)) + "2");

                chartPage.SetSourceData(chartRange, misValue);

                chartPage.ChartType = Excel.XlChartType.xl3DPie;
                chartPage.ChartTitle.Caption = "Animal Count Per Group";

                Excel.Series series1 = (Excel.Series)chartPage.SeriesCollection(1);

                series1.HasDataLabels = true;

                string filename = "csharp.net-" + System.DateTime.Now.Ticks + ".xls";
                xlWorkBook.SaveAs(filename, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                string filepath = xlWorkBook.Path + "\\" + filename;

                Console.WriteLine("Excel saved at " + xlWorkBook.Path);
                xlWorkBook.Close(true, misValue, misValue);

                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);


                FileInfo fi = new FileInfo(filepath);
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start(filepath);
                }
                else
                {
                    //file doesn't exist
                }
            }
            catch (Exception e)
            {

            }
        }

    }
}
