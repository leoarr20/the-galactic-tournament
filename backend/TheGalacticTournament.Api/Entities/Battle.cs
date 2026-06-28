namespace TheGalacticTournament.Api.Entities
{
    /// <summary>
    /// Entity that represents a battle between two species in the Galactic Tournament.
    /// 
    /// This entity is mapped to the Battles table in the database.
    /// Each battle stores the two participating species, the winning species
    /// and the date when the battle was executed.
    /// </summary>
    public class Battle
    {
        /// <summary>
        /// Unique identifier of the battle.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the first species participating in the battle.
        /// 
        /// This is a foreign key that references the Species table.
        /// </summary>
        public int SpeciesAId { get; set; }

        /// <summary>
        /// Navigation property for the first species participating in the battle.
        /// 
        /// Entity Framework Core uses this property to load the related species data.
        /// </summary>
        public Species SpeciesA { get; set; } = null!;

        /// <summary>
        /// Identifier of the second species participating in the battle.
        /// 
        /// This is a foreign key that references the Species table.
        /// </summary>
        public int SpeciesBId { get; set; }

        /// <summary>
        /// Navigation property for the second species participating in the battle.
        /// 
        /// Entity Framework Core uses this property to load the related species data.
        /// </summary>
        public Species SpeciesB { get; set; } = null!;

        /// <summary>
        /// Identifier of the species that won the battle.
        /// 
        /// This is a foreign key that references the Species table.
        /// </summary>
        public int WinnerSpeciesId { get; set; }

        /// <summary>
        /// Navigation property for the winning species.
        /// 
        /// Entity Framework Core uses this property to load the winner information.
        /// </summary>
        public Species WinnerSpecies { get; set; } = null!;

        /// <summary>
        /// Date and time when the battle was executed.
        /// 
        /// By default, the value is assigned using UTC time when the battle object is created.
        /// </summary>
        public DateTime BattleDate { get; set; } = DateTime.UtcNow;
    }
}