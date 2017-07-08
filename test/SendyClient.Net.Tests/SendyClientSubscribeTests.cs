using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace Sendy.Client.Tests
{
    public class SendyClientSubscribeTests
    {
	    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
	    private readonly SendyClient _target;

	    public SendyClientSubscribeTests()
	    {
		    var baseUri = new Uri("http://foo");
		    var apiKey = "123ABC";
		    _httpMessageHandlerMock = new MockHttpMessageHandler();

		    var httpClient = _httpMessageHandlerMock.ToHttpClient();
		    httpClient.BaseAddress = baseUri;

			_target = new SendyClient(baseUri, apiKey, httpClient);
		}

		[Fact]
        public async Task Subscribe_WithValidData_ReturnsOk()
        {
			//arrange
	        var emailAddress = "foo@poo.nl";
	        var name = "Jeroen";
	        var listId = "xyz123";

	        var expectedPostData = new List<KeyValuePair<string, string>>
	        {
		        new KeyValuePair<string, string>("email", emailAddress),
		        new KeyValuePair<string, string>("name", name),
		        new KeyValuePair<string, string>("list", listId)
	        };

	        _httpMessageHandlerMock.Expect("/subscribe")
				.WithFormData(expectedPostData)
		        .Respond("text/plain", "1");

			//act
	        var result = await _target.Subscribe(emailAddress, name, listId);

			//assert
	        _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
			Assert.True(result.IsSuccess);
        }

		[Fact]
	    public async Task Subscribe_WithInValidList_ReturnsError()
	    {
		    //arrange
		    var emailAddress = "foo@poo.nl";
		    var name = "Jeroen";
		    var listId = "invalidId";
		    var expectedError = "Invalid list ID.";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("email", emailAddress),
			    new KeyValuePair<string, string>("name", name),
			    new KeyValuePair<string, string>("list", listId)
		    };

		    _httpMessageHandlerMock.Expect("/subscribe")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedError);

		    //act
		    var result = await _target.Subscribe(emailAddress, name, listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.False(result.IsSuccess);
			Assert.Equal("Invalid list ID.", result.ErrorMessage);
	    }
	}
}
