using Prototipo.DB_conntext;
using Prototipo.Models;
using System.Linq;
using System.Windows;

namespace Prototipo
{
    public partial class InicioSesion : Window
    {
        public InicioSesion()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            string contra = txtContra.Password.Trim();
            if (contra.Length < 8)
            {
                MessageBox.Show(
                    "Formato de contraseña inválido.",
                    "Seguridad",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contra))
            {
                MessageBox.Show("Por favor, ingresa correo y contraseña.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                var usuario = db.Usuario
                                .FirstOrDefault(u => u.Correo == correo && u.Contra == contra);

                if (usuario != null)
                {
                    MessageBox.Show($"¡Bienvenido, {usuario.Nombre}!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Abrir MainWindow pasando el nombre
                    MainWindow ventanaPrincipal = new MainWindow(usuario.Nombre);
                    ventanaPrincipal.Show();
                    this.Close(); // cerrar login
                }
                else
                {
                    MessageBox.Show("Correo o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

