using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using B2CAzureFunc.Models;
using B2CAzureFunc.Helpers;

namespace B2CAzureFunc
{
    /// <summary>
    /// GetTncDetails
    /// </summary>
    public class GetTncDetails
    {

        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public GetTncDetails(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        /// <summary>
        ///  GetTncDetails
        /// </summary>
        /// <verb>GET</verb>
        /// <url>http://localhost:7070/GetTncDetails</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="EmailFoundResponseModel"/>Tnc details</response>
        /// <response code="400"><see cref="Object"/>BadRequestObjectResult</response>
        [FunctionName("GetTncDetails")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                var fileName = _appSettings.TncFileName;
                var containerName = _appSettings.TncContainerName;
                var connectionString = _appSettings.StorageAccountConnectionString;
                BlobReader blobReader = new BlobReader();
                var tnCDetail = await blobReader.GetTnCDateDetails(fileName, containerName, connectionString);

                return (ActionResult)new OkObjectResult(tnCDetail);
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                    status = 400
                });
            }
        }
    }
}