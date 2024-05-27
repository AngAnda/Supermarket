using Supermarket.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
            return new ObservableCollection<Bill>(_context.Bills.Where(b => b.BillSum != 0).Select(b => b).OrderByDescending(b => b.BillId).ToList());
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
                UserId = userId,
                BillSum = totalSum
            };

            _context.Bills.Add(bill);
            _context.SaveChanges();

            int billId = bill.BillId;

            foreach (var billProduct in receiptProducts)
            {
                totalSum += billProduct.Sum;

                var newBillProduct = new BillProduct
                {
                    BillId = billId,
                    ProductId = billProduct.ProductId,
                    Quantity = billProduct.Quantity,
                    Sum = billProduct.Sum
                };

                _billProductService.Add(newBillProduct);
            }
        }

        internal ObservableCollection<Bill> GetGreatestBill(DateTime date)
        {
            date = date.Date;
            var bill = _context.Bills
                               .Include("BillProducts")
                               .Include("BillProducts.Product") // Include detalii despre produse
                               .Where(b => DbFunctions.TruncateTime(b.BillDate) == date) // Folosește DbFunctions.TruncateTime pentru a ignora ora
                               .OrderByDescending(b => b.BillSum) // Ordonează facturile descrescător după suma totală
                               .FirstOrDefault();

            return new ObservableCollection<Bill>(bill != null ? new List<Bill> { bill } : new List<Bill>());

        }
    }
}
