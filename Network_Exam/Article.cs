using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Network_Exam
{
    internal class Article
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string publishedAt { get; set; }
    }

    internal class NewsApiResponse
    {
        public List<Article> articles { get; set; }
    }
}
