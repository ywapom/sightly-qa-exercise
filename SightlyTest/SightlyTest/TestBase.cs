using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;

using Xunit.Abstractions;

namespace SightlyTest
{
    public class TestBase : IDisposable
    {

        public static IWebDriver driver;
        protected readonly ITestOutputHelper _testOutput;
        protected readonly CustomLogger logger;
        ChromeOptions options = new ChromeOptions();

        static TimeSpan span = new TimeSpan(0, 0, 30);
        static WebDriverWait wait;

        public TestBase(ITestOutputHelper output)
        {
            logger = new CustomLogger(output);
            this._testOutput = output;

            options.AddArguments("start-maximized");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, span);
        }


        public static void Login(string url, string user, string password)
        {
           
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.XPath("//input[@type='text']")).Clear();
            driver.FindElement(By.XPath("//input[@type='text']")).SendKeys(user);

            driver.FindElement(By.XPath("//input[@type='password']")).Clear();
            driver.FindElement(By.XPath("//input[@type='password']")).SendKeys(password);

            driver.FindElement(By.ClassName("login-button")).Click();
        }


        public string GetUrl()
        {
            return driver.Url;
        }

        public void WaitForUrl(string el)
        {       
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains($"{el}"));
        }

        public void ClickReportsMenuItem()
        {
            try
            {
                string src = "";
                var imgs = driver.FindElements(By.TagName("img"));
                foreach (var e in imgs)
                {
                    src = e.GetAttribute("src");
                    if (src.Contains("reports"))
                    {
                        e.Click();
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            
        }

        public bool SelectReportCheckbox(int checkbox=2)
        {

            var checkboxes = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("checkbox-style")));

            var counter = 0;
            foreach (var box in checkboxes)
            {
                
                if (counter != checkbox)
                {
                    counter++;
                    continue;
                }
                                   
                try
                {
                    
                    box.Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.ClassName("create-report-text"))).Click();

                }
                catch(Exception e)
                {
                    Debug.WriteLine(e);
                }

                if (box.Selected)
                    return true;              
            }
            return false;
        }

        public static void HandleReportGeneratorOverlay()
        {
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id='performanceDetailReportOption']/div/div[1]/input"))).Click();

                IWebElement costbasis = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists((By.ClassName("costBasisSelect"))));
                ExtensionMethods.SetDropDownSelection(costbasis, "All");

                IWebElement timeperiod = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists((By.ClassName("timePeriodSelect"))));
                ExtensionMethods.SetDropDownSelection(timeperiod, "All Time");

                //download the report
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.ClassName("get-report-text"))).Click();

                // wait for the download
                WebDriverWait longwait = new WebDriverWait(driver, TimeSpan.FromSeconds(310));
                longwait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("v-card__title")));

                if (driver.IsDialogPresent())
                    Debug.WriteLine("FAILED to download the report!");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {

                        driver.Close();
                        driver.Quit();
                        driver.Dispose();

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }



    }
}
