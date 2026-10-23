using AkilliDepoOtomasyonu.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Models
{
    public class StockMovement: IEntity
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public User PerformedBy { get; set; }
        public StockMovementType Type { get; set; }

        public StockMovement(int id, Product product, int quantity, DateTime date, User performedBy, StockMovementType type)
        {
            Id = id;
            Product = product;
            Quantity = quantity;
            Date = date;
            PerformedBy = performedBy;
            Type = type;
        }

    }
}
