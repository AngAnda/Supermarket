using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Supermarket.Business
{
    public class StockService
    {

        private readonly SupermarketEntities _context;
        private const decimal vta = 24.0m;
        public StockService()
        {
            _context = new SupermarketEntities();
        }

        public void Add(Stock stock)
        {
            stock.IsEnabled = true;
            stock.StockSellingPrice = stock.StockPurchasePrice + (stock.StockPurchasePrice * vta / 100);
            _context.Stocks.Add(stock);
            _context.SaveChanges();
        }

        public void Update(Stock stock)
        {

            if (stock.StockSellingPrice < stock.StockPurchasePrice)
            {
                throw new InvalidStockPurchasePrice();
            }
            var stockToUpdate = _context.Stocks.First(s => s.StockId == stock.StockId);
            stockToUpdate.StockUnitOfMeasure = stock.StockUnitOfMeasure;
            stockToUpdate.StockSupplyDate = stock.StockSupplyDate;
            stockToUpdate.StockExpirationDate = stock.StockExpirationDate;
            stockToUpdate.StockSellingPrice = stock.StockSellingPrice;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            if (id < 0)
                throw new InvalidStockId();

            var stockToDelete = _context.Stocks.First(s => s.StockId == id);
            stockToDelete.IsEnabled = false;
            _context.SaveChanges();
        }

        public ObservableCollection<Stock> GetAll()
        {
            return new ObservableCollection<Stock>(_context.Stocks.Select(s => s).Where(s => s.IsEnabled == true));
        }

        public Stock GetById(int id)
        {
            if (id < 0)
                throw new InvalidStockId();

            return _context.Stocks.First(s => s.StockId == id);
        }
    }

    [Serializable]
    internal class InvalidStockId : Exception
    {
        public InvalidStockId()
        {
        }

        public InvalidStockId(string message) : base(message)
        {
        }

        public InvalidStockId(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStockId(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
