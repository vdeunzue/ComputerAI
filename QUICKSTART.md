# 🚀 Quick Start Guide - ComputerAI v2.0

Get your AI companion up and running in 5 minutes!

## Step 1: Get an API Key

### Option A: Anthropic Claude (Recommended)
1. Go to https://console.anthropic.com/
2. Sign up or log in
3. Navigate to API Keys
4. Create a new key and copy it

### Option B: OpenAI (Legacy)
1. Go to https://platform.openai.com/
2. Sign up or log in
3. Create an API key
4. Copy the key

## Step 2: Configure the App

1. Open `ComputerAI/Constants.cs`
2. Find this section:
   ```csharp
   // AI Configuration
   public const bool UseAnthropic = true;
   public const string AnthropicApiKey = ""; // <- PASTE YOUR KEY HERE
   ```
3. Paste your API key between the quotes
4. Update your username:
   ```csharp
   public const string UserName = "The name of the user you're talking to is [YOUR NAME]";
   ```

## Step 3: Build & Run

```bash
cd ComputerAI
dotnet build
dotnet run
```

## Step 4: Chat!

You'll see:
```
╔═══════════════════════════════════════════════╗
║        ComputerAI v2.0 - Enhanced Edition     ║
║      Your AI companion with superpowers!      ║
╚═══════════════════════════════════════════════╝

[AI] Using Anthropic Claude
[AI] System monitoring enabled
[AI] File system monitoring enabled
[FileMonitor] Watching: C:\Users\YourName\Desktop
[FileMonitor] Watching: C:\Users\YourName\Documents
[FileMonitor] Watching: C:\Users\YourName\Downloads

✓ All systems initialized!

Arya: [Greeting message with system stats]

─────────────────────────────────────────────────
Type your message and press Enter to chat.
...
─────────────────────────────────────────────────

You: _
```

## First Commands to Try

```
You: hello
You: what's my system status?
You: open notepad
You: what files are on my desktop?
You: /status
```

## Customize It!

Want to change the personality? Edit this in `Constants.cs`:

```csharp
public const string Personality = "You are Arya, an AI living in this computer. You are funny and sassy and have an emo side. You're a good friend. You keep your sentences short.";
```

Try:
- "You are JARVIS from Iron Man. Professional and helpful."
- "You are GLaDOS. Sarcastic and darkly humorous."
- "You are a cheerful anime girl assistant. Use lots of energy!"

## Enable Voice Mode (Optional)

1. Get Azure Speech Services key from https://portal.azure.com/
2. In `Constants.cs`:
   ```csharp
   public const string SubscriptionKey = "your-azure-key";
   public const bool IsVoiceInteraction = true;
   ```
3. Restart the app
4. Press and hold middle mouse button to speak!

## Troubleshooting

**"No API key configured"**
→ Add your key to Constants.cs

**Build errors about .NET version**
→ Install .NET 8.0: https://dotnet.microsoft.com/download

**Performance counter errors**
→ Run as Administrator

**High CPU usage**
→ Set `SystemCheckIntervalSeconds = 60` in Constants.cs

## What's Next?

- Check out the full README.md for advanced features
- Explore the code and customize it
- Add your own actions in `ActionExecutor.cs`
- Modify system thresholds in `SystemMonitor.cs`

## Need Help?

- Read the full documentation in README.md
- Check error messages in the console
- Verify your API key is valid
- Make sure you have internet connection

---

**You're all set! Enjoy your AI companion! 🎉**
