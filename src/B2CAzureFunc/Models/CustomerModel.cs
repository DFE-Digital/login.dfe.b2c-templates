using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// CustomerModel
    /// </summary>
    public class CustomerModel
    {
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
        /// DateofBirth
        /// </summary>
        [JsonProperty("DateofBirth")]
        public DateTimeOffset? DateofBirth { get; set; }


        /// <summary>
        /// Gender
        /// </summary>
        [JsonProperty("Gender")]
        public long Gender { get; set; }

        /// <summary>
        /// UniqueLearnerNumber
        /// </summary>
        [JsonProperty("UniqueLearnerNumber")]
        public object UniqueLearnerNumber { get; set; }

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
        public long LastModifiedTouchpointId { get; set; }
    }
}
