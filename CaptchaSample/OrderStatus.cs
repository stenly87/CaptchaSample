using System;
using System.Collections.Generic;

namespace CaptchaSample;

public partial class OrderStatus
{
    public int StatusId { get; set; }

    public string OrderStatus1 { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
