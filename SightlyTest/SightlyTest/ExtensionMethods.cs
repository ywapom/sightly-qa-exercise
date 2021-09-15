using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SightlyTest
{
    public static class ExtensionMethods
    {

        public static void SetDropDownSelection(this IWebElement el, string value)
        {
            try
            {
                new SelectElement(el).SelectByValue(value);
            }
            catch
            {
                el.SendKeys(value);
            }
        }

        public static bool IsDialogPresent(this IWebDriver driver)
        {
            IAlert alert = SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent().Invoke(driver);
            return (alert != null);
        }
    }
}
