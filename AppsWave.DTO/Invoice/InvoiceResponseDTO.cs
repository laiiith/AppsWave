namespace AppsWave.DTO.Invoice;

public class InvoiceResponseDTO
{
    public int InvoiceId { get; set; }
    public DateTime Date { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<InvoiceItemDTO> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
}

public class InvoiceItemDTO
{
    public string ProductEnglishName { get; set; } = string.Empty;
    public string ProductArabicName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => Price * Quantity;
}
