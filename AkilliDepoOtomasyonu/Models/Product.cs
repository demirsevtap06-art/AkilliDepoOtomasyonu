using AkilliDepoOtomasyonu.Exceptions;
using AkilliDepoOtomasyonu.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
    public abstract class Product: IEntity, IReportable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }

        private int _stock;
        public int Stock => _stock;
        public int CriticalLevel { get; set; }
        public bool IsCritical => _stock <= CriticalLevel;

        public Product(int id, string name, Category category, Supplier supplier, decimal purchasePrice, decimal salePrice, int initialStock, int criticalLevel)
        {

            Id = id;
            Name = name;
            Category = category;
            Supplier = supplier;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            _stock = initialStock < 0 ? 0 : initialStock;
            CriticalLevel = criticalLevel;
        }

        public virtual decimal GetFinalPrice() => SalePrice;
        public abstract string GetReport();

        public void IncreaseStock(int amount)
        {

            if (amount > 0) _stock += amount;
        }

        public void DecreaseStock(int amount)
        {

            if (amount <= 0) return;
            if (_stock - amount < 0)
            {

                throw new InsufficientStockException($"Stok yetersiz! Mevcut Stok: {_stock}, İstenen: {amount}");
            }

            _stock -= amount;
        }
    }
}
