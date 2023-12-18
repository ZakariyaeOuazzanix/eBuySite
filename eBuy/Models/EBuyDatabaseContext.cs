using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eBuY.Models;

public partial class EBuyDatabaseContext : DbContext
{
    public EBuyDatabaseContext()
    {
    }

    public EBuyDatabaseContext(DbContextOptions<EBuyDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrateur> Administrateurs { get; set; }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Commentaire> Commentaires { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Panier> Paniers { get; set; }

    public virtual DbSet<Probleme> Problemes { get; set; }

    public virtual DbSet<Produit> Produits { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=eBuyDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrateur>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("PK__Administ__4C3F97F4269E06AA");

            entity.ToTable("Administrateur");

            entity.Property(e => e.IdAdmin).ValueGeneratedNever();

            entity.HasOne(d => d.IdUtilisateurNavigation).WithMany(p => p.Administrateurs)
                .HasForeignKey(d => d.IdUtilisateur)
                .HasConstraintName("IdUtilisateurC");
        });

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.IdCategorie).HasName("PK__Categori__A3C02A1C4D14E82D");

            entity.ToTable("Categorie");

            entity.Property(e => e.IdCategorie).ValueGeneratedNever();
            entity.Property(e => e.Nom).HasMaxLength(50);
        });

        modelBuilder.Entity<Commentaire>(entity =>
        {
            entity.HasKey(e => e.IdCommentaire).HasName("PK__Commenta__13D28D68C75F3092");

            entity.ToTable("Commentaire");

            entity.Property(e => e.IdCommentaire).ValueGeneratedNever();
            entity.Property(e => e.Texte).HasMaxLength(500);

            entity.HasOne(d => d.AuteurNavigation).WithMany(p => p.CommentaireAuteurNavigations)
                .HasForeignKey(d => d.Auteur)
                .HasConstraintName("AuteurConstraint");

            entity.HasOne(d => d.DestinataireNavigation).WithMany(p => p.CommentaireDestinataireNavigations)
                .HasForeignKey(d => d.Destinataire)
                .HasConstraintName("DestinataireConstraint");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.IdNote).HasName("PK__Note__4B2ACFF63EB22117");

            entity.ToTable("Note");

            entity.Property(e => e.IdNote).ValueGeneratedNever();

            entity.HasOne(d => d.AuteurNavigation).WithMany(p => p.NoteAuteurNavigations)
                .HasForeignKey(d => d.Auteur)
                .HasConstraintName("FK__Note__Auteur__33D4B598");

            entity.HasOne(d => d.DestinataireNavigation).WithMany(p => p.NoteDestinataireNavigations)
                .HasForeignKey(d => d.Destinataire)
                .HasConstraintName("FK__Note__Destinatai__32E0915F");
        });

        modelBuilder.Entity<Panier>(entity =>
        {
            entity.HasKey(e => e.IdPanier).HasName("PK__Panier__1DD649413B94D7C0");

            entity.ToTable("Panier");

            entity.Property(e => e.IdPanier).ValueGeneratedNever();

            entity.HasOne(d => d.ProduitNavigation).WithMany(p => p.Paniers)
                .HasForeignKey(d => d.Produit)
                .HasConstraintName("ProduitC");

            entity.HasOne(d => d.UtilisateurNavigation).WithMany(p => p.Paniers)
                .HasForeignKey(d => d.Utilisateur)
                .HasConstraintName("UtilisateurC");
        });

        modelBuilder.Entity<Probleme>(entity =>
        {
            entity.HasKey(e => e.IdProbleme).HasName("PK__Probleme__0FDB5F36FAAAD2A3");

            entity.ToTable("Probleme");

            entity.Property(e => e.IdProbleme).ValueGeneratedNever();
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Reponse).HasMaxLength(50);

            entity.HasOne(d => d.AuteurNavigation).WithMany(p => p.Problemes)
                .HasForeignKey(d => d.Auteur)
                .HasConstraintName("AuteurC");
        });

        modelBuilder.Entity<Produit>(entity =>
        {
            entity.HasKey(e => e.IdProduit).HasName("PK__Produit__2E8997F0C6E1D720");

            entity.ToTable("Produit");

            entity.Property(e => e.IdProduit).ValueGeneratedNever();
            entity.Property(e => e.Categorie).HasColumnName("categorie");
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Nom).HasMaxLength(50);
            entity.Property(e => e.Prix).HasMaxLength(50);

            entity.HasOne(d => d.CategorieNavigation).WithMany(p => p.Produits)
                .HasForeignKey(d => d.Categorie)
                .HasConstraintName("categorieConstraint");

            entity.HasOne(d => d.VendeurNavigation).WithMany(p => p.Produits)
                .HasForeignKey(d => d.Vendeur)
                .HasConstraintName("VendeurC");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.IdUtilisateur).HasName("PK__Utilisat__45A4C15761840AD5");

            entity.ToTable("Utilisateur");

            entity.Property(e => e.IdUtilisateur).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.ImageProfil).HasMaxLength(50);
            entity.Property(e => e.Mdp).HasMaxLength(50);
            entity.Property(e => e.Nom).HasMaxLength(50);
            entity.Property(e => e.NumTel).HasMaxLength(50);
            entity.Property(e => e.Prenom).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
