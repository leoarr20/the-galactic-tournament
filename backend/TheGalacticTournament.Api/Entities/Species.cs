namespace TheGalacticTournament.Api.Entities
{
    /// <summary>
    /// Entity that represents a species registered in thesharp id="t8 Galactic Tournament.
    /// 
    /// This entity is mapped to the Species table in the database.
    /// Each species has a name, a power level and a special ability.
    /// It can also be related to multiple battles as participant or winner.
    /// </summary>
    public class Species
    {
        /// <summary>
        /// Unique identifier of the species.
        /// 
        /// This value is used as the primary key in the Species table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the species.
        /// 
        /// This value identifies the species in the tournament.
        /// According to the business rules, the name must be unique.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Power level of the species.
        /// 
        /// This value is used during battles to determine the winner.
        /// The species with the highest power level wins.
        /// </summary>
        public int PowerLevel { get; set; }

        /// <summary>
        /// Special ability of the species.
        /// 
        /// This describes the main ability or characteristic of the species.
        /// </summary>
        public string SpecialAbility { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the species was registered.
        /// 
        /// By default, the value is assigned using UTC time when the species object is created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Battles where this species participated as the first competitor.
        /// 
        /// This is a navigation property used by Entity Framework Core
        /// to represent the relationship between Species and Battle.
        /// </summary>
        public ICollection<Battle> BattlesAsSpeciesA { get; set; } = new List<Battle>();

        /// <summary>
        /// Battles where this species participated as the second competitor.
        /// 
        /// This is a navigation property used by Entity Framework Core
        /// to represent the relationship between Species and Battle.
        /// </summary>
        public ICollection<Battle> BattlesAsSpeciesB { get; set; } = new List<Battle>();

        /// <summary>
        /// Battles won by this species.
        /// 
        /// This collection is used to calculate the number of victories
        /// for the tournament ranking.
        /// </summary>
        public ICollection<Battle> BattlesWon { get; set; } = new List<Battle>();
    }
}