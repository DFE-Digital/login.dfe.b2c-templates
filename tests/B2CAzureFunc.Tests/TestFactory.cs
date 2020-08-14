using B2CAzureFunc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace B2CAzureFunc.Tests
{
    public class TestFactory
    {
        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
        public static HttpRequest CreateHttpRequest(string body)
        {
            var reqMock = new Mock<HttpRequest>();

            //reqMock.Setup(req => req.Query).Returns(new QueryCollection(query));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            reqMock.Setup(req => req.Body).Returns(stream);
            return (HttpRequest)reqMock.Object;
        }

        public static IEnumerable<object[]> ChangeEmailData()
        {
            var list = new List<object[]>
            {
                new object[] { JsonConvert.SerializeObject( new ChangeEmailModel { NewEmail = "aman.guptaz@yopmail.com", ObjectId = "3d76dca2-5dc9-40cd-bc59-6faebb6ee6be", IsResend = false }) }
            };
            return list;
        }

        public static IEnumerable<object[]> FindEmailData()
        {
            var list = new List<object[]>
            {
                new object[] { JsonConvert.SerializeObject(new FindEmailModel
                { Day = "26", Month = "07", Year = "2005", GivenName="TommyJo",Surname="Smith",PostalCode="DE21 5DE"}) }
            };
            return list;
        }

        public static IEnumerable<object[]> PasswordResetData()
        {
            var list = new List<object[]>
            {
                new object[] { JsonConvert.SerializeObject(new { email = "johnsmith123z@yopmail.com", givenName = "John", ObjectId = Guid.NewGuid().ToString()}) }
            };
            return list;
        }

        public static IEnumerable<object[]> SignupConfirmationData()
        {
            var list = new List<object[]>
            {
                new object[] { JsonConvert.SerializeObject(new SignupConfirmationModel
                {Email="amanguptaz@yopmail.com",GivenName="Aman", ObjectId=Guid.NewGuid().ToString()}) }
            };
            return list;
        }

        public static IEnumerable<object[]> SignupInvitationData()
        {
            var list = new List<object[]>
            {
                new object[] {JsonConvert.SerializeObject( new SignupInvitationModel
                {Email="johnsmith123z@yopmail.com",GivenName="John",LastName="Smith", CustomerId="f36f38a8-2dd6-4776-a304-b41df66ba105"}) }
            };
            return list;
        }

        public static IEnumerable<object[]> ValidateUserData()
        {
            var list = new List<object[]>
            {
                new object[] { JsonConvert.SerializeObject(new FindEmailModel
                { Day = "27", Month = "06", Year = "2005", GivenName="Aman",Surname="Gupta",Email="amangupta@yopmail.com"}) }
            };
            return list;
        }

        public static IEnumerable<object[]> CreateNCSUserData()
        {
            var list = new List<object[]>
            {
                new object[] {JsonConvert.SerializeObject( new UserCreationModel
                { Day = "27", Month = "06", Year = "2005", GivenName="john",Surname="smith"
                ,Email="johnsmithz2@yopmail.com",IsAided=false,ObjectId="05a0b92d-308a-4bcf-b046-200e77073a48"}) }
            };
            return list;
        }
    }
}