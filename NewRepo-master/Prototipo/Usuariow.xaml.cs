using Prototipo.DB_conntext;
using System.Linq;
using System.Windows;
using Prototipo.Models;

namespace Prototipo
{
    public partial class Usuario : Window
    {
        public Usuario()
        {
            InitializeComponent();
        }

        private void BtnClose_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtUsuario.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string contra = txtPassword.Password.Trim();

            if (contra.Length < 8)
            {
                MessageBox.Show(
                    "La contraseña debe tener al menos 8 caracteres.",
                    "Seguridad",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!contra.Any(char.IsDigit))
            {
                MessageBox.Show(
                    "La contraseña debe contener al menos un número.",
                    "Seguridad",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!contra.Any(char.IsUpper))
            {
                MessageBox.Show(
                    "La contraseña debe contener al menos una letra mayúscula.",
                    "Seguridad",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contra))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                if (db.Usuario.Any(u => u.Correo == correo))
                {
                    MessageBox.Show("Este correo ya está registrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var nuevoUsuario = new UsuarioModel
                {
                    Nombre = nombre,
                    Correo = correo,
                    Contra = contra
                };

                db.Usuario.Add(nuevoUsuario);
                db.SaveChanges();

                MessageBox.Show("Usuario registrado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                InicioSesion loginVentana = new InicioSesion();
                loginVentana.Show();
                this.Close();
            }
        }

        private void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            InicioSesion ventana = new InicioSesion();
            ventana.Show();
            this.Close();
        }
    }
}


