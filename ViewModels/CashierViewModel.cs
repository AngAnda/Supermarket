using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Supermarket.ViewModels
{
    public class CashierViewModel : BaseViewModel
    {
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly ProducersService producersService;
        private readonly BillProductService billProductService;
        private readonly BillService billService;

        private ObservableCollection<Product> filteredProducts;
        private ObservableCollection<BillProduct> receiptProducts;
        private ObservableCollection<Producer> producers;
        private ObservableCollection<Category> categories;
        private ObservableCollection<string> barcodes;

        private int selectedProduct;
        private int selectedCategory;
        private int selectedProducer;
        private int quantity;
        private string selectedBarcode;
        private DateTime expirationDate;
        private int userId;

        private Product selectedProductToAdd;
        public Product SelectedProductToAdd
        {
            get => selectedProductToAdd;
            set
            {
                selectedProductToAdd = value;
                OnPropertyChanged(nameof(SelectedProductToAdd));
            }
        }

        public CashierViewModel()
        {
            Messenger.Default.Register<GenericMessage<int>>(this, "CashierLogin", OnUserIdReceived);

            productService = new ProductService();
            categoryService = new CategoryService();
            producersService = new ProducersService();
            billProductService = new BillProductService();
            billService = new BillService();

            Producers = producersService.GetAll();
            Categories = categoryService.GetAll();
            Barcodes = productService.GetBarcodes();

            SelectedCategory = SelectedProducer = SelectedProduct = -1;
            ExpirationDate = DateTime.MinValue;
            SelectedBarcode = null;
            FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);
            ReceiptProducts = new ObservableCollection<BillProduct>();

            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Login")));
            AddProductCommand = new RelayCommand(AddProduct);
            GenerateReceiptCommand = new RelayCommand(GenerateReceipt);
        }

        private void GenerateReceipt()
        {
            billService.Add(ReceiptProducts, userId);
        }

        private void AddProduct()
        {
            try
            {
                billProductService.AddProduct(ReceiptProducts, SelectedProductToAdd, Quantity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand AddProductCommand { get; private set; }
        public RelayCommand GenerateReceiptCommand { get; private set; }

        public ObservableCollection<Producer> Producers
        {
            get => producers;
            set
            {
                producers = value;
                OnPropertyChanged(nameof(Producers));
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public ObservableCollection<string> Barcodes
        {
            get => barcodes;
            set
            {
                barcodes = value;
                OnPropertyChanged(nameof(Barcodes));
            }
        }

        public ObservableCollection<Product> FilteredProducts
        {
            get => filteredProducts;
            set
            {
                filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }

        public ObservableCollection<BillProduct> ReceiptProducts
        {
            get => receiptProducts;
            set
            {
                receiptProducts = value;
                OnPropertyChanged(nameof(ReceiptProducts));
            }
        }
        public int SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }

        public int SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }

        public int SelectedProducer
        {
            get => selectedProducer;
            set
            {
                selectedProducer = value;
                OnPropertyChanged(nameof(SelectedProducer));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }

        public string SelectedBarcode
        {
            get => selectedBarcode;
            set
            {
                selectedBarcode = value;
                OnPropertyChanged(nameof(SelectedBarcode));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }


        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public DateTime ExpirationDate
        {
            get => expirationDate;
            set
            {
                expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }

        private void OnUserIdReceived(GenericMessage<int> message)
        {
            userId = message.Content;
        }
    }
}

