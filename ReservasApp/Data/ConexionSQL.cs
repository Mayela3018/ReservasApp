using System.Data.SqlClient;

namespace ReservasApp.Data
{
    // Centraliza la cadena de conexión en un solo lugar (buena práctica:
    // evita repetir el connection string en cada ViewModel).
    public static class ConexionSQL
    {
        // Ajusta "LAPTOP-GB67MAS4\\SQLEXPRESS" si tu instancia tiene otro nombre.
        private const string CadenaConexion =
            @"Data Source=LAPTOP-GB67MAS4\SQLEXPRESS;Initial Catalog=ReservasDB;Integrated Security=True";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}