using System;
using System.Collections.Generic;

namespace api.models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Alias { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public int SellerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool Deleted { get; set; }

    public string? Image { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User Seller { get; set; } = null!;
}
