using Prototipo.Estados;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Prototipo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private void IniciarReloj()
        {
            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += (s, e) =>
            {
                txtHora.Text = DateTime.Now.ToString("HH:mm:ss");
                txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            };

            timer.Start();
        }

        public MainWindow(string nombreUsuario)

        {
            InitializeComponent();
            lblUsuario.Content = $"¡Hola, {nombreUsuario}!";
            IniciarReloj();
        }
        public MainWindow()
        {
            
            InitializeComponent();
            IniciarReloj();
        }

        private void UsuarioLabel_Click(object sender, RoutedEventArgs e)
        {
            Usuario ventana = new();
            ventana.Show();
        }



        private void BtnRojo_click(object sender, RoutedEventArgs e)
        {
            EsSi ventana = new();
            ventana.Show();
        }

        private void BtnAmarillo_click(object sender, RoutedEventArgs e)
        {
            CPU ventana = new();
            ventana.Show();
        }

        private void BtnNaranja_click(object sender, RoutedEventArgs e)
        {
            RAM ventana = new();
            ventana.Show();
        }

        private void BtnAzul_click(object sender, RoutedEventArgs e)
        {
            Espacio ventana = new();
            ventana.Show();
        }

        private void BtnMorado_click(object sender, RoutedEventArgs e)
        {
            Red ventana = new();
            ventana.Show();
        }

        private void BtnRo_click(object sender, RoutedEventArgs e)
        {
            Alertas ventana = new();
            ventana.Show();
        }

        private void BtnVerde_click(object sender, RoutedEventArgs e)
        {
            EsGeSi ventana = new();
            ventana.Show();
        }

        private void Inventario_Click(object sender, RoutedEventArgs e)
        {
           Inventario ventana = new();
            ventana.Show();
        }

        private void Mantenimiento_Click(object sender, RoutedEventArgs e)
        {
            Mantenimiento vetana = new();
            vetana.Show();
        }

        private void Alertas_Click(object sender, RoutedEventArgs e)
        {
            Alertaas ventana = new();
            ventana.Show();
        }

        private void Estado_Click(object sender, RoutedEventArgs e)
        {
            Estado vetana = new();
            vetana.Show();
        }

        private void BtnPeque_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnCerrar_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}