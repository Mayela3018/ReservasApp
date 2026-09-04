using System.Windows;
using ReservasApp.Helpers;
using ReservasApp.Views;

namespace ReservasApp.ViewModels
{
    public class MenuPrincipalViewModel : ViewModelBase
    {
        public RelayCommand AbrirAulasDataTableCommand { get; }
        public RelayCommand AbrirAulasObjetosCommand { get; }
        public RelayCommand AbrirReservasDataTableCommand { get; }
        public RelayCommand AbrirReservasObjetosCommand { get; }
        public RelayCommand AbrirNuevaReservaCommand { get; }

        public MenuPrincipalViewModel()
        {
            AbrirAulasDataTableCommand = new RelayCommand(_ => new AulasDataTableView().Show());
            AbrirAulasObjetosCommand = new RelayCommand(_ => new AulasObjetosView().Show());
            AbrirReservasDataTableCommand = new RelayCommand(_ => new ReservasDataTableView().Show());
            AbrirReservasObjetosCommand = new RelayCommand(_ => new ReservasObjetosView().Show());
            AbrirNuevaReservaCommand = new RelayCommand(_ => new NuevaReservaView().Show());
        }
    }
}