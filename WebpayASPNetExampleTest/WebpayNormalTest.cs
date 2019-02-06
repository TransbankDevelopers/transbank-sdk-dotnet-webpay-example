using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TransbankWebpayExampleTest
{
    [TestClass]
    public class WebpayNormalTest : SeleniumTest
    {
        public WebpayNormalTest() : base("Webpay Test") { }

        [TestMethod]
        public void VerifyPageTitle()
        {
            // Replace with your own test logic
            _driver.Url = "https://transbankdevelopers.cl";
            Assert.AreEqual(@"•tbk. | DEVELOPERS", _driver.Title);
        }
    }
}
