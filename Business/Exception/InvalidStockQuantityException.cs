using System;

namespace Supermarket.Business
{
    [Serializable]
    internal class InvalidStockQuantityException : Exception
    {
        const string message = "The quantity of the product is not available in stock";
        public InvalidStockQuantityException() : base(message)
        {
        }
    }
}