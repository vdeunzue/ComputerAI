using System.Diagnostics;
using System.Management;

namespace ComputerAI
{
    public class SystemMonitor
    {
        private PerformanceCounter? cpuCounter;
        private PerformanceCounter? ramCounter;
        private readonly Dictionary<string, float> baselineMetrics = new();
        private DateTime lastCheck = DateTime.Now;

        public SystemMonitor()
        {
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");

                // Initialize baseline
                cpuCounter.NextValue();
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not initialize performance counters: {ex.Message}");
            }
        }

        public SystemMetrics GetCurrentMetrics()
        {
            var metrics = new SystemMetrics
            {
                Timestamp = DateTime.Now
            };

            try
            {
                if (cpuCounter != null)
                {
                    metrics.CpuUsage = cpuCounter.NextValue();
                }

                if (ramCounter != null)
                {
                    var availableRamMB = ramCounter.NextValue();
                    var totalRamMB = GetTotalPhysicalMemory() / (1024 * 1024);
                    metrics.RamUsedMB = totalRamMB - (long)availableRamMB;
                    metrics.RamTotalMB = totalRamMB;
                    metrics.RamUsagePercent = (float)(metrics.RamUsedMB * 100.0 / totalRamMB);
                }

                metrics.DiskUsage = GetDiskUsage();
                metrics.ProcessCount = Process.GetProcesses().Length;
                metrics.TopProcesses = GetTopProcesses(5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting metrics: {ex.Message}");
            }

            return metrics;
        }

        public List<string> DetectAnomalies(SystemMetrics current)
        {
            var anomalies = new List<string>();

            // CPU spike detection
            if (current.CpuUsage > 85)
            {
                anomalies.Add($"High CPU usage: {current.CpuUsage:F1}%");
            }

            // RAM usage detection
            if (current.RamUsagePercent > 90)
            {
                anomalies.Add($"High RAM usage: {current.RamUsagePercent:F1}% ({current.RamUsedMB:N0} MB used)");
            }

            // Disk usage detection
            foreach (var disk in current.DiskUsage)
            {
                if (disk.Value > 90)
                {
                    anomalies.Add($"Low disk space on {disk.Key}: {disk.Value:F1}% full");
                }
            }

            return anomalies;
        }

        private Dictionary<string, float> GetDiskUsage()
        {
            var diskUsage = new Dictionary<string, float>();

            try
            {
                var drives = DriveInfo.GetDrives();
                foreach (var drive in drives)
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    {
                        var usedSpace = drive.TotalSize - drive.AvailableFreeSpace;
                        var usagePercent = (float)(usedSpace * 100.0 / drive.TotalSize);
                        diskUsage[drive.Name] = usagePercent;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting disk usage: {ex.Message}");
            }

            return diskUsage;
        }

        private List<ProcessInfo> GetTopProcesses(int count)
        {
            var processes = new List<ProcessInfo>();

            try
            {
                var allProcesses = Process.GetProcesses()
                    .Where(p => !string.IsNullOrEmpty(p.ProcessName))
                    .OrderByDescending(p =>
                    {
                        try { return p.WorkingSet64; }
                        catch { return 0; }
                    })
                    .Take(count);

                foreach (var proc in allProcesses)
                {
                    try
                    {
                        processes.Add(new ProcessInfo
                        {
                            Name = proc.ProcessName,
                            MemoryMB = proc.WorkingSet64 / (1024 * 1024),
                            Id = proc.Id
                        });
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting top processes: {ex.Message}");
            }

            return processes;
        }

        private long GetTotalPhysicalMemory()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in searcher.Get())
                {
                    return Convert.ToInt64(obj["TotalVisibleMemorySize"]) * 1024;
                }
            }
            catch { }

            return 8L * 1024 * 1024 * 1024; // Default to 8GB
        }

        public void Dispose()
        {
            cpuCounter?.Dispose();
            ramCounter?.Dispose();
        }
    }

    public class SystemMetrics
    {
        public DateTime Timestamp { get; set; }
        public float CpuUsage { get; set; }
        public long RamUsedMB { get; set; }
        public long RamTotalMB { get; set; }
        public float RamUsagePercent { get; set; }
        public Dictionary<string, float> DiskUsage { get; set; } = new();
        public int ProcessCount { get; set; }
        public List<ProcessInfo> TopProcesses { get; set; } = new();

        public override string ToString()
        {
            var disk = DiskUsage.Count > 0 ? string.Join(", ", DiskUsage.Select(d => $"{d.Key}:{d.Value:F0}%")) : "N/A";
            var procs = TopProcesses.Count > 0 ? string.Join(", ", TopProcesses.Take(3).Select(p => $"{p.Name}({p.MemoryMB}MB)")) : "N/A";

            return $"CPU: {CpuUsage:F1}% | RAM: {RamUsagePercent:F1}% ({RamUsedMB:N0}/{RamTotalMB:N0} MB) | Disk: {disk} | Processes: {ProcessCount} | Top: {procs}";
        }
    }

    public class ProcessInfo
    {
        public string Name { get; set; } = "";
        public long MemoryMB { get; set; }
        public int Id { get; set; }
    }
}
