using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace SendyClient.Net.Tests
{
    public class SendyClientSubscriptionStatusTests
    {
	    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
	    private readonly SendyClient _target;

		public SendyClientSubscriptionStatusTests()
	    {
			var baseUri = new Uri("http://foo");
		    var apiKey = "123ABC";
		    _httpMessageHandlerMock = new MockHttpMessageHandler();

		    var httpClient = _httpMessageHandlerMock.ToHttpClient();
		    httpClient.BaseAddress = baseUri;

		    _target = new SendyClient(baseUri, apiKey, httpClient);
		}

	    [Fact]
	    public async Task SubscriptionStatus_WithSubscribedEmail_ReturnsSubscribed()
	    {
		    //arrange
		    var emailAddress = "foo@poo.nl";
		    var listId = "xyz123";
		    var expectedStatus = "Subscribed";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("email", emailAddress),
			    new KeyValuePair<string, string>("list_id", listId)
		    };

		    _httpMessageHandlerMock.Expect("/api/subscribers/subscription-status.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedStatus);

		    //act
		    var result = await _target.SubscriptionStatus(emailAddress, listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.True(result.IsSuccess);
		    Assert.Equal(expectedStatus, result.Response);
	    }

	    [Fact]
	    public async Task SubscriptionStatus_WithInvalidEmail_ReturnsError()
	    {
		    //arrange
		    var emailAddress = "foo@poo.nl";
		    var listId = "xyz123";
		    var expectedError = "Email does not exist in list";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("email", emailAddress),
			    new KeyValuePair<string, string>("list_id", listId)
		    };

		    _httpMessageHandlerMock.Expect("/api/subscribers/subscription-status.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedError);

		    //act
		    var result = await _target.SubscriptionStatus(emailAddress, listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.False(result.IsSuccess);
		    Assert.Equal(expectedError, result.ErrorMessage);
	    }
	}
}
