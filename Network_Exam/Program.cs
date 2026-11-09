using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network_Exam
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var newsService = new NewsService();
            var storageService = new StorageService();

            while (true)
            {
                Console.ResetColor();
                Console.WriteLine("\n=== NEWS READER ===");
                Console.WriteLine("1. Переглянути топ-новини по категорії");
                Console.WriteLine("2. Пошук новин за ключовим словом");
                Console.WriteLine("3. Переглянути збережені новини");
                Console.WriteLine("0. Вийти");
                Console.Write("Оберіть дію: ");

                string choice = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                if (choice == "1")
                {
                    Console.Write("Введіть категорію (technology, sports, health, science, business): ");
                    string category = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;

                    if (category != "technology" && category != "sports" && category != "health" && category != "science" && category != "business")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Некоректна категорія. Спробуйте ще раз.");
                        continue;
                    }

                    int page = 1;
                    bool exitPaging = false;

                    while (!exitPaging)
                    {
                        var articles = await newsService.GetTopHeadlinesAsync(category, page);

                        if (articles == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Відсутній інтернет або помилка запиту.");
                            break;
                        }

                        if (articles.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Новин не знайдено.");
                            break;
                        }

                        DisplayArticles(articles);

                        Console.WriteLine("\n[N] - завантажити ще, [S <номер>] - зберегти статтю, [Exit] - назад в меню");
                        string action = Console.ReadLine()?.Trim().ToLower();

                        if (action == "n")  page++;
                        else if (action != null && action.StartsWith("s "))
                        {
                            if (int.TryParse(action.Substring(2), out int index) && index >= 1 && index <= articles.Count)
                            {
                                storageService.SaveArticle(articles[index - 1]);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Статтю збережено!");
                            }
                            else
                            {
                                Console.WriteLine("Невірний номер статті.");
                            }
                        }
                        else
                        {
                            exitPaging = true;
                        }
                    }
                }
                else if (choice == "2")
                {
                    Console.Write("Введіть ключове слово для пошуку: ");
                    string query = Console.ReadLine() ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(query))
                    {
                        Console.WriteLine("Пошуковий запит не може бути порожнім.");
                        continue;
                    }

                    int page = 1;
                    bool exitPaging = false;

                    while (!exitPaging)
                    {
                        var articles = await newsService.SearchNewsAsync(query, page);

                        if (articles == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Відсутній інтернет або помилка запиту.");
                            break;
                        }

                        if (articles.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Порожній результат пошуку.");
                            break;
                        }

                        DisplayArticles(articles);

                        Console.WriteLine("\n[N] - завантажити ще, [S <номер>] - зберегти статтю, [Exit] - назад в меню");
                        string action = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;

                        if (action == "n") page++;
                        else if (action != null && action.StartsWith("s "))
                        {
                            if (int.TryParse(action.Substring(2), out int index) && index >= 1 && index <= articles.Count)
                            {
                                storageService.SaveArticle(articles[index - 1]);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Статтю збережено!");
                            }
                            else
                            {
                                Console.WriteLine("Невірний номер статті.");
                            }
                        }
                        else
                        {
                            exitPaging = true;
                        }
                    }
                }
                else if (choice == "3")
                {
                    var saved = storageService.LoadArticles();
                    if (saved.Count == 0)
                        Console.WriteLine("Немає збережених статей.");
                    else
                        DisplayArticles(saved);
                }
                else if (choice == "0") break;
            }
        }

        private static void DisplayArticles(List<Article> articles)
        {
            for (int i = 0; i < articles.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{i + 1}. {articles[i].title}");
                Console.ResetColor();
                Console.WriteLine($"{articles[i].description}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Дата: {articles[i].publishedAt} | URL: {articles[i].url}");
                Console.ResetColor();
                Console.WriteLine(new string('-', 40));
            }
        }
    }
}
