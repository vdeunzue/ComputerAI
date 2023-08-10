using OpenAI_API.Completions;
using OpenAI_API;

namespace ComputerAI
{
    public static class OpenAI
    {
        public static async Task<CompletionResult> GetResponseAsync(string inputText)
        {
            inputText = Constants.Personality + inputText;
            var openAI = new OpenAIAPI(Constants.OpenAIApiKey);
            var prompt = $"{inputText}\nAI:";
            var response = await openAI.Completions.CreateCompletionAsync(prompt, model: new OpenAI_API.Models.Model("text-davinci-003"), max_tokens: Constants.MaxTokens);
            return response;
        }
    }
}
