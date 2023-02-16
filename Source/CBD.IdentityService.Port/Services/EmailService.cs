using System.Net;
using System.Net.Mail;
using System.Text;

using CBD.IdentityService.Core.Options;
using CBD.IdentityService.Core.Services;

using Microsoft.Extensions.Options;

namespace CBD.IdentityService.Port.Services; 

public class EmailService : IEmailService {
	private readonly EmailHostOptions _emailHostOptions;
	
	public EmailService(IOptions<EmailHostOptions> emailHostOptions) {
		this._emailHostOptions = emailHostOptions.Value;
	}

	public async Task SendEmailAsync(string to, string subject, string message) {
		var smtpClient = new SmtpClient {
			Host = this._emailHostOptions.AuthEmailHost,
			UseDefaultCredentials = false,
			Credentials = new NetworkCredential(this._emailHostOptions.AuthEmail, this._emailHostOptions.AuthEmailPassword),
			Port = this._emailHostOptions.AuthEmailHostPort,
			EnableSsl = true
		};

		var mailMessage = new MailMessage(this._emailHostOptions.AuthEmail, to) {
			Subject = subject,
			Body = message,
			BodyEncoding = Encoding.UTF8,
			IsBodyHtml = true
		};

		await smtpClient.SendMailAsync(mailMessage);
	}
}