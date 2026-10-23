using AkilliDepoOtomasyonu.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
    public class Supplier: IEntity, IReportable
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Supplier(int id, string companyName, string contactName, string phone, string email)
        {
            Id = id;
            CompanyName = companyName;
            ContactName = contactName;
            Phone = phone;
            Email = email;
        }
        public string GetReport()
        {
            return $"[Tedarikçi #{Id}] Şirket: {CompanyName} | Yetkili: {ContactName} | Tel: {Phone}";
        }

    }
}
