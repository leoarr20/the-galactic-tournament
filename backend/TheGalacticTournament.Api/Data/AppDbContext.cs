using Microsoft.EntityFrameworkCore;
using TheGalacticTournament.Api.Entities;

namespace TheGalacticTournament.Api.Data
{
    /// <summary>
    /// Main database context for the Galactic Tournament API.
    /// 
    /// This class represents the connection between the application and the database.
    /// It defines the available tables and configures entity relationships, constraints,
    /// required fields, maximum lengths and delete behavior.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the AppDbContext.
        /// 
        /// The DbContextOptions are injected by dependency injection and contain
        /// the database provider and connection string configuration.
        /// </summary>
        /// <param name="options">
        /// Entity Framework Core configuration options for this context.
        /// </param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Represents the Species table in the database.
        /// 
        /// Each record contains the information of a species registered
        /// in the Galactic Tournament.
        /// </summary>
        public DbSet<Species> Species { get; set; }

        /// <summary>
        /// Represents the Battles table in the database.
        /// 
        /// Each record stores the result of a battle between two species.
        /// </summary>
        public DbSet<Battle> Battles { get; set; }

        /// <summary>
        /// Configures the database model using Fluent API.
        /// 
        /// This method is used to define table names, primary keys, required fields,
        /// maximum lengths, unique indexes and entity relationships.
        /// </summary>
        /// <param name="modelBuilder">
        /// Model builder used by Entity Framework Core to configure the database schema.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
             * Species entity configuration.
             * 
             * This table stores the species that participate in the tournament.
             */
            modelBuilder.Entity<Species>(entity =>
            {
                // Database table name.
                entity.ToTable("Species");

                // Primary key.
                entity.HasKey(x => x.Id);

                // Unique index to prevent duplicated species names.
                // This guarantees uniqueness at database level.
                entity.HasIndex(x => x.Name)
                    .IsUnique();

                // Species name is required and has a maximum length of 100 characters.
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                // Power level is required.
                entity.Property(x => x.PowerLevel)
                    .IsRequired();

                // Special ability is required and has a maximum length of 100 characters.
                entity.Property(x => x.SpecialAbility)
                    .IsRequired()
                    .HasMaxLength(100);

                // Creation date is required.
                entity.Property(x => x.CreatedAt)
                    .IsRequired();
            });

            /*
             * Battle entity configuration.
             * 
             * This table stores the battles performed between two species and
             * keeps the winner of each battle.
             */
            modelBuilder.Entity<Battle>(entity =>
            {
                // Database table name.
                entity.ToTable("Battles");

                // Primary key.
                entity.HasKey(x => x.Id);

                // Battle date is required.
                entity.Property(x => x.BattleDate)
                    .IsRequired();

                /*
                 * Relationship: Battle -> SpeciesA
                 * 
                 * SpeciesA represents the first species participating in the battle.
                 * DeleteBehavior.Restrict prevents deleting a species that already has
                 * battle history as SpeciesA.
                 */
                entity.HasOne(x => x.SpeciesA)
                    .WithMany(x => x.BattlesAsSpeciesA)
                    .HasForeignKey(x => x.SpeciesAId)
                    .OnDelete(DeleteBehavior.Restrict);

                /*
                 * Relationship: Battle -> SpeciesB
                 * 
                 * SpeciesB represents the second species participating in the battle.
                 * DeleteBehavior.Restrict prevents deleting a species that already has
                 * battle history as SpeciesB.
                 */
                entity.HasOne(x => x.SpeciesB)
                    .WithMany(x => x.BattlesAsSpeciesB)
                    .HasForeignKey(x => x.SpeciesBId)
                    .OnDelete(DeleteBehavior.Restrict);

                /*
                 * Relationship: Battle -> WinnerSpecies
                 * 
                 * WinnerSpecies represents the species that won the battle.
                 * DeleteBehavior.Restrict prevents deleting a species that appears
                 * as winner in the battle history.
                 */
                entity.HasOne(x => x.WinnerSpecies)
                    .WithMany(x => x.BattlesWon)
                    .HasForeignKey(x => x.WinnerSpeciesId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}