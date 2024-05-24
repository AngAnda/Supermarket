using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels
{
    public class ProducerViewModel : BaseViewModel
    {
        private readonly ProducersService producersService;
        public ObservableCollection<Producer> producers;
        private Producer _selectedProducer;
        private string name;
        private string country;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
                AddProducerCommand.RaiseCanExecuteChanged();
                EditProducerCommand.RaiseCanExecuteChanged();
            }
        }

        public string Country
        {
            get => country;
            set
            {
                country = value;
                OnPropertyChanged(nameof(Country));
                AddProducerCommand.RaiseCanExecuteChanged();
                EditProducerCommand.RaiseCanExecuteChanged();
            }
        }

        public Producer SelectedProducer
        {
            get => _selectedProducer;
            set
            {
                if (_selectedProducer != value)
                {
                    _selectedProducer = value;
                    OnPropertyChanged(nameof(SelectedProducer));
                    Name = SelectedProducer.ProducerName;
                    Country = SelectedProducer.ProducerCountry;
                    DeleteProducerCommand.RaiseCanExecuteChanged();
                    EditProducerCommand.RaiseCanExecuteChanged();
                }
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

        public RelayCommand AddProducer;
        public RelayCommand EditProducer;
        public RelayCommand DeleteProducer;
        public RelayCommand SaveProducer;
        public RelayCommand RefreshFields;
        public RelayCommand ListProducts;
        public RelayCommand GoBack;

        public ProducerViewModel()
        {
            producersService = new ProducersService();
            Producers = producersService.GetAll();
        }

        public RelayCommand AddProducerCommand
        {
            get
            {
                return AddProducer ?? (AddProducer = new RelayCommand(() =>
                {
                    Producer newProducer = new Producer() { ProducerName = Name, ProducerCountry = Country };

                    producersService.Add(newProducer);
                    SelectedProducer = newProducer;
                    Producers = producersService.GetAll();
                    Name = "";
                    Country = "";
                }, CanAdd));
            }

        }

        public RelayCommand EditProducerCommand
        {
            get
            {
                return EditProducer ?? (EditProducer = new RelayCommand(() =>
                {
                    producersService.Update(SelectedProducer);
                    Producers = producersService.GetAll();
                    Name = "";
                    Country = "";
                }, CanEdit));
            }
        }

        public RelayCommand DeleteProducerCommand
        {
            get
            {
                return DeleteProducer ?? (DeleteProducer = new RelayCommand(() =>
                {
                    if (SelectedProducer != null)
                    {
                        producersService.Delete(SelectedProducer.ProducerId);
                        SelectedProducer = null;
                        Producers = producersService.GetAll();
                        Name = "";
                        Country = "";
                    }
                }, CanDelete));
            }
        }

        public RelayCommand RefreshFieldsCommand
        {
            get
            {
                return RefreshFields ?? (RefreshFields = new RelayCommand(() =>
                {
                    SelectedProducer = null;
                    Name = "";
                    Country = "";
                }));
            }
        }

        public RelayCommand GoBackCommand
        {
            get
            {
                return GoBack ?? (GoBack = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("Admin"));
                }));
            }
        }

        public RelayCommand ListProductsCommand
        {
            get
            {
                return ListProducts ?? (ListProducts = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("ProductProducers"));
                }));
            }
        }

        public bool CanAdd()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Country))
                return false;
            return true;
        }

        public bool CanEdit()
        {
            return CanAdd() && CanDelete();
        }

        public bool CanDelete()
        {
            return (SelectedProducer != null);
        }

    }
}