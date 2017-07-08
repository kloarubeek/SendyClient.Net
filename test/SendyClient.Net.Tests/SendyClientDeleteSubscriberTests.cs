using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace SendyClient.Net.Tests
{
    public class SendyClientDeleteSubscriberTests
    {
	    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
	    private readonly SendyClient _target;

		public SendyClientDeleteSubscriberTests()
	    {
			var baseUri = new Uri("http://foo");
		    var apiKey = "123ABC";
		    _httpMessageHandlerMock = new MockHttpMessageHandler();

		    var httpClient = _httpMessageHandlerMock.ToHttpClient();
		    httpClient.BaseAddress = baseUri;

		    _target = new SendyClient(baseUri, apiKey, httpClient);
		}

	    [Fact]
	    public async Task DeleteSubscriber_WithValidData_ReturnsOk()
	    {
		    //arrange
		    var emailAddress = "foo@poo.nl";
		    var listId = "xyz123";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("email", emailAddress),
			    new KeyValuePair<string, string>("list_id", listId)
		    };

		    _httpMessageHandlerMock.Expect("/api/subscribers/delete.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", "1");

		    //act
		    var result = await _target.DeleteSubscriber(emailAddress, listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.True(result.IsSuccess);
	    }

	    [Fact]
	    public async Task DeleteSubscriber_WithInvalidEmail_ReturnsError()
	    {
		    //arrange
		    var emailAddress = "foo@poo.nl";
		    var listId = "xyz123";
		    var expectedError = "Subscriber does not exist";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("email", emailAddress),
			    new KeyValuePair<string, string>("list_id", listId)
		    };

		    _httpMessageHandlerMock.Expect("/api/subscribers/delete.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedError);

		    //act
		    var result = await _target.DeleteSubscriber(emailAddress, listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.False(result.IsSuccess);
		    Assert.Equal(expectedError, result.ErrorMessage);
	    }
	}
}
