using Eneru.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Eneru.Services
{
    public class AssistantService : IAssistantService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;
        private readonly HttpClient _http;

        public AssistantService(IConfiguration config, AppDbContext db)
        {
            _config = config;
            _db = db;
            _http = new HttpClient();
        }

        public async Task<string> AskAsync(string userMessage)
        {
            var apiKey = _config["Groq:ApiKey"];

            // Load available products from database
            var products = await _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsAvailable)
                .Select(p => new {
                    p.Name,
                    p.Brand,
                    p.Price,
                    Category = p.Category!.Name,
                    p.Description
                })
                .ToListAsync();

            var productList = string.Join("\n", products.Select(p =>
                $"- {p.Name} by {p.Brand} | {p.Category} | ${p.Price:0.00} | {p.Description}"));

            // Groq uses OpenAI-compatible API format
            // It has system message + user message separately
            var requestBody = new
            {
                model = "llama-3.3-70b-versatile", // best free model on Groq
                max_tokens = 500,
                temperature = 0.7,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = $"""
                            You are a helpful fashion assistant for Eneru, a luxury fashion store.
                            Help customers find products, answer questions about shipping and returns,
                            and provide style advice.

                            STORE POLICIES:
                            - Free shipping on orders over $150
                            - Standard delivery 3-5 business days
                            - Free returns within 30 days
                            - Secure payment guaranteed

                            AVAILABLE PRODUCTS:
                            {productList}

                            RULES:
                            - Answer in the same language the customer uses
                            - Keep responses concise and helpful
                            - When recommending products always mention the price
                            - Never make up products or prices not listed above
                            - Be warm and professional
                            """
                    },
                    new
                    {
                        role = "user",
                        content = userMessage
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Groq uses OpenAI-compatible endpoint
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await _http.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            // Check for API errors
            if (root.TryGetProperty("error", out var error))
            {
                var msg = error.GetProperty("message").GetString();
                return $"Error: {msg}";
            }

            // Groq returns: choices[0].message.content
            var text = root
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return text ?? "Sorry, I could not generate a response.";
        }
    }
}