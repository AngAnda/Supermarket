using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.Business
{
    public class ProducersService
    {

        SupermarketEntities _context;

        public ProducersService()
        {
            _context = new SupermarketEntities();
        }

        public void Add(Producer producer)
        {
            _context.spCreateProducer(producer.ProducerName, producer.ProducerCountry);
        }

        public void Update(Producer producer)
        {
            var producerFound = _context.Producers.First(p => p.ProducerId == producer.ProducerId);

            producerFound.ProducerName = producer.ProducerName;
            producerFound.ProducerCountry = producer.ProducerCountry;

            _context.SaveChanges();
        }

        public ObservableCollection<Producer> GetAll()
        {
            return new ObservableCollection<Producer>(_context.Producers.ToList());
        }

        public void Delete(int id)
        {
            _context.spDeleteProducer(id);
        }

    }
}
