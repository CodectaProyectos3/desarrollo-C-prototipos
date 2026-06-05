using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Lógica de interacción para CPU.xaml
    /// </summary>
    public partial class CPU : Window
    {
        private PerformanceCounter cpuCounter;
        private DateTime lastUpdate = DateTime.MinValue;

        public CPU()
        {
            InitializeComponent();

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue(); // Ignora primer valor

            CompositionTarget.Rendering += UpdateCpuUsage;

            // Datos del sistema (sin System.Management)
            CpuNameLabel.Text = $"CPU: {GetSimpleProcessorName()}";
            LogicalCoresLabel.Text = $"Núcleos lógicos: {Environment.ProcessorCount}";
        }

        private void UpdateCpuUsage(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastUpdate).TotalSeconds < 1)
                return;

            lastUpdate = DateTime.Now;

            float usage = cpuCounter.NextValue();

            CpuLabel.Text = $"CPU: {usage:F1}%";
            CpuBar.Width = (usage / 100.0) * 300;

            ClockLabel.Text = $"Hora: {DateTime.Now:T}";
        }

        // ✅ Alternativa simple y compatible para obtener nombre de CPU
        private string GetSimpleProcessorName()
        {
            return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Desconocido";
        }

        private void BtnPeque_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnCerrar_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
