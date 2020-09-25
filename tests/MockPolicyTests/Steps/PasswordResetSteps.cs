using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using TechTalk.SpecFlow;

namespace PolicyTests.Steps
{
    [Binding]
    public class PasswordResetSteps
    {

        private IWebDriver _webDriver;

        public PasswordResetSteps()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _webDriver = new FirefoxDriver(options);
        }

        [Given(@"I'm in password reset page")]
        public void GivenIMInPasswordResetPage()
        {
            _webDriver.Navigate().GoToUrl("https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1A_Password_Reset_Mock&client_id=162b73a0-4972-42de-ad41-f15c5b4d9b51&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fjwt.ms&scope=openid&response_type=id_token&prompt=login");
        }

        [Given(@"I enter my email address")]
        public void GivenIEnterMyEmailAddress()
        {
            _webDriver.FindElement(By.Id("email")).SendKeys("amanguptaz117@yopmail.com");
        }

        [When(@"I click click reset password")]
        public void WhenIClickClickResetPassword()
        {
            _webDriver.FindElement(By.Id("continue")).Click();
        }

        [Then(@"I redirected to we've sent an email page")]
        public void ThenIRedirectedToWeVeSentAnEmailPage()
        {
            var elemet = _webDriver.FindElement(By.Id("successMessage"));

            Assert.AreEqual(elemet != null, true);
            _webDriver.Quit();

        }
    }
}