using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Network_Exam
{
    internal class StorageService
    {
        private readonly string _filePath = "saved_articles.json";

        public List<Article> LoadArticles()
        {
            if (!File.Exists(_filePath))
                return new List<Article>();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Article>>(json) ?? new List<Article>();
            }
            catch
            {
                return new List<Article>();
            }
        }

        public void SaveArticle(Article article)
        {
            var articles = LoadArticles();
            if (!articles.Exists(a => a.url == article.url))
            {
                articles.Add(article);
                string json = JsonSerializer.Serialize(articles, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }   
    }
}
