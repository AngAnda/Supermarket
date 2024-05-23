using Supermarket.Business;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Supermarket.Views.Convertors
{
    public class IdToNameConvertor : IValueConverter
    {

        private readonly CategoryService categoryService;
        private readonly ProducersService producersService;

        public IdToNameConvertor()
        {
            categoryService = new CategoryService();
            producersService = new ProducersService();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id && parameter is string type)
            {
                switch (type)
                {
                    case "Category":
                        var category = categoryService.GetById(id);
                        return category?.CategoryName ?? "Unknown";
                    case "Producer":
                        var producer = producersService.GetById(id);
                        return producer?.ProducerName ?? "Unknown";
                }
            }
            return "Invalid Id";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
