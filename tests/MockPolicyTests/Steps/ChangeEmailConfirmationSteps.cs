using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace PolicyTests.Steps
{
    [Binding]
    public class ChangeEmailConfirmationSteps
    {
        private IWebDriver _webDriver;
        private bool SkipStep = false;
        public ChangeEmailConfirmationSteps()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _webDriver = new FirefoxDriver(options);
        }

        [Given(@"I'm navigated to change email confirmation page")]
        public void GivenIMInChangeEmailConfirmationPage()
        {
            _webDriver.Navigate().GoToUrl("https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/B2C_1A_Change_Email_mock/oauth2/v2.0/authorize?client_id=162b73a0-4972-42de-ad41-f15c5b4d9b51&nonce=607662aae86c4eff9602d0297a43d008&redirect_uri=https://jwt.ms/&scope=openid&response_type=id_token&id_token_hint=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2F1dGhuY3MtZGV2LWZ1bmN0aW9uLmF6dXJld2Vic2l0ZXMubmV0LyIsImlhdCI6MTU5OTIwMzk5MywiZXhwIjoxOTE0NzM2NzkzLCJhdWQiOiIxNjJiNzNhMC00OTcyLTQyZGUtYWQ0MS1mMTVjNWI0ZDliNTEiLCJzdWIiOiJBbWFuIiwiZW1haWwiOiJhbWFuZ3VwdGF6MTE3QHlvcG1haWwuY29tIiwiZXhwaXJ5IjoiMjAzMC0wOS0wMlQxNzoyODozOSIsIm9iamVjdElkIjoiNGYyNjgwNTAtYTMyOS00MjhkLWJjOWEtMzMyYWRlZDJlNjM1IiwibmV3RW1haWwiOiJhbWFuZ3VwdGF6MTE2QHlvcG1haWwuY29tIiwiam91cm5leSI6ImNoYW5nZWVtYWlsIn0.T9m05vDmtnwTJGWxUj0ynYQytX7HBiDVF6iQ689iOfk&idtokenhint=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2F1dGhuY3MtZGV2LWZ1bmN0aW9uLmF6dXJld2Vic2l0ZXMubmV0LyIsImlhdCI6MTU5OTIwMzk5MywiZXhwIjoxOTE0NzM2NzkzLCJhdWQiOiIxNjJiNzNhMC00OTcyLTQyZGUtYWQ0MS1mMTVjNWI0ZDliNTEiLCJzdWIiOiJBbWFuIiwiZW1haWwiOiJhbWFuZ3VwdGF6MTE3QHlvcG1haWwuY29tIiwiZXhwaXJ5IjoiMjAzMC0wOS0wMlQxNzoyODozOSIsIm9iamVjdElkIjoiNGYyNjgwNTAtYTMyOS00MjhkLWJjOWEtMzMyYWRlZDJlNjM1IiwibmV3RW1haWwiOiJhbWFuZ3VwdGF6MTE2QHlvcG1haWwuY29tIiwiam91cm5leSI6ImNoYW5nZWVtYWlsIn0.T9m05vDmtnwTJGWxUj0ynYQytX7HBiDVF6iQ689iOfk");
        }

        [Then(@"Check whether account is already activated")]
        public void ThenCheckWhetherAccountIsAlreadyActivated()
        {
            try
            {
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
                var messageElement = wait.Until(drv => drv.FindElement(By.Id("errorMessage")));

                var message = messageElement.GetAttribute("aria-label");
                if (message.ToLower() == "the activation link has expired")
                {
                    var nextButton = _webDriver.FindElement(By.Id("continue"));
                    nextButton?.Click();
                    try
                    {
                        messageElement = wait.Until(drv => drv.FindElement(By.Id("claimVerificationServerError")));
                        message = messageElement.GetAttribute("innerHTML");
                        Assert.AreEqual(message.Trim().ToLower(), "account_activated");
                        SkipStep = true;
                    }
                    catch { }

                    try
                    {
                        if (!SkipStep)
                        {
                            wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5));
                            messageElement = wait.Until(drv => drv.FindElement(By.Id("confirmationMessage")));
                            Assert.AreEqual(true, messageElement != null);
                            _webDriver.Quit();

                        }
                    }
                    catch { }
                }
            }
            catch (Exception)
            { }
        }

        [Then(@"If account is not activated them should redirected to we've changed email page")]
        public void ThenIShouldRedirectedToWeVeChangedEmailPage()
        {
            if (!SkipStep)
            {
                var messageElement = _webDriver.FindElement(By.Id("confirmationMessage"));
                var message = messageElement.GetAttribute("aria-label");
                Assert.AreEqual(message.ToLower(), "you can start using your account to access your information.");
                _webDriver.Quit();
            }
            _webDriver.Quit();
        }
    }
}