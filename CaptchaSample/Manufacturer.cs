using System;
using System.Collections.Generic;

namespace CaptchaSample;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string ProductManufacturer { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
