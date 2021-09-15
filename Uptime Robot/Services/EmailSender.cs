using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Uptime_Robot.Services
{
    public class EmailSender:IEmailSender
    {
	    private readonly ILogger<EmailSender> _logger;

	    public EmailSender(ILogger<EmailSender> logger)
	    {
		    _logger = logger;
	    }


	    //public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        //{
        //    Options = optionsAccessor.Value;
        //}

        //public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute("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", subject, message, email);
        }

        public async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("iobojbssivqegrovat@uivvn.net", "Uptime Robot"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            var clientResponse = await client.SendEmailAsync(msg);
            if (clientResponse.StatusCode != HttpStatusCode.Accepted)
            {
	            _logger.LogCritical("An e-mail has not been sent!", subject,message,email);
            }
            return clientResponse;
        }
    }
}
