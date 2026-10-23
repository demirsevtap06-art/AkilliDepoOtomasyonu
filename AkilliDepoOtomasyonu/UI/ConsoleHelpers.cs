using AkilliDepoOtomasyonu.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.UI
{
    public static class ConsoleHelpers
    {
        public static int ReadInt(string message, int min, int max)
        {
            int result;
            while (true)
            {
                Console.Write($"{message}: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out result) && result >= min && result <= max)
                {
                    return result;
                }
                Console.WriteLine($"[HATA] Lütfen {min} ile {max} arasında geçerli bir sayı giriniz.");
            }
        }
        public static decimal ReadDecimal(string message)
        {
            decimal result;
            while (true)
            {
                Console.Write($"{message}: ");
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out result) && result >= 0)
                {
                    return result;
                }
                Console.WriteLine("[HATA] Lütfen pozitif geçerli bir tutar giriniz.");
            }
        }
        public static void PrintProductTable(IEnumerable<Product> products)
        {
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"{"Id",-5} {"Ürün",-22} {"Kategori",-12} {"Stok",-6} {"Kritik",-8} {"Tedarikçi",-18}");
            Console.WriteLine(new string('-', 85));
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id,-5} {p.Name,-22} {p.Category.Name,-12} {p.Stock,-6} {p.CriticalLevel,-8} {p.Supplier.CompanyName,-18}");
            }
            Console.WriteLine(new string('-', 85));
        }
    } }

    

