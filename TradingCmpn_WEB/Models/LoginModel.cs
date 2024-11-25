using System.ComponentModel.DataAnnotations;

namespace TradingCmpn_WEB.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Password must be at least 2 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
