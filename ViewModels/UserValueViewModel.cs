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

        private int selectedUser { get; set; }

        private string selectedMonth { get; set; }

        public RelayCommand GoBackCommand { get; private set; }

        public RelayCommand ListValueCommand { get; private set; }

        public int SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(selectedUser));
                LoadDailySalesData();
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
                LoadDailySalesData();
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
                LoadDailySalesData();
            }
        }

        private readonly UserService _userService;

        public UserValueViewModel()
        {
            _userService = new UserService();
            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Users")));
            ListValueCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("UserValue")));


            Users = _userService.GetCashiers();
            Months = GenerateMonthCollection();
            LoadDailySalesData();
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

        private void LoadDailySalesData()
        {
            if (!string.IsNullOrEmpty(SelectedMonth) && SelectedUser > 0)
            {
                // Parse the month part from "MM - MMMM"
                int monthNumber = int.Parse(SelectedMonth.Substring(0, 2));
                var year = DateTime.Now.Year; // Or a selected year if applicable

                var dailySales = _userService.GetValueByUserIdAndMonth(SelectedUser, year, monthNumber);
                DailySales = new ObservableCollection<DailySale>(dailySales.Select(d => new DailySale(d.Date, d.TotalValue)).ToList());
            }
        }
    }
}
