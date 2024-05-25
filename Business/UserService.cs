using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Supermarket.Business
{
    public class UserService
    {
        private readonly SupermarketEntities _context;

        public UserService()
        {
            _context = new SupermarketEntities();
        }

        public void Add(User user)
        {
            try
            {
                _context.spCreateUser(user.Username, user.Password, user.IsAdmin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Update(User user)
        {
            try
            {
                var foundUser = _context.Users.First(u => u.UserId == user.UserId);
                if (foundUser != null)
                {
                    foundUser.Username = user.Username;
                    foundUser.Password = user.Password;
                    foundUser.IsAdmin = user.IsAdmin;
                }
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }


        public void Delete(int id)
        {
            try
            {
                _context.spDeleteUserById(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<User> GetAll()
        {
            return new ObservableCollection<User>(_context.Users.Select(u => u).Where(u => u.IsEnabled == true).ToList());
        }

        internal ObservableCollection<User> GetCashiers()
        {
            return new ObservableCollection<User>(_context.Users.Select(u => u).Where(u => u.IsEnabled == true && u.IsAdmin == false).ToList());
        }

        public ObservableCollection<(string Date, decimal TotalValue)> GetValueByUserIdAndMonth(int userId, string month)
        {
            // Presupunem că month este în formatul "YYYY-MM"
            if (userId == null || month == null)
                return new ObservableCollection<(string Date, decimal TotalValue)>();

            var monthNumber = int.Parse(month.Substring(5, 2));

            var result = _context.Bills
                .Where(b => b.UserId == userId &&
                            b.BillDate.Year == DateTime.Now.Year &&
                            b.BillDate.Month == monthNumber)
                .GroupBy(b => b.BillDate.Date)
                .Select(g => new { Date = g.Key.ToString("yyyy-MM-dd"), TotalValue = g.Sum(b => b.BillSum) })
                .ToList()
                .Select(x => (x.Date, x.TotalValue))
                .ToList();

            return new ObservableCollection<(string Date, decimal TotalValue)>(result);
        }
    }
}
