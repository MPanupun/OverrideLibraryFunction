using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace Milim.Service
{
    public class WriteReportService
    {
        public static List<string> writeReport<T>(List<T> reportModel, string reportName)
        {
            Console.WriteLine("writeReport :" + reportName);
            string path = @"temp\";
            string fullPath = reportName;
            List<string> file = new List<string>();
            Directory.CreateDirectory(path);
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(fullPath, false, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(writer))
            {
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    csvWriter.WriteField(GetAttributeDisplayName(propertyInfo));
                }
                csvWriter.NextRecord();
                foreach (var record in reportModel)
                {
                    csvWriter.WriteRecord(record);
                    csvWriter.NextRecord();
                }
                writer.Flush();
                file.Add(fullPath);

                string GetAttributeDisplayName(PropertyInfo property)
                {
                    var atts = property.GetCustomAttributes(
                        typeof(DisplayNameAttribute), true);
                    if (atts.Length == 0)
                        return null;
                    return (atts[0] as DisplayNameAttribute).DisplayName;
                }

                return file;
            }
        }
    }
}
