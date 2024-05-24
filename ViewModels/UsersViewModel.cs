using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Windows;

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
                AddUserCommand.RaiseCanExecuteChanged();
                EditUserCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => password;

            set
            {
                password = value;
                OnPropertyChanged(nameof(password));
                AddUserCommand.RaiseCanExecuteChanged();
                EditUserCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsAdmin
        {
            get => isAdmin;

            set
            {
                isAdmin = value;
                OnPropertyChanged(nameof(isAdmin));
                AddUserCommand.RaiseCanExecuteChanged();
                EditUserCommand.RaiseCanExecuteChanged();

            }
        }



        private ObservableCollection<User> users;

        private UserService userService;

        private RelayCommand AddUsers;

        private RelayCommand EditUsers;

        private RelayCommand DeleteUsers;

        private RelayCommand RefreshFields;

        private RelayCommand GoBack;

        private User selectedUser;

        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                if (value != null)
                {
                    Username = SelectedUser.Username;
                    Password = SelectedUser.Password;
                    IsAdmin = SelectedUser.IsAdmin;
                }
                else
                    Username = Password = "";
                DeleteUserCommand.RaiseCanExecuteChanged();
                EditUserCommand.RaiseCanExecuteChanged();
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
            userService = new UserService();

            Users = userService.GetAll();

        }

        public RelayCommand AddUserCommand
        {
            get
            {
                return AddUsers ?? (AddUsers = new RelayCommand(() =>
                {
                    User user = new User() { Username = username, Password = password, IsAdmin = isAdmin };

                    userService.Add(user);
                    Users = userService.GetAll();

                }, CanAdd));


            }
        }

        public RelayCommand EditUserCommand
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

                }, CanEdit));
            }
        }

        public RelayCommand DeleteUserCommand
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

                }, CanDelete));
            }
        }

        public RelayCommand RefreshFieldsCommand
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

        public RelayCommand GoBackCommand
        {
            get
            {
                return GoBack ?? (GoBack = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("Admin"));
                }));
            }
        }


        public bool CanAdd()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                return false;
            return true;
        }

        public bool CanDelete()
        {
            return (SelectedUser != null);
        }

        public bool CanEdit()
        {
            return CanDelete() && CanAdd();
        }
    }
}
