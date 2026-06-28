using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheGalacticTournament.Api.DTOs;
using TheGalacticTournament.Api.Services;

namespace TheGalacticTournament.Api.Controllers
{
    /// <summary>
    /// Controller responsible for managing battles in the Galactic Tournament.
    /// 
    /// This controller exposes endpoints to:
    /// - List all registered battles.
    /// - Start a battle between two selected species.
    /// - Start a random battle between two available species.
    /// 
    /// The battle rules and business logic are delegated to the IBattleService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BattlesController : ControllerBase
    {
        private readonly IBattleService _battleService;

        /// <summary>
        /// Initializes a new instance of the BattlesController.
        /// </summary>
        /// <param name="battleService">
        /// Service responsible for handling battle logic and battle history.
        /// </param>
        public BattlesController(IBattleService battleService)
        {
            _battleService = battleService;
        }

        /// <summary>
        /// Gets all battles registered in the tournament.
        /// </summary>
        /// <returns>
        /// A list of battle results, including the participating species, the winner and the battle date.
        /// </returns>
        /// <response code="200">
        /// Returns the list of registered battles.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(List<BattleResultDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BattleResultDto>>> GetAll()
        {
            // The controller delegates the data retrieval to the service layer.
            // This keeps the controller focused only on HTTP request/response handling.
            var battles = await _battleService.GetAllAsync();

            return Ok(battles);
        }

        /// <summary>
        /// Starts a battle between two selected species.
        /// </summary>
        /// <param name="request">
        /// Request containing the IDs of the two species that will participate in the battle.
        /// </param>
        /// <returns>
        /// The result of the battle, including the winner.
        /// </returns>
        /// <response code="200">
        /// Battle completed successfully.
        /// </response>
        /// <response code="400">
        /// The battle request is invalid. For example, the same species was selected twice.
        /// </response>
        /// <response code="404">
        /// One or both species were not found.
        /// </response>
        [HttpPost]
        [ProducesResponseType(typeof(BattleResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BattleResultDto>> StartBattle([FromBody] StartBattleDto request)
        {
            try
            {
                // The battle service applies the tournament rules:
                // - The species with the highest power level wins.
                // - If both species have the same power level,
                //   the species with the alphabetically first name wins.
                // - The battle result is stored in the battle history.
                var result = await _battleService.StartBattleAsync(request);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                // Returns HTTP 404 when one or both species do not exist.
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Returns HTTP 400 when the request violates a validation rule,
                // for example when trying to battle the same species against itself.
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Starts a random battle between two registered species.
        /// </summary>
        /// <returns>
        /// The result of the random battle, including the selected species and the winner.
        /// </returns>
        /// <response code="200">
        /// Random battle completed successfully.
        /// </response>
        /// <response code="400">
        /// There are not enough species registered to start a random battle.
        /// </response>
        [HttpPost("random")]
        [ProducesResponseType(typeof(BattleResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BattleResultDto>> StartRandomBattle()
        {
            try
            {
                // The service randomly selects two different species,
                // applies the battle rules and stores the result.
                var result = await _battleService.StartRandomBattleAsync();

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Returns HTTP 400 when the random battle cannot be created,
                // usually because there are not enough species registered.
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}