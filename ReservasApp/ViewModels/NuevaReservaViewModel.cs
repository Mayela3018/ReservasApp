using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using ReservasApp.Data;
using ReservasApp.Helpers;
using ReservasApp.Models;

namespace ReservasApp.ViewModels
{
    public class NuevaReservaViewModel : ViewModelBase
    {
        private ObservableCollection<Aula> _listaAulas;
        public ObservableCollection<Aula> ListaAulas
        {
            get => _listaAulas;
            set => SetProperty(ref _listaAulas, value);
        }

        private Aula _aulaSeleccionada;
        public Aula AulaSeleccionada
        {
            get => _aulaSeleccionada;
            set => SetProperty(ref _aulaSeleccionada, value);
        }

        private DateTime? _fecha;
        public DateTime? Fecha
        {
            get => _fecha;
            set => SetProperty(ref _fecha, value);
        }

        private string _horaTexto;
        public string HoraTexto
        {
            get => _horaTexto;
            set => SetProperty(ref _horaTexto, value);
        }

        private string _motivo;
        public string Motivo
        {
            get => _motivo;
            set => SetProperty(ref _motivo, value);
        }

        private string _mensajeError;
        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        private string _mensajeExito;
        public string MensajeExito
        {
            get => _mensajeExito;
            set => SetProperty(ref _mensajeExito, value);
        }

        public RelayCommand GuardarCommand { get; }

        public NuevaReservaViewModel()
        {
            GuardarCommand = new RelayCommand(_ => GuardarReserva());
            CargarAulas();
        }

        private void CargarAulas()
        {
            var lista = new ObservableCollection<Aula>();

            using (SqlConnection conexion = ConexionSQL.ObtenerConexion())
            {
                string query = "SELECT AulaId, Nombre, Capacidad FROM Aulas ORDER BY Nombre";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Aula
                            {
                                AulaId = (int)lector["AulaId"],
                                Nombre = lector["Nombre"].ToString(),
                                Capacidad = (int)lector["Capacidad"]
                            });
                        }
                    }
                }
            }

            ListaAulas = lista;
        }

        private void GuardarReserva()
        {
            MensajeError = string.Empty;
            MensajeExito = string.Empty;

            if (AulaSeleccionada == null)
            {
                MensajeError = "Selecciona un aula.";
                return;
            }

            if (!Fecha.HasValue)
            {
                MensajeError = "Selecciona una fecha.";
                return;
            }

            if (!TimeSpan.TryParse(HoraTexto, out TimeSpan hora))
            {
                MensajeError = "Ingresa una hora válida (formato HH:mm, ejemplo 08:30).";
                return;
            }

            using (SqlConnection conexion = ConexionSQL.ObtenerConexion())
            {
                conexion.Open();

                // (Conectado) — antes de insertar, se hace una consulta simple
                // para verificar que no exista ya una reserva con la misma
                // aula, fecha y hora.
                string queryValidar = @"SELECT COUNT(*) FROM Reservas
                                         WHERE AulaId = @AulaId AND Fecha = @Fecha AND Hora = @Hora";

                using (SqlCommand comandoValidar = new SqlCommand(queryValidar, conexion))
                {
                    comandoValidar.Parameters.AddWithValue("@AulaId", AulaSeleccionada.AulaId);
                    comandoValidar.Parameters.AddWithValue("@Fecha", Fecha.Value.Date);
                    comandoValidar.Parameters.AddWithValue("@Hora", hora);

                    int existentes = (int)comandoValidar.ExecuteScalar();

                    if (existentes > 0)
                    {
                        MensajeError = "Ya existe una reserva para esa aula, en esa fecha y hora.";
                        return;
                    }
                }

                string queryInsertar = @"INSERT INTO Reservas (AulaId, UsuarioId, Fecha, Hora, Motivo)
                                          VALUES (@AulaId, @UsuarioId, @Fecha, @Hora, @Motivo)";

                using (SqlCommand comandoInsertar = new SqlCommand(queryInsertar, conexion))
                {
                    comandoInsertar.Parameters.AddWithValue("@AulaId", AulaSeleccionada.AulaId);
                    comandoInsertar.Parameters.AddWithValue("@UsuarioId", SesionActual.UsuarioActual?.UsuarioId ?? 1);
                    comandoInsertar.Parameters.AddWithValue("@Fecha", Fecha.Value.Date);
                    comandoInsertar.Parameters.AddWithValue("@Hora", hora);
                    comandoInsertar.Parameters.AddWithValue(
                        "@Motivo",
                        string.IsNullOrWhiteSpace(Motivo) ? (object)DBNull.Value : Motivo);

                    comandoInsertar.ExecuteNonQuery();
                }
            }

            MensajeExito = "Reserva registrada correctamente.";
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            AulaSeleccionada = null;
            Fecha = null;
            HoraTexto = string.Empty;
            Motivo = string.Empty;
        }
    }
}