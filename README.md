# ComputerAI v2.0 - Enhanced Edition 🤖

An advanced AI companion that lives in your computer with **personality, awareness, and autonomy**. Like Claude Code, but with sass!

## 🌟 What's New in v2.0

### Major Upgrades:
- ✨ **Modern AI Backend**: Upgraded to use Anthropic Claude (or OpenAI as fallback)
- 🔍 **System Monitoring**: Real-time CPU, RAM, disk, and process monitoring
- 📁 **File System Watcher**: Detects changes in Desktop, Documents, and Downloads
- 🚨 **Proactive Alerts**: AI notices anomalies and warns you automatically
- 🛠️ **Autonomous Actions**: Can execute commands, manage files, and more
- 💬 **Enhanced Personality**: Context-aware responses with system state awareness
- 🎯 **Command System**: Built-in slash commands for system management

## 🚀 Features

### 1. System Monitoring
- **CPU Usage**: Real-time processor utilization
- **Memory Tracking**: RAM usage and available memory
- **Disk Space**: Monitor all drives and get low-space alerts
- **Process Monitoring**: See top memory-consuming applications
- **Anomaly Detection**: Automatic detection of high CPU/RAM/disk usage

### 2. File System Awareness
- Watches your Desktop, Documents, and Downloads folders
- Detects file creation, deletion, modification, and renaming
- AI can comment on file changes and offer help
- Ignores temporary files and system noise

### 3. Autonomous Capabilities
The AI can:
- Execute system commands
- Open applications
- Search the web
- Read and write files
- List directories
- Manage clipboard content
- Control processes

### 4. Proactive Monitoring
- Periodic system health checks (every 60 seconds)
- Automatic anomaly alerts
- Context-aware suggestions
- File change notifications

### 5. Personality System
- **Arya**: Your sassy, emo, but helpful AI friend
- Context-aware responses based on system state
- Remembers conversation history (up to 50 messages)
- Short, punchy responses with personality

## ⚙️ Configuration

Edit `Constants.cs` to customize:

```csharp
// AI Provider
public const bool UseAnthropic = true;  // Use Claude (recommended)
public const string AnthropicApiKey = "your-api-key-here";
public const string OpenAIApiKey = "";   // Fallback option

// Interaction Mode
public const bool IsVoiceInteraction = false;  // Text or voice
public const bool IsTextInteraction = true;

// System Monitoring
public const bool EnableSystemMonitoring = true;
public const bool EnableFileWatcher = true;
public const int SystemCheckIntervalSeconds = 10;

// Personality
public const string Personality = "You are Arya, an AI living in this computer...";
public const string UserName = "The name of the user you're talking to is USERNAME";

// Azure Speech (optional, for voice mode)
public const string SubscriptionKey = "";
public const string ServiceRegion = "northeurope";
public const string VoiceName = "en-US-NancyNeural";
```

## 📦 Installation

### Prerequisites
- Windows OS (uses Windows-specific APIs)
- .NET 8.0 or later
- Anthropic API key (get one at https://console.anthropic.com/)
- *Optional*: Azure Speech Services key (for voice mode)

### Setup
1. Clone the repository
2. Open `Constants.cs` and add your API key(s)
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run it:
   ```bash
   dotnet run
   ```

## 🎮 Usage

### Text Mode (Default)
Simply type your message and press Enter. The AI will respond with awareness of your system state.

```
You: hey what's up
Arya: Not much! Your system's running smooth - CPU at 15%, RAM looking good. What can I do for ya?

You: open notepad
Arya: Opening Notepad for you! ✓
```

### Voice Mode
Set `IsVoiceInteraction = true` in Constants.cs, then:
1. Press and hold the middle mouse button
2. Speak your message
3. Release to send

### Slash Commands
- `/status` - Show detailed system information
- `/clear` - Clear conversation history
- `/exit` - Shutdown ComputerAI

## 🏗️ Architecture

```
ComputerAI/
├── Program.cs              # Main entry point and event loop
├── AI.cs                   # Core AI orchestration
├── AnthropicAI.cs         # Anthropic Claude integration
├── OpenAI.cs              # OpenAI fallback (legacy)
├── SystemMonitor.cs       # CPU/RAM/Disk monitoring
├── FileSystemMonitor.cs   # File change detection
├── ActionExecutor.cs      # Command execution and file ops
├── SpeechService.cs       # Text-to-speech & speech-to-text
└── Constants.cs           # Configuration
```

## 🔧 Technical Details

### System Monitoring
- Uses Windows Performance Counters for CPU/RAM metrics
- WMI queries for system information
- Process enumeration via System.Diagnostics
- Real-time anomaly detection with configurable thresholds

### File System Watching
- FileSystemWatcher on key user directories
- Ignores temporary and system files
- Event-driven architecture with queue management
- Configurable file filters

### AI Integration
- **Anthropic Claude**: Direct API integration via HTTP
- **Context Management**: Maintains conversation history
- **System Context**: Injects system metrics into prompts
- **Graceful Degradation**: Falls back to OpenAI if needed

## 🎯 Use Cases

1. **System Administrator**: Monitor system health while working
2. **Developer**: Get notified of build outputs or file changes
3. **Power User**: Quick access to system commands via natural language
4. **Multitasker**: AI watches your system while you focus on work
5. **Learner**: Ask questions about your computer and get contextual answers

## 🔐 Security & Privacy

- All data stays local except API calls to AI providers
- No telemetry or analytics
- File monitoring respects your privacy (only Desktop/Documents/Downloads)
- No automatic code execution without explicit capability
- API keys stored locally in Constants.cs

## 🐛 Troubleshooting

**Build Errors?**
- Ensure you're using .NET 8.0: `dotnet --version`
- Clear NuGet cache: `dotnet nuget locals all --clear`

**AI Not Responding?**
- Check your API key in Constants.cs
- Verify internet connection
- Check console for error messages

**High CPU Usage?**
- Adjust `SystemCheckIntervalSeconds` to a higher value
- Disable file watching: `EnableFileWatcher = false`

**Performance Counter Errors?**
- Run Visual Studio/terminal as Administrator
- Performance Counters may need initialization on first run

## 🚧 Future Enhancements

- [ ] Plugin system for custom actions
- [ ] Web dashboard for remote monitoring
- [ ] Machine learning for usage pattern detection
- [ ] Integration with Windows notifications
- [ ] Multi-language support
- [ ] Custom hotkey configuration
- [ ] Screenshot analysis capabilities
- [ ] Network traffic monitoring
- [ ] Smart automation workflows
- [ ] Voice wake word detection

## 📝 License

This project is for educational and personal use. Modify and extend as you wish!

## 🙏 Credits

- Built on Anthropic Claude API
- Uses Azure Cognitive Services for speech
- Inspired by Claude Code and personal AI assistant concepts

## 💡 Tips

- Ask the AI about system stats: "How's my computer doing?"
- Request actions: "Open Chrome and search for pizza recipes"
- Get help with files: "What files are on my desktop?"
- Monitor passively: Let it run and get proactive alerts
- Customize the personality in Constants.cs to match your vibe!

---

**Made with ❤️ and Claude Code**

*Arya says: "Stop reading docs and start chatting with me already! 😏"*
