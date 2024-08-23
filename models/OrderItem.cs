using System;
using System.Collections.Generic;

namespace api.models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? Alias { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool Deleted { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
