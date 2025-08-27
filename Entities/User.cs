using System;
using System.Collections.Generic;

namespace ASPNetProject.Entities;

public partial class User
{
    public int Uid { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Roleid
    { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Role? Role { get; set; }
}
