using System.Windows;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        // Único código en el code-behind: PasswordBox no permite binding directo
        // por seguridad, así que se envía su valor como parámetro del comando.
        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)DataContext;

            if (viewModel.LoginCommand.CanExecute(TxtPassword.Password))
            {
                viewModel.LoginCommand.Execute(TxtPassword.Password);
            }
        }
    }
}