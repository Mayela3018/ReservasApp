using System;
using System.Data.SqlClient;
using System.Windows;
using ReservasApp.Data;
using ReservasApp.Helpers;
using ReservasApp.Models;
using ReservasApp.Views;

namespace ReservasApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _mensajeError;
        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        public RelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(EjecutarLogin);
        }

        private void EjecutarLogin(object parametro)
        {
            string password = parametro as string;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
            {
                MensajeError = "Ingresa usuario y contraseña.";
                return;
            }

            Usuario usuarioEncontrado = ValidarUsuario(Username, password);

            if (usuarioEncontrado == null)
            {
                MensajeError = "Usuario o contraseña incorrectos.";
                return;
            }

            MensajeError = string.Empty;

            // Guardamos el usuario logueado para usarlo en el resto de la app.
            SesionActual.UsuarioActual = usuarioEncontrado;

            var menuWindow = new MenuPrincipalView();
            menuWindow.Show();

            Application.Current.Windows[0]?.Close();
        }

        private Usuario ValidarUsuario(string username, string password)
        {
            Usuario usuario = null;

            using (SqlConnection conexion = ConexionSQL.ObtenerConexion())
            {
                string query = @"SELECT UsuarioId, Username, Password, NombreCompleto
                                  FROM Usuarios
                                  WHERE Username = @Username AND Password = @Password";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Username", username);
                    comando.Parameters.AddWithValue("@Password", password);

                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            usuario = new Usuario
                            {
                                UsuarioId = (int)lector["UsuarioId"],
                                Username = lector["Username"].ToString(),
                                Password = lector["Password"].ToString(),
                                NombreCompleto = lector["NombreCompleto"].ToString()
                            };
                        }
                    }
                }
            }

            return usuario;
        }
    }
}