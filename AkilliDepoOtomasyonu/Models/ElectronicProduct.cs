using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
   public class ElectronicProduct: Product
    {
        public int WarrantyMonths { get; set; }

        public ElectronicProduct(int id, string name, Category category, Supplier supplier, decimal purchasePrice, decimal salePrice, int initialStock, int criticalLevel, int warrantyMonths)
            : base(id, name, category, supplier, purchasePrice, salePrice, initialStock, criticalLevel)
        {
            WarrantyMonths = warrantyMonths;
        }
        public override string GetReport()
        {
            return $"[Elektronik #{Id}] {Name} | Stok: {Stock} | Fiyat: {GetFinalPrice():C2} | Garanti: {WarrantyMonths} Ay";
        }

        }
}
