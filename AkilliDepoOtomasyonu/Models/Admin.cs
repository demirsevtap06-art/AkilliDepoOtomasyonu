using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
    public class Admin:User
    {
        public Admin(int id, string username, string password, string fullName)
           : base(id, username, password, fullName) { }

        public override bool CanDeleteProduct() => true;
        public override bool CanManageSuppliers() => true;

    }
}
