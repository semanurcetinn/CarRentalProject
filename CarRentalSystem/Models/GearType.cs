using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class GearType
{
    public int GearId { get; set; }

    public string? GearName { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
