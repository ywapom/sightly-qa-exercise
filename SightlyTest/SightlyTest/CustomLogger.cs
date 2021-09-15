using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace SightlyTest
{
    public class CustomLogger
    {
        private ITestOutputHelper iTestOutputHelper;

        public CustomLogger(ITestOutputHelper iTestOutputHelper)
        {
            this.iTestOutputHelper = iTestOutputHelper;
        }

        public void WriteLine(string description, bool bPass)
        {

            var result = bPass ? "Pass" : "FAIL";
            var list = new List<string>();
            list.Add($"Result: {result}");
            list.Add($"Description: {description}");
            this.iTestOutputHelper.WriteLine(string.Join("\t", list));

        }
    }
}
