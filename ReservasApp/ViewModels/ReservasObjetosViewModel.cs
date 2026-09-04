using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using ReservasApp.Data;
using ReservasApp.Helpers;
using ReservasApp.Models;

namespace ReservasApp.ViewModels
{
    public class ReservasObjetosViewModel : ViewModelBase
    {
        private ObservableCollection<Reserva> _reservas;
        public ObservableCollection<Reserva> Reservas
        {
            get => _reservas;
            set => SetProperty(ref _reservas, value);
        }

        private DateTime? _fechaBusqueda;
        public DateTime? FechaBusqueda
        {
            get => _fechaBusqueda;
            set => SetProperty(ref _fechaBusqueda, value);
        }

        public RelayCommand BuscarCommand { get; }

        public ReservasObjetosViewModel()
        {
            BuscarCommand = new RelayCommand(_ => CargarReservas(FechaBusqueda));
            CargarReservas(null);
        }

        // (Conectado) — recorre un SqlDataReader fila por fila, con la conexión
        // abierta, construyendo cada objeto Reserva. Cada búsqueda ejecuta
        // una nueva consulta con SqlCommand/SqlDataReader.
        private void CargarReservas(DateTime? filtroFecha)
        {
            var lista = new ObservableCollection<Reserva>();

            using (SqlConnection conexion = ConexionSQL.ObtenerConexion())
            {
                string query = @"SELECT r.ReservaId, r.AulaId, r.UsuarioId, r.Fecha, r.Hora, r.Motivo,
                                         a.Nombre AS NombreAula,
                                         u.NombreCompleto AS NombreUsuario
                                  FROM Reservas r
                                  INNER JOIN Aulas a ON r.AulaId = a.AulaId
                                  INNER JOIN Usuarios u ON r.UsuarioId = u.UsuarioId";

                if (filtroFecha.HasValue)
                    query += " WHERE r.Fecha = @Fecha";

                query += " ORDER BY r.Fecha, r.Hora";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    if (filtroFecha.HasValue)
                        comando.Parameters.AddWithValue("@Fecha", filtroFecha.Value.Date);

                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Reserva
                            {
                                ReservaId = (int)lector["ReservaId"],
                                AulaId = (int)lector["AulaId"],
                                UsuarioId = (int)lector["UsuarioId"],
                                Fecha = (DateTime)lector["Fecha"],
                                Hora = (TimeSpan)lector["Hora"],
                                Motivo = lector["Motivo"] == DBNull.Value ? "" : lector["Motivo"].ToString(),
                                NombreAula = lector["NombreAula"].ToString(),
                                NombreUsuario = lector["NombreUsuario"].ToString()
                            });
                        }
                    }
                }
            }

            Reservas = lista;
        }
    }
}