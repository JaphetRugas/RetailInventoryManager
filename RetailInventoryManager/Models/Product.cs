using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailInventoryManager.Models
{
    public class Product
    {
        [Key]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive integer.")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative integer.")]
        public int QuantityInStock { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number.")]
        public decimal Price { get; set; }
    }
}