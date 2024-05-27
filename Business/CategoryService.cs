using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Supermarket.Business
{
    public class CategoryService
    {

        public readonly SupermarketEntities _context;

        public CategoryService()
        {
            _context = new SupermarketEntities();
        }

        public void Add(Category category)
        {
            _context.spCreateCategory(category.CategoryName);
        }

        public void Edit(Category category)
        {
            var categoryFound = _context.Categories.First(c => c.CategoryId == category.CategoryId);

            //categoryFound.CategoryName = category.CategoryName;

            //_context.SaveChanges();

            _context.UpdateCategory(categoryFound.CategoryId, categoryFound.CategoryName);
        }

        public void Delete(int id)
        {
            if (id < 0)
                throw new InvalidCategoryId();
            _context.spDeleteCategory(id);
        }

        public ObservableCollection<Category> GetAll()
        {
            return new ObservableCollection<Category>(_context.Categories.Select(c => c).Where(c => c.IsEnabled == true).ToList());
        }

        public Category GetById(int id)
        {
            if (id < 0)
                throw new InvalidCategoryId();

            return _context.Categories.First(c => c.CategoryId == id);
        }

        public ObservableCollection<(string Name, decimal Value)> GetTotalValues()
        {
            var categorySales = _context.Categories
                .Select(c =>
                new
                {
                    CategoryName = c.CategoryName,
                    TotalSales = c.Products
                          .SelectMany(p => p.Stocks)
                          .Sum(p => (decimal?)p.StockSellingPrice) ?? 0M

                })
                .ToList()
                .Select(c => (c.CategoryName, c.TotalSales))
                .ToList();
            return new ObservableCollection<(string Name, decimal Value)>(categorySales);
        }
    }

    [Serializable]
    internal class InvalidCategoryId : Exception
    {
        public InvalidCategoryId()
        {
        }

        public InvalidCategoryId(string message) : base(message)
        {
        }

        public InvalidCategoryId(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCategoryId(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
