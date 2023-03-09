using System;
using System.Collections.Generic;

namespace CaptchaSample;

public partial class Provider
{
    public int ProviderId { get; set; }

    public string ProductProvider { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
