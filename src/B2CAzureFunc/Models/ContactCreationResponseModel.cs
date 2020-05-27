using System;
using Newtonsoft.Json;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// ContactCreationResponseModel
    /// </summary>
    public partial class ContactCreationResponseModel
    {
        [JsonProperty("ContactId")]
        public Guid ContactId { get; set; }

        [JsonProperty("CustomerId")]
        public Guid CustomerId { get; set; }

        [JsonProperty("PreferredContactMethod")]
        public long PreferredContactMethod { get; set; }

        [JsonProperty("MobileNumber")]
        public object MobileNumber { get; set; }

        [JsonProperty("HomeNumber")]
        public object HomeNumber { get; set; }

        [JsonProperty("AlternativeNumber")]
        public object AlternativeNumber { get; set; }

        [JsonProperty("EmailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("LastModifiedDate")]
        public DateTimeOffset LastModifiedDate { get; set; }

        [JsonProperty("LastModifiedTouchpointId")]
        public long LastModifiedTouchpointId { get; set; }
    }
}
