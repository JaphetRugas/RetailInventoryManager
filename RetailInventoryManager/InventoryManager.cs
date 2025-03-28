using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetailInventoryManager.Models;
using Microsoft.EntityFrameworkCore;

namespace RetailInventoryManager
{
    public class InventoryManager
    {
        private readonly InventoryDbContext _context;

        public InventoryManager(InventoryDbContext context)
        {
            _context = context;
        }

        // Adds a new product to the inventory with validation
        public async Task<bool> AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (product.Price < 0)
            {
                Console.WriteLine("Price must be a non-negative real number.");
                return false;
            }

            if (product.QuantityInStock < 0)
            {
                Console.WriteLine("Quantity must be a non-negative integer.");
                return false;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        // Removes a product by ID
        public async Task<bool> RemoveProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        // Updates the quantity of an existing product
        public async Task<bool> UpdateProduct(int productId, int newQuantity)
        {

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return false;
            }

            if (newQuantity < 0)
            {
                Console.WriteLine("Quantity must be a non-negative integer.");
                return false;
            }

            product.QuantityInStock = newQuantity;
            await _context.SaveChangesAsync();
            return true;
        }

        // Retrieves all products in the inventory
        public async Task<List<Product>> ListProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // Calculates the total value of inventory
        public async Task<decimal> GetTotalValue()
        {
            return await _context.Products.SumAsync(p => p.QuantityInStock * p.Price);
        }
    }
}