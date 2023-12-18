using System;
using System.Collections.Generic;

namespace eBuY.Models;

public partial class Categorie
{
    public int IdCategorie { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
}
