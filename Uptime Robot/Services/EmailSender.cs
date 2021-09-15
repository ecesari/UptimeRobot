using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Uptime_Robot.Infrastructure;

namespace Uptime_Robot.Services
{
    public class EmailSender:IEmailSender
    {
	    private readonly ILogger<EmailSender> _logger;
	    public AuthMessageSenderOptions Options { get; }
	    public EmailSender(ILogger<EmailSender> logger, IOptions<AuthMessageSenderOptions> optionsAccessor)
	    {
		    _logger = logger;
		    Options = optionsAccessor.Value;
	    }


	    public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey ?? "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", subject, message, email);
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
	            _logger.LogError("An e-mail has not been sent!", subject,message,email);
            }
            return clientResponse;
        }
    }
}
