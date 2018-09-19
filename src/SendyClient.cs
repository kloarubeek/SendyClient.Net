using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Sendy.Client.Model;

[assembly: InternalsVisibleTo("Sendy.Client.Tests")]

namespace Sendy.Client
{
	public class SendyClient : IDisposable
	{
		private readonly string _apiKey;
        private readonly Version _apiVer;
        private readonly HttpClient _httpClient;

		public SendyClient(Uri baseUri, string apiKey, Version apiVer = null) : this(baseUri, apiKey, apiVer, null)
		{
		}

		/// <summary>
		/// This one should only be used for unit tests to support injecting of the httpClient
		/// </summary>
		internal SendyClient(Uri baseUri, string apiKey, Version apiVer = null, HttpClient httpClient = null)
		{
			_apiKey = apiKey;
            _apiVer = apiVer ?? new Version(2, 1);
			_httpClient = httpClient ?? new HttpClient();
			_httpClient.BaseAddress = baseUri;
		}

		public Task<SendyResponse> SubscribeAsync(string emailAddress, string name, string listId)
		{
			return SubscribeAsync(emailAddress, name, listId, null);
		}

		/// <param name="customFields">For custom fields, use Sendy fieldname as key value.</param>
		public async Task<SendyResponse> SubscribeAsync(string emailAddress, string name, string listId, Dictionary<string, string> customFields, string country = null, string ipaddress = null, string referrer = null, bool gdpr = false)
		{
			var postData = GetPostData();
			postData.Add(new KeyValuePair<string, string>("email", emailAddress));
			postData.Add(new KeyValuePair<string, string>("name", name));
			postData.Add(new KeyValuePair<string, string>("list", listId));

            if (_apiVer >= new Version(3, 0, 5))
            {
                if (country != null)
                    postData.Add(new KeyValuePair<string, string>("country", country));
                if (ipaddress != null)
                    postData.Add(new KeyValuePair<string, string>("ipaddress", ipaddress));
                if (referrer != null)
                    postData.Add(new KeyValuePair<string, string>("referrer", referrer));
                if (gdpr)
                    postData.Add(new KeyValuePair<string, string>("gdpr", "1"));
            }

            AppendCustomFields(postData, customFields);
			var subscribeData = new FormUrlEncodedContent(postData);

			var result = await _httpClient.PostAsync("subscribe", subscribeData);

			return await SendyResponseHelper.HandleResponse(result, SendyResponseHelper.SendyActions.Subscribe);
		}

		public async Task<SendyResponse> UnsubscribeAsync(string emailAddress, string listId)
		{
			var postData = GetPostData();
			postData.Add(new KeyValuePair<string, string>("email", emailAddress));
			postData.Add(new KeyValuePair<string, string>("list", listId));

			var subscribeData = new FormUrlEncodedContent(postData);

			var result = await _httpClient.PostAsync("unsubscribe", subscribeData);

			return await SendyResponseHelper.HandleResponse(result, SendyResponseHelper.SendyActions.Unsubscribe);
		}

		public async Task<SendyResponse> DeleteSubscriberAsync(string emailAddress, string listId)
		{
			var postData = GetPostData();
			postData.Add(new KeyValuePair<string, string>("email", emailAddress));
			postData.Add(new KeyValuePair<string, string>("list_id", listId));

			var subscribeData = new FormUrlEncodedContent(postData);

			var result = await _httpClient.PostAsync("api/subscribers/delete.php", subscribeData);

			return await SendyResponseHelper.HandleResponse(result, SendyResponseHelper.SendyActions.DeleteSubscriber);
		}

		public async Task<SendyResponse> SubscriptionStatusAsync(string emailAddress, string listId)
		{
			var postData = GetPostData();
			postData.Add(new KeyValuePair<string, string>("email", emailAddress));
			postData.Add(new KeyValuePair<string, string>("list_id", listId));

			var subscribeData = new FormUrlEncodedContent(postData);

			var result = await _httpClient.PostAsync("api/subscribers/subscription-status.php", subscribeData);

			return await SendyResponseHelper.HandleResponse(result, SendyResponseHelper.SendyActions.SubscriptionStatus);
		}

