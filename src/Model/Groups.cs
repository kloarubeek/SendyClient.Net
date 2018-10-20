using System.Collections.Generic;

namespace Sendy.Client.Model
{
	public class Groups
	{
		public IEnumerable<string> ListIds { get; set; }
		public IEnumerable<string> SegmentIds { get; set; }
		public IEnumerable<string> OmitListIds { get; set; }
		public IEnumerable<string> OmitSegmentsIds { get; set; }

		public Groups()
		{
		}

		public Groups(IEnumerable<string> listIds)
		{
			ListIds = listIds;
		}
	}
}
