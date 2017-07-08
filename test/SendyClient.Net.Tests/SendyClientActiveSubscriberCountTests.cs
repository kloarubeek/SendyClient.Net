using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace SendyClient.Net.Tests
{
    public class SendyClientActiveSubscriberCountTests
	{
	    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
	    private readonly SendyClient _target;

		public SendyClientActiveSubscriberCountTests()
	    {
			var baseUri = new Uri("http://foo");
		    var apiKey = "123ABC";
		    _httpMessageHandlerMock = new MockHttpMessageHandler();

		    var httpClient = _httpMessageHandlerMock.ToHttpClient();
		    httpClient.BaseAddress = baseUri;

		    _target = new SendyClient(baseUri, apiKey, httpClient);
		}

	    [Fact]
	    public async Task ActiveSubscriberCount_WithValidListId_ReturnsNumber()
	    {
		    //arrange
		    var listId = "xyz123";
		    var expectedNumber = "10";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("list_id", listId)
		    };

		    _httpMessageHandlerMock.Expect("/api/subscribers/active-subscriber-count.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedNumber);

		    //act
		    var result = await _target.ActiveSubscriberCount(listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.True(result.IsSuccess);
		    Assert.Equal(expectedNumber, result.Response);
	    }

		[Fact]
	    public async Task ActiveSubscriberCount_WithInvalidListId_ReturnsError()
	    {
		    //arrange
		    var listId = "xyz123";
		    var expectedError = "List does not exist";

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("list_id", listId)
		    };

		    _httpMessageHandlerMock.Expect("/api/subscribers/active-subscriber-count.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedError);

		    //act
		    var result = await _target.ActiveSubscriberCount(listId);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.False(result.IsSuccess);
		    Assert.Equal(expectedError, result.ErrorMessage);
	    }
	}
}
