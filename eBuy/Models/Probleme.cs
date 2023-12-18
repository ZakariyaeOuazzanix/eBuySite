using System;
using System.Collections.Generic;

namespace eBuY.Models;

public partial class Probleme
{
    public int IdProbleme { get; set; }

    public string? Message { get; set; }

    public int? Auteur { get; set; }

    public string? Reponse { get; set; }

    public virtual Utilisateur? AuteurNavigation { get; set; }
}
