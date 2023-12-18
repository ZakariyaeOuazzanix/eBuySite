using eBuY.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace eBuY.Controllers
{
    public class eBuyController : Controller
    {
        public IActionResult PostProduct()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }
        //IL FAUT DEVELOPPER LA METHODE AFFICHERINFOS DE PRODUIT ??
        public IActionResult ProfilChange()
        {
            return View();
        }


        // Cette méthode vérifie si l'email et le mot de passe correspondent à un utilisateur dans la table Utilisateur
        // Si oui, elle crée une identité et une session pour l'utilisateur et renvoie true
        // Sinon, elle renvoie false
        public bool seConnecter(string email, string mdp)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher l'utilisateur dans la table Utilisateur avec l'email et le mdp donnés
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.Email == email && u.Mdp == mdp);

                // Si l'utilisateur existe
                if (utilisateur != null)
                {
                    // Créer un tableau de revendications avec le nom et le rôle de l'utilisateur
                    var claims = new[]
                    {
                new Claim(ClaimTypes.Name, utilisateur.Nom),
                new Claim(ClaimTypes.Role, utilisateur.Role.ToString())
            };

                    // Créer une identité avec les revendications et le schéma d'authentification par cookie
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Créer une session pour l'utilisateur avec l'identité
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    // Renvoyer true pour indiquer que la connexion a réussi
                    return true;
                }
                else
                {
                    // Renvoyer false pour indiquer que la connexion a échoué
                    return false;
                }
            }
        }

        // Cette méthode termine la session de l'utilisateur et renvoie à la page de connexion
        public void seDeconnecter()
        {
            // Terminer la session de l'utilisateur
             HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Rediriger vers la page de connexion
             RedirectToAction("Login", "Account");//Action login du controlleur account 
            
        }

        public void acheterProduit(Produit produit)
        {
            if (produit == null) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher tous les paniers qui contiennent le produit
                var paniers = context.Paniers.Where(p => p.Produit == produit.IdProduit);
                // Supprimer tous les paniers trouvés de la base de données
                context.Paniers.RemoveRange(paniers);
                // Enregistrer les changements
                context.SaveChanges();
            }
        }


        public List<Produit> RechercherProduit(string nom)
        {
            using (var context = new EBuyDatabaseContext())
            {
                // Créer une liste vide pour stocker les produits trouvés
                List<Produit> resultats = new List<Produit>();

                // Parcourir tous les produits de la base de données
                foreach (Produit produit in context.Produits)
                {
                    // Si le nom du produit contient le nom recherché
                    if (produit.Nom.Contains(nom))
                    {
                        // Ajouter le produit à la liste des résultats
                        resultats.Add(produit);
                    }
                }

                // Renvoyer la liste des résultats
                return resultats;
            }
        }

        public List<Produit> rechercherProduitParCategorie(Categorie categorie)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher tous les produits qui ont la clé étrangère de la catégorie donnée
                var produits = context.Produits.Where(p => p.Categorie == categorie.IdCategorie);
                // Convertir le résultat en une liste de produits
                var liste = produits.ToList();
                // Renvoyer la liste
                return liste;
            }
        }

        /*public void ajouterProduitAuPanier(Produit produit)
        {
            // Vérifier si le produit et l'utilisateur sont valides
            if (produit == null || this == null) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Créer une nouvelle instance de la classe Panier avec les clés étrangères
                var panier = new Panier
                {
                    Utilisateur = this.IdUtilisateur,
                    Produit = produit.IdProduit
                };
                // Ajouter le panier à la base de données
                context.Paniers.Add(panier);
                // Enregistrer les changements
                context.SaveChanges();
            }
        }*/

        /*public void removeFromPanier(Produit produit)
        {
            // Vérifier si le produit est valide
            if (produit == null) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher le panier qui contient le produit et l'utilisateur
                var panier = context.Paniers.FirstOrDefault(p => p.Produit == produit.IdProduit && p.Utilisateur == this.IdUtilisateur);
                // Si le panier existe
                if (panier != null)
                {
                    // Supprimer le panier de la base de données
                    context.Paniers.Remove(panier);
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }
        */
        /*public void envoyerMessageProbleme(string message)
        {
            // Vérifier si le message est valide
            if (string.IsNullOrEmpty(message)) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Créer une nouvelle instance de la classe Probleme avec le message et l'auteur
                var probleme = new Probleme
                {
                    Message = message,
                    Auteur = this.IdUtilisateur
                };
                // Ajouter le probleme à la base de données
                context.Problemes.Add(probleme);
                // Enregistrer les changements
                context.SaveChanges();
            }
        }*/

        public List<Probleme> rechercherProblemeEtReponse(string motCle)
        {
            // Créer une liste vide pour stocker les problèmes trouvés
            List<Probleme> resultats = new List<Probleme>();
            // Vérifier si le mot clé est valide
            if (string.IsNullOrEmpty(motCle)) return resultats;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Parcourir tous les problèmes de la base de données
                foreach (Probleme probleme in context.Problemes)
                {
                    // Si le message ou la réponse du problème contient le mot clé
                    if (probleme.Message.Contains(motCle) || probleme.Reponse.Contains(motCle))
                    {
                        // Ajouter le problème à la liste des résultats
                        resultats.Add(probleme);
                    }
                }
                // Renvoyer la liste des résultats
                return resultats;
            }
        }


        public void vendreProduit(Produit produit)
        {
            //maybe check if the produit already exists in the database
            // Vérifier si le produit est valide
            if (produit == null) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Ajouter le produit à la table Produit de la base de données
                context.Produits.Add(produit);
                // Enregistrer les changements
                context.SaveChanges();
            }
        }

        /*public void posterCommentaire(Utilisateur utilisateur, string texte)
        {
            // Vérifier si l'utilisateur et le texte sont valides
            if (utilisateur == null || string.IsNullOrEmpty(texte)) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Créer une nouvelle instance de la classe Commentaire avec le texte, l'auteur et le destinataire
                var commentaire = new Commentaire
                {
                    Texte = texte,
                    Auteur = this.IdUtilisateur,
                    Destinataire = utilisateur.IdUtilisateur
                };
                // Ajouter le commentaire à la base de données
                context.Commentaires.Add(commentaire);
                // Enregistrer les changements
                context.SaveChanges();
            }
        }*/

        public void supprimerCommentaire(Utilisateur utilisateur, string texte)
        {
            // Vérifier si l'utilisateur et le texte sont valides
            if (utilisateur == null || string.IsNullOrEmpty(texte)) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher le commentaire concerné
                var commentaire = context.Commentaires.FirstOrDefault(cm => cm.Texte == texte && cm.Destinataire == utilisateur.IdUtilisateur);
                //si le commentaire existe
                if(commentaire != null) 
                {
                    // supprimer le commentaire de la base de données
                    context.Commentaires.Remove(commentaire);
                    // Enregistrer les changements
                    context.SaveChanges();
                };
                
            }
        }

        /* public void noterUtilisateur(Utilisateur utilisateur, int valeur)
         {
             // Vérifier si l'utilisateur et la valeur sont valides
             if (utilisateur == null || valeur < 0 || valeur > 10) return;
             // Créer une instance du contexte de la base de données
             using (var context = new EBuyDatabaseContext())
             {
                 // Créer une nouvelle instance de la classe Note avec la valeur, l'auteur et le destinataire
                 var note = new Note
                 {
                     Valeur = valeur,
                     Auteur = this.IdUtilisateur,
                     Destinataire = utilisateur.IdUtilisateur
                 };
                 // Ajouter la note à la base de données
                 context.Notes.Add(note);
                 // Enregistrer les changements
                 context.SaveChanges();
             }
         }*/


        public void modifierNote(Utilisateur utilisateur, int nouvelleValeur)
        {
            // Vérifier si l'utilisateur et la nouvelle valeur sont valides
            if (utilisateur == null || nouvelleValeur < 0 || nouvelleValeur > 10) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher la note concernée
                var note = context.Notes.FirstOrDefault(nt => nt.Destinataire == utilisateur.IdUtilisateur);
                //si la note existe
                if (note != null)
                {
                    // modifier la note de la base de données
                    note.Valeur = nouvelleValeur;
                    // Enregistrer les changements
                    context.SaveChanges();
                };
            }
        }

        /* public void modifierProfil(string nom, string email, string mdp, string numTel, string imageProfil)
         {
             // Vérifier si les paramètres sont valides
             if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(mdp) || string.IsNullOrEmpty(numTel) || string.IsNullOrEmpty(imageProfil)) return;
             // Créer une instance du contexte de la base de données
             using (var context = new EBuyDatabaseContext())
             {
                 // Chercher l'utilisateur qui a le même identifiant que this
                 var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.IdUtilisateur == this.IdUtilisateur);
                 // Si l'utilisateur existe
                 if (utilisateur != null)
                 {
                     // Modifier les informations de l'utilisateur avec les paramètres
                     utilisateur.Nom = nom;
                     utilisateur.Email = email;
                     utilisateur.Mdp = mdp;
                     utilisateur.NumTel = numTel;
                     utilisateur.ImageProfil = imageProfil;
                     // Enregistrer les changements
                     context.SaveChanges();
                 }
             }
         }*/

        /*public void modifierPoste(Produit produit, string nomProduit, string nom_categorie, string image, string description, string prix)
        {
            // Vérifier si les paramètres sont valides
            if (produit == null || string.IsNullOrEmpty(nomProduit) || string.IsNullOrEmpty(nom_categorie) || string.IsNullOrEmpty(image) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(prix)) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher le produit qui appartient à l'utilisateur actuel
                var produitChercheDeLaBd = context.Produits.FirstOrDefault(p => p.IdProduit == produit.IdProduit && p.Vendeur == this.IdUtilisateur);
                // Si le produit existe
                if (produit != null)
                {
                    // Modifier les informations du produit avec les paramètres
                    produit.Nom = nomProduit;
                    produit.Image = image;
                    produit.Description = description;
                    produit.Prix = prix;
                    // Chercher la catégorie qui a le nom donné
                    var categorie = context.Categories.FirstOrDefault(c => c.Nom == nom_categorie);
                    // Si la catégorie existe
                    if (categorie != null)
                    {
                        // Modifier la clé étrangère de la catégorie du produit
                        produit.Categorie = categorie.IdCategorie;
                    }
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }*/

        public void supprimerPoste(Produit produit)
        {
            // Vérifier si le produit est valide
            if (produit == null) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Supprimer le produit de la table Produit de la base de données
                context.Produits.Remove(produit);
                // Enregistrer les changements
                context.SaveChanges();
            }
        }

        /*
        public List<Produit> afficherMesPostes()
        {
            // Créer une liste vide pour stocker les produits trouvés
            List<Produit> resultats = new List<Produit>();
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Parcourir tous les produits de la base de données
                foreach (Produit produit in context.Produits)
                {
                    // Si le produit appartient à l'utilisateur actuel
                    if (produit.Vendeur == this.IdUtilisateur)
                    {
                        // Ajouter le produit à la liste des résultats
                        resultats.Add(produit);
                    }
                }
                // Renvoyer la liste des résultats
                return resultats;
            }
        }
        */

        //////////////////// ADMINISTRATEUR /////////////////////////////
        ///


        public void creerCategorie(string nom)
        {
            // Vérifier si le nom est valide
            if (string.IsNullOrEmpty(nom)) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher si la catégorie avec le nom existe déjà
                var categorie = context.Categories.FirstOrDefault(c => c.Nom == nom);
                // Si la catégorie n'existe pas
                if (categorie == null)
                {
                    // Créer une nouvelle instance de la classe Categorie avec le nom
                    categorie = new Categorie
                    {
                        Nom = nom
                    };
                    // Ajouter la catégorie à la base de données
                    context.Categories.Add(categorie);
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }


        public void modifierCategorie(int id, string nom)
        {
            // Vérifier si le nom est valide
            if (string.IsNullOrEmpty(nom)) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher la catégorie qui a l'identifiant donné
                var categorie = context.Categories.FirstOrDefault(c => c.IdCategorie == id);
                // Si la catégorie existe
                if (categorie != null)
                {
                    // Modifier le nom de la catégorie avec le paramètre
                    categorie.Nom = nom;
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }


        public void supprimerCategorie(int id)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher la catégorie qui a l'identifiant donné
                var categorie = context.Categories.FirstOrDefault(c => c.IdCategorie == id);
                // Si la catégorie existe
                if (categorie != null)
                {
                    // Supprimer la catégorie de la base de données
                    context.Categories.Remove(categorie);
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }



        public void creerUtilisateur(string nom, string prenom, string email, string mdp, string numTel, string imageProfil, bool role)
        {
            // Vérifier si les paramètres sont valides
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(mdp) || string.IsNullOrEmpty(numTel) || string.IsNullOrEmpty(imageProfil)) return;
            // Vérifier si l'email est de la format correcte
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return;
            // Vérifier si le mot de passe dépasse 6 caractères
            if (mdp.Length < 6) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher si l'utilisateur avec l'email existe déjà
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.Email == email);
                // Si l'utilisateur n'existe pas
                if (utilisateur == null)
                {
                    // Créer une nouvelle instance de la classe Utilisateur avec les paramètres
                    utilisateur = new Utilisateur
                    {
                        Nom = nom,
                        Prenom = prenom,
                        Email = email,
                        Mdp = mdp,
                        NumTel = numTel,
                        ImageProfil = imageProfil,
                        Role = role,
                        AppartientListeFavoris = false,
                        AppartientListeNoire = false
                    };
                    // Ajouter l'utilisateur à la base de données
                    context.Utilisateurs.Add(utilisateur);
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }

        public void modifierUtilisateur(int idUtilisateur, string nom, string prenom, string email, string mdp, string numTel, string imageProfil)
        {
            // Vérifier si les paramètres sont valides
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(mdp) || string.IsNullOrEmpty(numTel) || string.IsNullOrEmpty(imageProfil)) return;
            // Vérifier si l'email est de la format correcte
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return;
            // Vérifier si le mot de passe dépasse 6 caractères
            if (mdp.Length < 6) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher si l'utilisateur existe 
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.IdUtilisateur == idUtilisateur);
                // Si l'utilisateur existe
                if (utilisateur != null)
                {
                    // Changer les infos de l'utilisateur passé en paramètre
                    utilisateur.Nom = nom;
                    utilisateur.Prenom = prenom;
                    utilisateur.Email = email;
                    utilisateur.Mdp = mdp;
                    utilisateur.NumTel = numTel;
                    utilisateur.ImageProfil = imageProfil;
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }

        public void supprimerUtilisateur(int idUtilisateur)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher l'utilisateur qui a l'identifiant donné
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.IdUtilisateur== idUtilisateur);
                // Si l'utilisateur existe
                if (utilisateur != null)
                {
                    // Supprimer l'utilisateur de la base de données
                    context.Utilisateurs.Remove(utilisateur);
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }

        public void ajouterAListeFavoris(int idUtilisateur)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher l'utilisateur qui a l'identifiant donné
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.IdUtilisateur == idUtilisateur);
                // Si l'utilisateur existe
                if (utilisateur != null)
                {
                    //l'ajouter à liste des favoris
                    utilisateur.AppartientListeFavoris = true;
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }

        public void supprimerDeListeFavoris(int idUtilisateur)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher l'utilisateur qui a l'identifiant donné
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.IdUtilisateur == idUtilisateur);
                // Si l'utilisateur existe
                if (utilisateur != null)
                {
                    //l'ajouter à liste des favoris
                    utilisateur.AppartientListeFavoris = false;
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }


        public void ajouterAListeNoire(int idUtilisateur)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher l'utilisateur qui a l'identifiant donné
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.IdUtilisateur == idUtilisateur);
                // Si l'utilisateur existe
                if (utilisateur != null)
                {
                    //l'ajouter à liste noire
                    utilisateur.AppartientListeNoire = true;
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }

        public void supprimerDeListeNoire(int idUtilisateur)
        {
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher l'utilisateur qui a l'identifiant donné
                var utilisateur = context.Utilisateurs.FirstOrDefault(u => u.IdUtilisateur == idUtilisateur);
                // Si l'utilisateur existe
                if (utilisateur != null)
                {
                    //l'ajouter à liste noire
                    utilisateur.AppartientListeNoire = false;
                    // Enregistrer les changements
                    context.SaveChanges();
                }
            }
        }



        public void repondreProbleme(int idProbleme, string reponse)
        {
            // Vérifier si le probleme et la reponse sont valides
            if (idProbleme == null || string.IsNullOrEmpty(reponse)) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher le probleme concernée
                var problemeRecherche = context.Problemes.FirstOrDefault(pr => pr.IdProbleme== idProbleme);
                //si le probleme existe
                if (problemeRecherche != null)
                {
                    // ajouter une reponse à ce probleme
                    problemeRecherche.Reponse = reponse;
                    // Enregistrer les changements
                    context.SaveChanges();
                };
            }
        }

        public void supprimerProbleme(int idProbleme)
        {
            // Vérifier si le probleme et la reponse sont valides
            if (idProbleme == null ) return;
            // Créer une instance du contexte de la base de données
            using (var context = new EBuyDatabaseContext())
            {
                // Chercher le probleme concernée
                var problemeRecherche = context.Problemes.FirstOrDefault(pr => pr.IdProbleme == idProbleme);
                //si le probleme existe
                if (problemeRecherche != null)
                {
                    // ajouter une reponse à ce probleme
                    context.Problemes.Remove(problemeRecherche);
                    // Enregistrer les changements
                    context.SaveChanges();
                };
            }
        }













    }
}
