using AkilliDepoOtomasyonu.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
    public class Category: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            if (obj is Category other) return Id == other.Id;
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => Name;

    }
}
