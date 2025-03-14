using System.ServiceModel.Syndication;
using System.Xml;

namespace Gitec.Services;

public class RssFeedService
{
    private readonly HttpClient _httpClient;

    public RssFeedService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SyndicationItem>> GetFeedItemsAsync(string feedUrl, int count = 0)
    {
        try
        {
            using var response = await _httpClient.GetAsync(feedUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch RSS feed: {response.StatusCode}");
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var xmlReader = XmlReader.Create(stream);
            var feed = SyndicationFeed.Load(xmlReader);

            if (count == 0)
            {
                return feed.Items.ToList();
            } else
            {
                return feed.Items.Take(count).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching RSS feed: {ex.Message}");
            return new List<SyndicationItem>();
        }
    }
}