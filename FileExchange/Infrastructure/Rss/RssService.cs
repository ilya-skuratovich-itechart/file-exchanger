using FileExchange.Core.Services;
using System;
using System.ServiceModel.Syndication;
using FileExchange.Core.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace FileExchange.Infrastructure.Rss
{
    public static class RssService
    {
        public static Rss20FeedFormatter GetRssNews(List<News> news, string newsDetailUrl, string siteUrl)
        {

            SyndicationFeed feed = new SyndicationFeed("News feed", "This is a news feed", new Uri(siteUrl));
            feed.Categories.Add(new SyndicationCategory("News"));
            feed.Description = new TextSyndicationContent("rss description");
            if (news != null && news.Any())
            {
                List<SyndicationItem> items = new List<SyndicationItem>();
                foreach (var newsItem in news)
                {
                    if (news != null)
                    {
                        var item = new SyndicationItem(
                            newsItem.Header,
                            newsItem.Text,
                            new Uri(newsDetailUrl),
                            newsItem.NewsId.ToString(),
                            newsItem.ModifyDate.HasValue ? newsItem.ModifyDate.Value : newsItem.CreateDate);
                        items.Add(item);
                    }
                }
                feed.Items = items;
            }
            return new Rss20FeedFormatter(feed);
        }
    }
}