using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.FunctionalTests.Tests;
using ClosedXML.Excel;
using System.Data;
using NUnit.Framework;

namespace Application.FunctionalTests.PageObjects
{
    public class ExportPageObject : PagesPageObjectBase
    {
        private const string excelFileNameFirstPart = "Trade";

        public ExportPageObject(IAutomateBrowser browser) : base(browser)
        {
        }

        public void AssertExcelExportNonEmpty()
        {
            string startCell = "A7";
            string endCell = "G7";

            var fileName = WaitForFile();
            var actualData = GetDataFromExcel(fileName, startCell, endCell);
            var emptyExcelflag = CheckIfExcelHasNoData(actualData);
            Assert.IsFalse(emptyExcelflag);
        }

        private string WaitForFile()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var excelFileName = string.Format("{0}_{1}", excelFileNameFirstPart, date);
            ResultsTablePageObject resultsTablePageObject = new ResultsTablePageObject(Browser);
            var file = Browser.WaitForFile(excelFileName, "xlsx", resultsTablePageObject.ExportButtonClick);
            return file;
        }

        private List<List<string>> GetDataFromExcel(string path, string startCell, string endCell)
        {
            
            var castedBook = GetRange(path, startCell, endCell);
            return castedBook;
        }

        private static List<List<string>> GetRange(string fileName, string startCell, string endCell,
            int worksheetNumber = 1)
        {
            var castedRange = new List<List<string>>();
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    castedRange = ListsFromExcelWorkbookCreation(fileName, startCell, endCell, worksheetNumber);
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(5000);
                }
            }

            return castedRange;
        }

        private static List<List<string>> ListsFromExcelWorkbookCreation(string fileName, string startCell, string endCell,
            int worksheetNumber = 1)
        {
            var castedRange = new List<List<string>>();
            using (var workBook = new XLWorkbook(fileName))
            {
                var workSheet = workBook.Worksheet(worksheetNumber);
                var sheetRange = workSheet.Range(startCell, endCell);
                foreach (var row in sheetRange.Rows())
                {
                    var castedRow = new List<string>();
                    foreach (var cell in row.Cells())
                    {
                        if (!string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            castedRow.Add(cell.Value.ToString());
                        }
                        else
                        {
                            castedRow.Add("");
                        }
                    }
                    castedRange.Add(castedRow);
                }
            }
            return castedRange;
        }

        private bool CheckIfExcelHasNoData(List<List<string>> expectedData)
        {
            var emptyExcelFlag = true;
            for (var i = 0; i < expectedData.Count; i++)
            {
                for (var j = 0; j < expectedData[i].Count; j++)
                {
                    if (expectedData[i][j] != "")
                        emptyExcelFlag = false;
                }
            }

            return emptyExcelFlag;
        }

        public void AssertExcelAndGridHaveTheSameData()
        {
            ResultsTablePageObject _resultTable = new ResultsTablePageObject(Browser);
            var rowsCount = _resultTable.GridTableRowsCount;
            var gridData = _resultTable.GridTableItems();


            string startCell = "A7";
            string endCell = string.Format("{0}{1}", "G", (rowsCount + 6).ToString());

            var fileName = WaitForFile();
            var excelData = GetDataFromExcel(fileName, startCell, endCell);

            CheckIfExcelAndGridHaveTheSameData(gridData, excelData);
        }

        private void CheckIfExcelAndGridHaveTheSameData(List<List<string>> gridData, List<List<string>> excelData)
        {
            for (var i = 0; i < gridData.Count; i++)
            {
                for (var j = 0; j < gridData[i].Count; j++)
                {
                    if (gridData[i][j] == " ")
                        gridData[i][j] = "";

                    Assert.AreEqual(gridData[i][j], excelData[i][j]);
                }
            }
        }
    }
}
