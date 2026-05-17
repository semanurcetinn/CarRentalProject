using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class FuelsType
{
    public int FuelId { get; set; }

    public string? FuelName { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
