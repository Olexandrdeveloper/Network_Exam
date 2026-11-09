using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Network_Exam
{
    internal class NewsService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey = "88d28acb43ec4074a78944d7cddc21f2";

        public NewsService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("NewsReaderApp/1.0");
        }

        public async Task<List<Article>?> GetTopHeadlinesAsync(string category, int page = 1)
        {
            try
            {
                string url = $"https://newsapi.org/v2/top-headlines?country=us&category={category}&page={page}&apiKey={_apiKey}";
                var response = await _client.GetFromJsonAsync<NewsApiResponse>(url);
                return response?.articles ?? new List<Article>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Article>?> SearchNewsAsync(string query, int page = 1)
        {
            try
            {
                string fromDate = DateTime.UtcNow.AddDays(-7).ToString("yyyy-MM-dd");
                string url = $"https://newsapi.org/v2/everything?q={Uri.EscapeDataString(query)}&from={fromDate}&sortBy=publishedAt&page={page}&apiKey={_apiKey}";
                var response = await _client.GetFromJsonAsync<NewsApiResponse>(url);
                return response?.articles ?? new List<Article>();
            }
            catch
            {
                return null;
            }
        }
    }
}
