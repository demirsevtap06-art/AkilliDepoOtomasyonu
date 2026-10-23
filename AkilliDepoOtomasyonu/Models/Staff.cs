using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
   public class Staff:User
    {
        public Staff(int id, string username, string password, string fullName)
            : base(id, username, password, fullName) { }

        public override bool CanDeleteProduct() => false;
        public override bool CanManageSuppliers() => false;

    }
}
