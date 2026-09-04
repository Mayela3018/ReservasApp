using System.Data;
using System.Data.SqlClient;
using ReservasApp.Data;
using ReservasApp.Helpers;

namespace ReservasApp.ViewModels
{
    public class AulasDataTableViewModel : ViewModelBase
    {
        private DataView _aulas;
        public DataView Aulas
        {
            get => _aulas;
            set => SetProperty(ref _aulas, value);
        }

        public RelayCommand RecargarCommand { get; }

        public AulasDataTableViewModel()
        {
            RecargarCommand = new RelayCommand(_ => CargarAulas());
            CargarAulas();
        }

        private void CargarAulas()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = ConexionSQL.ObtenerConexion())
            {
                string query = "SELECT AulaId, Nombre, Capacidad FROM Aulas ORDER BY AulaId";
                SqlDataAdapter adaptador = new SqlDataAdapter(query, conexion);

                adaptador.Fill(tabla);
            }

            Aulas = tabla.DefaultView;
        }
    }
}