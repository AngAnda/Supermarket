﻿using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;

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

            categoryFound.CategoryName = category.CategoryName;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.spDeleteCategory(id);
        }

        public ObservableCollection<Category> GetAll()
        {
            return new ObservableCollection<Category>(_context.Categories.Select(c => c).Where(c => c.IsEnabled == true).ToList());
        }
    }
}