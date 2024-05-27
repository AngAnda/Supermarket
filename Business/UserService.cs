using Supermarket.DataAccess;
using System;
using System.Collections.Generic;
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

        public IEnumerable<(string Date, decimal TotalValue)> GetValueByUserIdAndMonth(int userId, int year, int month)
        {
            var query = _context.Bills
                .Where(b => b.UserId == userId && b.BillDate.Year == year && b.BillDate.Month == month)
                .GroupBy(b => new { Year = b.BillDate.Year, Month = b.BillDate.Month, Day = b.BillDate.Day })
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Day, TotalValue = g.Sum(b => b.BillSum) })
                .ToList();

            var result = query.Select(x =>
                (Date: new DateTime(x.Year, x.Month, x.Day).ToString("yyyy-MM-dd"), x.TotalValue));

            return result;
        }
    }
}
