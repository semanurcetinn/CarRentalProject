using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Explanation { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
