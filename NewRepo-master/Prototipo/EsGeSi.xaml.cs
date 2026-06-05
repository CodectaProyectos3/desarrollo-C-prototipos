using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;

namespace Prototipo
{
    public partial class EsGeSi : Window
    {
        private NetworkInterface activeInterface;
        private long lastBytesReceived = 0;
        private long lastBytesSent = 0;
        private DateTime lastCheck = DateTime.UtcNow;
        private Random rnd = new Random();

        // 🔹 NUEVA VARIABLE DE CONTROL
        private bool appActiva = true;

        public EsGeSi()
        {
            InitializeComponent();
            PcNameText.Text = Environment.MachineName;
            IdentificarInterfaz();
            IniciarActualizacion();
        }

        private void IdentificarInterfaz()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up &&
                            n.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                            n.Speed > 0);

            activeInterface = interfaces
                .OrderByDescending(n => n.Speed)
                .FirstOrDefault();

            if (activeInterface != null)
            {
                var stats = activeInterface.GetIPv4Statistics();
                lastBytesReceived = stats.BytesReceived;
                lastBytesSent = stats.BytesSent;
                lastCheck = DateTime.UtcNow;
            }
        }

        // 🔹 MODIFICADO para que use appActiva
        private async void IniciarActualizacion()
        {
            while (appActiva)
            {
                try
                {
                    // 🔹 Datos simulados de CPU y RAM (o reales si los tienes)
                    double cpu = rnd.Next(10, 100); // Porcentaje de CPU
                    double ram = rnd.Next(20, 100); // Porcentaje de RAM

                    // 🔹 Disco
                    DriveInfo drive = new DriveInfo("C");
                    double diskUsage = 0;
                    if (drive.IsReady)
                    {
                        double total = drive.TotalSize;
                        double used = total - drive.AvailableFreeSpace;
                        diskUsage = (used / total) * 100;
                    }

                    // 🔹 Actualizar UI
                    CpuValueText.Text = $"{cpu:F0}%";
                    CpuBar.Value = cpu;
                    RamValueText.Text = $"{ram:F0}%";
                    RamBar.Value = ram;
                    DiskValueText.Text = $"{diskUsage:F0}%";
                    DiskBar.Value = diskUsage;

                    // 🔹 ALERTAS AUTOMÁTICAS
                    if (cpu > 50)
                        MostrarAlerta($"⚠️ CPU alta: {cpu:F0}%");

                    if (ram > 50)
                        MostrarAlerta($"⚠️ RAM alta: {ram:F0}%");

                    if (diskUsage > 90)
                        MostrarAlerta($"⚠️ Disco casi lleno: {diskUsage:F0}%");

                }
                catch {  }

                await Task.Delay(1000); // 1 segundo entre actualizaciones
            }
        }


        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnPeque_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
       


        private void MostrarAlerta(string mensaje)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var alerta = new AlertaNotificacion(mensaje);
                alerta.Show();
            });
        }

        // 🔹 ESTE MÉTODO NUEVO DETIENE EL BUCLE AL CERRAR
        protected override void OnClosed(EventArgs e)
        {
            appActiva = false;
            base.OnClosed(e);
        }
    }
}
