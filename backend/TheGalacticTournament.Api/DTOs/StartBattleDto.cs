using System.ComponentModel.DataAnnotations;

namespace TheGalacticTournament.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object used to start a battle between two species.
    /// 
    /// This DTO receives the identifiers of the two species selected by the user
    /// and sends them to the battle service so the tournament rules can be applied.
    /// </summary>
    public class StartBattleDto
    {
        /// <summary>
        /// Identifier of the first species that will participate in the battle.
        /// 
        /// Rules:
        /// - Must be greater than 0.
        /// - Must belong to an existing species.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int SpeciesAId { get; set; }

        /// <summary>
        /// Identifier of the second species that will participate in the battle.
        /// 
        /// Rules:
        /// - Must be greater than 0.
        /// - Must belong to an existing species.
        /// - Must be different from SpeciesAId.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int SpeciesBId { get; set; }
    }
}