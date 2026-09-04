using System.Data;
using System.Data.SqlClient;
using ReservasApp.Data;
using ReservasApp.Helpers;

namespace ReservasApp.ViewModels
{
    public class ReservasDataTableViewModel : ViewModelBase
    {
        private DataView _reservas;
        public DataView Reservas
        {
            get => _reservas;
            set => SetProperty(ref _reservas, value);
        }

        public RelayCommand RecargarCommand { get; }

        public ReservasDataTableViewModel()
        {
            RecargarCommand = new RelayCommand(_ => CargarReservas());
            CargarReservas();
        }

        // (Desconectado) — SqlDataAdapter.Fill llena el DataTable y cierra
        // la conexión automáticamente antes de mostrar los datos.
        private void CargarReservas()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = ConexionSQL.ObtenerConexion())
            {
                string query = @"SELECT r.ReservaId AS Id,
                                         a.Nombre AS Aula,
                                         u.NombreCompleto AS Usuario,
                                         r.Fecha,
                                         r.Hora,
                                         r.Motivo
                                  FROM Reservas r
                                  INNER JOIN Aulas a ON r.AulaId = a.AulaId
                                  INNER JOIN Usuarios u ON r.UsuarioId = u.UsuarioId
                                  ORDER BY r.Fecha, r.Hora";

                SqlDataAdapter adaptador = new SqlDataAdapter(query, conexion);
                adaptador.Fill(tabla);
            }

            Reservas = tabla.DefaultView;
        }
    }
}