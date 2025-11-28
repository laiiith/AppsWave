using System.ComponentModel.DataAnnotations;

namespace AppsWave.DTO.Product;

public class UpsertProductDTO
{
    [Required(ErrorMessage = "English name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string EnglishName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Arabic name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string ArabicName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }
}