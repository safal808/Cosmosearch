using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SearchEngineApp
{
    public class SearchData
    {
        public string Word { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class SearchEngine
    {
        private List<SearchData> _searchContent;

        public SearchEngine(string filePath)
        {
            _searchContent = new List<SearchData>();
            ParseDataStructure(filePath);
        }

        private void ParseDataStructure(string path)
        {
            string page = null;
            string title = null;

            foreach (var line in File.ReadLines(path))
            {
                SearchData searchData = new SearchData();

                if (line.StartsWith("*PAGE:"))
                {
                    page = line.Substring(6);
                    title = null;
                }
                else
                {
                    if (title == null)
                    {
                        title = line.Trim();
                    }
                }

                if (page != null && title != null)
                {
                    searchData.Url = page;
                    searchData.Title = title;
                    searchData.Word = line.Trim();

                    _searchContent.Add(searchData);
                }
            }
        }

        public List<SearchData> FindWord(string word)
        {
            var results = _searchContent.Where(sd => string.Equals(sd.Word, word, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return results;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }

    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                if (context.Request.Path == "/search")
                {
                    string word = context.Request.Query["word"];
                    string filePath = "./data.txt";
                    var searchEngine = new SearchEngine(filePath);
                    var results = searchEngine.FindWord(word);
                    string json = JsonConvert.SerializeObject(results);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync("index.html");
                }
            });
        }
    }
}
