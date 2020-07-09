using B2CAzureFunc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Providers.Email;
using Providers.Email.Model;
using System;
using System.Collections.Generic;
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
                Assert.Equal(400, result.status);
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
                Assert.Equal(400, result.status);
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
                Assert.Equal(400, result.status);
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
                Assert.Equal(400, result.status);
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
            var response = await SignupInvitation.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(400, result.status);
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
            var response = await AidedRegistrationValidateUserDetails.Run(request, logger);

            try
            {
                var result = (OkObjectResult)response;
                Assert.Equal(200, result.StatusCode);
            }
            catch (InvalidCastException)
            {
                var result = (ResponseContentModel)((BadRequestObjectResult)response).Value;
                Assert.Equal(400, result.status);
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
                Assert.Equal(400, result.status);
            }
        }

        /// <summary>
        /// TestEmail
        /// </summary>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        //[Fact]
        //public void TestEmail()
        //{
        //    EmailModel model = new EmailModel();
        //    model.To = "aman.gupta@education.gov.uk";
        //    model.EmailTemplate = "3a128969-3d8f-4000-8fc9-2cdb6805691f";
        //    model.Personalisation = new Dictionary<string, dynamic>
        //    { {"name", "Aman"},
        //      {"link", "https://u14104624.ct.sendgrid.net/ls/click?upn=ov4hBpeGva9xEKAuJLxALia4yFiZJCDcHS83r9YQke56P1hOVvMDUP0XpykR0niYc6WDm1Mx9n-2BfsVQb38DU5ty0hrkDFNQJ844m6WNzmTg0GN6f-2BG70WinUu24KLDRps89DAk3-2Bf6ZJZTlZXhQyBDVgltBfPyUAIy495rOSBhZxfrvnMUMHB-2BVKOXAilOXW0PHt2I5OsRwrncBoqk5cRzRXKGwmeVNQtlfSM3u5wP7wU2272BgCcQVOFRtM3vT2OFcAbiNZTjSIB-2Bvg1sFnMSEwaPK8Y6g8NB2TKHAFgSmJqoDxwymDPf0Dz3YtO7TvUypSLTyaeLH9khKJbwO-2FObGy17Zlz3622Y-2B0hxPAjNByBtUZUKKRyOyb2SwUpOdPxmGZhawNQTjf6OSATCE2s4CmKWZIJL6IcjiDc0lOMZLtWy1VRD-2FlNjRCcj0RKGIKc6YIEoN-2Fpl8HhFvj3CaUWnYSBNWoWN38dRTX09acHWVCtp-2BuMORWgHzQhR6magWWR5DvN4dYlCcpDMAFq-2BYBn3xSPhtfrXnNR2mklR7hRyLUtS0kgjPL8ZUeuzoGRgDWPlJZwPlp8x-2B8SNtQSgKkFyc1Q3hrkrj3CcGCEL9uqJzUW8ZL1o7DcdzmHiP-2Fhw2a70Bo-2BSL6n2U-2B-2FqR9S-2Fk-2BUd36wBiusevF-2FMkExsD4RICSYib1KpUExtEY2Z1DQZNP0zqvHDwpoTdJtrwVxBihhou2HQRPMpFc-2FAzRguhLKNtmUv2tys4-2Bmn2y8IfW40UqhMb5qtu1hRR5ta-2Bzi2BIS0Z5avUYa-2BN-2FcTMOAAXtpIi3uLudWoigr93GFQotGEEBHi3LBaKKxve-2F7-2FnheY4-2FfQYsE7sSCTtD9VhSy5P2YrsZ7hHeukhQ7FSXTocMWJPL2-2FH06JOw1IusIYsA55q7DxuFrfS1l8IyqIqJqqF7ulo0t8edbJjKK5ji6qM12ZL9Ie-2BOpJdFYxtMVKY6f1mxmA-3D-3DSRFb_52QZ21t1dafOfDyNahmoWGZevd-2Fzyl-2BdEmluPcPnEZS2BeOOrLvb5T6YFUzR-2B9LB-2F7GCO7HTut40Bl4OML9KRr9ispTGU2QyQoheYUYUlpg8m-2Fd5QZWDT-2FropBlDX3Ra1-2BOyWaGJr-2F2LbcjT38dP1lcSrYGtdnwEJLV5vX68MGG1F8eqVFslCtsJUD4wPB3oeaD6TljW7zKDOzt6qHCETGDyVfunXBUYoLIYXfeItIA-3D"}
        //    };
        //    var result = EmailService.Send(model);
        //    Assert.True(result);
        //}
    }
}