using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Exceptions
{
    public class InsufficientStockException: Exception
    {
        public InsufficientStockException(string message) : base(message)
        {

        }

    }
}
