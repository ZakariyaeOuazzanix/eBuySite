using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace eBuY.Models;



public partial class Utilisateur
{
    public int IdUtilisateur { get; set; }

    public string? Nom { get; set; }

    public string? Prenom { get; set; }

    public string? Email { get; set; }

    public string? Mdp { get; set; }
    
    public string? NumTel { get; set; }

    // Ajouter l'attribut Role  1: Utilisateur Normal , 0 : Admin
    public bool? Role {  get; set; }
    public string? ImageProfil { get; set; }

    public bool? AppartientListeFavoris { get; set; }

    public bool? AppartientListeNoire { get; set; }

    public virtual ICollection<Administrateur> Administrateurs { get; set; } = new List<Administrateur>();

    public virtual ICollection<Commentaire> CommentaireAuteurNavigations { get; set; } = new List<Commentaire>();

    public virtual ICollection<Commentaire> CommentaireDestinataireNavigations { get; set; } = new List<Commentaire>();

    public virtual ICollection<Note> NoteAuteurNavigations { get; set; } = new List<Note>();

    public virtual ICollection<Note> NoteDestinataireNavigations { get; set; } = new List<Note>();

    public virtual ICollection<Panier> Paniers { get; set; } = new List<Panier>();

    public virtual ICollection<Probleme> Problemes { get; set; } = new List<Probleme>();

    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();

    
}
