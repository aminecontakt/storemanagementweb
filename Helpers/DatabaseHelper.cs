using Npgsql;
using System;
using System.Collections.Generic;
using StoreManagementWeb.Models;

namespace StoreManagementWeb.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTableIfNotExists()
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS items (
                            id SERIAL PRIMARY KEY,
                            name VARCHAR(255) NOT NULL,
                            description TEXT,
                            price DECIMAL(10,2) NOT NULL,
                            quantity INT NOT NULL
                        );
                    ";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddItem(Item item)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO items (name, description, price, quantity) VALUES (@name, @description, @price, @quantity)";
                    cmd.Parameters.AddWithValue("name", item.Name);
                    cmd.Parameters.AddWithValue("description", item.Description);
                    cmd.Parameters.AddWithValue("price", item.Price);
                    cmd.Parameters.AddWithValue("quantity", item.Quantity);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Item> GetAllItems()
        {
            var items = new List<Item>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT id, name, description, price, quantity FROM items";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new Item
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Quantity = reader.GetInt32(4)
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public Item GetItemById(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT id, name, description, price, quantity FROM items WHERE id = @id";
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Item
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Quantity = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateItem(Item item)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE items SET name = @name, description = @description, price = @price, quantity = @quantity WHERE id = @id";
                    cmd.Parameters.AddWithValue("id", item.Id);
                    cmd.Parameters.AddWithValue("name", item.Name);
                    cmd.Parameters.AddWithValue("description", item.Description);
                    cmd.Parameters.AddWithValue("price", item.Price);
                    cmd.Parameters.AddWithValue("quantity", item.Quantity);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteItem(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM items WHERE id = @id";
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Item> SearchItems(string searchTerm)
        {
            var items = new List<Item>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT id, name, description, price, quantity FROM items WHERE name ILIKE @searchTerm OR description ILIKE @searchTerm";
                    cmd.Parameters.AddWithValue("searchTerm", "%" + searchTerm + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new Item
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Quantity = reader.GetInt32(4)
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }
    }
}
