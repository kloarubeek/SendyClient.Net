using System.Collections.Generic;

namespace Sendy.Client.Model
{
    public class MailingList
    {
	    public string Name { get; set; }
	    public int BrandId { get; set; }
	    public List<CustomField> CustomFields { get; }

		public MailingList()
	    {
		    CustomFields = new List<CustomField>();
	    }
    }
}
