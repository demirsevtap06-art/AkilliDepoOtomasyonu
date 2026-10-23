using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
    public class PerishableProduct: Product
    {
        public DateTime ExpirationDate { get; set; }

        public PerishableProduct(int id, string name, Category category, Supplier supplier, decimal purchasePrice, decimal salePrice, int initialStock, int criticalLevel, DateTime expirationDate)
            : base(id, name, category, supplier, purchasePrice, salePrice, initialStock, criticalLevel)
        {

            ExpirationDate = expirationDate;
        }


        public override decimal GetFinalPrice()
        {
            if ((ExpirationDate - DateTime.Now).TotalDays <= 7)
                return SalePrice * 0.80m;
            return SalePrice;

        }
        public override string GetReport()
        {
            return $"[Bozulabilir #{Id}] {Name} | Stok: {Stock} | Fiyat: {GetFinalPrice():C2} | SKT: {ExpirationDate:yyyy-MM-dd}";
        }
    }

    }

