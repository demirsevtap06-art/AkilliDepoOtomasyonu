using AkilliDepoOtomasyonu.Models;
using AkilliDepoOtomasyonu.Repositories;
using AkilliDepoOtomasyonu.Services;
using AkilliDepoOtomasyonu.UI;

Repository<User> userRepo = new Repository<User>();
AuthService authService = new AuthService(userRepo);
InventoryService inventoryService = new InventoryService();
ReportService reportService = new ReportService(inventoryService);
    SeedData(userRepo, inventoryService);
ConsoleMenu menu = new ConsoleMenu(authService, inventoryService, reportService);
menu.Run();

static void SeedData(Repository<User> userRepo, InventoryService inv)
{
    userRepo.Add(new Admin(1, "admin", "1234", "Ayşe Yılmaz"));
    userRepo.Add(new Staff(2, "personel", "1234", "Mehmet Demir"));

    // Kategoriler
    var catKirtasiye = new Category(1, "Kırtasiye", "Ofis Malzemeleri");
    var catTemizlik = new Category(2, "Temizlik", "Hijyen Malzemeleri");
    var catElektronik = new Category(3, "Elektronik", "Cihaz ve Kablolar");
    var catGida = new Category(4, "Gıda", "Gıda Malzemeleri");

    inv.CategoryRepo.Add(catKirtasiye);
    inv.CategoryRepo.Add(catTemizlik);
    inv.CategoryRepo.Add(catElektronik);
    inv.CategoryRepo.Add(catGida);

    // Tedarikçiler
    var supKagit = new Supplier(10, "Kağıt A.Ş.", "Ahmet Bey", "05320000000", "info@kagit.com");
    var supTemiz = new Supplier(15, "Temiz Ltd.", "Veli Bey", "05330000000", "info@temiz.com");
    var supTekno = new Supplier(8, "Tekno Dağıtım", "Selin Hanım", "05340000000", "info@tekno.com");

    inv.SupplierRepo.Add(supKagit);
    inv.SupplierRepo.Add(supTemiz);
    inv.SupplierRepo.Add(supTekno);

    // Örnek Ekran Çıktısında (Resim 3) Birebir Yer Alan Ürünler[cite: 13]:
    // 104 - A4 Fotokopi Kağıdı (Kırtasiye, Stok: 3, Kritik: 10, Kağıt A.Ş.)[cite: 13]
    // 211 - Bulaşık Deterjanı (Temizlik, Stok: 5, Kritik: 15, Temiz Ltd.)[cite: 13]
    // 305 - USB-C Kablo (Elektronik, Stok: 2, Kritik: 8, Tekno Dağıtım)[cite: 13]

    inv.AddProduct(new StandardProduct(104, "A4 Fotokopi Kağıdı", catKirtasiye, supKagit, 80m, 120m, 3, 10)); // Kritik[cite: 13]
    inv.AddProduct(new StandardProduct(211, "Bulaşık Deterjanı", catTemizlik, supTemiz, 30m, 50m, 5, 15));   // Kritik[cite: 13]
    inv.AddProduct(new ElectronicProduct(305, "USB-C Kablo", catElektronik, supTekno, 40m, 90m, 2, 8, 12));  // Kritik[cite: 13]

    // Tamamlayıcı Diğer Ürünler
    inv.AddProduct(new StandardProduct(101, "Tükenmez Kalem", catKirtasiye, supKagit, 10m, 20m, 50, 5));
    inv.AddProduct(new ElectronicProduct(102, "Kablosuz Mouse", catElektronik, supTekno, 200m, 350m, 20, 5, 24));
}

