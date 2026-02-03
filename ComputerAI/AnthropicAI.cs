using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ComputerAI
{
    public class AnthropicAI
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly List<ConversationMessage> conversationHistory = new();
        private readonly ActionExecutor actionExecutor = new();
        private const int MaxMessages = 50;

        public AnthropicAI(string apiKey)
        {
            this.apiKey = apiKey;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        }

        public async Task<string> GetResponseAsync(string userMessage, SystemMetrics? metrics = null)
        {
            try
            {
                // Build context-aware system prompt
                var systemPrompt = BuildSystemPrompt(metrics);

                // Add user message to history
                conversationHistory.Add(new ConversationMessage { Role = "user", Content = userMessage });

                // Prepare request
                var request = new
                {
                    model = "claude-3-haiku-20240307",
                    max_tokens = 1024,
                    system = systemPrompt,
                    messages = conversationHistory.Select(m => new { role = m.Role, content = m.Content }).ToArray()
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://api.anthropic.com/v1/messages", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[Anthropic API Error] {response.StatusCode}: {responseBody}");
                    return "Sorry, I'm having trouble connecting right now.";
                }

                var apiResponse = JsonSerializer.Deserialize<AnthropicResponse>(responseBody);
                var aiResponse = apiResponse?.Content?.FirstOrDefault()?.Text ?? "I'm having trouble responding.";

                // Add assistant response to history
                conversationHistory.Add(new ConversationMessage { Role = "assistant", Content = aiResponse });

                // Trim history if too long
                while (conversationHistory.Count > MaxMessages)
                {
                    conversationHistory.RemoveAt(0);
                }

                return aiResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AnthropicAI] Error: {ex.Message}");
                return "Sorry, I encountered an error. Let me try that again.";
            }
        }

        private string BuildSystemPrompt(SystemMetrics? metrics)
        {
            var prompt = Constants.Personality + "\n\n" + Constants.UserName;

            prompt += "\n\nYou're running inside a Windows computer and can see system stats. Be helpful and conversational!";

            if (metrics != null)
            {
                prompt += $"\n\nCurrent System Status:\n{metrics}";
            }

            return prompt;
        }

        public void ClearHistory()
        {
            conversationHistory.Clear();
        }
    }

    public class ConversationMessage
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
    }

    public class AnthropicResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public List<ContentBlock>? Content { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("stop_reason")]
        public string? StopReason { get; set; }
    }

    public class ContentBlock
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
