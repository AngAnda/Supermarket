using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace Supermarket.Business
{
    public class ProductService
    {
        SupermarketEntities _context;

        public ProductService()
        {
            _context = new SupermarketEntities();
        }

        internal void AddProduct(Product newProduct)
        {
            _context.spCreateProduct(newProduct.ProductName, newProduct.Barcode, newProduct.CategoryId, newProduct.ProducerId);
        }

        internal void Delete(int productId)
        {
            _context.spDeleteProduct(productId);
        }

        internal ObservableCollection<Product> GetAll()
        {
            return new ObservableCollection<Product>(_context.Products.Select(p => p).Where(p => p.IsEnabled == true).ToList());
        }

        internal Product GetById(int productId)
        {
            return _context.Products.First(p => p.ProductId == productId);
        }

        internal void Update(object productToUpdate)
        {
            var foundProduct = _context.Products.First(p => p.ProductId == ((Product)productToUpdate).ProductId);
            if (foundProduct != null)
            {
                foundProduct.ProductName = ((Product)productToUpdate).ProductName;
                foundProduct.Barcode = ((Product)productToUpdate).Barcode;
                foundProduct.CategoryId = ((Product)productToUpdate).CategoryId;
                foundProduct.ProducerId = ((Product)productToUpdate).ProducerId;
                foundProduct.IsEnabled = ((Product)productToUpdate).IsEnabled;
            }
        }

        public ObservableCollection<Product> GetByProducer(int? producerId = null)
        {
            IQueryable<Product> query = _context.Products.Select(p => p).Where(p => p.IsEnabled == true);

            if (producerId.HasValue)
            {
                query = query.Where(p => p.ProducerId == producerId);
            }

            return new ObservableCollection<Product>(query.ToList());
        }

        internal ObservableCollection<string> GetBarcodes()
        {
            return new ObservableCollection<string>(_context.Products.Where(p => p.IsEnabled == true).Select(p => p.Barcode).ToList());
        }

        internal ObservableCollection<Product> GetFilteredProducts(int selectedProduct, int selectedCategory, int selectedProducer, string barcode, DateTime expirationDate)
        {
            IQueryable<Product> products = _context.Products.Where(p => p.IsEnabled);

            if (selectedProduct > 0)
            {
                products = products.Where(p => p.ProductId == selectedProduct);
            }

            if (selectedCategory > 0)
            {
                products = products.Where(p => p.CategoryId == selectedCategory);
            }

            if (selectedProducer > 0)
            {
                products = products.Where(p => p.ProducerId == selectedProducer);
            }

            if (expirationDate != DateTime.MinValue)
            {
                DateTime normalizedExpirationDate = expirationDate.Date;

                products = products.Where(p => p.Stocks.Any(s => DbFunctions.TruncateTime(s.StockExpirationDate) > normalizedExpirationDate && s.IsEnabled));
            }

            if (barcode != null)
            {
                products = products.Where(p => p.Barcode == barcode);
            }

            var productList = products.ToList();

            return new ObservableCollection<Product>(productList);
        }

        public bool CanAddProduct(Product selectedProductToAdd, int quantity)
        {
            var quantityInStock = selectedProductToAdd.Stocks.Where(s => s.IsEnabled).Sum(s => s.StockQuantity);
            return selectedProductToAdd != null && quantity > 0 && quantityInStock >= quantity;
        }


    }
}