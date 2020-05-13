using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Xml.Linq;
using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration;
using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration.Models;
using System.Linq;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi;
using System.Text;
using B2CAzureFunc.Constants;
using Microsoft.Extensions.Options;
using B2CAzureFunc.Config;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace B2CAzureFunc
{
    /// <summary>
    ///     FunctionDefault
    /// </summary>
    public class FunctionDefault
    {
        private readonly B2COpenApiConfig _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public FunctionDefault(IOptions<B2COpenApiConfig> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// GenerateSwagger
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        [FunctionName("Swagger")]
        public HttpResponseMessage GenerateSwagger([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log, ExecutionContext ctx)
        {
            try
            {
                log.LogInformation("Starting Swagger Retrieval");

                var documentPath = Path.GetFullPath(Path.Combine(ctx.FunctionAppDirectory, $"{FunctionConstants.FunctionName}.xml"));

                var input = new OpenApiGeneratorConfig(
                    annotationXmlDocuments: new List<XDocument>()
                    {
                        XDocument.Load(documentPath),
                    },
                    assemblyPaths: new List<string>()
                    {
                        @$"bin\{FunctionConstants.FunctionName}.dll"
                    },
                    openApiDocumentVersion: "V1",
                    filterSetVersion: FilterSetVersion.V1
                );
                input.OpenApiInfoDescription = _options.ServiceDescription ?? "B2CAzureFunc";

                var generator = new OpenApiGenerator();
                var openApiDocuments = generator.GenerateDocuments(
                    openApiGeneratorConfig: input,
                    generationDiagnostic: out GenerationDiagnostic result
                );

                OpenApiSpecVersion openApiVersion;

                switch (_options.OpenApiVersion)
                {
                    default:
                        openApiVersion = OpenApiSpecVersion.OpenApi2_0;
                        break;
                    case "V3":
                        openApiVersion = OpenApiSpecVersion.OpenApi3_0;
                        break;
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(openApiDocuments.First().Value.SerializeAsJson(openApiVersion), Encoding.UTF8, "application/json")
                };
            }
            catch (System.Exception ex)
            {
                log.LogError("Exception Encountered", ex);
                throw;
            }
        }

        /// <summary>
        ///     Hearbeat - Required for EAPIM health check
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="string"/>Hearbeat Ok</response>
        [FunctionName("Heartbeat")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            return new OkObjectResult("Service Ok");
        }
    }
}
