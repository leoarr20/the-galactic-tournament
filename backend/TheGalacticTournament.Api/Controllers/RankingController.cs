using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheGalacticTournament.Api.DTOs;
using TheGalacticTournament.Api.Services;

namespace TheGalacticTournament.Api.Controllers
{
    /// <summary>
    /// Controller responsible for managing the tournament ranking.
    /// 
    /// This controller exposes endpoints to:
    /// - Get the current ranking based on the number of victories.
    /// - Reset the ranking by removing all registered battle results.
    /// 
    /// The ranking is not stored as a fixed value in the Species table.
    /// Instead, it is calculated from the battle history.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RankingController : ControllerBase
    {
        private readonly IBattleService _battleService;
        private readonly ISpeciesService _speciesService;

        /// <summary>
        /// Initializes a new instance of the RankingController.
        /// </summary>
        /// <param name="battleService">
        /// Service responsible for managing battles and battle history.
        /// </param>
        /// <param name="speciesService">
        /// Service responsible for managing species and calculating the ranking.
        /// </param>
        public RankingController(
            IBattleService battleService,
            ISpeciesService speciesService)
        {
            _battleService = battleService;
            _speciesService = speciesService;
        }

        /// <summary>
        /// Gets the current tournament ranking.
        /// </summary>
        /// <returns>
        /// A ranking list ordered by the number of victories for each species.
        /// </returns>
        /// <response code="200">
        /// Returns the current ranking.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(List<RankingItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRanking()
        {
            // The ranking is calculated from the battle history.
            // Each species is ordered according to its number of victories.
            var ranking = await _speciesService.GetRankingAsync();

            return Ok(ranking);
        }

        /// <summary>
        /// Resets the tournament ranking.
        /// </summary>
        /// <returns>
        /// A result indicating how many battle records were removed.
        /// </returns>
        /// <response code="200">
        /// Ranking reset successfully.
        /// </response>
        [HttpPost("reset")]
        [ProducesResponseType(typeof(ResetRankingResultDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetRanking()
        {
            // The ranking is not stored directly as a column.
            // It is calculated based on the Battles table.
            // Therefore, resetting the ranking means removing all battle results,
            // while keeping the registered species unchanged.
            var result = await _battleService.ResetRankingAsync();

            return Ok(result);
        }
    }
}