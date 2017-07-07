# SendyClient.Net
A Sendy client to interact in .Net Core applications with the [Sendy API](https://sendy.co)!

[![BCH compliance](https://bettercodehub.com/edge/badge/kloarubeek/SendyClient.Net?branch=master)](https://bettercodehub.com/)

# Sendy
It can be used to perform the following [Sendy API actions](https://sendy.co/api):
- Subscribe
- Unsubscribe
- Delete subscriber
- Subscription status
- Active subscriber count
- Create campaign (and send)

It has been built to interact with version v2.1.2.8.

## Available on Nuget

SendyClient.Net is available to download via [NuGet!](https://www.nuget.org/packages/SendyClient.Net/)

## How to use

```c#
var sendyClient = new SendyClient.Net.SendyClient(new Uri("https://mysendy"), "mySendySecret");

var result = await sendyClient.Subscribe("sjaan@banaan.nl", "Sjaan", "myListId");
```

If you would like to use the campaign API, [download it first.](http://forum.sendy.co/discussion/768/added-some-api-functionality/p1)

```c#
var sendyClient = new SendyClient.Net.SendyClient(new Uri("https://mysendy"), "mySendySecret");

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

result = await client.CreateCampaign(campaign, false, null);
```

## Questions
Feel free to create an issue, or even better: submit a pull request.
