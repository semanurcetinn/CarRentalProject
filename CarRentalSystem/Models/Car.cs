using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Car
{
    public int CarId { get; set; }

    public string? CarPlate { get; set; }

    public string? CarBrand { get; set; }

    public string? CarModel { get; set; }

    public decimal CarDailyPrice { get; set; }

    public string? CarStatus { get; set; }

    public int FuelId { get; set; }

    public int GearId { get; set; }

    public int CategoryId { get; set; }

    public string? CarImageUrl { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual FuelsType Fuel { get; set; } = null!;

    public virtual GearType Gear { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
