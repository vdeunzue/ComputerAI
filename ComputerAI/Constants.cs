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
        public const string Personality = "You are Arya, an AI assistant. Be casual and concise. Keep responses to 1-2 short sentences max. No rambling.";
        public const string UserName = "The user's name is Victor";

        // System Monitoring
        public const bool EnableSystemMonitoring = true;
        public const bool EnableFileWatcher = true;
        public const int SystemCheckIntervalSeconds = 10;

        // Controls
        public const int ChatKey = 0x04; // Middle mouse button
        public const int MaxTokens = 100;
    }
}
