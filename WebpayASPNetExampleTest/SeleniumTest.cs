using System;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TransbankWebpayExampleTest
{
    [TestClass]
    public abstract class SeleniumTest
    {
        protected const int iisPort = 64890;
        private readonly string _applicationName;
        private Process _iisProcess;
        private ChromeOptions _option;
        protected IWebDriver _driver;
        protected IJavaScriptExecutor _js;
        protected OpenQA.Selenium.Support.UI.WebDriverWait _wait;

        protected SeleniumTest(string applicationName)
        {
            _applicationName = applicationName;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // Start IISExpress
            StartIIS();

            _option = new ChromeOptions();
            _option.AddArgument("--headless");
            _option.AddAdditionalCapability(CapabilityType.Version, "latest", true);
            _option.AddAdditionalCapability(CapabilityType.Platform, "WIN10", true);
            _driver = new ChromeDriver(_option);
            _driver.Manage().Window.Maximize();
            _js = _driver as IJavaScriptExecutor;
            _wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, new TimeSpan(0, 0, 20));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Ensure IISExpress is stopped
            if (_iisProcess.HasExited == false)
            {
                _iisProcess.Kill();
            }

            _driver.Close();
        }

        private void StartIIS()
        {
            var applicationPath = GetApplicationPath(_applicationName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + "\\IIS Express\\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:{0} /port:{1}", applicationPath, iisPort);
            _iisProcess.Start();
        }
        protected virtual string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return Path.Combine(solutionFolder, applicationName);
        }
        public string GetAbsoluteUrl(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return String.Format("http://localhost:{0}{1}", iisPort, relativeUrl);
        }
    }
}
