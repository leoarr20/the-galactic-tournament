using TheGalacticTournament.Api.DTOs;

namespace TheGalacticTournament.Api.Services
{
    /// <summary>
    /// Defines the contract for species-related business operations.
    /// 
    /// This interface exposes the methods required to manage species,
    /// including listing, creating, updating, deleting and generating
    /// the tournament ranking.
    /// 
    /// The implementation of this interface is responsible for applying
    /// the species business rules and interacting with the database.
    /// </summary>
    public interface ISpeciesService
    {
        /// <summary>
        /// Gets all registered species.
        /// </summary>
        /// <returns>
        /// A list of species registered in the Galactic Tournament.
        /// </returns>
        Task<List<SpeciesDto>> GetAllAsync();

        /// <summary>
        /// Creates a new species.
        /// </summary>
        /// <param name="request">
        /// DTO containing the information required to create a species,
        /// such as name, power level and special ability.
        /// </param>
        /// <returns>
        /// The created species.
        /// </returns>
        Task<SpeciesDto> CreateAsync(CreateSpeciesDto request);

        /// <summary>
        /// Gets the current tournament ranking.
        /// 
        /// The ranking is calculated from the battle history,
        /// based on the number of victories of each species.
        /// </summary>
        /// <returns>
        /// A list of ranking items ordered by number of victories.
        /// </returns>
        Task<List<RankingItemDto>> GetRankingAsync();

        /// <summary>
        /// Updates an existing species.
        /// </summary>
        /// <param name="id">
        /// Identifier of the species to update.
        /// </param>
        /// <param name="dto">
        /// DTO containing the new information for the species.
        /// </param>
        /// <returns>
        /// The updated species if it exists; otherwise, null.
        /// </returns>
        Task<SpeciesDto?> UpdateAsync(int id, UpdateSpeciesDto dto);

        /// <summary>
        /// Deletes an existing species.
        /// </summary>
        /// <param name="id">
        /// Identifier of the species to delete.
        /// </param>
        /// <returns>
        /// True if the species was deleted successfully; false if the species was not found.
        /// </returns>
        Task<bool> DeleteAsync(int id);
    }
}