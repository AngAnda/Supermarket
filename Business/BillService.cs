using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.Business
{
    public class BillService
    {


        private readonly SupermarketEntities _context;
        private readonly BillProductService _billProductService;

        public BillService()
        {
            _context = new SupermarketEntities();
            _billProductService = new BillProductService();
        }

        public ObservableCollection<Bill> GetAll()
        {
            return new ObservableCollection<Bill>(_context.Bills.Select(b => b).ToList());
        }

        public void Add(ObservableCollection<BillProduct> receiptProducts, int userId)
        {
            decimal totalSum = 0M;

            foreach (var billProduct in receiptProducts)
            {
                totalSum += billProduct.Sum;
            }

            Bill bill = new Bill
            {
                BillDate = DateTime.Now,
                UserId = 1,
                BillSum = totalSum
            };

            _context.Bills.Add(bill);
            _context.SaveChanges();

            int billId = bill.BillId;

            foreach (var billProduct in receiptProducts)
            {
                totalSum += billProduct.Sum;
                billProduct.BillId = billId;
                _billProductService.Add(billProduct);
            }
        }
    }
}
