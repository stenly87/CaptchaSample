using System;
using System.Collections.Generic;

namespace CaptchaSample;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDates { get; set; }

    public DateOnly OrderDeliveryDate { get; set; }

    public int OrderPickupPointId { get; set; }

    public int? UserId { get; set; }

    public string OrderCode { get; set; } = null!;

    public int OrderStatusId { get; set; }

    public virtual PickupPoint OrderPickupPoint { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual User? User { get; set; }
}
