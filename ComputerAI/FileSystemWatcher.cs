using System.Collections.Concurrent;

namespace ComputerAI
{
    public class FileSystemMonitor
    {
        private readonly List<FileSystemWatcher> watchers = new();
        private readonly ConcurrentQueue<FileChangeEvent> recentChanges = new();
        private readonly int maxQueueSize = 100;
        private readonly HashSet<string> ignoredExtensions = new()
        {
            ".tmp", ".temp", ".log", ".cache", ".etl", ".regtrans-ms", ".blf"
        };

        public event EventHandler<FileChangeEvent>? FileChanged;

        public void StartWatching(List<string> paths)
        {
            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                    continue;

                try
                {
                    var watcher = new FileSystemWatcher(path)
                    {
                        NotifyFilter = NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.Size
                                     | NotifyFilters.LastWrite,
                        IncludeSubdirectories = false
                    };

                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Changed += OnChanged;
                    watcher.Renamed += OnRenamed;

                    watcher.EnableRaisingEvents = true;
                    watchers.Add(watcher);

                    Console.WriteLine($"[FileMonitor] Watching: {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FileMonitor] Error watching {path}: {ex.Message}");
                }
            }
        }

        public void StartWatchingDesktop()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            StartWatching(new List<string> { desktop, documents, downloads });
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (ShouldIgnore(e.Name ?? ""))
                return;

            var changeEvent = new FileChangeEvent
            {
                Timestamp = DateTime.Now,
                ChangeType = e.ChangeType.ToString(),
                FilePath = e.FullPath,
                FileName = e.Name ?? ""
            };

            AddToQueue(changeEvent);
            FileChanged?.Invoke(this, changeEvent);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (ShouldIgnore(e.Name ?? ""))
                return;

            var changeEvent = new FileChangeEvent
            {
                Timestamp = DateTime.Now,
                ChangeType = "Renamed",
                FilePath = e.FullPath,
                FileName = e.Name ?? "",
                OldFileName = e.OldName
            };

            AddToQueue(changeEvent);
            FileChanged?.Invoke(this, changeEvent);
        }

        private bool ShouldIgnore(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return ignoredExtensions.Contains(extension) || fileName.StartsWith("~");
        }

        private void AddToQueue(FileChangeEvent changeEvent)
        {
            recentChanges.Enqueue(changeEvent);

            while (recentChanges.Count > maxQueueSize)
            {
                recentChanges.TryDequeue(out _);
            }
        }

        public List<FileChangeEvent> GetRecentChanges(int count = 10)
        {
            return recentChanges.TakeLast(count).ToList();
        }

        public void StopWatching()
        {
            foreach (var watcher in watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            watchers.Clear();
        }
    }

    public class FileChangeEvent
    {
        public DateTime Timestamp { get; set; }
        public string ChangeType { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string FileName { get; set; } = "";
        public string? OldFileName { get; set; }

        public override string ToString()
        {
            var time = Timestamp.ToString("HH:mm:ss");
            if (OldFileName != null)
            {
                return $"[{time}] {ChangeType}: {OldFileName} → {FileName}";
            }
            return $"[{time}] {ChangeType}: {FileName}";
        }
    }
}
