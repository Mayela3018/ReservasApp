using System.Collections.ObjectModel;
using System.Data.SqlClient;
using ReservasApp.Data;
using ReservasApp.Helpers;
using ReservasApp.Models;

namespace ReservasApp.ViewModels
{
    public class AulasObjetosViewModel : ViewModelBase
    {
        private ObservableCollection<Aula> _aulas;
        public ObservableCollection<Aula> Aulas
        {
            get => _aulas;
            set => SetProperty(ref _aulas, value);
        }

        private string _textoBusqueda;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set => SetProperty(ref _textoBusqueda, value);
        }

        public RelayCommand BuscarCommand { get; }

        public AulasObjetosViewModel()
        {
            BuscarCommand = new RelayCommand(_ => CargarAulas(TextoBusqueda));
            CargarAulas(null);
        }

        // (Conectado) — recorre un SqlDataReader fila por fila, con la conexión
        // abierta durante todo el recorrido, construyendo cada objeto Aula.
        // Cada búsqueda ejecuta una nueva consulta con SqlCommand/SqlDataReader.
        private void CargarAulas(string filtroNombre)
        {
            var lista = new ObservableCollection<Aula>();

            using (SqlConnection conexion = ConexionSQL.ObtenerConexion())
            {
                string query = "SELECT AulaId, Nombre, Capacidad FROM Aulas";

                if (!string.IsNullOrWhiteSpace(filtroNombre))
                    query += " WHERE Nombre LIKE @Filtro";

                query += " ORDER BY AulaId";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    if (!string.IsNullOrWhiteSpace(filtroNombre))
                        comando.Parameters.AddWithValue("@Filtro", "%" + filtroNombre + "%");

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

            Aulas = lista;
        }
    }
}