using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal TotalAmount { get; set; }

    public int UserId { get; set; }

    public int CarId { get; set; }

    public string? ReservationStatus { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
