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
            if (GetAsyncKeyState(Constants.ChatKey) != 0)
            {
                var input = await SpeechService.SpeechToTextAsync();

                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                Console.WriteLine(string.Format("You: {0}", input));

                await AI.AnswerHuman(input);
            }

            Thread.Sleep(200);
        }
    }
}