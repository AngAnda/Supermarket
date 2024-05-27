using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.Business
{
    public class BillProductService
    {
        private readonly SupermarketEntities _context;
        public BillProductService()
        {
            _context = new SupermarketEntities();
        }

        public void Add(BillProduct billProduct)
        {
            _context.BillProducts.Add(billProduct);
            _context.SaveChanges();

            _context.Entry(billProduct).Reference(bp => bp.Product).Load();
            _context.Entry(billProduct.Product).Collection(p => p.Stocks).Load();

            var stock = billProduct.Product.Stocks.Where(s => s.IsEnabled == true && s.StockQuantity != 0).FirstOrDefault();
            if (stock != null)
            {
                stock.StockQuantity -= billProduct.Quantity;
                if (stock.StockQuantity == 0)
                {
                    stock.IsEnabled = false;
                }

                _context.SaveChanges();

            }
            else
            {
                throw new InvalidOperationException("No stock available for this product.");
            }
        }

        public void AddProduct(ObservableCollection<BillProduct> billProducts, Product product, int quantity)
        {

            ValidateStock(billProducts, product, quantity);

            decimal price = product.Stocks.Select(s => s.StockSellingPrice).FirstOrDefault();

            BillProduct billProduct = new BillProduct
            {
                ProductId = product.ProductId,
                Quantity = quantity,
                Sum = quantity * price
            };

            if (billProducts.Any(b => b.ProductId == product.ProductId))
            {
                var existingBillProduct = billProducts.First(b => b.ProductId == product.ProductId);
                existingBillProduct.Quantity += quantity;
                existingBillProduct.Sum += quantity * price;
                return;
            }

            billProducts.Add(billProduct);
        }


        private void ValidateStock(ObservableCollection<BillProduct> billProducts, Product product, int quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidStockQuantityException();
            }


            int quantityBill = billProducts
                .Where(b => b.ProductId == product.ProductId)
                .Sum(b => b.Quantity);
            int stockAmount = product.Stocks.Where(s => s.IsEnabled == true && s.StockQuantity != 0).Select(s => s.StockQuantity).First();

            if (quantityBill + quantity > stockAmount)
                throw new ExceedingQuantityException();

        }

        public ObservableCollection<BillProduct> GetById(int id)
        {
            return new ObservableCollection<BillProduct>(_context.BillProducts.Where(b => b.BillId == id).Select(b => b).ToList());
        }
    }
}
