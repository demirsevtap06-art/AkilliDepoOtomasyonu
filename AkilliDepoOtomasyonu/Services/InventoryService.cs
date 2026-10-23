using AkilliDepoOtomasyonu.Models;
using AkilliDepoOtomasyonu.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Services
{
   public class InventoryService
    {
        public Repository<Product> ProductRepo { get; } = new Repository<Product>();
        public Repository<Category> CategoryRepo { get; } = new Repository<Category>();
        public Repository<Supplier> SupplierRepo { get; } = new Repository<Supplier>();
        public Repository<StockMovement> MovementRepo { get; } = new Repository<StockMovement>();
        public Dictionary<Category, List<Product>> CategoryProductMap { get; private set; } = new Dictionary<Category, List<Product>>();

        private int _movementIdCounter = 1;
        public void AddProduct(Product product)
        {
            ProductRepo.Add(product);

            if (!CategoryProductMap.ContainsKey(product.Category))
            {
                CategoryProductMap[product.Category] = new List<Product>();
            }
            CategoryProductMap[product.Category].Add(product);
        }
        public bool RemoveProduct(int id)
        {
            var product = ProductRepo.GetById(id);
            if (product != null)
            {
                if (CategoryProductMap.ContainsKey(product.Category))
                {
                    CategoryProductMap[product.Category].Remove(product);
                }
                return ProductRepo.Remove(id);
            }
            return false;
        }
        public void ChangeProductCategory(Product product, Category newCategory)
        {
            if (CategoryProductMap.ContainsKey(product.Category))
            {
                CategoryProductMap[product.Category].Remove(product);
            }
            product.Category = newCategory;

            if (!CategoryProductMap.ContainsKey(newCategory))
            {
                CategoryProductMap[newCategory] = new List<Product>();
            }
            CategoryProductMap[newCategory].Add(product);
        }
        public List<Product> LinearSearchByName(string keyword)
        {
            List<Product> results = new List<Product>();
            var allProducts = ProductRepo.GetAll();
            for (int i = 0; i < allProducts.Count; i++)
            {
                if (allProducts[i].Name.ToLower().Contains(keyword.ToLower()))
                    results.Add(allProducts[i]);
            }
            return results;
        }

        // Binary Search (Elle Yazılan Arama Algoritması)[cite: 7]
        public Product BinarySearchById(int id)
        {
            var list = ProductRepo.GetAll().ToList();
            list.Sort((p1, p2) => p1.Id.CompareTo(p2.Id));

            int left = 0, right = list.Count - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (list[mid].Id == id) return list[mid];
                if (list[mid].Id < id) left = mid + 1;
                else right = mid - 1;
            }
            return null;
        }

        public void AddStock(Product product, int quantity, User user)
        {
            product.IncreaseStock(quantity);
            MovementRepo.Add(new StockMovement(_movementIdCounter++, product, quantity, DateTime.Now, user, StockMovementType.StockIn));
        }

        public void RemoveStock(Product product, int quantity, User user)
        {
            product.DecreaseStock(quantity); // Yetersiz stokta Exception fırlatır
            MovementRepo.Add(new StockMovement(_movementIdCounter++, product, quantity, DateTime.Now, user, StockMovementType.StockOut));
        }

        public void RecordSale(Product product, int quantity, User user)
        {
            product.DecreaseStock(quantity); // Yetersiz stokta Exception fırlatır[cite: 12]
            MovementRepo.Add(new StockMovement(_movementIdCounter++, product, quantity, DateTime.Now, user, StockMovementType.Sale));
        }
    }
}
    

