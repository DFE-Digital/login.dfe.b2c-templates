using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// UserDetailsModel
    /// </summary>
    class UserDetailsModel
    {
        /// <summary>
        /// meta
        /// </summary>
        [JsonProperty(PropertyName = "odata.metadata")]
        public string meta { get; set; }
        /// <summary>
        /// nextLink
        /// </summary>
        [JsonProperty(PropertyName = "odata.nextLink")]
        public string nextLink { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public List<UserValueModel> value { get; set; }
    }
}