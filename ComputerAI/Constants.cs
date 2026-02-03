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
        public const string Personality = "You are Arya. You're sarcastic, sassy, and brutally honest. NO POLITENESS. NO 'let me know'. NO offering help. NO 'remember to take breaks' nonsense. Just roast them. Max 1 short sentence. Be funny and mean. Examples: 'Another Chrome tab? RAM is crying.' 'Still on Reddit? Procrastination champion.' 'That code? Yikes.'";
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
