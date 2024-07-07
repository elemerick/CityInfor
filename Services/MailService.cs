namespace CityInfor.API.Services
{
    public class MailService : IMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailFrom = _configuration["mailSettings:mailFromAddress"];
            _mailTo = _configuration["mailSettings:mailToAddress"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Sending a mail from {_configuration["mailSettings:mailFromAddress"]} to {_mailTo}, with the {nameof(MailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
