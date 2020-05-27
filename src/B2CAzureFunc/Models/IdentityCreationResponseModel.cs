using System;
using Newtonsoft.Json;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// IdentityCreationResponseModel
    /// </summary>
    public partial class IdentityCreationResponseModel
    {
        [JsonProperty("IdentityID")]
        public Guid IdentityId { get; set; }

        [JsonProperty("CustomerId")]
        public Guid CustomerId { get; set; }

        [JsonProperty("IdentityStoreId")]
        public Guid IdentityStoreId { get; set; }

        [JsonProperty("LegacyIdentity")]
        public object LegacyIdentity { get; set; }

        [JsonProperty("id_token")]
        public object IdToken { get; set; }

        [JsonProperty("LastLoggedInDateTime")]
        public object LastLoggedInDateTime { get; set; }

        [JsonProperty("LastModifiedDate")]
        public DateTimeOffset LastModifiedDate { get; set; }

        [JsonProperty("LastModifiedTouchpointId")]
        public string LastModifiedTouchpointId { get; set; }

        [JsonProperty("DateOfTermination")]
        public object DateOfTermination { get; set; }
    }
}
