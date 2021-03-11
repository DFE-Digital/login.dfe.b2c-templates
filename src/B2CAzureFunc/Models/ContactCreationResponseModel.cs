using System;
using Newtonsoft.Json;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// ContactCreationResponseModel
    /// </summary>
    public partial class ContactCreationResponseModel
    {
        /// <summary>
        /// ContactId
        /// </summary>
        [JsonProperty("ContactId")]
        public Guid ContactId { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        [JsonProperty("CustomerId")]
        public Guid CustomerId { get; set; }
        /// <summary>
        /// PreferredContactMethod
        /// </summary>
        [JsonProperty("PreferredContactMethod")]
        public long PreferredContactMethod { get; set; }
        /// <summary>
        /// MobileNumber
        /// </summary>
        [JsonProperty("MobileNumber")]
        public object MobileNumber { get; set; }
        /// <summary>
        /// HomeNumber
        /// </summary>
        [JsonProperty("HomeNumber")]
        public object HomeNumber { get; set; }
        /// <summary>
        /// AlternativeNumber
        /// </summary>
        [JsonProperty("AlternativeNumber")]
        public object AlternativeNumber { get; set; }
        /// <summary>
        /// EmailAddress
        /// </summary>
        [JsonProperty("EmailAddress")]
        public string EmailAddress { get; set; }
        /// <summary>
        /// LastModifiedDate
        /// </summary>
        [JsonProperty("LastModifiedDate")]
        public DateTimeOffset LastModifiedDate { get; set; }
        /// <summary>
        /// LastModifiedTouchpointId
        /// </summary>
        [JsonProperty("LastModifiedTouchpointId")]
        public long LastModifiedTouchpointId { get; set; }
    }
}
