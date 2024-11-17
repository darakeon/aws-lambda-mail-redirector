using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Keon.Eml;

namespace Redirector;

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

	private readonly List<String> to = new List<String>();

	private readonly String[] from;
	private readonly String subject;
	private readonly String messageId;
	private readonly String email;

	internal String Body { get; private set; }

	private Attachment attachment;

	public Email BuildBody(DateTime date, String[] to, params String[] messageIds)
	{
		foreach (var receiver in to)
		{
			var redirect =
				Cfg.Smtp.To.ContainsKey(receiver)
					? Cfg.Smtp.To[receiver]
					: Cfg.Smtp.To["default"];

			if (this.to.Contains(redirect))
			{
				Console.WriteLine($"Receiver [{redirect}] of [{receiver}] already exists");
			}
			else
			{
				Console.WriteLine($"Added [{redirect}] because of [{receiver}]");
				this.to.Add(redirect);
			}
		}

		var toAll = String.Join(",", to);
		var fromAll = String.Join(",", from);
		var messageIdsAll = String.Join(",", messageIds);

		Body = $"From: {fromAll}<br />"
		       + $"To: {toAll}<br />"
		       + $"Date: {date}<br />"
		       + $"Message IDs: {messageIdsAll}<br />"
		       + "<br />"
		       + new EmlReader(email).Body;

		attachment = new Attachment(
			new MemoryStream(
				Encoding.UTF8.GetBytes(email)
			),
			"original.eml"
		);

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
			Attachments = {attachment}
		};

		foreach (var receiver in to)
		{
			message.To.Add(new MailAddress(receiver));
		}

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
