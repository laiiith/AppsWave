using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppsWave.DTO.Invoice;

public class PurchaseRequestDTO
{
    public List<PurchaseItemDTO> Items { get; set; } = new();
}

public class PurchaseItemDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
