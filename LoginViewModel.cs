using GalaSoft.MvvmLight.Messaging;
using SupermarketManagementSystem.Business;
using System.Windows;
using System.Windows.Input;

namespace SupermarketManagementSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private readonly AuthenticateService _authenticateService;
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(AuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
            LoginCommand = new RelayCommand<object>(o =>
            {
                Role role = _authenticateService.Authenticate(Username, Password);
                if (role == Role.Admin)
                {
                    Messenger.Default.Send(new NotificationMessage("Admin"));
                }
                else if (role == Role.User)
                {
                    Messenger.Default.Send(new NotificationMessage("Cashier"));
                }
                else if (role == Role.None)
                {
                    MessageBox.Show("Invalid username or password");
                }


            });
        }
    }
}