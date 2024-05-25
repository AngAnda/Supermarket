using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Supermarket.ViewModels
{
    public class UserValueViewModel : BaseViewModel
    {

        public class DailySale
        {
            public DailySale(string date, decimal value)
            {
                Date = date;
                Value = value;
            }

            public string Date { get; set; }

            public decimal Value { get; set; }

        }

        private ObservableCollection<User> users { get; set; }
        private ObservableCollection<string> months { get; set; }
        private ObservableCollection<DailySale> dailySales { get; set; }

        private int selectedId { get; set; }

        private string selectedMonth { get; set; }

        public RelayCommand GoBackCommand { get; private set; }

        public RelayCommand ListValueCommand { get; private set; }

        public int SelectedUser
        {
            get => selectedId;
            set
            {
                selectedId = value;
                OnPropertyChanged(nameof(selectedId));
                var dailySales = _userService.GetValueByUserIdAndMonth(SelectedUser, SelectedMonth);
                DailySales = new ObservableCollection<DailySale>(dailySales.Select(d => new DailySale(d.Date, d.TotalValue)).ToList());
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

        public ObservableCollection<string> Months
        {
            get => months;
            set
            {
                months = value;
                OnPropertyChanged(nameof(Months));
                var dailySales = _userService.GetValueByUserIdAndMonth(SelectedUser, SelectedMonth);
                DailySales = new ObservableCollection<DailySale>(dailySales.Select(d => new DailySale(d.Date, d.TotalValue)).ToList());

            }

        }


        public ObservableCollection<DailySale> DailySales
        {
            get => dailySales;
            set
            {
                dailySales = value;
                OnPropertyChanged(nameof(DailySales));
            }
        }

        public string SelectedMonth
        {
            get => selectedMonth;
            set
            {
                selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }

        private readonly UserService _userService;

        public UserValueViewModel()
        {
            _userService = new UserService();
            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Users")));
            ListValueCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("UserValue")));

            var res = _userService.GetValueByUserIdAndMonth(SelectedUser, SelectedMonth);

            Users = _userService.GetCashiers();
            Months = GenerateMonthCollection();
            var dailySales = _userService.GetValueByUserIdAndMonth(SelectedUser, SelectedMonth);
            DailySales = new ObservableCollection<DailySale>(dailySales.Select(d => new DailySale(d.Date, d.TotalValue)).ToList());
        }

        private ObservableCollection<string> GenerateMonthCollection()
        {
            var months = new ObservableCollection<string>();
            for (int month = 1; month <= 12; month++)
            {
                DateTime date = new DateTime(DateTime.Now.Year, month, 1);
                months.Add(date.ToString("MM - MMMM", CultureInfo.CurrentCulture));
            }
            return months;
        }
    }
}
