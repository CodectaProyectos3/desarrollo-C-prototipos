using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Lógica de interacción para Red.xaml
    /// </summary>
    public partial class Red : Window
    {
        private DispatcherTimer timer;

        public Red()
        {
            InitializeComponent();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateNetworkStatus();
        }

        private void UpdateNetworkStatus()
        {
            bool isConnected = NetworkInterface.GetIsNetworkAvailable();

            if (!isConnected)
            {
                ConnectionStatusLabel.Text = "Estado de conexión: No conectado";
                NetworkSpeedValue.Text = "0";
                PingLabel.Text = "0 ms";
                JitterLabel.Text = "0 ms";
                DownloadLabel.Text = "0 Mbps";
                UploadLabel.Text = "-- Mbps";
                return;
            }

            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
                              nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                              nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                .OrderByDescending(nic => nic.Speed);

            var activeNic = interfaces.FirstOrDefault();

            if (activeNic != null)
            {
                string type = activeNic.NetworkInterfaceType.ToString();
                long speedMbps = activeNic.Speed / 1_000_000;

                ConnectionStatusLabel.Text = $"Estado de conexión: Conectado ({type})";
                NetworkSpeedValue.Text = speedMbps.ToString();
                DownloadLabel.Text = $"{speedMbps} Mbps";

                // Simulación de subida (puedes reemplazar con tu lógica o API)
                UploadLabel.Text = $"{speedMbps / 2} Mbps";

                // Ping real a google
                long ping = PingGoogle();
                PingLabel.Text = $"{ping} ms";

                // Fluctuación simulada (puedes medir múltiples pings si deseas)
                Random r = new Random();
                int jitter = r.Next(1, 5);
                JitterLabel.Text = $"{jitter} ms";
            }
            else
            {
                ConnectionStatusLabel.Text = "Estado de conexión: No conectado";
                NetworkSpeedValue.Text = "0";
                DownloadLabel.Text = "0 Mbps";
                UploadLabel.Text = "-- Mbps";
                PingLabel.Text = "0 ms";
                JitterLabel.Text = "0 ms";
            }
        }

        private long PingGoogle()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 1000); // Google DNS
                    if (reply.Status == IPStatus.Success)
                        return reply.RoundtripTime;
                    else
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
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
