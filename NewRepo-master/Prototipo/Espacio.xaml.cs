using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para Espacio.xaml
    /// </summary>
    public partial class Espacio : Window
    {
        public Espacio()
        {

            InitializeComponent();
            MostrarEspacioDisco();
        }

        private void MostrarEspacioDisco()
        {
            try
            {
                DriveInfo drive = new DriveInfo("C");

                if (drive.IsReady)
                {
                    // Espacio total y libre en bytes
                    long totalBytes = drive.TotalSize;
                    long freeBytes = drive.TotalFreeSpace;
                    long usedBytes = totalBytes - freeBytes;

                    // Convertir a GB para mostrar
                    double totalGB = totalBytes / 1_073_741_824.0;
                    double usedGB = usedBytes / 1_073_741_824.0;

                    // Actualizar texto
                    DriveLabel.Text = $"Unidad C:";
                    SpaceInfoLabel.Text = $"{usedGB:F2} GB usados / {totalGB:F2} GB totales";

                    // Calcular porcentaje usado para la barra
                    double porcentaje = (double)usedBytes / totalBytes;
                    double maxWidth = 400; // mismo ancho del borde que contiene la barra

                    DriveBar.Width = porcentaje * maxWidth;
                }
                else
                {
                    DriveLabel.Text = "Unidad C: No disponible";
                    SpaceInfoLabel.Text = "No se pudo obtener información del disco.";
                    DriveBar.Width = 0;
                }
            }
            catch (Exception ex)
            {
                DriveLabel.Text = "Error al obtener datos";
                SpaceInfoLabel.Text = ex.Message;
                DriveBar.Width = 0;
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
    }
}
