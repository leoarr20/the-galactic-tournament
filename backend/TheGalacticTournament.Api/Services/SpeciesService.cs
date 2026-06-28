using Microsoft.EntityFrameworkCore;
using TheGalacticTournament.Api.Data;
using TheGalacticTournament.Api.DTOs;
using TheGalacticTournament.Api.Entities;

namespace TheGalacticTournament.Api.Services
{
    /// <summary>
    /// Service responsible for handling all species-related business logic.
    /// 
    /// This service manages:
    /// - Listing registered species.
    /// - Creating new species.
    /// - Updating existing species.
    /// - Deleting species.
    /// - Calculating the tournament ranking.
    /// 
    /// It uses AppDbContext to interact with the database through Entity Framework Core.
    /// </summary>
    public class SpeciesService : ISpeciesService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the SpeciesService.
        /// </summary>
        /// <param name="context">
        /// Application database context used to access species and battles.
        /// </param>
        public SpeciesService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all registered species ordered alphabetically by name.
        /// </summary>
        /// <returns>
        /// A list of species registered in the Galactic Tournament.
        /// </returns>
        public async Task<List<SpeciesDto>> GetAllAsync()
        {
            // AsNoTracking is used because this is a read-only query.
            // It improves performance by avoiding unnecessary change tracking.
            return await _context.Species
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new SpeciesDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PowerLevel = x.PowerLevel,
                    SpecialAbility = x.SpecialAbility,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new species.
        /// </summary>
        /// <param name="dto">
        /// DTO containing the information required to create the species.
        /// </param>
        /// <returns>
        /// The created species as a SpeciesDto.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when another species with the same name already exists.
        /// </exception>
        public async Task<SpeciesDto> CreateAsync(CreateSpeciesDto dto)
        {
            // Removes extra spaces from the beginning and end of the name.
            var normalizedName = dto.Name.Trim();

            // Validates that the species name is unique.
            // The comparison is case-insensitive, so "Saiyan" and "saiyan"
            // are considered the same name.
            var nameExists = await _context.Species
                .AnyAsync(x => x.Name.ToLower() == normalizedName.ToLower());

            if (nameExists)
            {
                throw new InvalidOperationException("A species with this name already exists.");
            }

            // Creates a new Species entity from the received DTO.
            var species = new Species
            {
                Name = normalizedName,
                PowerLevel = dto.PowerLevel,
                SpecialAbility = dto.SpecialAbility.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            // Adds the new species to the database.
            _context.Species.Add(species);
            await _context.SaveChangesAsync();

            // Returns a DTO instead of exposing the database entity directly.
            return new SpeciesDto
            {
                Id = species.Id,
                Name = species.Name,
                PowerLevel = species.PowerLevel,
                SpecialAbility = species.SpecialAbility,
                CreatedAt = species.CreatedAt
            };
        }

        /// <summary>
        /// Gets the current tournament ranking.
        /// 
        /// The ranking is calculated from the battle history.
        /// Each species is ranked according to the number of battles it has won.
        /// </summary>
        /// <returns>
        /// A list of ranking items ordered by number of victories descending.
        /// If two species have the same number of victories, they are ordered by name.
        /// </returns>
        public async Task<List<RankingItemDto>> GetRankingAsync()
        {
            // For each species, calculates the number of battles where it appears
            // as the winner.
            var rankingData = await _context.Species
                .AsNoTracking()
                .Select(species => new
                {
                    Species = species,
                    Victories = _context.Battles.Count(battle => battle.WinnerSpeciesId == species.Id)
                })
                .OrderByDescending(x => x.Victories)
                .ThenBy(x => x.Species.Name)
                .ToListAsync();

            // Assigns ranking position after ordering the data.
            return rankingData
                .Select((x, index) => new RankingItemDto
                {
                    Position = index + 1,
                    SpeciesId = x.Species.Id,
                    Name = x.Species.Name,
                    PowerLevel = x.Species.PowerLevel,
                    SpecialAbility = x.Species.SpecialAbility,
                    Victories = x.Victories
                })
                .ToList();
        }

        /// <summary>
        /// Updates an existing species.
        /// </summary>
        /// <param name="id">
        /// Identifier of the species to update.
        /// </param>
        /// <param name="dto">
        /// DTO containing the updated species information.
        /// </param>
        /// <returns>
        /// The updated species if it exists; otherwise, null.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when another species with the same name already exists.
        /// </exception>
        public async Task<SpeciesDto?> UpdateAsync(int id, UpdateSpeciesDto dto)
        {
            // Searches the species by its primary key.
            var species = await _context.Species.FindAsync(id);

            // If the species does not exist, return null so the controller
            // can return HTTP 404 Not Found.
            if (species == null)
            {
                return null;
            }

            var normalizedName = dto.Name.Trim();

            // Validates that the new name is not used by another species.
            // The current species is excluded from the validation using x.Id != id.
            var nameExists = await _context.Species
                .AnyAsync(x => x.Id != id && x.Name.ToLower() == normalizedName.ToLower());

            if (nameExists)
            {
                throw new InvalidOperationException("A species with this name already exists.");
            }

            // Updates the entity values.
            species.Name = normalizedName;
            species.PowerLevel = dto.PowerLevel;
            species.SpecialAbility = dto.SpecialAbility.Trim();

            await _context.SaveChangesAsync();

            // Returns the updated species as a DTO.
            return new SpeciesDto
            {
                Id = species.Id,
                Name = species.Name,
                PowerLevel = species.PowerLevel,
                SpecialAbility = species.SpecialAbility,
                CreatedAt = species.CreatedAt
            };
        }

        /// <summary>
        /// Deletes an existing species.
        /// </summary>
        /// <param name="id">
        /// Identifier of the species to delete.
        /// </param>
        /// <returns>
        /// True if the species was deleted successfully.
        /// False if the species was not found.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the species cannot be deleted because it already has battle history.
        /// </exception>
        public async Task<bool> DeleteAsync(int id)
        {
            // Searches the species by its primary key.
            var species = await _context.Species.FindAsync(id);

            // If the species does not exist, return false so the controller
            // can return HTTP 404 Not Found.
            if (species == null)
            {
                return false;
            }

            // Checks if the species has participated in any battle or has won any battle.
            // If it has battle history, it should not be deleted to preserve data integrity.
            var hasBattles = await _context.Battles.AnyAsync(b =>
                b.SpeciesAId == id ||
                b.SpeciesBId == id ||
                b.WinnerSpeciesId == id
            );

            if (hasBattles)
            {
                throw new InvalidOperationException(
                    "This species cannot be deleted because it already has battle history."
                );
            }

            // Deletes the species from the database.
            _context.Species.Remove(species);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}