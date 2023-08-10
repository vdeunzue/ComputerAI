using OpenAI_API;
using OpenAI_API.Chat;

namespace ComputerAI
{
    public static class OpenAI
    {
        private static Conversation conversation;

        public static async Task<string> GetResponseAsync(string inputText)
        {
            var openAI = new OpenAIAPI(Constants.OpenAIApiKey);

            if (conversation == null)
            {
                conversation = openAI.Chat.CreateConversation(new ChatRequest());
                conversation.AppendSystemMessage(Constants.Personality);
            }

            var prompt = $"{inputText}\nAI:";
            conversation.AppendUserInput(prompt);


            var response = await conversation.GetResponseFromChatbotAsync();

            return response;
        }
    }
}
