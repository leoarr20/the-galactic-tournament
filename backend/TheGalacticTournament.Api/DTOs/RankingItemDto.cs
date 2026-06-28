namespace TheGalacticTournament.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object used to represent one item in the tournament ranking.
    /// 
    /// Each item contains the species information, its current position in the ranking
    /// and the total number of victories obtained from the battle history.
    /// </summary>
    public class RankingItemDto
    {
        /// <summary>
        /// Position of the species in the ranking.
        /// 
        /// The position is calculated based on the number of victories.
        /// Species with more victories appear first.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Unique identifier of the species.
        /// </summary>
        public int SpeciesId { get; set; }

        /// <summary>
        /// Name of the species.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Power level of the species.
        /// 
        /// This value is used during battles to determine the winner.
        /// </summary>
        public int PowerLevel { get; set; }

        /// <summary>
        /// Special ability of the species.
        /// </summary>
        public string SpecialAbility { get; set; } = string.Empty;

        /// <summary>
        /// Total number of battles won by the species.
        /// 
        /// This value is calculated from the battle history,
        /// not stored directly as a fixed value in the Species table.
        /// </summary>
        public int Victories { get; set; }
    }
}