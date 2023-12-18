using System;
using System.Collections.Generic;

namespace eBuY.Models;

public partial class Administrateur
{
    public int IdAdmin { get; set; }

    public int? IdUtilisateur { get; set; }

    public virtual Utilisateur? IdUtilisateurNavigation { get; set; }
}
