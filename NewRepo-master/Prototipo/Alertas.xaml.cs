using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Alertas.xaml
    /// </summary>
    public partial class Alertas : Window
    {
        public Alertas()
        {
            InitializeComponent();

            // Ejemplos de alertas activas
            AgregarAlerta("⚠", "Memoria RAM Alta", "Se ha detectado un uso de memoria superior al 90%.", Brushes.Orange);
            AgregarAlerta("🔥", "CPU Sobrecalentado", "La temperatura ha superado los 85°C.", Brushes.Red);
            AgregarAlerta("✅", "Sistema Estable", "No se han detectado problemas críticos.", Brushes.LightGreen);
        }

        private void BtnPeque_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnCerrar_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Agrega visualmente una alerta al contenedor en la interfaz
        /// </summary>
        private void AgregarAlerta(string icono, string titulo, string descripcion, Brush color)
        {
            var alerta = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(42, 42, 61)), // Fondo oscuro
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(0, 5, 0, 5)
            };

            var dock = new DockPanel();

            var iconoText = new TextBlock
            {
                Text = icono,
                FontSize = 20,
                Foreground = color,
                Margin = new Thickness(0, 0, 10, 0)
            };

            var textoStack = new StackPanel();

            var tituloText = new TextBlock
            {
                Text = titulo,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };

            var descText = new TextBlock
            {
                Text = descripcion,
                Foreground = Brushes.Gray,
                FontSize = 12
            };

            textoStack.Children.Add(tituloText);
            textoStack.Children.Add(descText);

            dock.Children.Add(iconoText);
            dock.Children.Add(textoStack);

            alerta.Child = dock;

            AlertContainer.Children.Add(alerta);
        }
    }
}