		public async Task<SendyResponse> ActiveSubscriberCountAsync(string listId)
		{
			var postData = GetPostData();
			postData.Add(new KeyValuePair<string, string>("list_id", listId));

			var subscribeData = new FormUrlEncodedContent(postData);

			var result = await _httpClient.PostAsync("api/subscribers/active-subscriber-count.php", subscribeData);
			//test return value
			return await SendyResponseHelper.HandleResponse(result, SendyResponseHelper.SendyActions.ActiveSubscriberCount);
		}

		/// <param name="campaign"></param>
		/// <param name="send">True to send the campaign as well. In that case <paramref name="listIds" /> is also required.</param>
		/// <param name="listIds">Lists to send to campaign to. Only required if <paramref name="send"/> is true.</param>
		public async Task<SendyResponse> CreateCampaignAsync(Campaign campaign, bool send, IEnumerable<string> listIds = null, IEnumerable<string> segmentIds = null, IEnumerable<string> omitListIds = null, IEnumerable<string> omitSegmentsIds = null)
        {
			if (send && listIds == null && segmentIds == null)
				throw new ArgumentNullException(nameof(listIds), "Please provide one or more list ids to send this campaign to.");

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
                if (listIds != null)
                    postData.Add(new KeyValuePair<string, string>("list_ids", string.Join(",", listIds)));

                if (_apiVer >= new Version(3, 0, 6))
                {
                    if (segmentIds != null)
                        postData.Add(new KeyValuePair<string, string>("segment_ids", string.Join(",", segmentIds)));
                    if (omitListIds != null)
                        postData.Add(new KeyValuePair<string, string>("exclude_list_ids", string.Join(",", omitListIds)));
                    if (omitSegmentsIds != null)
                        postData.Add(new KeyValuePair<string, string>("exclude_segments_ids", string.Join(",", omitSegmentsIds)));
                }
            }
            var subscribeData = new FormUrlEncodedContent(postData);

			var result = await _httpClient.PostAsync("api/campaigns/create.php", subscribeData);

			return await SendyResponseHelper.HandleResponse(result, SendyResponseHelper.SendyActions.CreateCampaign);
		}

		/// <summary>
		/// Creates a new mailing list.
		/// </summary>
		/// <param name="mailingList"></param>
		/// <returns>The id of the list or the error message.</returns>
		public async Task<SendyResponse> CreateListAsync(MailingList mailingList)
		{
			var postData = GetPostData();
			postData.Add(new KeyValuePair<string, string>("list_name", mailingList.Name));
			postData.Add(new KeyValuePair<string, string>("brand_id", mailingList.BrandId.ToString()));

			if(mailingList.CustomFields.Any())
			{
				postData.Add(new KeyValuePair<string, string>("custom_fields", string.Join(",", mailingList.CustomFields.Select(c => c.Name))));
				postData.Add(new KeyValuePair<string, string>("field_types", string.Join(",", mailingList.CustomFields.Select(c => c.DataType.ToString()))));
			}
			var subscribeData = new FormUrlEncodedContent(postData);

			var result = await _httpClient.PostAsync("api/lists/create.php", subscribeData);

			return await SendyResponseHelper.HandleResponse(result, SendyResponseHelper.SendyActions.CreateList);
		}

		private List<KeyValuePair<string, string>> GetPostData()
		{
			return new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("api_key", _apiKey),
				new KeyValuePair<string, string>("boolean", "true") //otherwise it could return a whole html page
			};
		}

		private static void AppendCustomFields(List<KeyValuePair<string, string>> postData, Dictionary<string, string> customFields)
		{
			if (customFields != null && customFields.Any())
			{
				foreach (var customField in customFields)
				{
					postData.Add(new KeyValuePair<string, string>(customField.Key, customField.Value));
				}
			}
		}

		public void Dispose()
		{
			_httpClient?.Dispose();
		}
	}
}
