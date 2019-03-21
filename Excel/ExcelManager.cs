using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utilities.Excel
{
    public static class ExcelManager<T> where T : new()
    {
        #region PUBLIC METHODS
        /// <summary>
        /// Reads an excel, csv file. receives a list of properties that corresponds to the columns of the excel. Its order must be equals that to the excel. 
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="propertyNamesByOrder"></param>
        /// <returns></returns>
        public static List<T> ReadExcel(byte[] excel, List<string> propertyNamesByOrder)
        {
            using (ExcelPackage package = new ExcelPackage(new System.IO.MemoryStream(excel)))
            {
                List<T> lst = new List<T>();

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int excelColCount = worksheet.Dimension.End.Column;
                int typePropertyCount = typeof(T).GetType().GetProperties().Count();
                int rowCount = worksheet.Dimension.End.Row;

                if (propertyNamesByOrder.Count != excelColCount)
                {
                    throw new Exception("names of properties must match the number of columns in the excel!");
                }

                for (var row = 0; row < rowCount; row++)
                {
                    var obj = InstantiateT();

                    for (var col = 0; col < typePropertyCount; typePropertyCount++)
                    {
                        if (col + 1 > propertyNamesByOrder.Count)
                        {
                            continue;
                        }

                        obj.GetType().GetProperty(propertyNamesByOrder[col])?.SetValue(obj, worksheet.Cells[row, col]);
                    }

                    lst.Add(obj);
                }

                return lst;
            }
        }

        /// <summary>
        /// Creates an excel based on a a list of objects.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="worksheetname"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static FileInfo CreateExcel(string filename, string worksheetname, List<T> objects)
        {
            var newFile = new FileInfo(filename);

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                xlPackage.Workbook.Worksheets.Add(worksheetname);
                var worksheet = xlPackage.Workbook.Worksheets[0];


                for (var i = 0; i < objects.Count; i++)
                {
                    var propertiesList = typeof(T).GetType().GetProperties();
                    for (var j = 0; j < propertiesList.Count(); j++)
                    {
                        worksheet.SetValue(i, j, objects[i].GetType().GetProperty(propertiesList[j].Name));
                    }                   
                }
                           
                xlPackage.Save();
            }

            return newFile;
        }
        #endregion

        #region AUX
        private static T InstantiateT()
        {
            return new T();
        }
        #endregion
    }
}