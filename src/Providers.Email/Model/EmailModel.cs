namespace Providers.Email.Model
{
    public class EmailModel
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string EmailTemplate { get; set; }
    }
}