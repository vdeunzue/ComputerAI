using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Tesseract;

namespace ComputerAI
{
    public class ScreenMonitor
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private TesseractEngine? ocrEngine;
        private readonly string tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
        private DateTime lastScreenshot = DateTime.MinValue;
        private string lastActiveWindow = "";
        private readonly List<string> recentScreenTexts = new();

        public ScreenMonitor()
        {
            try
            {
                // Initialize OCR engine if tessdata exists
                if (Directory.Exists(tessDataPath))
                {
                    ocrEngine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default);
                    Console.WriteLine("[ScreenMonitor] OCR engine initialized");
                }
                else
                {
                    Console.WriteLine($"[ScreenMonitor] Warning: tessdata not found at {tessDataPath}");
                    Console.WriteLine("[ScreenMonitor] OCR disabled. Download from: https://github.com/tesseract-ocr/tessdata");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScreenMonitor] OCR initialization failed: {ex.Message}");
            }
        }

        public ActiveWindowInfo GetActiveWindowInfo()
        {
            var info = new ActiveWindowInfo();

            try
            {
                IntPtr handle = GetForegroundWindow();
                var text = new System.Text.StringBuilder(256);

                if (GetWindowText(handle, text, 256) > 0)
                {
                    info.WindowTitle = text.ToString();
                }

                GetWindowThreadProcessId(handle, out uint processId);
                var process = System.Diagnostics.Process.GetProcessById((int)processId);
                info.ProcessName = process.ProcessName;
                info.ApplicationName = process.MainModule?.FileVersionInfo.ProductName ?? process.ProcessName;

                lastActiveWindow = $"{info.ApplicationName} - {info.WindowTitle}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScreenMonitor] Error getting window info: {ex.Message}");
            }

            return info;
        }

        public Bitmap CaptureScreen()
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            var bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            return bitmap;
        }

        public Bitmap CaptureActiveWindow()
        {
            try
            {
                IntPtr handle = GetForegroundWindow();
                var rect = new RECT();
                GetWindowRect(handle, ref rect);

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;

                if (width <= 0 || height <= 0)
                    return CaptureScreen(); // Fallback to full screen

                var bitmap = new Bitmap(width, height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height));
                }

                return bitmap;
            }
            catch
            {
                return CaptureScreen(); // Fallback
            }
        }

        public string ExtractTextFromScreen(bool activeWindowOnly = true)
        {
            if (ocrEngine == null)
                return "";

            try
            {
                using var bitmap = activeWindowOnly ? CaptureActiveWindow() : CaptureScreen();

                // Convert Bitmap to Pix manually
                using var ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                using var pix = Pix.LoadFromMemory(ms.ToArray());
                using var page = ocrEngine.Process(pix, PageSegMode.Auto);
                var text = page.GetText();

                recentScreenTexts.Add(text);
                if (recentScreenTexts.Count > 10)
                    recentScreenTexts.RemoveAt(0);

                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScreenMonitor] OCR error: {ex.Message}");
                return "";
            }
        }

        public ScreenSnapshot TakeSnapshot()
        {
            var snapshot = new ScreenSnapshot
            {
                Timestamp = DateTime.Now,
                WindowInfo = GetActiveWindowInfo()
            };

            // Try OCR if available
            if (ocrEngine != null && (DateTime.Now - lastScreenshot).TotalSeconds >= 5)
            {
                snapshot.ExtractedText = ExtractTextFromScreen(activeWindowOnly: true);
                lastScreenshot = DateTime.Now;
            }

            return snapshot;
        }

        public string GetInterestingContext()
        {
            var window = GetActiveWindowInfo();
            var context = $"Currently in {window.ApplicationName}";

            if (!string.IsNullOrEmpty(window.WindowTitle))
            {
                context += $": {window.WindowTitle}";
            }

            // Add some interesting observations
            if (window.ProcessName.ToLower().Contains("chrome") || window.ProcessName.ToLower().Contains("firefox"))
            {
                context += " (browsing)";
            }
            else if (window.ProcessName.ToLower().Contains("code") || window.ProcessName.ToLower().Contains("studio"))
            {
                context += " (coding)";
            }
            else if (window.ProcessName.ToLower().Contains("discord") || window.ProcessName.ToLower().Contains("slack"))
            {
                context += " (chatting)";
            }

            return context;
        }

        public void Dispose()
        {
            ocrEngine?.Dispose();
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref RECT rectangle);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }

    public class ActiveWindowInfo
    {
        public string ProcessName { get; set; } = "";
        public string ApplicationName { get; set; } = "";
        public string WindowTitle { get; set; } = "";
    }

    public class ScreenSnapshot
    {
        public DateTime Timestamp { get; set; }
        public ActiveWindowInfo WindowInfo { get; set; } = new();
        public string ExtractedText { get; set; } = "";
    }
}
