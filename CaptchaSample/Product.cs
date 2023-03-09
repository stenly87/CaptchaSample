using System;
using System.Collections.Generic;

namespace CaptchaSample;

public partial class Product
{
    public string ProductArticleNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string PtoductUnit { get; set; } = null!;

    public string ProductCost { get; set; } = null!;

    public string ProductMaximumPossibleDiscount { get; set; } = null!;

    public int ProductManufacturerId { get; set; }

    public int ProductProviderId { get; set; }

    public int ProductCategoryId { get; set; }

    public string ProductDiscountAmount { get; set; } = null!;

    public string ProductQuantityInStock { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public byte[]? ProductPhoto { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual Category ProductCategory { get; set; } = null!;

    public virtual Manufacturer ProductManufacturer { get; set; } = null!;

    public virtual Provider ProductProvider { get; set; } = null!;
}
