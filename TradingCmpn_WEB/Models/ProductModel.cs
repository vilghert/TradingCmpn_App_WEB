using System.ComponentModel.DataAnnotations;

namespace TradingCmpn_WEB.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна повинна бути більшою за 0.")]
        public decimal Price { get; set; }

    }
}