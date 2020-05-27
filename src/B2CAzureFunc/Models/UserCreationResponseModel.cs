namespace B2CAzureFunc.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    /// <summary>
    /// UserCreationResponseModel
    /// </summary>
    public partial class UserCreationResponseModel
    {
        [JsonProperty("CustomerId")]
        public Guid CustomerId { get; set; }

        [JsonProperty("DateOfRegistration")]
        public DateTimeOffset DateOfRegistration { get; set; }

        [JsonProperty("Title")]
        public long Title { get; set; }

        [JsonProperty("GivenName")]
        public string GivenName { get; set; }

        [JsonProperty("FamilyName")]
        public string FamilyName { get; set; }

        [JsonProperty("DateofBirth")]
        public DateTimeOffset? DateofBirth { get; set; }

        [JsonProperty("Gender")]
        public long Gender { get; set; }

        [JsonProperty("UniqueLearnerNumber")]
        public object UniqueLearnerNumber { get; set; }

        [JsonProperty("OptInUserResearch")]
        public bool OptInUserResearch { get; set; }

        [JsonProperty("OptInMarketResearch")]
        public bool OptInMarketResearch { get; set; }

        [JsonProperty("DateOfTermination")]
        public object DateOfTermination { get; set; }

        [JsonProperty("ReasonForTermination")]
        public object ReasonForTermination { get; set; }

        [JsonProperty("IntroducedBy")]
        public long IntroducedBy { get; set; }

        [JsonProperty("IntroducedByAdditionalInfo")]
        public object IntroducedByAdditionalInfo { get; set; }

        [JsonProperty("LastModifiedDate")]
        public DateTimeOffset LastModifiedDate { get; set; }

        [JsonProperty("LastModifiedTouchpointId")]
        public long LastModifiedTouchpointId { get; set; }
    }
}