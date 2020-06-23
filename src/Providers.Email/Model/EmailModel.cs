namespace Providers.Email.Model
{
    /// <summary>
    ///     Email Model
    /// </summary>
    public class EmailModel
    {
        /// <summary>
        /// To
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// From
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Name for from email address
        /// </summary>
        public string FromDisplayName { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Email Template
        /// </summary>
        public string EmailTemplate { get; set; }
    }
}