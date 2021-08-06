using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Keon.Eml;

namespace Redirector
{
	public class Email
	{
		public Email(String[] from, String subject, String messageId, String email)
		{
			this.from = from;
			this.subject = subject;
			this.messageId = messageId;
			this.email = email.Replace("\r", "");

			Console.WriteLine($"[{messageId}] {subject}");
		}

		private readonly String[] from;
		private readonly String subject;
		private readonly String messageId;
		private readonly String email;

		internal String Body { get; private set; }

		public Email BuildBody(DateTime date, String[] to, params String[] messageIds)
		{
			var toAll = String.Join(",", to);
			var messageIdsAll = String.Join(",", messageIds);

			Body = $"To: {toAll}<br />"
				+ $"Date: {date}<br />"
				+ $"Message IDs: {messageIdsAll}<br />"
				+ new EmlReader(email).Body;

			return this;
		}

		public void Send()
		{
			var message = buildMessage();

			var credential = new NetworkCredential(
				Cfg.Smtp.Username,
				Cfg.Smtp.Password
			);

			var host = $"email-smtp.{Cfg.Region.SystemName}.amazonaws.com";
			var client = new SmtpClient(host, Cfg.Smtp.Port)
			{
				Credentials = credential,
				EnableSsl = true,
			};

			Console.WriteLine($"Trying to send the e-mail {messageId}");
			client.Send(message);
			Console.WriteLine($"E-mail {messageId} sent successfully!");
		}

		private MailMessage buildMessage()
		{
			var message = new MailMessage
			{
				IsBodyHtml = true,
				From = new MailAddress(Cfg.Smtp.From, Cfg.Smtp.FromName),
				Subject = subject,
				Body = Body,
			};
			message.To.Add(new MailAddress(Cfg.Smtp.To));

			foreach (var contact in from)
			{
				add(message.ReplyToList, contact);
			}

			return message;
		}

		private void add(MailAddressCollection list, String contact)
		{
			var regex = new Regex("^(.+) ?<(.+)>");

			if (regex.IsMatch(contact))
			{
				var pieces = regex.Match(contact)
					.Groups.Values
					.Select(g => g.Value).ToList();
				var fromName = pieces[1];
				var fromMail = pieces[2];

				list.Add(new MailAddress(fromMail, fromName));
			}
			else
			{
				list.Add(contact);
			}
		}
	}
}
