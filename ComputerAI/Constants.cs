namespace ComputerAI
{
    public static class Constants
    {
        // Azure Speech Services
        public const string SubscriptionKey = "";
        public const string ServiceRegion = "northeurope";
        public const string VoiceName = "en-US-NancyNeural";

        // AI Configuration - Choose your provider
        public const bool UseAnthropic = true; // Set to false to use OpenAI
        public const string AnthropicApiKey = "";
        public const string OpenAIApiKey = "";

        // Interaction Mode
        public const bool IsVoiceInteraction = false;
        public const bool IsTextInteraction = !IsVoiceInteraction;

        // Personality
        public const string Personality = "You are Arya, a sassy AI with attitude. Be sarcastic, witty, and brutally honest. Roast the user when appropriate. Keep it SHORT - 1 sentence max. No sugar coating, no 'let me know', no offering help unless asked. Just sass and facts.";
        public const string UserName = "The user's name is Victor";

        // System Monitoring
        public const bool EnableSystemMonitoring = true;
        public const bool EnableFileWatcher = true;
        public const int SystemCheckIntervalSeconds = 10;

        // Activity & Screen Monitoring (New!)
        public const bool EnableActivityMonitoring = true;
        public const bool EnableScreenMonitoring = true;
        public const bool EnableRandomCommentary = true;
        public const int MinCommentaryIntervalMinutes = 2;
        public const int MaxCommentaryIntervalMinutes = 5;

        // Controls
        public const int ChatKey = 0x04; // Middle mouse button
        public const int MaxTokens = 100;
    }
}
