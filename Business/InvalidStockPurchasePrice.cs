using System;

namespace Supermarket.Business
{
    [Serializable]
    internal class InvalidStockPurchasePrice : Exception
    {
        private static string message = "The selling price cannot be lower than the purchase price";

        public InvalidStockPurchasePrice() : base(message)
        {
        }

    }
}