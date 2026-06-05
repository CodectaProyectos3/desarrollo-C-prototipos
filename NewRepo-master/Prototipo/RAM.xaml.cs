using System;
using System.Collections.Generic;
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

namespace Prototipo
{
    /// <summary>
    /// Lógica de interacción para RAM.xaml
    /// </summary>
    public partial class RAM : Window
    {
        private DateTime lastUpdate = DateTime.MinValue;

        public RAM()
        {
            InitializeComponent();
            CompositionTarget.Rendering += UpdateRamUsage;

            LogicalCoresLabel.Text = $"Núcleos lógicos: {Environment.ProcessorCount}";
        }

        private void UpdateRamUsage(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastUpdate).TotalSeconds < 1)
                return;

            lastUpdate = DateTime.Now;

            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                double totalGB = Math.Round(memStatus.ullTotalPhys / (1024.0 * 1024 * 1024), 2);
                double availGB = Math.Round(memStatus.ullAvailPhys / (1024.0 * 1024 * 1024), 2);
                double usedGB = totalGB - availGB;
                double percentUsed = (usedGB / totalGB) * 100;

                RamLabel.Text = $"RAM: {usedGB:F2} GB / {totalGB:F2} GB ({percentUsed:F1}%)";
                RamBar.Width = (percentUsed / 100.0) * 300;
                ClockLabel.Text = $"Hora: {DateTime.Now:T}";
            }
            else
            {
                RamLabel.Text = "Error al obtener uso de RAM";
            }
        }

        private void BtnPeque_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnCerrar_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // ✅ Estructura para obtener RAM desde Windows API
        [DllImport("kernel32.dll")]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }
    }
}
