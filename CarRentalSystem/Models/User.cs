using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserSurname { get; set; }

    public string? UserEmail { get; set; }

    public int? UserPassword { get; set; }

    public int UserLicenseNo { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Role Role { get; set; } = null!;
}
