using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Sendy.Client.Model;
using Xunit;

namespace Sendy.Client.Tests
{
    public class SendyClientCreateCampaignTests
	{
	    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
	    private readonly SendyClient _target;

		public SendyClientCreateCampaignTests()
	    {
			var baseUri = new Uri("http://foo");
		    var apiKey = "123ABC";
		    _httpMessageHandlerMock = new MockHttpMessageHandler();

		    var httpClient = _httpMessageHandlerMock.ToHttpClient();
		    httpClient.BaseAddress = baseUri;

		    _target = new SendyClient(baseUri, apiKey, httpClient);
		}

	    [Fact]
	    public async Task CreateCampaign_WithValidData_NoSend_ReturnsCampaignCreated()
	    {
		    //arrange
		    var expectedResponse = "Campaign created";
			var campaign = new Campaign
			{
				BrandId = 1,
				FromEmail = "jeroen@klarenbeek.nl",
				FromName = "Jeroen",
				HtmlText = "<html><body><b>Hi</b></body></html>",
				PlainText = "Hi",
				Querystring = "querystring=sjaak",
				ReplyTo = "hank@klarenbeek.nl",
				Subject = "Subjectje",
				Title = "Title 1"
			};

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("from_name", campaign.FromName),
			    new KeyValuePair<string, string>("from_email", campaign.FromEmail),
			    new KeyValuePair<string, string>("reply_to", campaign.ReplyTo),
			    new KeyValuePair<string, string>("title", campaign.Title),
			    new KeyValuePair<string, string>("subject", campaign.Subject),
			    new KeyValuePair<string, string>("plain_text", campaign.PlainText),
			    new KeyValuePair<string, string>("html_text", campaign.HtmlText),
			    new KeyValuePair<string, string>("brand_id", campaign.BrandId.ToString()),
			    new KeyValuePair<string, string>("query_string", campaign.Querystring)
		    };

		    _httpMessageHandlerMock.Expect("/api/campaigns/create.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedResponse);

		    //act
		    var result = await _target.CreateCampaign(campaign, false, null);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.True(result.IsSuccess);
		    Assert.Equal(expectedResponse, result.Response);
	    }

		[Fact]
		public async Task CreateCampaign_WithValidData_AndSend_ReturnsCampaignCreatedAndSending()
		{
			//arrange
			var expectedResponse = "Campaign created and now sending";
			var listIds = new List<string> {"listId"};
			var campaign = new Campaign
			{
				BrandId = 1,
				FromEmail = "jeroen@klarenbeek.nl",
				FromName = "Jeroen",
				HtmlText = "<html><body><b>Hi</b></body></html>",
				PlainText = "Hi",
				Querystring = "querystring=sjaak",
				ReplyTo = "hank@klarenbeek.nl",
				Subject = "Subjectje",
				Title = "Title 1"
			};

			var expectedPostData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("from_name", campaign.FromName),
				new KeyValuePair<string, string>("from_email", campaign.FromEmail),
				new KeyValuePair<string, string>("reply_to", campaign.ReplyTo),
				new KeyValuePair<string, string>("title", campaign.Title),
				new KeyValuePair<string, string>("subject", campaign.Subject),
				new KeyValuePair<string, string>("plain_text", campaign.PlainText),
				new KeyValuePair<string, string>("html_text", campaign.HtmlText),
				new KeyValuePair<string, string>("brand_id", campaign.BrandId.ToString()),
				new KeyValuePair<string, string>("query_string", campaign.Querystring),
				new KeyValuePair<string, string>("send_campaign", "1"),
				new KeyValuePair<string, string>("list_ids", string.Join(",", listIds))

		};

			_httpMessageHandlerMock.Expect("/api/campaigns/create.php")
				.WithFormData(expectedPostData)
				.Respond("text/plain", expectedResponse);

			//act
			var result = await _target.CreateCampaign(campaign, true, listIds);

			//assert
			_httpMessageHandlerMock.VerifyNoOutstandingExpectation();
			Assert.True(result.IsSuccess);
			Assert.Equal(expectedResponse, result.Response);
		}


		[Fact]
		public async Task CreateCampaign_WithInvalidData_ReturnsError()
		{
			//arrange
			var expectedResponse = "From name not passed";
			var campaign = new Campaign
			{
				BrandId = 1,
				FromEmail = "jeroen@klarenbeek.nl",
				HtmlText = "<html><body><b>Hi</b></body></html>",
				PlainText = "Hi",
				Querystring = "querystring=sjaak",
				ReplyTo = "hank@klarenbeek.nl",
				Subject = "Subjectje",
				Title = "Title 1"
			};

			var expectedPostData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("from_email", campaign.FromEmail),
				new KeyValuePair<string, string>("reply_to", campaign.ReplyTo),
				new KeyValuePair<string, string>("title", campaign.Title),
				new KeyValuePair<string, string>("subject", campaign.Subject),
				new KeyValuePair<string, string>("plain_text", campaign.PlainText),
				new KeyValuePair<string, string>("html_text", campaign.HtmlText),
				new KeyValuePair<string, string>("brand_id", campaign.BrandId.ToString()),
				new KeyValuePair<string, string>("query_string", campaign.Querystring)
			};

			_httpMessageHandlerMock.Expect("/api/campaigns/create.php")
				.WithFormData(expectedPostData)
				.Respond("text/plain", expectedResponse);

			//act
			var result = await _target.CreateCampaign(campaign, true, new List<string> { "listId" });

			//assert
			_httpMessageHandlerMock.VerifyNoOutstandingExpectation();
			Assert.False(result.IsSuccess);
			Assert.Equal(expectedResponse, result.ErrorMessage);
		}
	}
}
