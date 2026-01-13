using System;
using System.Collections.Generic;

namespace Lopushok.Models;

public partial class Productmaterial
{
    public int Productmaterialid { get; set; }

    public int Productid { get; set; }

    public int Materialid { get; set; }

    public decimal? Count { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
