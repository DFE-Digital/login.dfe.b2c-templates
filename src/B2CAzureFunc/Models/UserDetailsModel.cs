using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    class UserDetailsModel
    {
        [JsonProperty(PropertyName = "odata.metadata")]
        public string meta { get; set; }
        [JsonProperty(PropertyName = "odata.nextLink")]
        public string nextLink { get; set; }
        public List<UserValueModel> value { get; set; }
    }
}