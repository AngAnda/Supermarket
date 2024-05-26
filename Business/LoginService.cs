using Supermarket.DataAccess;
using System.Linq;

namespace Supermarket.Business
{
    public class LoginService
    {
        public enum UserType
        {
            NONE,
            ADMIN,
            CASHIER
        }

        private readonly SupermarketEntities _context;

        public LoginService()
        {
            _context = new SupermarketEntities();
        }

        public UserType Login(string username, string password)
        {
            var result = _context.spGetUserByCredentials(username, password).FirstOrDefault();
            if (result == null)
            {
                return UserType.NONE;
            }

            else if (result.IsAdmin)
            {
                return UserType.ADMIN;
            }
            else
            {
                return UserType.CASHIER;
            }
        }

        public int GetID(string username, string password)
        {
            var result = _context.spGetUserByCredentials(username, password).FirstOrDefault();

            if (result != null)
            {
                return result.UserId;
            }

            return -1;
        }
    }
}
