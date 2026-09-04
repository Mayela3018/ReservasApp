using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReservasApp.Helpers
{
    // Clase base para todos los ViewModels.
    // Implementa INotifyPropertyChanged para que la UI se actualice
    // automáticamente cuando cambian los datos (data binding).
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Método de ayuda: asigna el valor y notifica el cambio solo si realmente cambió.
        protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string propertyName = null)
        {
            if (Equals(campo, valor))
                return false;

            campo = valor;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}