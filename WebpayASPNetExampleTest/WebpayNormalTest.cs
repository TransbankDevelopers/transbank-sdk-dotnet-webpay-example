using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TransbankWebpayExampleTest
{
    [TestClass]
    public class WebpayNormalTest
    {
        private ChromeDriver _driver;

        [TestInitialize]
        public void CromeDriverInitialize()
        {
            // Initialize edge driver 
            var options = new  ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            _driver = new ChromeDriver(options);
        }

        [TestMethod]
        public void VerifyPageTitle()
        {
            // Replace with your own test logic
            _driver.Url = "https://transbankdevelopers.cl";
            Assert.AreEqual(@"•tbk. | DEVELOPERS", _driver.Title);
        }

        [TestCleanup]
        public void ChromeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
