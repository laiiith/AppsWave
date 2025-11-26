using System;
using System.Collections.Generic;
using System.Text;

namespace AppsWave.Entites;

public class Invoice
{
    public string? Date { get; set; }
    public User? User { get; set; }
    public List<Product>? Products{ get; set; }
    public double TotalAmount { get; set; }
}
