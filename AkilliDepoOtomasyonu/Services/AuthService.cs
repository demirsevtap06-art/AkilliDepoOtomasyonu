using AkilliDepoOtomasyonu.Models;
using AkilliDepoOtomasyonu.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Services
{
    public class AuthService
    {
        private readonly Repository<User> _userRepo;
        public User CurrentUser { get; private set; }

        public AuthService(Repository<User> userRepo)
        {
            _userRepo = userRepo;
        }
        public bool Login(string username, string password)
        {
            foreach (var user in _userRepo.GetAll())
            {
                if (user.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase) && user.CheckPassword(password))
                {
                    CurrentUser = user;
                    return true;
                }
            }
            return false;
        }
        public void Logout() => CurrentUser = null;
    }
}
