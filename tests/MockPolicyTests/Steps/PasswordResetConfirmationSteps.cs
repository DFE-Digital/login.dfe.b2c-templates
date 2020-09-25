using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using TechTalk.SpecFlow;

namespace PolicyTests.Steps
{
    [Binding]
    public class PasswordResetConfirmationSteps
    {
        private IWebDriver _webDriver;

        public PasswordResetConfirmationSteps()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _webDriver = new FirefoxDriver(options);
        }

        [Given(@"I'm in password reset confirmation page")]
        public void GivenIMInPasswordResetConfirmationPage()
        {
            _webDriver.Navigate().GoToUrl("https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/B2C_1A_Password_Reset_Confirmation_mock/oauth2/v2.0/authorize?client_id=162b73a0-4972-42de-ad41-f15c5b4d9b51&nonce=da477bb66436479e932c2e723d8f57c2&redirect_uri=https://jwt.ms/&scope=openid&response_type=id_token&id_token_hint=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2F1dGhuY3MtZGV2LWZ1bmN0aW9uLmF6dXJld2Vic2l0ZXMubmV0LyIsImlhdCI6MTU5OTIwMzk5MywiZXhwIjoxOTE0NzM2NzkzLCJhdWQiOiIxNjJiNzNhMC00OTcyLTQyZGUtYWQ0MS1mMTVjNWI0ZDliNTEiLCJzdWIiOiJBbWFuIiwiZW1haWwiOiJhbWFuZ3VwdGF6MTE3QHlvcG1haWwuY29tIiwiZ2l2ZW5OYW1lIjoiQW1hbiIsImV4cGlyeSI6IjIwMzAtMDktMDJUMTc6Mjg6MzkiLCJvYmplY3RJZCI6IjRmMjY4MDUwLWEzMjktNDI4ZC1iYzlhLTMzMmFkZWQyZTYzNSIsImpvdXJuZXkiOiJwYXNzd29yZHJlc2V0In0.LmK8Yr933Wh3FjoDwjtHOzOfMgAb-Q7fJQ6eMxVTyQU&idtokenhint=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2F1dGhuY3MtZGV2LWZ1bmN0aW9uLmF6dXJld2Vic2l0ZXMubmV0LyIsImlhdCI6MTU5OTIwMzk5MywiZXhwIjoxOTE0NzM2NzkzLCJhdWQiOiIxNjJiNzNhMC00OTcyLTQyZGUtYWQ0MS1mMTVjNWI0ZDliNTEiLCJzdWIiOiJBbWFuIiwiZW1haWwiOiJhbWFuZ3VwdGF6MTE3QHlvcG1haWwuY29tIiwiZ2l2ZW5OYW1lIjoiQW1hbiIsImV4cGlyeSI6IjIwMzAtMDktMDJUMTc6Mjg6MzkiLCJvYmplY3RJZCI6IjRmMjY4MDUwLWEzMjktNDI4ZC1iYzlhLTMzMmFkZWQyZTYzNSIsImpvdXJuZXkiOiJwYXNzd29yZHJlc2V0In0.LmK8Yr933Wh3FjoDwjtHOzOfMgAb-Q7fJQ6eMxVTyQU");
        }

        [Given(@"I enter new password and confirmation password")]
        public void GivenIEnterNewPasswordAndConfirmationPassword()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            _webDriver.FindElement(By.Id("newPassword")).SendKeys("P@ssw0rd");
            _webDriver.FindElement(By.Id("reenterPassword")).SendKeys("P@ssw0rd");
        }

        [When(@"I click reset password button")]
        public void WhenIClickResetPasswordButton()
        {
            _webDriver.FindElement(By.Id("continue")).Click();
        }

        [Then(@"I should redirected to password reset confirmation message page")]
        public void ThenIShouldRedirectedToPasswordResetConfirmationMessagePage()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var messageElement = _webDriver.FindElement(By.Id("confirmationMessage"));
            var message = messageElement.GetAttribute("aria-label");
            Assert.AreEqual(message.ToLower(), "we've changed your password");
            _webDriver.Quit();
        }
    }
}