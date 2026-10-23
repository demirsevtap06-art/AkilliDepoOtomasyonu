using AkilliDepoOtomasyonu.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Services
{
    public class ReportService
    {
        private readonly InventoryService _inventoryService;

        public ReportService(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // 1. LINQ: Where[cite: 11]
        public List<Product> GetCriticalStockReport()
        {
            return _inventoryService.ProductRepo.GetAll()
                .Where(p => p.IsCritical)
                .ToList();
        }

        // 2. LINQ: Sum
        public decimal GetTotalInventoryValue()
        {
            return _inventoryService.ProductRepo.GetAll()
                .Sum(p => p.Stock * p.PurchasePrice);
        }

        // 3 & 4 & 5. LINQ: GroupBy, OrderByDescending, Take[cite: 11, 12]
        public List<ProductSalesInfo> GetTop5SellingProducts()
        {
            return _inventoryService.MovementRepo.GetAll()
                .Where(m => m.Type == StockMovementType.Sale)
                .GroupBy(m => m.Product)
                .Select(g => new ProductSalesInfo
                {
                    Product = g.Key,
                    TotalQuantitySold = g.Sum(m => m.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(5)
                .ToList();
        }

        // 6 & 7. LINQ: GroupBy + Count (Tedarikçi bazlı ürün sayısı)[cite: 11, 12]
        public Dictionary<string, int> GetProductCountBySupplier()
        {
            return _inventoryService.ProductRepo.GetAll()
                .GroupBy(p => p.Supplier.CompanyName)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // 8. LINQ: Average (Ortalama Ürün Satış Fiyatı)[cite: 11]
        public decimal GetAverageProductPrice()
        {
            var products = _inventoryService.ProductRepo.GetAll();
            return products.Any() ? products.Average(p => p.SalePrice) : 0m; // 9. LINQ: Any[cite: 11]
        }

        // 10. LINQ: Join (Ürünler ve Tedarikçiler birleştirmesi)[cite: 11]
        public List<string> GetProductSupplierJoin()
        {
            return _inventoryService.ProductRepo.GetAll()
                .Join(_inventoryService.SupplierRepo.GetAll(),
                      p => p.Supplier.Id,
                      s => s.Id,
                      (p, s) => $"Ürün: {p.Name} -> Tedarikçi Şirket: {s.CompanyName}")
                .ToList();
        }

        // 11. LINQ: FirstOrDefault[cite: 11]
        public Product GetMostExpensiveProduct()
        {
            return _inventoryService.ProductRepo.GetAll()
          .OrderByDescending(p => p.SalePrice)
                .FirstOrDefault();
        }
    }

    public class ProductSalesInfo
    {
        public Product Product { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
    

