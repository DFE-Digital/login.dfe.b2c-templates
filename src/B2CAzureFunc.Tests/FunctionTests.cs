using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace B2CAzureFunc.Tests
{
    public class FunctionTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();
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
            var response = (OkObjectResult)await ChangeEmail.Run(request, logger);
            Assert.Equal(200, response.StatusCode);
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
            var response = (OkObjectResult)await FindEmail.Run(request, logger);
            Assert.Equal(200, response.StatusCode);
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
            var response = (OkObjectResult)await PasswordResetConfirmation.Run(request, logger);
            Assert.Equal(200, response.StatusCode);
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
            var response = (OkObjectResult)await SignupConfirmation.Run(request, logger);
            Assert.Equal(200, response.StatusCode);
        }

        /// <summary>
        /// SignupInvitationAsync
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.SignupConfirmationData), MemberType = typeof(TestFactory))]
        public async Task SignupInvitationAsync(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var response = (OkObjectResult)await SignupInvitation.Run(request, logger);
            Assert.Equal(200, response.StatusCode);
        }

        /// <summary>
        /// AidedRegValidaeUserDetails
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFactory.ValidateUserData), MemberType = typeof(TestFactory))]
        public async Task AidedRegValidaeUserDetails(string queryStringValue)
        {
            var body = queryStringValue;
            var request = TestFactory.CreateHttpRequest(body);
            var response = (OkObjectResult)await AidedRegistrationValidateUserDetails.Run(request, logger);
            Assert.Equal(200, response.StatusCode);
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
            var response = (OkObjectResult)await NCSDSSUserCreation.Run(request, logger);
            Assert.Equal(200, response.StatusCode);
        }
    }
}