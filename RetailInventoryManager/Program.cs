using System.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailInventoryManager.Models;
using Microsoft.EntityFrameworkCore;

namespace RetailInventoryManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // Ensure proper Unicode output

            using var context = new InventoryDbContext();
            var inventoryManager = new InventoryManager(context);

            while (true)
            {
                Console.Clear();
                DisplayHeader();    // Display at the top of the menu
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Remove Product");
                Console.WriteLine("3. Update Product Quantity");
                Console.WriteLine("4. List Products");
                Console.WriteLine("5. Get Total Inventory Value");
                Console.WriteLine("6. Exit\n");
                Console.Write("Select an option: ");

                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int choice) || choice < 1 || choice > 6)
                {
                    Console.WriteLine($"\n'{input}' is not a valid option. Please enter a number between 1 and 6.");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    Console.Clear();
                    DisplayHeader();    // Display header for each operation

                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter Product Name: ");
                            string name = Console.ReadLine()?.Trim() ?? string.Empty;

                            if (string.IsNullOrWhiteSpace(name))
                            {
                                Console.WriteLine("Product name cannot be empty.");
                                break;
                            }

                            // Check if product already exists before adding
                            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == name);
                            if (existingProduct != null)
                            {
                                Console.WriteLine("A product with this name already exists. Try updating the quantity instead.");
                                break;
                            }

                            int quantity;
                            while (true)
                            {
                                Console.Write("Enter Quantity: ");
                                string quantityInput = Console.ReadLine()?.Trim() ?? string.Empty;

                                if (int.TryParse(quantityInput, out quantity) && quantity >= 0)
                                    break;

                                Console.WriteLine("Invalid input! Quantity must be a non-negative integer.");
                            }

                            decimal price;
                            while (true)
                            {
                                Console.Write("Enter Price: ");
                                string priceInput = Console.ReadLine()?.Trim() ?? string.Empty;

                                if (decimal.TryParse(priceInput, out price) && price >= 0)
                                    break;

                                Console.WriteLine("Invalid input! Price must be a non-negative real number.");
                            }

                            await inventoryManager.AddProduct(new Product { Name = name, QuantityInStock = quantity, Price = price });
                            Console.WriteLine("Product added successfully.");
                            break;

                        case 2:
                            Console.Write("Enter Product ID to remove: ");
                            if (!int.TryParse(Console.ReadLine(), out int removeId) || removeId <= 0)
                            {
                                Console.WriteLine("Invalid Product ID! Must be a positive integer.");
                                break;
                            }

                            // Check if product exists before removing
                            var productToRemove = await context.Products.FindAsync(removeId);
                            if (productToRemove == null)
                            {
                                Console.WriteLine("Product not found.");
                                break;
                            }

                            await inventoryManager.RemoveProduct(removeId);
                            Console.WriteLine($"Product '{productToRemove.Name}' removed successfully.");
                            break;

                        case 3:
                            Console.Write("Enter Product ID to update: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId) || updateId <= 0)
                            {
                                Console.WriteLine("Invalid Product ID! Must be a positive integer.");
                                break;
                            }

                            // Check if product exists before updating
                            var productToUpdate = await context.Products.FindAsync(updateId);
                            if (productToUpdate == null)
                            {
                                Console.WriteLine("Product not found.");
                                break;
                            }

                            Console.Write("Enter new Quantity: ");
                            if (!int.TryParse(Console.ReadLine(), out int newQuantity) || newQuantity < 0)
                            {
                                Console.WriteLine("Invalid input! Quantity must be a non-negative integer.");
                                break;
                            }

                            await inventoryManager.UpdateProduct(updateId, newQuantity);
                            Console.WriteLine($"Product '{productToUpdate.Name}' updated successfully.");
                            break;

                        case 4:
                            var products = await inventoryManager.ListProducts();
                            if (products.Count == 0)
                            {
                                Console.WriteLine("No products in inventory.");
                            }
                            else
                            {
                                Console.WriteLine("\n Product Inventory \n");
                                products.ForEach(p =>
                                    Console.WriteLine($"ID: {p.ProductId}, Name: {p.Name}, Quantity: {p.QuantityInStock}, Price: {FormatCurrency(p.Price)}")
                                );
                            }
                            break;

                        case 5:
                            decimal totalValue = await inventoryManager.GetTotalValue();
                            Console.WriteLine($"Total Inventory Value: {FormatCurrency(totalValue)}");
                            break;

                        case 6:
                            Console.WriteLine("Exiting...");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        private static void DisplayHeader()
        {
            Console.WriteLine("\n--------------------------------- Inventory Management System ---------------------------------\n");
        }

        // Formats the currency amount with the peso sign
        private static string FormatCurrency(decimal amount)
        {
            return $"\u20B1{amount:N2}"; // Ensures the peso sign is displayed correctly
        }
    }
}