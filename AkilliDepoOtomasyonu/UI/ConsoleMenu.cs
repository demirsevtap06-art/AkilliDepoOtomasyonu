using AkilliDepoOtomasyonu.Exceptions;
using AkilliDepoOtomasyonu.Models;
using AkilliDepoOtomasyonu.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.UI
{
   public class ConsoleMenu
    {
        private readonly AuthService _authService;
        private readonly InventoryService _inventoryService;
        private readonly ReportService _reportService;

        public ConsoleMenu(AuthService authService, InventoryService inventoryService, ReportService reportService)
        {
            _authService = authService;
            _inventoryService = inventoryService;
            _reportService = reportService;
        }
        public void Run()
        {
            Console.WriteLine("===== AKILLI DEPO SİSTEMİ =====");

            bool loggedIn = false;
            int loginAttempts = 0;

            while (!loggedIn && loginAttempts < 3)
            {
                Console.Write("Kullanıcı adı: ");
                string username = Console.ReadLine();
                Console.Write("Şifre: ");
                string password = Console.ReadLine();

                if (_authService.Login(username, password))
                {
                    loggedIn = true;
                    var roleText = _authService.CurrentUser is Admin ? "Yönetici" : "Personel";
                    Console.WriteLine($"Hoş geldiniz, {_authService.CurrentUser.FullName} ({roleText})\n"); 
                }
                else
                {
                    loginAttempts++;
                    Console.WriteLine($"\n[HATA] Geçersiz kullanıcı adı veya şifre! (Kalan Hak: {3 - loginAttempts})\n");
                }
            }
            if (!loggedIn)
            {
                Console.WriteLine("3 kez hatalı giriş yapıldı. Program kapatılıyor.");
                return;
            }
            while (true)
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine("1) Ürün İşlemleri");
                Console.WriteLine("2) Kategori / Tedarikçi İşlemleri");
                Console.WriteLine("3) Arama ve Filtreleme");
                Console.WriteLine("4) Stok Hareketleri");
                Console.WriteLine("5) Satış Yap");
                Console.WriteLine("6) Raporlar");
                Console.WriteLine("7) Oturumu Kapat");
                Console.WriteLine("0) Çıkış");

                int choice = ConsoleHelpers.ReadInt("Seçiminiz", 0, 7);

                try
                {
                    switch (choice)
                    {
                        case 1: ProductMenu(); break;
                        case 2: CategorySupplierMenu(); break;
                        case 3: SearchMenu(); break;
                        case 4: StockMovementMenu(); break;
                        case 5: SalesMenu(); break;
                        case 6: ShowCriticalReportFormatted(); break; // Örnek Konsol Çıktısındaki Rapor Ekranı[cite: 13]
                        case 7:
                            _authService.Logout();
                            Console.WriteLine("Oturum kapatıldı. Program sonlandırılıyor.");
                            return;
                        case 0: return;
                    }
                
                }
                catch (InsufficientStockException ex) // Görseldeki Özel Exception Catch Yapısı[cite: 12]
                {
                    Console.WriteLine($"\n[STOK HATASI] {ex.Message}\n");
                }
                catch (Exception ex) // Genel çökme engelleme[cite: 12]
                {
                    Console.WriteLine($"\n[SİSTEM HATASI] {ex.Message}\n");
                }
            }

        }
        private void ShowCriticalReportFormatted()
        {
            Console.WriteLine("\n===== KRİTİK STOK RAPORU =====");
            var criticals = _reportService.GetCriticalStockReport();

            Console.WriteLine($"{"Id",-4} {"Ürün",-21} {"Kategori",-10} {"Stok",-5} {"Kritik",-7} {"Tedarikçi",-15}");
            Console.WriteLine("------------------------------------------------------------------");

            foreach (var p in criticals)
            {
                Console.WriteLine($"{p.Id,-4} {p.Name,-21} {p.Category.Name,-10} {p.Stock,-5} {p.CriticalLevel,-7} {p.Supplier.CompanyName,-15}");

            }
            Console.WriteLine($"\nToplam {criticals.Count} ürün kritik seviyede.\n");
        }
        private void ProductMenu()
        {
            Console.WriteLine("\n--- Ürün İşlemleri ---");
            Console.WriteLine("1. Tüm Ürünleri Listele");
            Console.WriteLine("2. Yeni Ürün Ekle");
            Console.WriteLine("3. Ürün Sil (Yalnız Admin)");

            int c = ConsoleHelpers.ReadInt("Seçiminiz", 1, 3);
            if (c == 1)
            {
                ConsoleHelpers.PrintProductTable(_inventoryService.ProductRepo.GetAll());
            }
            else if (c == 2)
            {
                Console.Write("Ürün Adı: ");
                string name = Console.ReadLine();
                var cat = _inventoryService.CategoryRepo.GetById(1);
                var sup = _inventoryService.SupplierRepo.GetById(1);
                int id = _inventoryService.ProductRepo.Count + 100;

                var newProduct = new StandardProduct(id, name, cat, sup, 50m, 100m, 20, 5);
                _inventoryService.AddProduct(newProduct); // Dinamik sözlüğü de günceller[cite: 11]
                Console.WriteLine("Ürün başarıyla eklendi.");
            }
            else if (c == 3)
            {
                if (!_authService.CurrentUser.CanDeleteProduct())
                {
                    Console.WriteLine("\n[YETKİ HATASI] Bu işlem için yetkiniz yok!\n");
                    return;
                }
                int deleteId = ConsoleHelpers.ReadInt("Silinecek Ürün ID", 1, 9999);
                if (_inventoryService.RemoveProduct(deleteId)) // Dinamik sözlükten de siler[cite: 11]
                    Console.WriteLine("Ürün silindi.");
                else
                    Console.WriteLine("Ürün bulunamadı.");
            }
        }
        private void CategorySupplierMenu()
        {
            Console.WriteLine("\n--- Kategoriler ve Canlı Sözlük Takibi ---");
            foreach (var kvp in _inventoryService.CategoryProductMap)
            {
                Console.WriteLine($"Kategori: {kvp.Key.Name} | Ürün Sayısı: {kvp.Value.Count}");
            }
        }
        private void SearchMenu()
        {
            Console.WriteLine("\n1. Linear Search (Ad ile)");
            Console.WriteLine("2. Binary Search (ID ile)");
            int c = ConsoleHelpers.ReadInt("Seçim", 1, 2);
            if (c == 1)
            {
                Console.Write("Aranacak İsim: ");
                var res = _inventoryService.LinearSearchByName(Console.ReadLine());
                ConsoleHelpers.PrintProductTable(res);
            }
            else
            {
                int id = ConsoleHelpers.ReadInt("Aranacak ID", 1, 9999);
                var res = _inventoryService.BinarySearchById(id);
                if (res != null) ConsoleHelpers.PrintProductTable(new List<Product> { res });
                else Console.WriteLine("Bulunamadı.");
            
        }
        }
        private void StockMovementMenu()
        {
            int pId = ConsoleHelpers.ReadInt("Ürün ID", 1, 9999);
            var p = _inventoryService.ProductRepo.GetById(pId);
            if (p != null)
            {
                int qty = ConsoleHelpers.ReadInt("Düşülecek Miktar", 1, 500);
                _inventoryService.RemoveStock(p, qty, _authService.CurrentUser); // Exception fırlatabilir[cite: 12]
                Console.WriteLine("Stok güncellendi.");
            }
        }
        private void SalesMenu()
        {
            int pId = ConsoleHelpers.ReadInt("Satılacak Ürün ID", 1, 9999);
            var p = _inventoryService.ProductRepo.GetById(pId);
            if (p != null)
            {
                int qty = ConsoleHelpers.ReadInt("Satış Adedi", 1, 500);
                _inventoryService.RecordSale(p, qty, _authService.CurrentUser); // Exception fırlatabilir[cite: 12]
                Console.WriteLine($"Satış Yapıldı! Toplam Tutar: {p.GetFinalPrice() * qty:C2}");
            }
        }
        }

        }

    

