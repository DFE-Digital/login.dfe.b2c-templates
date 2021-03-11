using B2CAzureFunc.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace B2CAzureFunc.Helpers
{
    /// <summary>
    /// BlobReader
    /// </summary>
    public class BlobReader
    {
        /// <summary>
        /// GetTnCDateDetails
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="containerName"></param>
        /// <param name="storageConnectionString"></param>
        /// <returns>TnCDetailModel</returns>
        public async Task<TnCDetailModel> GetTnCDateDetails(string fileName, string containerName, string storageConnectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            using (var memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                string content = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                var tncDetails = JsonConvert.DeserializeObject<TnCDetailModel>(content);
                return tncDetails;
            }
        }
    }
}