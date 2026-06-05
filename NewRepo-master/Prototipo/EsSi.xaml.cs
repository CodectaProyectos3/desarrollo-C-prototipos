using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Prototipo
{
    /// <summary>
    /// Lógica de interacción para EsSi.xaml
    /// </summary>
    public partial class EsSi : Window
    {
        private DispatcherTimer timer;
        private PerformanceCounter availableMemoryCounter;
        private PerformanceCounter cpuCounter;

        private ulong totalMemory;

        public EsSi()
        {
            InitializeComponent();

            // Inicializa los PerformanceCounters
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();

            availableMemoryCounter = new PerformanceCounter("Memory", "Available Bytes");

            // Obtiene la memoria física total usando P/Invoke
            totalMemory = GetTotalPhysicalMemory();

            // Timer para actualizar
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCpuUsage();
            UpdateRamUsage();
            UpdateDiskUsage();
        }

        private void UpdateCpuUsage()
        {
            float cpuUsage = cpuCounter.NextValue();
            CpuLabel.Text = $"CPU: {cpuUsage:F1}%";
            CpuBar.Width = Math.Min(350, 3.5 * cpuUsage);
        }

        private void UpdateRamUsage()
        {
            float availableBytes = availableMemoryCounter.NextValue();
            ulong usedBytes = totalMemory - (ulong)availableBytes;

            double totalGB = totalMemory / (1024.0 * 1024 * 1024);
            double usedGB = usedBytes / (1024.0 * 1024 * 1024);
            double percent = (double)usedBytes / totalMemory * 100;

            RamLabel.Text = $"RAM: {usedGB:F2} GB / {totalGB:F2} GB";
            RamBar.Width = Math.Min(350, 3.5 * percent);
        }

        private void UpdateDiskUsage()
        {
            try
            {
                DriveInfo drive = new DriveInfo("C");
                if (drive.IsReady)
                {
                    long totalBytes = drive.TotalSize;
                    long freeBytes = drive.TotalFreeSpace;
                    long usedBytes = totalBytes - freeBytes;

                    double totalGB = totalBytes / 1_073_741_824.0;
                    double usedGB = usedBytes / 1_073_741_824.0;
                    double percent = (double)usedBytes / totalBytes * 100;

                    DiskLabel.Text = $"Disco C: {usedGB:F2} GB usados / {totalGB:F2} GB totales";
                    DiskBar.Width = Math.Min(350, 3.5 * percent);
                }
                else
                {
                    DiskLabel.Text = "Disco C: No disponible";
                    DiskBar.Width = 0;
                }
            }
            catch (Exception ex)
            {
                DiskLabel.Text = $"Error disco: {ex.Message}";
                DiskBar.Width = 0;
            }
        }

        // P/Invoke para obtener memoria total física
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public void Init()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        private ulong GetTotalPhysicalMemory()
        {
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            memStatus.Init();

            if (GlobalMemoryStatusEx(ref memStatus))
            {
                return memStatus.ullTotalPhys;
            }
            return 0;
        }

        private void BtnPeque_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnCerrar_click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.Close();
        }
    }
}