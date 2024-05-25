using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
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
            return new ObservableCollection<string>(_context.Products.Select(p => p.Barcode).ToList());
        }

        internal ObservableCollection<Product> GetFilteredProducts(int? selectedProduct = null, int? selectedCategory = null, int? selectedProducer = null, DateTime? expirationDate = null)
        {
            // to fully implemente

            IQueryable<Product> products = _context.Products.Select(p => p).Where(p => p.IsEnabled == true);

            if (selectedProduct.HasValue)
            {
                products = products.Where(p => p.ProductId == selectedProduct.Value);
            }

            if (selectedCategory.HasValue)
            {
                products = products.Where(p => p.CategoryId == selectedCategory.Value);
            }

            if (selectedProducer.HasValue)
            {
                products = products.Where(p => p.ProducerId == selectedProducer.Value);
            }

            if (expirationDate.HasValue)
            {
                // aici are treaba cu stocurile, de vazut
            }

            return new ObservableCollection<Product>(products);
        }
    }
}