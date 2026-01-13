using System;
using System.Collections.Generic;

namespace Lopushok.Models;

public partial class Material
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int Countinpack { get; set; }

    public string? Unit { get; set; }

    public decimal? Countinstock { get; set; }

    public decimal Mincount { get; set; }

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public string? Image { get; set; }

    public int Materialtypeid { get; set; }

    public virtual ICollection<Materialcounthistory> Materialcounthistories { get; set; } = new List<Materialcounthistory>();

    public virtual ICollection<Materialsupplier> Materialsuppliers { get; set; } = new List<Materialsupplier>();

    public virtual Materialtype Materialtype { get; set; } = null!;

    public virtual ICollection<Productmaterial> Productmaterials { get; set; } = new List<Productmaterial>();
}
