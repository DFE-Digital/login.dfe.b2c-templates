using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    public partial class SearchAPIResponseModel
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public Value[] Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("@search.score")]
        public double SearchScore { get; set; }

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

        [JsonProperty("UniqueLearnerNumber")]
        public object UniqueLearnerNumber { get; set; }

        [JsonProperty("DateofBirth")]
        public DateTimeOffset DateofBirth { get; set; }

        [JsonProperty("Gender")]
        public long Gender { get; set; }

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
        public string LastModifiedTouchpointId { get; set; }

        [JsonProperty("Address1")]
        public string Address1 { get; set; }

        [JsonProperty("PostCode")]
        public string PostCode { get; set; }

        [JsonProperty("MobileNumber")]
        public object MobileNumber { get; set; }

        [JsonProperty("HomeNumber")]
        public object HomeNumber { get; set; }

        [JsonProperty("AlternativeNumber")]
        public object AlternativeNumber { get; set; }

        [JsonProperty("EmailAddress")]
        public object EmailAddress { get; set; }
    }
}
