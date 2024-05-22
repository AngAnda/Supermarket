using Supermarket.DataAccess;
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
    }
}