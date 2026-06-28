namespace TheGalacticTournament.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object used to return the result of a battle.
    /// 
    /// This DTO contains the information of both participating species,
    /// the winner, the reason why the winner was selected and the date
    /// when the battle was executed.
    /// </summary>
    public class BattleResultDto
    {
        /// <summary>
        /// Unique identifier of the battle.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the first species participating in the battle.
        /// </summary>
        public int SpeciesAId { get; set; }

        /// <summary>
        /// Name of the first species participating in the battle.
        /// </summary>
        public string SpeciesAName { get; set; } = string.Empty;

        /// <summary>
        /// Power level of the first species.
        /// </summary>
        public int SpeciesAPowerLevel { get; set; }

        /// <summary>
        /// Identifier of the second species participating in the battle.
        /// </summary>
        public int SpeciesBId { get; set; }

        /// <summary>
        /// Name of the second species participating in the battle.
        /// </summary>
        public string SpeciesBName { get; set; } = string.Empty;

        /// <summary>
        /// Power level of the second species.
        /// </summary>
        public int SpeciesBPowerLevel { get; set; }

        /// <summary>
        /// Identifier of the species that won the battle.
        /// </summary>
        public int WinnerSpeciesId { get; set; }

        /// <summary>
        /// Name of the species that won the battle.
        /// </summary>
        public string WinnerName { get; set; } = string.Empty;

        /// <summary>
        /// Explanation of why the winner was selected.
        /// 
        /// For example:
        /// - The winner had a higher power level.
        /// - Both species had the same power level, so the winner was selected
        ///   by alphabetical order.
        /// </summary>
        public string ResultReason { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the battle was executed.
        /// </summary>
        public DateTime BattleDate { get; set; }
    }
}