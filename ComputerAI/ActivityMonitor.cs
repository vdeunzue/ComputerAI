using System.Text;

namespace ComputerAI
{
    public class ActivityMonitor
    {
        private readonly ScreenMonitor screenMonitor;
        private readonly ActionExecutor actionExecutor;
        private readonly Random random = new();

        private string lastActivity = "";
        private DateTime lastCommentary = DateTime.MinValue;
        private readonly List<string> clipboardHistory = new();
        private readonly Dictionary<string, int> appUsageCount = new();
        private readonly List<string> interestingMoments = new();

        public ActivityMonitor()
        {
            screenMonitor = new ScreenMonitor();
            actionExecutor = new ActionExecutor();
        }

        public bool ShouldMakeComment()
        {
            // Random interval between 2-5 minutes
            var minInterval = TimeSpan.FromMinutes(2);
            var maxInterval = TimeSpan.FromMinutes(5);

            var timeSinceLastComment = DateTime.Now - lastCommentary;
            var randomInterval = TimeSpan.FromMinutes(random.Next(2, 6));

            return timeSinceLastComment >= randomInterval && timeSinceLastComment >= minInterval;
        }

        public string GetActivityContext()
        {
            var context = new StringBuilder();

            // Current window/app
            var windowInfo = screenMonitor.GetActiveWindowInfo();
            context.AppendLine($"Active: {windowInfo.ApplicationName}");

            if (!string.IsNullOrEmpty(windowInfo.WindowTitle))
            {
                context.AppendLine($"Window: {windowInfo.WindowTitle}");
            }

            // Track app usage
            var appKey = windowInfo.ApplicationName;
            if (!string.IsNullOrEmpty(appKey))
            {
                appUsageCount[appKey] = appUsageCount.GetValueOrDefault(appKey, 0) + 1;
            }

            // Interesting context
            context.AppendLine(screenMonitor.GetInterestingContext());

            // Recent clipboard activity
            TrackClipboard();
            if (clipboardHistory.Count > 0)
            {
                context.AppendLine($"Recent clipboard: {clipboardHistory.Count} items");
            }

            // Most used apps
            if (appUsageCount.Any())
            {
                var topApp = appUsageCount.OrderByDescending(x => x.Value).First();
                context.AppendLine($"Most used: {topApp.Key} ({topApp.Value} times)");
            }

            lastActivity = context.ToString();
            return lastActivity;
        }

        public async Task<string?> GenerateRandomCommentary(Func<string, Task<string>> aiResponseFunc)
        {
            if (!ShouldMakeComment())
                return null;

            var context = GetActivityContext();
            var windowInfo = screenMonitor.GetActiveWindowInfo();

            // Create interesting observations
            var observations = new List<string>();

            // Window title observations
            if (windowInfo.WindowTitle.ToLower().Contains("error") ||
                windowInfo.WindowTitle.ToLower().Contains("exception"))
            {
                observations.Add("Another error? Shocking.");
            }
            else if (windowInfo.WindowTitle.ToLower().Contains("stackoverflow"))
            {
                observations.Add("StackOverflow again? What broke this time?");
            }
            else if (windowInfo.WindowTitle.ToLower().Contains("youtube") ||
                     windowInfo.WindowTitle.ToLower().Contains("netflix") ||
                     windowInfo.WindowTitle.ToLower().Contains("twitch"))
            {
                observations.Add("'Working hard' I see.");
            }
            else if (windowInfo.WindowTitle.ToLower().Contains("reddit") ||
                     windowInfo.WindowTitle.ToLower().Contains("twitter") ||
                     windowInfo.WindowTitle.ToLower().Contains("facebook"))
            {
                observations.Add("Procrastination station, population: you.");
            }
            else if (windowInfo.ApplicationName.ToLower().Contains("chrome") ||
                     windowInfo.ApplicationName.ToLower().Contains("firefox"))
            {
                var tabCount = random.Next(8, 25); // Random guess for comedy
                observations.Add($"{tabCount} Chrome tabs open. Memory screaming rn.");
            }
            else if (windowInfo.ApplicationName.ToLower().Contains("code") ||
                     windowInfo.ApplicationName.ToLower().Contains("studio"))
            {
                observations.Add("Coding again? Your eyes okay?");
            }
            else if (windowInfo.ApplicationName.ToLower().Contains("discord") ||
                     windowInfo.ApplicationName.ToLower().Contains("slack"))
            {
                observations.Add("More chatting, less working. Nice.");
            }

            // App usage patterns
            if (appUsageCount.Any())
            {
                var topApp = appUsageCount.OrderByDescending(x => x.Value).First();
                if (topApp.Value > 20)
                {
                    observations.Add($"Still stuck in {topApp.Key}? Commitment issues much?");
                }
            }

            // Clipboard behavior
            if (clipboardHistory.Count > 10)
            {
                observations.Add("Copy paste warrior over here.");
            }
            else if (clipboardHistory.Count > 20)
            {
                observations.Add("Ctrl+C Ctrl+V. That's basically your whole skillset rn.");
            }

            if (observations.Any())
            {
                // Pick a random observation and ask AI to comment
                var observation = observations[random.Next(observations.Count)];
                var prompt = $"Roast the user based on this: {observation}. Be sarcastic and funny. One sentence max.";

                try
                {
                    var comment = await aiResponseFunc(prompt);
                    lastCommentary = DateTime.Now;
                    interestingMoments.Add($"[{DateTime.Now:HH:mm}] {observation}");

                    if (interestingMoments.Count > 20)
                        interestingMoments.RemoveAt(0);

                    return comment;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ActivityMonitor] Error generating commentary: {ex.Message}");
                }
            }

            return null;
        }

        private void TrackClipboard()
        {
            try
            {
                var clipboardText = actionExecutor.GetClipboardText();
                if (!string.IsNullOrEmpty(clipboardText) &&
                    (clipboardHistory.Count == 0 || clipboardHistory.Last() != clipboardText))
                {
                    clipboardHistory.Add(clipboardText.Substring(0, Math.Min(100, clipboardText.Length)));

                    if (clipboardHistory.Count > 50)
                        clipboardHistory.RemoveAt(0);
                }
            }
            catch { }
        }

        public Dictionary<string, object> GetActivityStats()
        {
            return new Dictionary<string, object>
            {
                ["CurrentActivity"] = lastActivity,
                ["ClipboardItems"] = clipboardHistory.Count,
                ["AppUsage"] = appUsageCount,
                ["InterestingMoments"] = interestingMoments.TakeLast(5).ToList(),
                ["LastCommentary"] = lastCommentary
            };
        }

        public void ResetCommentaryTimer()
        {
            lastCommentary = DateTime.Now;
        }

        public void Dispose()
        {
            screenMonitor?.Dispose();
        }
    }
}
