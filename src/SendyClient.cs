using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SendyClient.Net.Model;
using static SendyClient.Net.SendyResponseHelper;

namespace SendyClient.Net
{
    public class SendyClient
    {
	    private readonly Uri _baseUri;
	    private readonly string _apiKey;

	    public SendyClient(Uri baseUri, string apiKey)
	    {
		    _baseUri = baseUri;
		    _apiKey = apiKey;
	    }

		public async Task<SendyResponse> Subscribe(string emailAddress, string name, string listId)
		{
			using (var httpClient = GetHttpClient())
			{
				var postData = GetPostData();
				postData.Add(new KeyValuePair<string, string>("email", emailAddress));
				postData.Add(new KeyValuePair<string, string>("name", name));
				postData.Add(new KeyValuePair<string, string>("list", listId));

				var subscribeData = new FormUrlEncodedContent(postData);

				var result = await httpClient.PostAsync("subscribe", subscribeData);

				return await HandleResponse(result, SendyActions.Subscribe);
			}
		}

	    public async Task<SendyResponse> Unsubscribe(string emailAddress, string listId)
	    {
		    using (var httpClient = GetHttpClient())
		    {
			    var postData = GetPostData();
			    postData.Add(new KeyValuePair<string, string>("email", emailAddress));
			    postData.Add(new KeyValuePair<string, string>("list", listId));

			    var subscribeData = new FormUrlEncodedContent(postData);

			    var result = await httpClient.PostAsync("unsubscribe", subscribeData);

			    return await HandleResponse(result, SendyActions.Unsubscribe);
		    }
	    }

	    public async Task<SendyResponse> DeleteSubscriber(string emailAddress, string listId)
	    {
		    using (var httpClient = GetHttpClient())
		    {
			    var postData = GetPostData();
			    postData.Add(new KeyValuePair<string, string>("email", emailAddress));
			    postData.Add(new KeyValuePair<string, string>("list_id", listId));

			    var subscribeData = new FormUrlEncodedContent(postData);

			    var result = await httpClient.PostAsync("api/subscribers/delete.php", subscribeData);

			    return await HandleResponse(result, SendyActions.DeleteSubscriber);
		    }
	    }

	    public async Task<SendyResponse> SubscriptionStatus(string emailAddress, string listId)
	    {
		    using (var httpClient = GetHttpClient())
		    {
			    var postData = GetPostData();
			    postData.Add(new KeyValuePair<string, string>("email", emailAddress));
			    postData.Add(new KeyValuePair<string, string>("list_id", listId));

			    var subscribeData = new FormUrlEncodedContent(postData);

			    var result = await httpClient.PostAsync("api/subscribers/subscription-status.php", subscribeData);

			    return await HandleResponse(result, SendyActions.SubscriptionStatus);
		    }
	    }

	    public async Task<SendyResponse> ActiveSubscriberCount(string listId)
	    {
		    using (var httpClient = GetHttpClient())
		    {
			    var postData = GetPostData();
			    postData.Add(new KeyValuePair<string, string>("list_id", listId));

			    var subscribeData = new FormUrlEncodedContent(postData);

			    var result = await httpClient.PostAsync("api/subscribers/active-subscriber-count.php", subscribeData);
				//test return value
			    return await HandleResponse(result, SendyActions.ActiveSubscriberCount);
		    }
	    }

		/// <param name="campaign"></param>
		/// <param name="send">True to send the campaign as well. In that case <paramref name="listIds" /> is also required.</param>
		/// <param name="listIds">Lists to send to campaign to. Only required if <paramref name="send"/> is true.</param>
	    public async Task<SendyResponse> CreateCampaign(Campaign campaign, bool send, List<string> listIds)
		{
			if (send && listIds == null)
				throw new ArgumentNullException(nameof(listIds), "Please provide one or more list ids to send this campaign to.");

		    using (var httpClient = GetHttpClient())
		    {
			    var postData = GetPostData();
			    postData.Add(new KeyValuePair<string, string>("from_name", campaign.FromName));
			    postData.Add(new KeyValuePair<string, string>("from_email", campaign.FromEmail));
			    postData.Add(new KeyValuePair<string, string>("reply_to", campaign.ReplyTo));
			    postData.Add(new KeyValuePair<string, string>("title", campaign.Title));
			    postData.Add(new KeyValuePair<string, string>("subject", campaign.Subject));
			    postData.Add(new KeyValuePair<string, string>("plain_text", campaign.PlainText));
			    postData.Add(new KeyValuePair<string, string>("html_text", campaign.HtmlText));
			    postData.Add(new KeyValuePair<string, string>("brand_id", campaign.BrandId.ToString()));
			    postData.Add(new KeyValuePair<string, string>("query_string", campaign.Querystring));

			    if (send)
			    {
				    postData.Add(new KeyValuePair<string, string>("send_campaign", "1"));
				    postData.Add(new KeyValuePair<string, string>("list_ids", string.Join(",", listIds)));
			    }
				var subscribeData = new FormUrlEncodedContent(postData);

			    var result = await httpClient.PostAsync("api/campaigns/create.php", subscribeData);

			    return await HandleResponse(result, SendyActions.CreateCampaign);
		    }
	    }

		private HttpClient GetHttpClient()
	    {
		    return new HttpClient { BaseAddress = _baseUri };
	    }

	    private List<KeyValuePair<string, string>> GetPostData()
	    {
		    return new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("api_key", _apiKey),
			    new KeyValuePair<string, string>("boolean", "true") //otherwise it could return a whole html page
		    };
	    }
    }
}
