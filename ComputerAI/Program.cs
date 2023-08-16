using System.Runtime.InteropServices;
using ComputerAI;

class Program
{
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    static async Task Main()
    {
        Console.WriteLine("ComputerAI started.\n");
        await AI.GreetHuman();

        while (true)
        {
            if (GetAsyncKeyState(Constants.ChatKey) != 0 || Constants.IsTextInteraction)
            {
                if (Constants.IsTextInteraction)
                {
                    Console.Write("You: ");
                }

                var input = Constants.IsVoiceInteraction ? await SpeechService.SpeechToTextAsync() : Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (Constants.IsVoiceInteraction)
                {
                    Console.WriteLine(string.Format("You: {0}\n", input));
                }

                await AI.AnswerHuman(input);
            }

            Thread.Sleep(200);
        }
    }
}