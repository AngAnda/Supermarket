using GalaSoft.MvvmLight.Command;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {

        private string username;
        private string password;
        private bool isAdmin;

        public string Username
        {
            get => username;

            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => password;

            set
            {
                password = value;
                OnPropertyChanged(nameof(password));
            }
        }

        public bool IsAdmin
        {
            get => isAdmin;

            set
            {
                isAdmin = value;
                OnPropertyChanged(nameof(isAdmin));
            }
        }



        private ObservableCollection<User> users;

        private SupermarketEntities supermarketEntities;

        private UserService userService;

        private ICommand AddUsers;

        private ICommand EditUsers;

        private ICommand DeleteUsers;

        private ICommand RefreshFields;

        private User selectedUser;

        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                Username = SelectedUser.Username;
                Password = SelectedUser.Password;
                IsAdmin = SelectedUser.IsAdmin;
            }
        }

        public ObservableCollection<User> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public UsersViewModel()
        {
            supermarketEntities = new SupermarketEntities();
            userService = new UserService();

            Users = userService.GetAll();

            //Users = supermarketEntities.Users.ToList();
        }

        public ICommand AddUserCommand
        {
            get
            {
                return AddUsers ?? (AddUsers = new RelayCommand(() =>
                {
                    User user = new User() { Username = username, Password = password, IsAdmin = isAdmin };

                    userService.Add(user);

                    Users.Add(user);
                    supermarketEntities.Users.Add(user);
                }));


            }
        }

        public ICommand EditUserCommand
        {
            get
            {
                return EditUsers ?? (EditUsers = new RelayCommand(() =>
                {
                    User user = new User()
                    {
                        UserId = selectedUser.UserId,
                        Username = username,
                        Password = password,
                        IsAdmin = isAdmin
                    };
                    userService.Update(user);

                    Users = userService.GetAll();

                }));
            }
        }

        public ICommand DeleteUserCommand
        {
            get
            {
                return DeleteUsers ?? (DeleteUsers = new RelayCommand(() =>
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to delete this user?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        userService.Delete(SelectedUser.UserId);
                        Users = userService.GetAll();
                    }

                }));
            }
        }

        public ICommand RefreshFieldsCommand
        {
            get
            {
                return RefreshFields ?? (RefreshFields = new RelayCommand(() =>
                {
                    Username = "";
                    Password = "";
                    IsAdmin = false;
                }));
            }
        }

    }
}
