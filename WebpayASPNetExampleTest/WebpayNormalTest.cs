using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace TransbankWebpayExampleTest
{
    [TestClass]
    public class WebpayNormalTest : SeleniumTest
    {
        public WebpayNormalTest() : base("WebpayASPNetExample") { }

        [TestMethod]
        public void VerifyPageTitle()
        {
            // Replace with your own test logic
            _driver.Url = "http://localhost:54128";
            Assert.AreEqual("Ejemplos Webpay", _driver.FindElement(By.TagName("h1")).Text);
        }
    }
}
