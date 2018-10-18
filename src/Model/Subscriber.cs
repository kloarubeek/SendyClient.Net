namespace Sendy.Client.Model
{
	public class Subscriber
	{
		public string EmailAddress { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
		public string IPAddress { get; set; }
		public string Referrer { get; set; }
		public bool GDPR { get; set; }

		public Subscriber()
		{
		}

		public Subscriber(string emailAddress, string name)
		{
			EmailAddress = emailAddress;
			Name = name;
		}
	}
}
