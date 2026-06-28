namespace TheGalacticTournament.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object used to return species information to the client.
    /// 
    /// This DTO represents a registered species in the Galactic Tournament
    /// and is used to expose only the necessary data to the frontend,
    /// instead of returning the database entity directly.
    /// </summary>
    public class SpeciesDto
    {
        /// <summary>
        /// Unique identifier of the species.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the species.
        /// 
        /// This value identifies the species inside the tournament.
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
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}