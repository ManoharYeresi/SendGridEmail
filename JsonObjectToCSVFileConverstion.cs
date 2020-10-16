using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            //string connectionstring = "DefaultEndpointsProtocol=https;AccountName=gac23;AccountKey=BxXt6MTxTdYfTKLBQH9WEwFCsVvSL87zO49YrcBMRSQXLKDm0oR6Jipq8vL/st+8XlqOgHsdBqXTOR88oAViYA==;EndpointSuffix=core.windows.net";
            //string fileName = "123.csv";
            //var storageAccount = CloudStorageAccount.Parse(connectionstring);
            //var client = storageAccount.CreateCloudBlobClient();
            //var container = client.GetContainerReference("partscontainer");
            //if (!container.Exists())
            //    container.Create();

            //var blobReference = container.GetBlockBlobReference(fileName);

            //var blockBlob = blobReference;
          


            string json = null;
            using (StreamReader r = new StreamReader(@"E:\11542_20200917113205.json"))
            {
                json = r.ReadToEnd();
            }
            var dt = GetDataTableFromJsonString(json);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                //IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString()); //original
                //IEnumerable<string> fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                IEnumerable<string> fields = row.ItemArray.Select(field => "'"+field.ToString()+"'");
                //IEnumerable<string> fields = row.ItemArray.Select(field => Regex.Replace(field.ToString(), "\r\n", String.Empty));
                //string[] fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\"")).ToArray();//scientific numbers
                //sb.AppendLine(string.Join(",", fields));
                sb.AppendLine("\"" + string.Join("\",\"", fields) + "\"");
            }
            //UploadToContainer(blockBlob,sb.ToString());
            //using (var ms = new MemoryStream())
            //{
            //    LoadStreamWithJson(ms, sb.ToString());
            //    blockBlob.UploadFromStream(ms);
            //}

            File.WriteAllText(@"E:\JsonToCsvFile.csv", sb.ToString());
            //var json = @"{
            //           ""employees"": [
            //           { ""firstName"":""John"" , ""lastName"":""Doe"" },
            //           { ""firstName"":""Anna"" , ""lastName"":""Smith"" },
            //           { ""firstName"":""Peter"" , ""lastName"":""Jones"" }
            //           ]
            //           }";
            //jsonStringToCSV(json);
        }

        public static void jsonStringToCSV(string jsonContent)
        {
            //used NewtonSoft json nuget package
            //XmlNode xml = JsonConvert.DeserializeXmlNode("{records:{record:" + jsonContent + "}}");
            XmlNode xml = JsonConvert.DeserializeXmlNode("{records:{" + jsonContent + "}");
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml.InnerXml);
            XmlReader xmlReader = new XmlNodeReader(xml);
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlReader);
            var dataTable = dataSet.Tables[1];

            //Datatable to CSV
            var lines = new List<string>();
            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = dataTable.AsEnumerable()
                               .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            File.WriteAllLines(@"E:/Export.csv", lines);
        }

        public static DataTable GetDataTableFromJsonString(string json)
        {
            var jsonLinq = JObject.Parse(json);
            // Find the first array using Linq  
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types  
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }
            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        private static void UploadToContainer(CloudBlockBlob blockBlob,string sb)
        {
            using (var ms = new MemoryStream())
            {
                LoadStreamWithJson(ms, sb);
                blockBlob.UploadFromStream(ms);
            }
        }
        private static void LoadStreamWithJson(Stream ms,string sb)
        {
            //var json = JsonConvert.SerializeObject(obj);
            StreamWriter writer = new StreamWriter(ms);
            writer.Write(sb);
            writer.Flush();
            ms.Position = 0;
        }
    }
}
