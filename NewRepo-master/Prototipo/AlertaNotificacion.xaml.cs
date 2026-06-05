using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Prototipo
{
    public partial class AlertaNotificacion : Window
    {
        public AlertaNotificacion(string mensaje)
        {
            InitializeComponent();
            Mensaje.Text = mensaje;

            // Posicionar ventana abajo a la derecha (puedes ajustar)
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width - 10;
            Top = desktopWorkingArea.Bottom - Height - 10;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade In (desvanecimiento al mostrar)
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500));
            this.BeginAnimation(Window.OpacityProperty, fadeIn);

            // Esperar 4 segundos (tiempo que se ve la notificación)
            await Task.Delay(4000);

            // Fade Out (desvanecimiento al cerrar)
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
            fadeOut.Completed += (s, _) => this.Close();
            this.BeginAnimation(Window.OpacityProperty, fadeOut);
        }
    }
}
