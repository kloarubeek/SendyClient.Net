using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Sendy.Client.Model;
using Xunit;

namespace Sendy.Client.Tests
{
    public class SendyClientCreateListTests
	{
	    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
	    private readonly SendyClient _target;

		public SendyClientCreateListTests()
	    {
			var baseUri = new Uri("http://foo");
		    var apiKey = "123ABC";
		    _httpMessageHandlerMock = new MockHttpMessageHandler();

		    var httpClient = _httpMessageHandlerMock.ToHttpClient();
		    httpClient.BaseAddress = baseUri;

		    _target = new SendyClient(baseUri, apiKey, null, httpClient);
		}

	    [Fact]
	    public async Task CreateList_WithValidData_ReturnsListId()
	    {
		    //arrange
		    var expectedResponse = "1";
			var list = new MailingList
			{
				BrandId = 1,
				Name = "Foo list"
			};

		    var expectedPostData = new List<KeyValuePair<string, string>>
		    {
			    new KeyValuePair<string, string>("list_name", list.Name),
			    new KeyValuePair<string, string>("brand_id", list.BrandId.ToString())
		    };

		    _httpMessageHandlerMock.Expect("/api/lists/create.php")
			    .WithFormData(expectedPostData)
			    .Respond("text/plain", expectedResponse);

		    //act
		    var result = await _target.CreateListAsync(list);

			//assert
		    _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
		    Assert.True(result.IsSuccess);
		    Assert.Equal(expectedResponse, result.Response);
	    }

		[Fact]
		public async Task CreateList_WithValidCustomFields_ReturnsListId()
		{
			//arrange
			var expectedResponse = "1";
			var list = new MailingList
			{
				BrandId = 1,
				Name = "Foo list"
			};

			list.CustomFields.Add(new CustomField("custom field 1"));
			list.CustomFields.Add(new CustomField("custom field 2", CustomField.DataTypes.Date));

			var expectedPostData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("list_name", list.Name),
				new KeyValuePair<string, string>("brand_id", list.BrandId.ToString()),
				new KeyValuePair<string, string>("custom_fields", "custom field 1,custom field 2"),
				new KeyValuePair<string, string>("field_types", "Text,Date")
			};

			_httpMessageHandlerMock.Expect("/api/lists/create.php")
				.WithFormData(expectedPostData)
				.Respond("text/plain", expectedResponse);

			//act
			var result = await _target.CreateListAsync(list);

			//assert
			_httpMessageHandlerMock.VerifyNoOutstandingExpectation();
			Assert.True(result.IsSuccess);
			Assert.Equal(expectedResponse, result.Response);
		}

		[Fact]
		public async Task CreateList_WithInValidData_ReturnsError()
		{
			//arrange
			var expectedResponse = "List name is required";
			var list = new MailingList
			{
				BrandId = 1,
				Name = string.Empty
			};

			var expectedPostData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("list_name", list.Name),
				new KeyValuePair<string, string>("brand_id", list.BrandId.ToString()),
			};

			_httpMessageHandlerMock.Expect("/api/lists/create.php")
				.WithFormData(expectedPostData)
				.Respond("text/plain", expectedResponse);

			//act
			var result = await _target.CreateListAsync(list);

			//assert
			_httpMessageHandlerMock.VerifyNoOutstandingExpectation();
			Assert.False(result.IsSuccess);
			Assert.Equal(expectedResponse, result.ErrorMessage);
		}
	}
}
