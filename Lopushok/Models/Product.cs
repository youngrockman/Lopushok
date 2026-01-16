using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lopushok.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? Producttypeid { get; set; }

    public string? Articlenumber { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public Bitmap ParseImage 
    { 
        get
        {
            if (string.IsNullOrEmpty(Image))
            {
                return new Bitmap("picture.png");
            }
            else 
            { 
                return new Bitmap(Image); 
            }

               
        } 
    }


    public string MaterialsList
    {
        get
        {
            if (Productmaterials == null || !Productmaterials.Any())
                return "Материалы не указаны";

            return string.Join(", ", Productmaterials.Where(x => x.Material != null).Select(x => $"{x.Material.Title} ({x.Count} шт.)"));
        }
    }


    public decimal TotalCost
    {
        get
        {
            using var context = new DemoContext();

            
            var productMaterials = context.Productmaterials.Where(x => x.Productid == this.Id).ToList();

            decimal total = 0;

            foreach (var pm in productMaterials)
            {
               
                var material = context.Materials.FirstOrDefault(x => x.Id == pm.Materialid);

                if (material != null && pm.Count != null)
                {
                    total += material.Cost * pm.Count.Value;
                }
            }

            return total;
        }
    }

    public int? Productionpersoncount { get; set; }

    public int? Productionworkshopnumber { get; set; }

    public decimal Mincostforagent { get; set; }

    public virtual ICollection<Productcosthistory> Productcosthistories { get; set; } = new List<Productcosthistory>();

    public virtual ICollection<Productmaterial> Productmaterials { get; set; } = new List<Productmaterial>();

    public virtual ICollection<Productsale> Productsales { get; set; } = new List<Productsale>();

    public virtual Producttype? Producttype { get; set; }
}
