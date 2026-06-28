using TheGalacticTournament.Api.DTOs;

namespace TheGalacticTournament.Api.Services
{
    /// <summary>
    /// Defines the contract for battle-related business operations.
    /// 
    /// This interface exposes the methods required to manage battle history,
    /// start manual battles, start random battles and reset the tournament ranking.
    /// 
    /// The implementation of this interface is responsible for applying the
    /// tournament battle rules and interacting with the database through the data layer.
    /// </summary>
    public interface IBattleService
    {
        /// <summary>
        /// Gets all registered battles.
        /// </summary>
        /// <returns>
        /// A list of battle results ordered according to the implementation logic,
        /// usually from newest to oldest.
        /// </returns>
        Task<List<BattleResultDto>> GetAllAsync();

        /// <summary>
        /// Starts a battle between two selected species.
        /// </summary>
        /// <param name="request">
        /// DTO containing the identifiers of the two species that will participate
        /// in the battle.
        /// </param>
        /// <returns>
        /// The result of the battle, including participating species, winner,
        /// result reason and battle date.
        /// </returns>
        Task<BattleResultDto> StartBattleAsync(StartBattleDto request);

        /// <summary>
        /// Starts a random battle between two different registered species.
        /// </summary>
        /// <returns>
        /// The result of the random battle, including selected species, winner,
        /// result reason and battle date.
        /// </returns>
        Task<BattleResultDto> StartRandomBattleAsync();

        /// <summary>
        /// Resets the tournament ranking.
        /// 
        /// Since the ranking is calculated from battle history, resetting the ranking
        /// means deleting all registered battle records while keeping the species data.
        /// </summary>
        /// <returns>
        /// A result containing the number of deleted battles and a descriptive message.
        /// </returns>
        Task<ResetRankingResultDto> ResetRankingAsync();
    }
}