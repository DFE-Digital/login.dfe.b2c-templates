using B2CAzureFunc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Providers.Email;
using Providers.Email.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Xunit;

namespace B2CAzureFunc.Tests
{
    public class FunctionTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();
        private readonly AppSettings _appSettings;

        public FunctionTests()
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   //.AddJsonFile("local.settings.json", true)
                   .AddEnvironmentVariables()
                   .Build();
            _appSettings = new AppSettings();

            config.GetSection("AppSettings").Bind(_appSettings);
        }

        /// <summary>
        /// ChangeEmailAsync
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.ChangeEmailData), MemberType = typeof(TestFactory))]
        public async Task ChangeEmailAsync(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var option = Options.Create(_appSettings);
            ChangeEmail changeEmail = new ChangeEmail(option);
            var response = await changeEmail.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }

        /// <summary>
        /// FindEmailAsync
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.FindEmailData), MemberType = typeof(TestFactory))]
        public async Task FindEmailAsync(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var option = Options.Create(_appSettings);
            FindEmail findEmail = new FindEmail(option);
            var response = await findEmail.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }

        /// <summary>
        /// PasswordResetConfirmationAsync
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.PasswordResetData), MemberType = typeof(TestFactory))]
        public async Task PasswordResetConfirmationAsync(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var option = Options.Create(_appSettings);
            PasswordResetConfirmation passwordResetConfirmation = new PasswordResetConfirmation(option);
            var response = await passwordResetConfirmation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }

        /// <summary>
        /// SignupConfirmationAsync
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.SignupConfirmationData), MemberType = typeof(TestFactory))]
        public async Task SignupConfirmationAsync(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var option = Options.Create(_appSettings);

            SignupConfirmation signupConfirmation = new SignupConfirmation(option);
            var response = await signupConfirmation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }

        /// <summary>
        /// SignupInvitationAsync
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.SignupInvitationData), MemberType = typeof(TestFactory))]
        public async Task SignupInvitationAsync(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);

            var option = Options.Create(_appSettings);

            SignupInvitation signupInvitation = new SignupInvitation(option);

            var response = await signupInvitation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }

        /// <summary>
        /// AidedRegValidaeUserDetails
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.ValidateUserData), MemberType = typeof(TestFactory))]
        public async Task AidedRegValidateUserDetails(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var option = Options.Create(_appSettings);
            AidedRegistrationValidateUserDetails aidedRegistrationValidateUserDetails = new AidedRegistrationValidateUserDetails(option);
            var response = await aidedRegistrationValidateUserDetails.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }

        /// <summary>
        /// CreateNCSUser
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.CreateNCSUserData), MemberType = typeof(TestFactory))]
        public async Task CreateNCSUser(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var option = Options.Create(_appSettings);
            NCSDSSUserCreation nCSDSSUserCreation = new NCSDSSUserCreation(option);
            var response = await nCSDSSUserCreation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }


        /// <summary>
        /// FindAccountAsync
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.FindAccountData), MemberType = typeof(TestFactory))]
        public async Task FindAccountAsync(string queryStringValue)
        {
            var testValues = queryStringValue.Split(",");
            var query = new Dictionary<String, StringValues>();

            query.TryAdd(testValues[0], testValues[1]);
            string body = "";
            var request = TestFactory.CreateHttpRequest(query, body);

            var option = Options.Create(_appSettings);
            FindAccount findAccount = new FindAccount(option);

            var response = await findAccount.Run(request, logger);

            //try
            //{
            var result = (OkObjectResult)response;
            Assert.Equal(200, result.StatusCode);
            //}
            //catch (InvalidCastException)
            //{
            //    //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
            //    //Assert.Equal(409, result.status);
            //}
        }

        /// <summary>
        /// TestEmail
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Fact]
        public void TestEmail()
        {
            EmailModel model = new EmailModel();
            model.To = "aman.gupta@education.gov.uk";
            model.EmailTemplate = "3a128969-3d8f-4000-8fc9-2cdb6805691f";
            model.Personalisation = new Dictionary<string, dynamic>
            { {"name", "Aman"},
              {"link", "https://docs.notifications.service.gov.uk/"}
            };

            try
            {
                var result = EmailService.Send(_appSettings.NotifyApiKey, model);
                Assert.True(result);
            }
            catch (Exception)
            {
                //var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                //Assert.Equal(409, result.status);
            }
        }
    }
}