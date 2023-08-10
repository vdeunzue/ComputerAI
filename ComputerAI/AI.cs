namespace ComputerAI
{
    public class AI
    {
        public static async Task GreetHuman()
        {
            var greetings = await OpenAI.GetResponseAsync("You have just booted up.");
            Console.WriteLine(string.Format("Arya: {0}\n", greetings.Replace("\n", string.Empty)));
            await SpeechService.TextToSpeechAsync(greetings);
        }

        public static async Task AnswerHuman(string input)
        {
            var response = await OpenAI.GetResponseAsync(input);
            Console.WriteLine(string.Format("Arya: {0}\n", response.Replace("\n", string.Empty)));
            await SpeechService.TextToSpeechAsync(response);
        }
    }
}
