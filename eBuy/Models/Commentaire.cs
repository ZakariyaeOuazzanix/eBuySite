using System;
using System.Collections.Generic;

namespace eBuY.Models;

public partial class Commentaire
{
    public int IdCommentaire { get; set; }

    public string? Texte { get; set; }

    public int? Destinataire { get; set; }

    public int? Auteur { get; set; }

    public virtual Utilisateur? AuteurNavigation { get; set; }

    public virtual Utilisateur? DestinataireNavigation { get; set; }
}
