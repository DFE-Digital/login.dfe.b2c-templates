using System;
using Newtonsoft.Json;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// IdentityCreationResponseModel
    /// </summary>
    public partial class IdentityCreationResponseModel
    {
        /// <summary>
        /// IdentityId
        /// </summary>
        [JsonProperty("IdentityID")]
        public Guid IdentityId { get; set; }
        /// <summary>
        /// CustomerId
        /// </summary>
        [JsonProperty("CustomerId")]
        public Guid CustomerId { get; set; }
        /// <summary>
        /// IdentityStoreId
        /// </summary>
        [JsonProperty("IdentityStoreId")]
        public Guid IdentityStoreId { get; set; }
        /// <summary>
        /// LegacyIdentity
        /// </summary>
        [JsonProperty("LegacyIdentity")]
        public object LegacyIdentity { get; set; }
        /// <summary>
        /// IdToken
        /// </summary>
        [JsonProperty("id_token")]
        public object IdToken { get; set; }
        /// <summary>
        /// LastLoggedInDateTime
        /// </summary>
        [JsonProperty("LastLoggedInDateTime")]
        public object LastLoggedInDateTime { get; set; }
        /// <summary>
        /// LastModifiedDate
        /// </summary>
        [JsonProperty("LastModifiedDate")]
        public DateTimeOffset LastModifiedDate { get; set; }
        /// <summary>
        /// LastModifiedTouchpointId
        /// </summary>
        [JsonProperty("LastModifiedTouchpointId")]
        public string LastModifiedTouchpointId { get; set; }
        /// <summary>
        /// DateOfTermination
        /// </summary>
        [JsonProperty("DateOfTermination")]
        public object DateOfTermination { get; set; }
    }
}
