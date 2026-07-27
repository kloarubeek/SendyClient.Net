using System;

namespace Sendy.Client.Model
{
    public class Campaign
    {
	    public string FromName { get; set; }
	    public string FromEmail { get; set; }
	    public string ReplyTo { get; set; }
	    public string Title { get; set; }
	    public string Subject { get; set; }
	    public string PlainText { get; set; }
	    public string HtmlText { get; set; }

		/// <summary>
		/// Required only when creating a 'Draft' campaign (i.e. not sending it straight away).
		/// </summary>
	    public int BrandId { get; set; }
	    public string Querystring { get; set; }

		/// <summary>
		/// Enable/disable/anonymize open tracking for this campaign. Leave null to use the Sendy default.
		/// </summary>
		public TrackingOption? TrackOpens { get; set; }

		/// <summary>
		/// Enable/disable/anonymize click tracking for this campaign. Leave null to use the Sendy default.
		/// </summary>
		public TrackingOption? TrackClicks { get; set; }

		/// <summary>
		/// The date and time to schedule this campaign for. Only the minutes, in 5-minute increments, are honoured by Sendy.
		/// Requires <see cref="ScheduleTimezone"/> to also be set, and the campaign to be sent (send_campaign = 1) with one or more lists/segments.
		/// </summary>
		public DateTime? ScheduleDateTime { get; set; }

		/// <summary>
		/// The PHP timezone (e.g. 'America/New_York') that <see cref="ScheduleDateTime"/> should be interpreted in.
		/// </summary>
		public string ScheduleTimezone { get; set; }
    }
}
