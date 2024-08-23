using System;
using System.Collections.Generic;

namespace api.models;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Alias { get; set; }

    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Deleted { get; set; }

    public virtual User User { get; set; } = null!;
}
