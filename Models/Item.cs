using System;

namespace StoreManagementWeb.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Item() { }

        public Item(string name, string description, decimal price, int quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Description: {Description}, Price: {Price:C}, Quantity: {Quantity}";
        }
    }
}
