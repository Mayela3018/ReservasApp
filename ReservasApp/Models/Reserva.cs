using System;

namespace ReservasApp.Models
{
    public class Reserva
    {
        public int ReservaId { get; set; }
        public int AulaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Motivo { get; set; }

        // Campos extra solo para mostrar en el DataGrid (no existen en la tabla,
        // se llenan con un JOIN al consultar).
        public string NombreAula { get; set; }
        public string NombreUsuario { get; set; }
    }
}