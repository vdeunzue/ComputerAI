namespace ComputerAI
{
    public class AI
    {
        private static AnthropicAI? anthropicAI;
        private static SystemMonitor? systemMonitor;
        private static FileSystemMonitor? fileMonitor;
        private static ActivityMonitor? activityMonitor;
        private static SystemMetrics? lastMetrics;
        private static DateTime lastSystemCheck = DateTime.MinValue;
        private static List<string> recentAlerts = new();

        public static void Initialize()
        {
            // Initialize AI provider
            if (Constants.UseAnthropic && !string.IsNullOrEmpty(Constants.AnthropicApiKey))
            {
                anthropicAI = new AnthropicAI(Constants.AnthropicApiKey);
                Console.WriteLine("[AI] Using Anthropic Claude");
            }
            else if (!string.IsNullOrEmpty(Constants.OpenAIApiKey))
            {
                Console.WriteLine("[AI] Using OpenAI (Legacy mode - limited features)");
            }
            else
            {
                Console.WriteLine("[AI] WARNING: No API key configured! Set AnthropicApiKey or OpenAIApiKey in Constants.cs");
            }

            // Initialize system monitoring
            if (Constants.EnableSystemMonitoring)
            {
                systemMonitor = new SystemMonitor();
                Console.WriteLine("[AI] System monitoring enabled");
            }

            // Initialize file system monitoring
            if (Constants.EnableFileWatcher)
            {
                fileMonitor = new FileSystemMonitor();
                fileMonitor.FileChanged += OnFileChanged;
                fileMonitor.StartWatchingDesktop();
                Console.WriteLine("[AI] File system monitoring enabled");
            }

            // Initialize activity monitoring
            if (Constants.EnableActivityMonitoring)
            {
                activityMonitor = new ActivityMonitor();
                Console.WriteLine("[AI] Activity & screen monitoring enabled");
                Console.WriteLine("[AI] Random commentary enabled (every 2-5 min)");
            }
        }

        private static void OnFileChanged(object? sender, FileChangeEvent e)
        {
            var alert = $"File change detected: {e}";
            recentAlerts.Add(alert);

            // Keep only last 10 alerts
            if (recentAlerts.Count > 10)
            {
                recentAlerts.RemoveAt(0);
            }

            Console.WriteLine($"[FileWatch] {e}");
        }

        public static async Task GreetHuman()
        {
            var context = "You have just booted up.";

            if (systemMonitor != null)
            {
                lastMetrics = systemMonitor.GetCurrentMetrics();
                context += $" Current system status: {lastMetrics}";
            }

            var greetings = await GetResponse(context);
            await OutputResponse("Arya", greetings);
        }

        public static async Task AnswerHuman(string input)
        {
            // Check for action commands first
            var actionResult = TryExecuteAction(input);
            if (actionResult != null)
            {
                await OutputResponse("Arya", actionResult);
                return;
            }

            // Check if it's time for a system update
            bool includeSystemUpdate = false;
            if (systemMonitor != null &&
                (DateTime.Now - lastSystemCheck).TotalSeconds >= Constants.SystemCheckIntervalSeconds)
            {
                lastMetrics = systemMonitor.GetCurrentMetrics();
                lastSystemCheck = DateTime.Now;

                var anomalies = systemMonitor.DetectAnomalies(lastMetrics);
                if (anomalies.Any())
                {
                    foreach (var anomaly in anomalies)
                    {
                        recentAlerts.Add(anomaly);
                    }
                    includeSystemUpdate = true;
                }
            }

            // Build contextual input
            var contextualInput = input;
            if (includeSystemUpdate && lastMetrics != null)
            {
                contextualInput += $"\n\n[System Update: {lastMetrics}]";
                if (recentAlerts.Any())
                {
                    contextualInput += $"\n[Recent Alerts: {string.Join("; ", recentAlerts.TakeLast(3))}]";
                }
            }

            // Add activity context if available
            if (activityMonitor != null && Constants.EnableActivityMonitoring)
            {
                var activityContext = activityMonitor.GetActivityContext();
                contextualInput += $"\n[Current Activity: {activityContext}]";
            }

            var response = await GetResponse(contextualInput, lastMetrics);
            await OutputResponse("Arya", response);

            // Clear old alerts
            if (recentAlerts.Count > 5)
            {
                recentAlerts.RemoveRange(0, recentAlerts.Count - 5);
            }
        }

        private static async Task<string> GetResponse(string input, SystemMetrics? metrics = null)
        {
            try
            {
                if (anthropicAI != null)
                {
                    return await anthropicAI.GetResponseAsync(input, metrics);
                }
                else if (!string.IsNullOrEmpty(Constants.OpenAIApiKey))
                {
                    return await OpenAI.GetResponseAsync(input);
                }
                else
                {
                    return "No AI provider configured. Please set an API key in Constants.cs";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AI Error] {ex.Message}");
                return "Oops, I had a brain glitch. Can you repeat that?";
            }
        }

        private static async Task OutputResponse(string speaker, string response)
        {
            // Clean up response
            response = response.Replace("\n\n", " ").Replace("\n", " ").Trim();

            Console.WriteLine($"{speaker}: {response}\n");

            // Text-to-speech if enabled
            if (Constants.IsVoiceInteraction && !string.IsNullOrEmpty(Constants.SubscriptionKey))
            {
                try
                {
                    await SpeechService.TextToSpeechAsync(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Speech Error] {ex.Message}");
                }
            }
        }

        public static async Task ProactiveCheck()
        {
            if (systemMonitor == null)
                return;

            var metrics = systemMonitor.GetCurrentMetrics();
            var anomalies = systemMonitor.DetectAnomalies(metrics);

            if (anomalies.Any())
            {
                var alert = string.Join(", ", anomalies);
                Console.WriteLine($"\n[!] Proactive Alert: {alert}");

                var proactiveMessage = $"I noticed something: {alert}. Should I be concerned?";
                var response = await GetResponse(proactiveMessage, metrics);
                await OutputResponse("Arya", response);
            }
        }

        public static async Task RandomCommentary()
        {
            if (activityMonitor == null || !Constants.EnableRandomCommentary)
                return;

            var commentary = await activityMonitor.GenerateRandomCommentary(async (input) => await GetResponse(input));
            if (commentary != null)
            {
                Console.WriteLine($"\n[💭 Random Thought]");
                await OutputResponse("Arya", commentary);
            }
        }

        private static string? TryExecuteAction(string input)
        {
            var executor = new ActionExecutor();
            var lowerInput = input.ToLower();

            // Open application commands
            if (lowerInput.Contains("open "))
            {
                var appName = lowerInput.Replace("open ", "").Trim();
                var success = executor.OpenApplication(appName);
                return success ? $"Opening {appName}." : $"Couldn't open {appName}. Sure that exists?";
            }

            // Search web
            if (lowerInput.Contains("search for ") || lowerInput.Contains("google "))
            {
                var query = lowerInput.Replace("search for ", "").Replace("google ", "").Trim();
                executor.SearchWeb(query);
                return $"Googling that for you.";
            }

            // List files
            if (lowerInput.Contains("list files") || lowerInput.Contains("what files") || lowerInput.Contains("show files"))
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (lowerInput.Contains("download")) path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                else if (lowerInput.Contains("document")) path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var files = executor.ListDirectory(path);
                if (files.Count == 0) return "Empty. Congrats on being organized, I guess.";
                var fileList = string.Join(", ", files.Take(10));
                return $"{files.Count} files: {fileList}{(files.Count > 10 ? "..." : "")}";
            }

            return null; // No action detected, proceed with normal AI response
        }

        public static void Cleanup()
        {
            systemMonitor?.Dispose();
            fileMonitor?.StopWatching();
            activityMonitor?.Dispose();
        }
    }
}
