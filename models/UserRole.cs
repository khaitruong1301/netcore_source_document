using System;
using System.Collections.Generic;

namespace api.models;

public partial class UserRole
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? RoleId { get; set; }
}
