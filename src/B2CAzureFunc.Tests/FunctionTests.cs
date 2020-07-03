using B2CAzureFunc.Models;
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
            var response = await ChangeEmail.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(409, result.status);
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
            var response = await FindEmail.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(409, result.status);
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
            var response = await PasswordResetConfirmation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(409, result.status);
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
            var response = await SignupConfirmation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(409, result.status);
            }
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
            var response = await SignupInvitation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(409, result.status);
            }
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
            var response = await AidedRegistrationValidateUserDetails.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(409, result.status);
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
            var response = await NCSDSSUserCreation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(409, result.status);
            }
        }
    }
}