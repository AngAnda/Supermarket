﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Windows;

namespace Supermarket.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private SupermarketEntities supermarketEntities;
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly ProducersService producerService;

        private int productId;
        private string productName;
        private string barcode;
        private int categoryId;
        private int producerId;
        private bool isEnabled;
        private ObservableCollection<Category> categories;
        private ObservableCollection<Producer> producers;

        public int ProductId
        {
            get => productId;
            set
            {
                productId = value;
                OnPropertyChanged(nameof(ProductId));
                AddProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string ProductName
        {
            get => productName;
            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));

                AddProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string Barcode
        {
            get => barcode;
            set
            {
                barcode = value;
                OnPropertyChanged(nameof(Barcode));

                AddProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        public int CategoryId
        {
            get => categoryId;
            set
            {
                categoryId = value;
                OnPropertyChanged(nameof(CategoryId));

                AddProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        public int ProducerId
        {
            get => producerId;
            set
            {
                producerId = value;
                OnPropertyChanged(nameof(ProducerId));
                AddProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));

                AddProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                if (selectedProduct != null)
                {
                    ProductId = selectedProduct.ProductId;
                    ProductName = selectedProduct.ProductName;
                    Barcode = selectedProduct.Barcode;
                    CategoryId = selectedProduct.CategoryId;
                    ProducerId = selectedProduct.ProducerId;
                    IsEnabled = selectedProduct.IsEnabled;

                    DeleteProductCommand.RaiseCanExecuteChanged();
                    EditProductCommand.RaiseCanExecuteChanged();
                }
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

        public ObservableCollection<Producer> Producers
        {
            get => producers;
            set
            {
                producers = value;
                OnPropertyChanged(nameof(Producers));
            }
        }


        public RelayCommand AddProductCommand { get; private set; }
        public RelayCommand EditProductCommand { get; private set; }
        public RelayCommand DeleteProductCommand { get; private set; }
        public RelayCommand RefreshFieldsCommand { get; private set; }
        public RelayCommand GoBackCommand { get; private set; }

        public ProductViewModel()
        {
            supermarketEntities = new SupermarketEntities();
            productService = new ProductService();
            categoryService = new CategoryService();
            producerService = new ProducersService();

            Products = productService.GetAll();
            Categories = categoryService.GetAll();
            Producers = producerService.GetAll();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddProductCommand = new RelayCommand(AddProduct, CanAdd);
            EditProductCommand = new RelayCommand(EditProduct, CanEdit);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDelete);
            RefreshFieldsCommand = new RelayCommand(RefreshFields);
            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Admin")));
        }

        private void AddProduct()
        {
            var newProduct = new Product
            {
                ProductName = ProductName,
                Barcode = Barcode,
                CategoryId = CategoryId,
                ProducerId = ProducerId,
                IsEnabled = IsEnabled
            };
            productService.AddProduct(newProduct);
            Products = productService.GetAll();
        }

        private void EditProduct()
        {
            Product productToUpdate = productService.GetById(ProductId);
            if (productToUpdate != null)
            {
                productToUpdate.ProductName = ProductName;
                productToUpdate.Barcode = Barcode;
                productToUpdate.CategoryId = CategoryId;
                productToUpdate.ProducerId = ProducerId;
                productToUpdate.IsEnabled = IsEnabled;
                productService.Update(productToUpdate);
                Products = productService.GetAll();
            }
        }

        private void DeleteProduct()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete this product?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                productService.Delete(SelectedProduct.ProductId);
                Products = productService.GetAll();
            }
        }

        private void RefreshFields()
        {
            ProductName = "";
            Barcode = "";
            CategoryId = 0;
            ProducerId = 0;
            IsEnabled = false;
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(ProductName) &&
                   !string.IsNullOrWhiteSpace(Barcode) &&
                   CategoryId > 0 &&
                   ProducerId > 0;
        }

        private bool CanDelete()
        {
            return SelectedProduct != null;
        }

        private bool CanEdit()
        {
            return CanAdd() && CanDelete();
        }
    }
}