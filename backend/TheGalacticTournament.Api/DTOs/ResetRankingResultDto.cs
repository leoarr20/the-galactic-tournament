namespace TheGalacticTournament.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object used to return the result of a ranking reset operation.
    /// 
    /// The ranking is calculated from the battle history, so resetting the ranking
    /// means deleting the registered battle results while keeping the species data.
    /// </summary>
    public class ResetRankingResultDto
    {
        /// <summary>
        /// Number of battle records deleted during the reset operation.
        /// 
        /// If the value is 0, it means there were no battles registered
        /// and the ranking was already reset.
        /// </summary>
        public int DeletedBattles { get; set; }

        /// <summary>
        /// Message describing the result of the reset operation.
        /// 
        /// For example:
        /// - "Ranking is already reset. No battles were found."
        /// - "Ranking was reset successfully. All battle results were removed."
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}