using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace Common.OSHelper
{
    public class TableWriter
    {
        public static void WriteToFile(IEnumerable data, string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(fileStream))
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
                    {
                        csv.WriteRecords(data);
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static byte[] WriteToMemory(IEnumerable data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var writer = new StreamWriter(ms))
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
                    {
                        csv.WriteRecords(data);
                        writer.Flush();
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
