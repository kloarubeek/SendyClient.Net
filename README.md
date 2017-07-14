# SendyClient.Net
A Sendy client to interact in .Net Core applications with the [Sendy API](https://sendy.co)!

[![BCH compliance](https://bettercodehub.com/edge/badge/kloarubeek/SendyClient.Net?branch=master)](https://bettercodehub.com/)
[![NuGet](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/SendyClient.Net/)
# Sendy
It can be used to perform the following [Sendy API actions](https://sendy.co/api):
- Subscribe (including custom fields)
- Unsubscribe
- Delete subscriber
- Subscription status
- Active subscriber count
- Create campaign (and send)
- [Create list](#createList) (**new!**)

It has been built to interact with version v2.1.2.8.

## Available on Nuget

SendyClient.Net is available to download via [NuGet!](https://www.nuget.org/packages/SendyClient.Net/)

## How to use

```c#
var sendyClient = new SendyClient(new Uri("https://mysendy"), "mySendySecret");

var result = await sendyClient.Subscribe("sjaan@banaan.nl", "Sjaan", "myListId");
```

Subscribe with custom fields 'birthday' and 'logintoken'

```c#
var sendyClient = new SendyClient(new Uri("https://mysendy"), "mySendySecret");
var customFields = new Dictionary<string, string> {{"birthday", "12/9/1976"}, {"logintoken", "x4bla9!bg"}};

var result = await sendyClient.Subscribe("sjaan@banaan.nl", "Sjaan", "myListId", customFields);
```

If you would like to use the campaign API, [download it first.](http://forum.sendy.co/discussion/768/added-some-api-functionality/p1)

```c#
var sendyClient = new SendyClient(new Uri("https://mysendy"), "mySendySecret");

var campaign = new Campaign
{
  BrandId = 1,
  FromEmail = "noreply@fromme.com",
  FromName = "Jeroen",
  HtmlText = "<html><body><b>Hi</b></body></html>",
  PlainText = "Hi",
  Querystring = "querystring=sjaak",
  ReplyTo = "sjaan@banaan.nl",
  Subject = "Sent with SendyClient.Net!",
  Title = "Campaign demo",
  ToName = "Jeroen"
};

result = await sendyClient.CreateCampaign(campaign, false, null);
```

## <a name="createList"></a>Create list API

The create list is a new API. Copy the [Sendy directory](Sendy) to your Sendy installation. This will add a new API call to create a list, including custom fields when necessary.

endpoint: /api/lists/create.php

POST data:
- api_key
- brand_id
- list_name - the name of the new list (mandatory).
- custom_fields - a comma separated list of new custom field names. Not allowed are *email* and *name* (similar to the UI validations)
- field_types - possible values: *Text* or *Date*

Return value
The id of the list that is created or an error message if something went wrong.

After this you can simply call:

```c#
var sendyClient = new SendyClient(new Uri("https://mysendy"), "mySendySecret");
var list = new MailingList
{
  BrandId = 1,
  Name = "Foo list"
};

list.CustomFields.Add(new CustomField("custom field 1"));
list.CustomFields.Add(new CustomField("custom field 2", CustomField.DataTypes.Date));

var result = await sendyClient.CreateList(list);
```


## Questions
Feel free to create an issue, or even better: submit a pull request.
