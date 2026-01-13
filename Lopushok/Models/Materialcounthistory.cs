using System;
using System.Collections.Generic;

namespace Lopushok.Models;

public partial class Materialcounthistory
{
    public int Id { get; set; }

    public int Materialid { get; set; }

    public DateOnly Changedate { get; set; }

    public decimal Countvalue { get; set; }

    public virtual Material Material { get; set; } = null!;
}
