Building Couchbase Url Shortener Service 
Adventures with Visual Studio 2013, C#, MVC 5 and WebApi 2

The following post is the first step in a series of tutorials that will walk you through a real life implementation of a great Url Shortener web application built with a fantastic NoSql database as the main data store.
There is a lot of interest in NoSQL databases today.

Only fraction of it is justified in my opinion but my opinion was formed by the years of software development experience and I am always ready to prove myself wrong. The best way for me to learn something new is to take on a limited scope project and attempt to implement it using the latest collection of the buzz words like Cloud Services, NoSQL, Big Data and Real-Time analytics. This way I can be comfortable knowing that my expanded skillset will be up to date and in demand for at least some time regardless of the latest headlines.

I have had a good experience with MongoDB and recently have tried working with Couchbase. Both have their own pros and cons and they are fairly well established out there. MongoDB has a lot more going for it in terms of samples and community contributions and this is exactly the reason why I have chosen to go with Couchbase. If I could contribute to something, it might as well be the underdog and I’m not talking about performance here. I was very impressed by how easy it was to get started with Couchbase and was able to setup a simple cluster in almost no time.

    Here is the quote from Couchbase website:
    Thanks to a flexible JSON model, Couchbase Server makes it easy to modify your applications without the constraints of a fixed database schema. Submillisecond, high-throughput reads and writes give you consistent high performance. Couchbase Server is easy to scale out, and supports topology changes with no downtime.

The idea is simple. Build and host the world class Url shortener service that combines most useful features from the top services such as Bit.ly, Goo.gl, is.gd and others. I would like to build it as a commercial grade product but host the source code on GitHub and open source it to the world. I would love to use it in real life as a platform for my other ideas. I’m sure I’m not alone.

Why another URL shortener application?
Because fast accountable http redirection solution (using http response code 301 or 302) is one of the essential services that powers countless online businesses. Think about all advertising networks and imagine number of impressions and clicks processed daily on the internet. They use complex systems for ad targeting but at the end almost all of them simply redirect user to their final destination. This is the important part. This is what advertisers pay for.

I’m going to attempt to implement only the redirection part. No targeting. I’m going to develop and host fast and easy to use Url redirection for the masses. Users should be able to store their links, bookmarks, newsfeeds, or anything that can be validated as properly formatted URLs. It’s up to users to decide what they would like to use these links for. I’m simply looking at it as a technical challenge.

What about our domain name? Shouldn’t it be short?
Yes. The shortest domain I was able to acquire was I.AF. .af is the Internet country code top-level domain (ccTLD) for Afghanistan. It is administered by AFGNIC, a service of the UNDP and the Islamic Republic of Afghanistan. I know, pretty crazy right? I will use i.af to host this service in real life and in fact it’s already live.

Go ahead, check it out. I’ll wait.

Are you back? Great. As you can see we are already ahead of competition.

i.af is a 3 letter domain vs. 5 letters in bit.ly and goo.gl. Not a bad start.

What about the cost?
Let’s examine business aspect of our project and figure out what do we need to make sure our project is sustainable in the long term. I hate to see open source projects that do not power anything in real life. To power a live website we need just that, electrical power and some servers.

Minimum recommended configuration for the Couchbase cluster in production is 3 servers. We are going to need at least 1 server for our development environment as well. For our .NET 4.5 MVC powered website I’m going to need 4 servers with IIS7 plus 2 more servers to power SQL server installation for our user authentication and login management. The main reason I must use SQL server at this stage is simply because I would like to use built in authentication providers that Microsoft has shipped with Visual Studio 2013 and MVC 5. This will help me to push website into production much sooner. Later on I will port authentication providers to Couchbase. It’s not an easy task to do it properly and it might take some time. We want to get to a proof of concept first.

So let’s summarize what is my initial startup cost.
3 Couchbase server (with 144 GB of RAM) + 1 Couchbase development (with 16 Gb of RAM) + 4 Cheap Front End Servers (8 GB of RAM each) + 32 GB of RAM SQL Server.

The good news is I already have these servers in my datacenter. All are used Dell servers bought from a reseller. All servers are certified and I never had any problems with them I can’t handle. Swap something and move on. Typical machine will cost around $2000 so we are looking at $2000 * 10 = $20,000 USD in hardware cost.

Just to put this into a perspective let’s say we will have to rely on advertising revenue to cover the startup cost. At a rate of $3 per 1000 impressions we will have to generate 20,000 / 3 *1000 = 6,666,666 ad impressions to cover the cost. Oh, oh. 666 is not a round number and I’m writing this around Halloween. It gives me chills in all the wrong places. Let’s say we need almost 7 million page views. That’s a lot.

OK. I won’t expect to break even in a year. So it gives us some room to breathe.

7,000,000 impressions / 12 month = 583,333 page views per month. Better.

Speaking of room. Colocation & bandwidth cost money too. Currently I’m paying $800 per month for half of 42U rack space in the datacenter. Bandwidth is included.

That means we need another:
$800 /3 * 1000 = 266,666 impressions.

We need roughly 1,000,000 impressions per month to cover hosting. Let’s say each user generates 25 page impressions per month.

1000,000 / 25 = 40,000 active monthly users.

Not an easy task at first but totally achievable at the end of the first year.
Remember: I am contributing my time and energy for free but I am planning to account for my time during entire development process. I must be able to estimate potential development cost (in time and money) of any additional features if such features are needed in the future. I’m thinking about it from a business perspective.

Now that this is out of the way we have a rough idea of what business targets we have to hit. Our system will have to be able to sustain about 10,000,000 http requests per month. Remember that bots and referral spam will add to our total hit count. Not a problem at all but this is just to keep development project afloat.

What we have here is:

10,000,000 requests / 2,628,000 seconds = 3.8 requests per seconds.

I can clearly see that I don’t really need a web scale solution here.

What about time?
I have a pretty good idea how long it would take me if I was using traditional Microsoft tools and technologies such as SQL Server and Entity Framework but I figured that I can get quickly up to speed with Couchbase. They provide excellent documentation on their site and they already have a C# client available as NuGet package to get started. This will help me a lot.

I’m not planning to reinvent the wheel here and I will use traditional 3 tier architecture with default MVC 5 template project that comes with new Visual Studio 2013. I’m looking at 2 essential entities (URL Entity and User Entity) that will be part of the main data layer. I’m also thinking about 3 basic CRUD services (URLService, UserService, LogService).

I’m not going to implement any Dependency Injection Containers at first, I’m not going to provide extended Unit testing project that will provide 100% code coverage but I will structure my code for potential refactoring in the future.

I’m looking at 3 weeks of part time development. I anticipate most of the problems will manifest from NOT KNOWING best practices when it comes to Couchbase. This is the biggest problem. There are not that many samples out there that can provide performance testing analyses as well as reliability analyses on a production systems. Well, I guess this is why I’m building this service it in the first place. I’m sure there will be plenty of surprises.

Let the project begin
