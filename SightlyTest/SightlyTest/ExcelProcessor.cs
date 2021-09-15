using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;

namespace SightlyTest
{
    class ExcelProcessor
    {
        CustomLogger logger;
        static Dictionary<string, Dictionary<string, string>> NewDataToCompare = new Dictionary<string, Dictionary<string, string>>() { };
        static Dictionary<string, Dictionary<string, string>> TestData = new Dictionary<string, Dictionary<string, string>>() { };

        //static string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        //static string PathToTestData = $@"{projectDirectory}\testdata\testdata.csv";
        static readonly string PathToNewData = $@"C:\Users\{Environment.UserName}\Downloads\newcsv.csv";




        public static List<string> ConvertAndCompareData(string datafile)
        {
            //after new xlxs is downloaded:

            List<string> ResultsList = new List<string>();

            ConvertLatestDownloadToCsv();
            CsvToDict(datafile, TestData);
            CsvToDict(PathToNewData, NewDataToCompare);
            ResultsList = CompareDictionaries();

            NewDataToCompare.Clear();
            TestData.Clear(); 

            return ResultsList;
        }

        public static List<string> CompareDictionaries()
        {
            //returns a list of differences if any

            var TestDataRows = TestData.Keys;
            List<string> results = new List<string>();
            
            foreach (var row in TestDataRows)
            {
                Debug.WriteLine(row);
                foreach(var kvp in TestData[row])
                {
                    Debug.WriteLine($"{kvp.Key}: {kvp.Value} | { NewDataToCompare[row][kvp.Key]} ");
                    if(kvp.Value != NewDataToCompare[row][kvp.Key])
                    {
                        Debug.WriteLine($"New data differs from test data: {row} {kvp.Key}: {NewDataToCompare[row][kvp.Key]} vs. {kvp.Value}");
                        results.Add($"New data differs from test data: {row} {kvp.Key}: {NewDataToCompare[row][kvp.Key]} vs. {kvp.Value}");
                    }
                }
            }
            return results;
        }


        public static string ConvertLatestDownloadToCsv()
        {
            /*
             * find the latest downloaded file and covert to csv
             * return the path to the csv file
             */
              
            var directory = new DirectoryInfo($@"C:\Users\{Environment.UserName}\Downloads");
            var NewerFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
            var newcsv = $@"C:\Users\{Environment.UserName}\Downloads\newcsv.csv";

            SaveAs(NewerFile.ToString(), newcsv);

            return newcsv;
        }

        public static void CsvToDict(string filepath, Dictionary<string, Dictionary<string, string>> dict)
        {
            string line;
            int rowcount = 0;
            bool intodata = false;

            System.IO.StreamReader file = new System.IO.StreamReader(filepath);
            while ((line = file.ReadLine()) != null)
            {
                while (!intodata)
                {
                    if (line.Contains("Start Date"))
                        intodata = true;
                    line = file.ReadLine();
                }

                string[] row = line.Split(",");
                rowcount++;

                dict.Add($"ROW{rowcount}", new Dictionary<string, string>()
                    {
                        { "StartDate" , row[0].ToString()},
                        { "EndDate" , row[1].ToString()},
                        { "CID" , row[2].ToString()},
                        { "CampaignName" , row[3].ToString()},
                        { "PlacementName" , row[4].ToString() },
                        { "Impressions" , row[5].ToString()},
                        { "views" , row[6].ToString()},
                        { "ViewRate" , row[7].ToString()},
                        { "clicks" , row[8].ToString()},
                        { "CTR" , row[9].ToString()},
                        { "CPV", row[10].ToString()},
                        { "CPM" , row[11].ToString()},
                        { "Cost" , row[12].ToString()},
                        { "VideoTo25Percent" , row[13].ToString()},
                        { "VideoTo50Percent" , row[14].ToString()},
                        { "VideoTo75Percent" , row[15].ToString()},
                        { "VideoTo100Percent" , row[16].ToString()}                       
                    }
                );
            }
        }
       
        private static void SaveAs(string sourceFilePath, string targetFilePath)
        {
            Application application = null;
            Workbook wb = null;
            Worksheet ws = null;

            try
            {
                application = new Application();
                application.DisplayAlerts = false;
                wb = application.Workbooks.Open(sourceFilePath);
                ws = (Worksheet)wb.Sheets[1];
                ws.SaveAs(targetFilePath, XlFileFormat.xlCSV);
            }
            catch (Exception e)
            {
                // Handle exception
            }
            finally
            {
                if (application != null) application.Quit();
                if (ws != null) Marshal.ReleaseComObject(ws);
                if (wb != null) Marshal.ReleaseComObject(wb);
                if (application != null) Marshal.ReleaseComObject(application);
            }
        }


    }
}
