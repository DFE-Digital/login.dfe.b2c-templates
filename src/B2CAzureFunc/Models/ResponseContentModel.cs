namespace B2CAzureFunc.Models
{
    /// <summary>
    /// Response Content Model
    /// </summary>
    public class ResponseContentModel
    {
        /// <summary>
        /// version
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// status
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// userMessage
        /// </summary>
        public string userMessage { get; set; }
        /// <summary>
        /// developerMessage
        /// </summary>
        public string developerMessage { get; set; }
        /// <summary>
        /// requestId
        /// </summary>
        public string requestId { get; set; }
        /// <summary>
        /// moreInfo
        /// </summary>
        public string moreInfo { get; set; }
        /// <summary>
        /// isFound
        /// </summary>
        public bool isFound { get; set; }
    }
}
