using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
            }
        }

        public string Country
        {
            get => country;
            set
            {
                country = value;
                OnPropertyChanged(nameof(Country));
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

        public ICommand AddProducer;
        public ICommand EditProducer;
        public ICommand DeleteProducer;
        public ICommand SaveProducer;
        public ICommand RefreshFields;
        public ICommand GoBack;

        public ProducerViewModel()
        {
            producersService = new ProducersService();
            Producers = producersService.GetAll();
        }

        public ICommand AddProducerCommand
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
                }));
            }

        }

        public ICommand EditProducerCommand
        {
            get
            {
                return EditProducer ?? (EditProducer = new RelayCommand(() =>
                {
                    producersService.Update(SelectedProducer);
                    Producers = producersService.GetAll();
                    Name = "";
                    Country = "";
                }));
            }
        }

        public ICommand DeleteProducerCommand
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
                }));
            }
        }

        public ICommand RefreshFieldsCommand
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

        public ICommand GoBackCommand
        {
            get
            {
                return GoBack ?? (GoBack = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("Admin"));
                }));
            }
        }

    }
}