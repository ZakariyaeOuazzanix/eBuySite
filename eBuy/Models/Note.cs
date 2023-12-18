using System;
using System.Collections.Generic;

namespace eBuY.Models;

public partial class Note
{
    public int IdNote { get; set; }

    public int? Valeur { get; set; }

    public int? Auteur { get; set; }

    public int? Destinataire { get; set; }

    public virtual Utilisateur? AuteurNavigation { get; set; }

    public virtual Utilisateur? DestinataireNavigation { get; set; }
}
