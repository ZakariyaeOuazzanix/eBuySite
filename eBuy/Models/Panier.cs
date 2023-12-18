using System;
using System.Collections.Generic;

namespace eBuY.Models;

public partial class Panier
{
    public int IdPanier { get; set; }

    public int? Utilisateur { get; set; }

    public int? Produit { get; set; }

    public virtual Produit? ProduitNavigation { get; set; }

    public virtual Utilisateur? UtilisateurNavigation { get; set; }
}
