using System.Diagnostics;

namespace ComputerAI
{
    public class ActionExecutor
    {
        public string ExecuteCommand(string command, string arguments = "")
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processInfo);
                if (process == null)
                    return "Failed to start process";

                process.WaitForExit(5000); // 5 second timeout

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                return !string.IsNullOrEmpty(output) ? output : error;
            }
            catch (Exception ex)
            {
                return $"Error executing command: {ex.Message}";
            }
        }

        public bool OpenApplication(string appName)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = appName,
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening {appName}: {ex.Message}");
                return false;
            }
        }

        public bool KillProcess(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var proc in processes)
                {
                    proc.Kill();
                    proc.WaitForExit(3000);
                }
                return processes.Length > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error killing {processName}: {ex.Message}");
                return false;
            }
        }

        public string GetClipboardText()
        {
            try
            {
                if (System.Windows.Forms.Clipboard.ContainsText())
                {
                    return System.Windows.Forms.Clipboard.GetText();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading clipboard: {ex.Message}");
            }
            return "";
        }

        public void SetClipboardText(string text)
        {
            try
            {
                System.Windows.Forms.Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting clipboard: {ex.Message}");
            }
        }

        public List<string> ListDirectory(string path, string pattern = "*")
        {
            try
            {
                if (Directory.Exists(path))
                {
                    return Directory.GetFiles(path, pattern)
                        .Select(Path.GetFileName)
                        .Where(f => f != null)
                        .Cast<string>()
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing directory: {ex.Message}");
            }
            return new List<string>();
        }

        public bool CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directory: {ex.Message}");
                return false;
            }
        }

        public bool MoveFile(string source, string destination)
        {
            try
            {
                File.Move(source, destination);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving file: {ex.Message}");
                return false;
            }
        }

        public bool DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
            return false;
        }

        public string ReadFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
            return "";
        }

        public bool WriteFile(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing file: {ex.Message}");
                return false;
            }
        }

        public string SearchWeb(string query)
        {
            try
            {
                var url = $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                return $"Opened web search for: {query}";
            }
            catch (Exception ex)
            {
                return $"Error searching web: {ex.Message}";
            }
        }
    }
}
