using System.Runtime.InteropServices;
using ComputerAI;

class Program
{
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    private static int proactiveCheckCounter = 0;
    private static readonly int proactiveCheckInterval = 300; // Check every 60 seconds (300 * 200ms)

    static async Task Main()
    {
        Console.WriteLine("╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║        ComputerAI v2.0 - Enhanced Edition     ║");
        Console.WriteLine("║      Your AI companion with superpowers!      ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

        // Initialize all systems
        AI.Initialize();

        Console.WriteLine("\n✓ All systems initialized!\n");

        // Greet the user
        await AI.GreetHuman();

        // Show help
        if (Constants.IsTextInteraction)
        {
            Console.WriteLine("─────────────────────────────────────────────────");
            Console.WriteLine("Type your message and press Enter to chat.");
            Console.WriteLine("Available commands:");
            Console.WriteLine("  /status  - Show current system status");
            Console.WriteLine("  /clear   - Clear conversation history");
            Console.WriteLine("  /exit    - Exit the application");
            Console.WriteLine("─────────────────────────────────────────────────\n");
        }
        else
        {
            Console.WriteLine("Press and hold the middle mouse button to speak.\n");
        }

        // Main event loop
        while (true)
        {
            try
            {
                // Check for input
                if (GetAsyncKeyState(Constants.ChatKey) != 0 || Constants.IsTextInteraction)
                {
                    if (Constants.IsTextInteraction)
                    {
                        Console.Write("You: ");
                    }

                    var input = Constants.IsVoiceInteraction
                        ? await SpeechService.SpeechToTextAsync()
                        : Console.ReadLine();

                    if (Constants.IsTextInteraction)
                    {
                        Console.WriteLine();
                    }

                    if (string.IsNullOrEmpty(input))
                    {
                        continue;
                    }

                    // Handle commands
                    if (input.StartsWith("/"))
                    {
                        await HandleCommand(input.ToLower().Trim());
                        continue;
                    }

                    if (Constants.IsVoiceInteraction)
                    {
                        Console.WriteLine($"You: {input}\n");
                    }

                    await AI.AnswerHuman(input);
                }

                // Proactive system check
                proactiveCheckCounter++;
                if (proactiveCheckCounter >= proactiveCheckInterval)
                {
                    proactiveCheckCounter = 0;
                    await AI.ProactiveCheck();
                }

                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                Console.WriteLine("Continuing...\n");
            }
        }
    }

    static async Task HandleCommand(string command)
    {
        switch (command)
        {
            case "/status":
                var monitor = new SystemMonitor();
                var metrics = monitor.GetCurrentMetrics();
                Console.WriteLine("\n╔════════ SYSTEM STATUS ════════╗");
                Console.WriteLine($"  CPU: {metrics.CpuUsage:F1}%");
                Console.WriteLine($"  RAM: {metrics.RamUsagePercent:F1}% ({metrics.RamUsedMB:N0} / {metrics.RamTotalMB:N0} MB)");
                Console.WriteLine($"  Processes: {metrics.ProcessCount}");
                Console.WriteLine("  Disk Usage:");
                foreach (var disk in metrics.DiskUsage)
                {
                    Console.WriteLine($"    {disk.Key} {disk.Value:F1}%");
                }
                Console.WriteLine("  Top Processes:");
                foreach (var proc in metrics.TopProcesses)
                {
                    Console.WriteLine($"    {proc.Name}: {proc.MemoryMB} MB");
                }
                Console.WriteLine("╚═══════════════════════════════╝\n");
                monitor.Dispose();
                break;

            case "/clear":
                Console.WriteLine("\n[System] Conversation history cleared.\n");
                // Note: You'd need to add a ClearHistory method to the AI classes
                break;

            case "/exit":
                Console.WriteLine("\nShutting down ComputerAI...");
                AI.Cleanup();
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine($"\nUnknown command: {command}");
                Console.WriteLine("Available: /status, /clear, /exit\n");
                break;
        }
    }
}
