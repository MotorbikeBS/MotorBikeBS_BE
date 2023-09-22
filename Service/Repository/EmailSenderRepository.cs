using Microsoft.Extensions.Configuration;
using Service.Service;
using System.Net;
using System.Net.Mail;

namespace Service.Repository
{
    public class EmailSenderRepository : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSenderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public Task SendEmailAsync(string email, string subject, string message)
        //{
        //	try
        //	{
        //		string mail = _configuration["MailSettings:Mail"];
        //		string password = _configuration["MailSettings:Password"];

        //		var client = new SmtpClient("smtp.gmail.com", 587)
        //		{
        //			EnableSsl = true,
        //			Credentials = new NetworkCredential(mail, password)
        //		};

        //		return client.SendMailAsync(
        //			new MailMessage(from: mail,
        //			to: email,
        //			subject: subject,
        //			message
        //			));
        //	}
        //	catch (Exception ex)
        //	{
        //		throw new Exception(ex.Message);
        //	}
        //}
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                string mail = _configuration["MailSettings:Mail"];
                string password = _configuration["MailSettings:Password"];

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true // Set this to true to indicate that the body contains HTML
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
