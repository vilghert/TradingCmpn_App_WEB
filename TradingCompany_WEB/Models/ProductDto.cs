using System.ComponentModel.DataAnnotations;

namespace TradingCompany_WEB.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}