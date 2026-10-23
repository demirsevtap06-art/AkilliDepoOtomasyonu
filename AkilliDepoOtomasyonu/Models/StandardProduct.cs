using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
   public class StandardProduct: Product
    {
        public StandardProduct(int id, string name, Category category, Supplier supplier, decimal purchasePrice, decimal salePrice, int initialStock, int criticalLevel)
            : base(id, name, category, supplier, purchasePrice, salePrice, initialStock, criticalLevel)
        {

        }
        public override string GetReport()
        {
            return $"[Standart #{Id}] {Name} | Stok: {Stock} | Fiyat: {GetFinalPrice():C2}";
        }

    }
}
