using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;

namespace PolicyTests.Steps
{
    [Binding]
    public class FindEmailSteps
    {
        private IWebDriver _webDriver;

        public FindEmailSteps()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _webDriver = new FirefoxDriver(options);
        }

        [Given(@"I'm in the find email page")]
        public void GivenIMInTheFindEmailPage()
        {
            _webDriver.Navigate().GoToUrl("https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/B2C_1A_Find_Email_Mock/oauth2/v2.0/authorize?client_id=162b73a0-4972-42de-ad41-f15c5b4d9b51&nonce=dca6e72f03974ccb81f618052a152454&redirect_uri=https://jwt.ms/&scope=openid&response_type=id_token");
        }

        [Given(@"I filled the find email form")]
        public void GivenIFilledTheFindEmailForm()
        {
            _webDriver.FindElement(By.Id("givenName")).SendKeys("Aman");
            _webDriver.FindElement(By.Id("surname")).SendKeys("Gupta");
            _webDriver.FindElement(By.Id("day")).SendKeys("01");
            _webDriver.FindElement(By.Id("month")).SendKeys("01");
            _webDriver.FindElement(By.Id("year")).SendKeys("2000");
            _webDriver.FindElement(By.Id("postCode")).SendKeys("NE1 1AB");
        }


        [When(@"I click on find email button")]
        public void WhenIClickOnFindEmailButton()
        {
            _webDriver.FindElement(By.Id("continue")).Click();
        }
        
        [Then(@"I shoud navigated to the result page")]
        public void ThenIShoudNavigatedToTheResultPage()
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5));
            var messageElement = wait.Until(drv => drv.FindElement(By.Id("foundEmailMessagelast")));

            var message = messageElement.GetAttribute("aria-label");
            Assert.AreEqual(message.ToLower(), "you'll need to use this email address to sign in to your account.");
        }
    }
}