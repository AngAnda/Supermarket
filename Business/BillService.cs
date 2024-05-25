using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.Business
{
    public class BillService
    {
        private readonly SupermarketEntities _context;

        public BillService()
        {
            _context = new SupermarketEntities();
        }

        public void Add(Bill bill)
        {
            // to do
            //_context.spCreateBill(bill);
        }

        public ObservableCollection<Bill> GetAll()
        {
            return new ObservableCollection<Bill>(_context.Bills.Select(b => b).ToList());
        }

    }
}
