using Newtonsoft.Json;
using System;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// SearchApiResponseModel
    /// </summary>
    public partial class SearchAPIResponseModel
    {
        /// <summary>
        /// OdataContext
        /// </summary>
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [JsonProperty("value")]
        public Value[] Value { get; set; }
    }

    /// <summary>
    /// Value
    /// </summary>
    public partial class Value
    {
        /// <summary>
        /// SearchScore
        /// </summary>
        [JsonProperty("@search.score")]
        public double SearchScore { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        [JsonProperty("CustomerId")]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// DateOfRegistration
        /// </summary>
        [JsonProperty("DateOfRegistration")]
        public DateTimeOffset DateOfRegistration { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [JsonProperty("Title")]
        public long Title { get; set; }

        /// <summary>
        /// GivenName
        /// </summary>
        [JsonProperty("GivenName")]
        public string GivenName { get; set; }

        /// <summary>
        /// FamilyName
        /// </summary>
        [JsonProperty("FamilyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// UniqueLearnerNumber
        /// </summary>
        [JsonProperty("UniqueLearnerNumber")]
        public object UniqueLearnerNumber { get; set; }

        /// <summary>
        /// DateOfBirth
        /// </summary>
        [JsonProperty("DateofBirth")]
        public DateTimeOffset DateOfBirth { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [JsonProperty("Gender")]
        public long Gender { get; set; }

        /// <summary>
        /// OptInUserResearch
        /// </summary>
        [JsonProperty("OptInUserResearch")]
        public bool OptInUserResearch { get; set; }

        /// <summary>
        /// OptInMarketResearch
        /// </summary>
        [JsonProperty("OptInMarketResearch")]
        public bool OptInMarketResearch { get; set; }

        /// <summary>
        /// DateOfTermination
        /// </summary>
        [JsonProperty("DateOfTermination")]
        public object DateOfTermination { get; set; }

        /// <summary>
        /// ReasonForTermination
        /// </summary>
        [JsonProperty("ReasonForTermination")]
        public object ReasonForTermination { get; set; }

        /// <summary>
        /// IntroducedBy
        /// </summary>
        [JsonProperty("IntroducedBy")]
        public long IntroducedBy { get; set; }

        /// <summary>
        /// IntroducedByAdditionalInfo
        /// </summary>
        [JsonProperty("IntroducedByAdditionalInfo")]
        public object IntroducedByAdditionalInfo { get; set; }

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
        /// Address1
        /// </summary>
        [JsonProperty("Address1")]
        public string Address1 { get; set; }

        /// <summary>
        /// PostCode
        /// </summary>
        [JsonProperty("PostCode")]
        public string PostCode { get; set; }

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
        public object EmailAddress { get; set; }
    }
}
