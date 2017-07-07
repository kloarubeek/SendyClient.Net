namespace SendyClient.Net.Model
{
    public class Campaign
    {
	    public string FromName { get; set; }
	    public string ToName { get; set; }
	    public string FromEmail { get; set; }
	    public string ReplyTo { get; set; }
	    public string Title { get; set; }
	    public string Subject { get; set; }
	    public string PlainText { get; set; }
	    public string HtmlText { get; set; }
	    public int BrandId { get; set; }
	    public string Querystring { get; set; }
    }
}
