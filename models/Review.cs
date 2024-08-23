using System;
using System.Collections.Generic;

namespace api.models;

public partial class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string? Alias { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool Deleted { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
