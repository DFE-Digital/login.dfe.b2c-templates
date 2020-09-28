using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;

namespace PolicyTests.Steps
{
    [Binding]
    public class SignInSteps
    {
        private IWebDriver _webDriver;

        public SignInSteps()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _webDriver = new FirefoxDriver(options);
        }

        [Given(@"I'm in the signin page")]
        public void GivenIMInTheSigninPage()
        {
            _webDriver.Navigate().GoToUrl("https://devauthncs.b2clogin.com/devauthncs.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1A_Signin_Invitation_mock&client_id=162b73a0-4972-42de-ad41-f15c5b4d9b51&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fjwt.ms&scope=openid&response_type=id_token&prompt=login");
        }

        [Given(@"I enter email id and password")]
        public void GivenIEnterEmailIdAndPassword()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(9);

            _webDriver.FindElement(By.Id("email")).SendKeys("amanguptaz117@yopmail.com");
            _webDriver.FindElement(By.Id("password")).SendKeys("P@ssw0rd");
        }

        [When(@"I click sign in")]
        public void WhenIClickSignIn()
        {
            _webDriver.FindElement(By.Id("next")).Click();
        }

        [Then(@"I may redirected to Ts & Cs page if new Ts & Cs rolled out")]
        public void ThenIMayRedirectedToTsCsPageIfNewTsCsRolledOut()
        {
            try
            {
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(9);

                var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
                var element = wait.Until(drv => drv.FindElement(By.Id("tncCheckbox_true")));
                if (element != null)
                {
                    element.Click();
                }
            }
            catch (Exception)
            { }
        }

        [Then(@"I should be redicected to jwt\.ms with a valid token")]
        public void ThenIShouldBeRedicectedToJwt_MsWithAValidToken()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(9);

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(drv => drv.FindElement(By.Id("decodedToken")));
            Assert.AreEqual(true, _webDriver.Url.StartsWith("https://jwt.ms/#id_token="));

            _webDriver.Quit();

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create("9e28727a-9abe-4d3b-80e8-367889846e1c")
                .WithTenantId("devauthncs.onmicrosoft.com")
                .WithClientSecret("S5EUqH6PyEI.2qqztYlC4Pqx.1B.~8GTe7")
                .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
            GraphServiceClient _graphClient = new GraphServiceClient(authProvider);

            var email = "amanguptaz117@yopmail.com";
            var user = _graphClient.Users
                            .Request()
                            .Filter($"identities/any(c:c/issuerAssignedId eq '{email}' and c/issuer eq '{email}')")
                            .GetAsync().Result;

            _graphClient.Users[user[0].Id]
                .Request()
                .DeleteAsync().Wait();
        }
    }
}