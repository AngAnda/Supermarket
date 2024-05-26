using System;

namespace Supermarket.Business
{
    [Serializable]
    internal class ExceedingQuantityException : Exception
    {

        private static string message = "The inserted quantity exceeds the quantity in stock";
        public ExceedingQuantityException() : base(message)
        {
        }

    }
}