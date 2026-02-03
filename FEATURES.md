# 🎮 ComputerAI v2.5 - All Features

## 🆕 NEW: Screen Snooping & Random Commentary!

### Activity Monitoring
Arya now **watches what you're doing** and makes random witty comments every 2-5 minutes!

**What it watches:**
- 👀 **Active window/app** - knows what you're working on
- 📋 **Clipboard history** - tracks your copy/paste behavior
- 🖥️ **App usage patterns** - remembers your most-used apps
- 🎯 **Screen OCR** (optional) - can read text from your screen!

**Examples:**
```
[💭 Random Thought]
Arya: Still coding in VS Code? Don't forget to take breaks!

[💭 Random Thought]
Arya: That's a lot of copying and pasting!

[💭 Random Thought]
Arya: StackOverflow dive? We've all been there.
```

### Configuration
In `Constants.cs`:
```csharp
// NEW Activity Monitoring Settings
public const bool EnableActivityMonitoring = true;
public const bool EnableScreenMonitoring = true;
public const bool EnableRandomCommentary = true;
public const int MinCommentaryIntervalMinutes = 2;
public const int MaxCommentaryIntervalMinutes = 5;
```

### How It Works
1. **Window Tracking** - Monitors which app/window you're in
2. **Clipboard Tracking** - Remembers what you copy (last 50 items)
3. **App Usage Stats** - Counts how often you use each app
4. **Smart Observations** - AI notices patterns and makes comments

**Detects interesting moments:**
- Error messages in window titles
- StackOverflow visits
- Long coding sessions
- Excessive copy/pasting
- App switching patterns

### OCR (Optional - Advanced)
To enable text extraction from screens:

1. Download **Tesseract Data**: https://github.com/tesseract-ocr/tessdata
2. Create folder: `ComputerAI/bin/Debug/net8.0-windows/tessdata/`
3. Place `eng.traineddata` in that folder
4. Arya can now **read text from your screen!**

**Note:** OCR is optional. Without it, Arya still tracks windows/apps/clipboard.

---

## 📊 System Monitoring

### Real-Time Metrics
- **CPU Usage** - Live processor utilization
- **RAM Usage** - Memory consumption with percentage
- **Disk Space** - All drives monitored
- **Top Processes** - Memory-hungry apps identified
- **Anomaly Detection** - Auto-alerts on high usage

### Proactive Alerts
Every 60 seconds, checks for:
- CPU > 85%
- RAM > 90%
- Disk > 90%

---

## 📁 File System Watching

### Monitored Locations
- Desktop
- Documents
- Downloads

### Detected Changes
- File creation
- File deletion
- File modification
- File renaming

### Smart Filtering
Ignores temp files: `.tmp`, `.cache`, `.log`, etc.

---

## 🎭 Personality System

### Current: Arya
- Casual and concise (1-2 sentences max)
- Context-aware responses
- Sees system stats and activity
- 50-message conversation memory

### Customizable
Edit `Constants.cs`:
```csharp
public const string Personality = "You are Arya, an AI assistant. Be casual and concise. Keep responses to 1-2 short sentences max. No rambling.";
```

Try other personalities:
- JARVIS (professional)
- GLaDOS (sarcastic)
- HAL 9000 (ominous)
- R2-D2 (beeps only... just kidding)

---

## 🛠️ Autonomous Actions

### What Arya Can Do
- Execute commands
- Open applications
- Search the web
- Read/write files
- List directories
- Manage clipboard
- Control processes

**Examples:**
```
You: open notepad
You: search for pizza recipes
You: what's in my downloads folder?
```

---

## 💬 Interaction Modes

### Text Mode (Current)
Type messages and press Enter

### Voice Mode (Optional)
Requires Azure Speech Services:
```csharp
public const string SubscriptionKey = "your-key";
public const bool IsVoiceInteraction = true;
```

Press middle mouse button to speak!

---

## 🎯 Slash Commands

- `/status` - Detailed system information
- `/clear` - Clear conversation history
- `/exit` - Shutdown ComputerAI

---

## 🔧 Technical Features

### Architecture
```
AI Brain (Claude/OpenAI)
├─ System Monitor (CPU/RAM/Disk/Processes)
├─ File System Watcher (Desktop/Docs/Downloads)
├─ Activity Monitor (Window/Clipboard/Apps) ← NEW!
├─ Screen Monitor (OCR/Screenshots) ← NEW!
└─ Action Executor (Commands/Files/Apps)
```

### Performance
- 200ms event loop
- Non-blocking async operations
- Minimal memory footprint
- Efficient monitoring with debouncing

### Privacy
- All local except AI API calls
- No telemetry
- No data upload
- Clipboard history stays local (max 50 items)
- Screen OCR is optional

---

## 🚀 More Cool Feature Ideas

Want even more? Here are ideas we can implement:

### 1. **Mood System**
Arya has moods that change based on system state:
- 😊 Happy (good performance)
- 😰 Stressed (high CPU/RAM)
- 😴 Sleepy (low activity)
- 🤔 Curious (new apps detected)

### 2. **Smart Suggestions**
- "You've been coding for 2 hours, time for a break?"
- "Chrome is using 2GB RAM, close some tabs?"
- "Want me to organize your messy desktop?"

### 3. **Task Automation**
```
You: remind me to take breaks every hour
You: auto-organize my downloads folder
You: alert me if CPU goes above 90%
```

### 4. **Integration Features**
- Spotify control ("what's playing?")
- Email notifications
- Calendar reminders
- Git status monitoring
- Build status tracking

### 5. **Advanced Screen Monitoring**
- Screenshot comparison (detect changes)
- Meme detection 😄
- Error message auto-googling
- Code review from screenshots

### 6. **Learning & Patterns**
- Learn your workflow patterns
- Predict what you'll need
- Custom shortcuts for your habits
- Time tracking per app

### 7. **Social Features**
- Share funny moments
- Generate status messages
- Auto-respond to messages (with approval)

---

## 📝 Current Limitations

1. **OCR requires Tesseract data** - not included by default
2. **Windows only** - uses Win32 APIs
3. **Single monitor** - primary screen only for now
4. **Text-based** - no GUI (yet!)
5. **English only** - OCR and personality

---

## 🎨 Customization Tips

### Change Commentary Frequency
```csharp
public const int MinCommentaryIntervalMinutes = 1;  // More frequent!
public const int MaxCommentaryIntervalMinutes = 3;
```

### Disable Features
```csharp
public const bool EnableRandomCommentary = false;  // Quiet mode
public const bool EnableActivityMonitoring = false;  // No tracking
```

### Adjust System Checks
```csharp
public const int SystemCheckIntervalSeconds = 30;  // Check more often
```

---

## 🤔 FAQ

**Q: Is Arya always watching?**
A: Only when enabled. Disable in Constants.cs

**Q: What about privacy?**
A: All data stays local. Only AI responses go to Anthropic API.

**Q: Can I disable random comments?**
A: Yes! Set `EnableRandomCommentary = false`

**Q: Does OCR work without Tesseract?**
A: No, but window/clipboard tracking still works!

**Q: Can Arya see passwords?**
A: Technically yes (if visible), but doesn't log or send them anywhere.

---

**Made with ❤️ and way too much coffee**

*Arya: "I see you're reading docs instead of chatting with me... 🤨"*
