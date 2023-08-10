namespace ComputerAI
{
    public class AI
    {
        public static async Task GreetHuman()
        {
            var greetings = await OpenAI.GetResponseAsync("You have just booted up.");
            Console.WriteLine(string.Format("Computer: {0}", greetings.Completions[0].Text.Replace("\n", string.Empty)));
            await SpeechService.TextToSpeechAsync(greetings.Completions[0].Text);
        }

        public static async Task AnswerHuman(string input)
        {
            var response = await OpenAI.GetResponseAsync(input);
            Console.WriteLine(string.Format("Computer: {0}", response.Completions[0].Text.Replace("\n", string.Empty)));
            await SpeechService.TextToSpeechAsync(response.Completions[0].Text);
        }
    }
}
