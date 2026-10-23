using AkilliDepoOtomasyonu.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
    public abstract class User: IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        private string _password; // Encapsulation
        public string FullName { get; set; }

        public User(int id, string username, string password, string fullName)
        {
            Id = id;
            Username = username;
            _password = password;
            FullName = fullName;
        }

        public bool CheckPassword(string input) => _password == input;
        public abstract bool CanDeleteProduct();
        public abstract bool CanManageSuppliers();

    }
}
