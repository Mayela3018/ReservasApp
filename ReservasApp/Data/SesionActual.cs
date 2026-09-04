using ReservasApp.Models;

namespace ReservasApp.Data
{
    // Guarda el usuario que inició sesión, para usarlo en el resto de la app
    // (por ejemplo, para saber quién registra una nueva reserva).
    public static class SesionActual
    {
        public static Usuario UsuarioActual { get; set; }
    }
}