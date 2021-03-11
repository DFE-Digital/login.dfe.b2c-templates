using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;

namespace PolicyTests.Steps
{
    [Binding]
    public class AidedSignupSteps
    {
        private IWebDriver _webDriver;

        public AidedSignupSteps()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _webDriver = new FirefoxDriver(options);
        }

        [Given(@"I am navigated to the email activation link")]
        public void GivenIAmNavigatedToTheEmailActivationLink()
        {
            _webDriver.Navigate().GoToUrl("https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/B2C_1A_Signup_Invitation_mock/oauth2/v2.0/authorize?client_id=162b73a0-4972-42de-ad41-f15c5b4d9b51&nonce=dca6e72f03974ccb81f618052a152454&redirect_uri=https://jwt.ms/&scope=openid&response_type=id_token&id_token_hint=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2F1dGhuY3MtZGV2LWZ1bmN0aW9uLmF6dXJld2Vic2l0ZXMubmV0LyIsImlhdCI6MTU5OTIwMzk5MywiZXhwIjoxOTE0NzM2NzkzLCJhdWQiOiIxNjJiNzNhMC00OTcyLTQyZGUtYWQ0MS1mMTVjNWI0ZDliNTEiLCJzdWIiOiJBbWFuIiwiZW1haWwiOiJhbWFuZ3VwdGF6MTE3QHlvcG1haWwuY29tIiwiZXhwaXJ5IjoiMjAzMC0wOS0wMlQxNzoyODozOSIsImdpdmVuTmFtZSI6IkFtYW4iLCJzdXJuYW1lIjoiR3VwdGEiLCJjdXN0b21lcklkIjoiMDAwMjExMjAtZWY3Ni00Nzc0LTg3ZGYtNzYxZWMzM2ZjMmUyIiwiam91cm5leSI6ImFpZGVkc2lnbnVwIn0.1tjjo3G1tvJmuQ4LZlcumDvi-z9llzrzZHunhTwHlf8&idtokenhint=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2F1dGhuY3MtZGV2LWZ1bmN0aW9uLmF6dXJld2Vic2l0ZXMubmV0LyIsImlhdCI6MTU5OTIwMzk5MywiZXhwIjoxOTE0NzM2NzkzLCJhdWQiOiIxNjJiNzNhMC00OTcyLTQyZGUtYWQ0MS1mMTVjNWI0ZDliNTEiLCJzdWIiOiJBbWFuIiwiZW1haWwiOiJhbWFuZ3VwdGF6MTE3QHlvcG1haWwuY29tIiwiZXhwaXJ5IjoiMjAzMC0wOS0wMlQxNzoyODozOSIsImdpdmVuTmFtZSI6IkFtYW4iLCJzdXJuYW1lIjoiR3VwdGEiLCJjdXN0b21lcklkIjoiMDAwMjExMjAtZWY3Ni00Nzc0LTg3ZGYtNzYxZWMzM2ZjMmUyIiwiam91cm5leSI6ImFpZGVkc2lnbnVwIn0.1tjjo3G1tvJmuQ4LZlcumDvi-z9llzrzZHunhTwHlf8");
        }

        [Given(@"I filled the form")]
        public void GivenIFilledTheForm()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(9);

            _webDriver.FindElement(By.Id("newPassword")).SendKeys("P@ssw0rd");
            _webDriver.FindElement(By.Id("reenterPassword")).SendKeys("P@ssw0rd");
            _webDriver.FindElement(By.Id("day")).SendKeys("01");
            _webDriver.FindElement(By.Id("month")).SendKeys("01");
            _webDriver.FindElement(By.Id("year")).SendKeys("2000");
            _webDriver.FindElement(By.Id("tncCheckbox_true")).Click();
        }

        [When(@"I click on Register button")]
        public void WhenIClickOnRegisterButton()
        {
            _webDriver.FindElement(By.Id("continue")).Click();
        }

        [Then(@"I am taken to the Account activated page")]
        public void ThenIAmTakenToTheAccountActivatedPage()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(9);

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            var messageElement = wait.Until(drv => drv.FindElement(By.Id("confirmationMessage")));

            var message = messageElement.GetAttribute("aria-label");
            Assert.AreEqual(message.ToLower(), "you can start using your account to access your information.");
            _webDriver.Quit();

        }
    }
}