using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace Sendy.Client.Tests
{
    public class SendyClientUnsubscribeTests
	{
	    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
	    private readonly SendyClient _target;

	    public SendyClientUnsubscribeTests()
	    {
		    var baseUri = new Uri("http://foo");
		    var apiKey = "123ABC";
		    _httpMessageHandlerMock = new MockHttpMessageHandler();

		    var httpClient = _httpMessageHandlerMock.ToHttpClient();
		    httpClient.BaseAddress = baseUri;

			_target = new SendyClient(baseUri, apiKey, null, httpClient);
		}

		[Fact]
	    public async Task UnSubscribe_WithValidData_ReturnsOk()
	    {
		    //arrange
		    var emailAddress = "foo@poo.nl";
		    var listId = "xyz123";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("email", emailAddress),
			    new KeyValuePair<string, string>("list", listId)
		    };

		    _httpMessageHandlerMock.Expect("/unsubscribe")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", "1");

		    //act
		    var result = await _target.UnsubscribeAsync(emailAddress, listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.True(result.IsSuccess);
	    }

	    [Fact]
	    public async Task UnSubscribe_WithInvalidEmail_ReturnsError()
	    {
		    //arrange
		    var emailAddress = "foo@poo.nl";
		    var listId = "xyz123";
		    var expectedError = "Invalid email address.";

			var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("email", emailAddress),
			    new KeyValuePair<string, string>("list", listId)
		    };

		    _httpMessageHandlerMock.Expect("/unsubscribe")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedError);

		    //act
		    var result = await _target.UnsubscribeAsync(emailAddress, listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.False(result.IsSuccess);
		    Assert.Equal(expectedError, result.ErrorMessage);
		}
	}
}
