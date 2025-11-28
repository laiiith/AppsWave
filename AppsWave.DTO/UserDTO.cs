using System.ComponentModel.DataAnnotations;

namespace AppsWave.DTO;

public class UserDTO
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}
public class ProductDTO
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "English name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string EnglishName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Arabic name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string ArabicName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0, 1000, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
}


public class UpsertProductDTO
{
    [Required(ErrorMessage = "English name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string EnglishName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Arabic name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string ArabicName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0, 1000, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
}