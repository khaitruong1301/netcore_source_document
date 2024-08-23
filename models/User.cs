using System;
using System.Collections.Generic;

namespace api.models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Alias { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool Deleted { get; set; }

    public string? Phone { get; set; }

    public string? FullName { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
