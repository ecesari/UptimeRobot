namespace Uptime_Robot.Models
{
	public class MailTemplate
	{
		public int Id { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public MailType MailType { get; set; }
	}

	public enum MailType
	{
		MonitorDown = 1,
		ConfirmEmailAddress = 2
	}
}
