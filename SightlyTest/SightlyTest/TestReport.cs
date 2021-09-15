using System;
using System.Configuration;
using System.IO;
using Xunit;
using Xunit.Abstractions;



namespace SightlyTest
{
    public class Tests : TestBase
    {
        public Tests(ITestOutputHelper output) : base(output) { }
        

        [Fact]
        public void AutomatedPath()
        {
            string username = "nick@sightly.com";
            string password = "a";
            string url = "https://staging-newtargetview.sightly.com";

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string PathToTestData = $@"{projectDirectory}\testdata\testdata.csv";

            // 1. Login
            Login(url, username, password);
            WaitForUrl(@"orders/list");

            //2. go to 'reports'
            ClickReportsMenuItem();
            WaitForUrl("reports");

            //3. select report
            Assert.True(SelectReportCheckbox());

            //4. Create Report
            HandleReportGeneratorOverlay();

            //5. Compare report to test data
            var results = ExcelProcessor.ConvertAndCompareData(PathToTestData);
            if(results.Count >0)
            {
                foreach (var line in results)
                    logger.WriteLine(line, false);
            }
            else
            {
                logger.WriteLine("Downloaded report matches test data", true);
            }
            Assert.True(results.Count == 0);
        }




        [Fact]
        public void AutomatedPathFailData()
        {
            string username = "nick@sightly.com";
            string password = "a";
            string url = "https://staging-newtargetview.sightly.com";

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string PathToTestData = $@"{projectDirectory}\testdata\badtestdata.csv";

            // 1. Login
            Login(url, username, password);
            WaitForUrl(@"orders/list");

            //2. go to 'reports'
            ClickReportsMenuItem();
            WaitForUrl("reports");

            //3. select report
            Assert.True(SelectReportCheckbox());

            //4. Create Report
            HandleReportGeneratorOverlay();

            //5. Compare report to test data
            var results = ExcelProcessor.ConvertAndCompareData(PathToTestData);
            if (results.Count > 0)
            {
                foreach (var line in results)
                    logger.WriteLine(line, false);
            }
            else
            {
                logger.WriteLine("Downloaded report matches test data", true);
            }
            Assert.True(results.Count == 0);
        }
    }
}
