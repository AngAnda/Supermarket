using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly LoginService _loginService;

        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            _loginService = new LoginService();
        }

        void Login()
        {
            LoginService.UserType userType = _loginService.Login(Username, Password);

            if (userType == LoginService.UserType.NONE)
            {
                MessageBox.Show("Invalid username or password");
                return;
            }
            else if (userType == LoginService.UserType.CASHIER)
            {

                Messenger.Default.Send(new GenericMessage<int>(_loginService.GetID(Username, Password)), "CashierLogin");
                MessageBox.Show("Cashier");
                LoginService.CashierId = _loginService.GetID(Username, Password);
                Messenger.Default.Send(new NotificationMessage("Cashier"));

            }
            else if (userType == LoginService.UserType.ADMIN)
            {
                MessageBox.Show("Admin");
                Messenger.Default.Send(new NotificationMessage("Admin"));
            }
        }
    }
}
