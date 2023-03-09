using System;
using System.Collections.Generic;

namespace CaptchaSample;

public partial class Category
{
    public int CategoryId { get; set; }

    public string ProductCategory { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
