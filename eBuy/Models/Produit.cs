using System;
using System.Collections.Generic;

namespace eBuY.Models;

public partial class Produit
{
    public int IdProduit { get; set; }

    public string? Nom { get; set; }

    public string? Prix { get; set; }
    
    public int? Vendeur { get; set; }

    public string? Image { get; set; }

    public int? Categorie { get; set; }

    public string? Description { get; set; }
    public virtual Categorie? CategorieNavigation { get; set; }

    public virtual ICollection<Panier> Paniers { get; set; } = new List<Panier>();

    public virtual Utilisateur? VendeurNavigation { get; set; }
}
