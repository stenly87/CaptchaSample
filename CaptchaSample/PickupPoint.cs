using System;
using System.Collections.Generic;

namespace CaptchaSample;

public partial class PickupPoint
{
    public int PickupPointId { get; set; }

    public string Index { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string? HomeSNumber { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
