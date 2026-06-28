using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheGalacticTournament.Api.DTOs;
using TheGalacticTournament.Api.Services;

namespace TheGalacticTournament.Api.Controllers
{
    /// <summary>
    /// Controller responsible for managing the species registered in the Galactic Tournament.
    /// 
    /// This controller exposes endpoints to list, create, update and delete species.
    /// The business rules are delegated to the SpeciesService through the ISpeciesService interface.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SpeciesController : ControllerBase
    {
        private readonly ISpeciesService _speciesService;

        /// <summary>
        /// Initializes a new instance of the SpeciesController.
        /// </summary>
        /// <param name="speciesService">
        /// Service responsible for handling the business logic related to species.
        /// </param>
        public SpeciesController(ISpeciesService speciesService)
        {
            _speciesService = speciesService;
        }

        /// <summary>
        /// Gets all registered species.
        /// </summary>
        /// <returns>
        /// A list of species registered in the tournament.
        /// </returns>
        /// <response code="200">
        /// Returns the list of species.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(List<SpeciesDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SpeciesDto>>> GetAll()
        {
            // The controller does not access the database directly.
            // It delegates the operation to the service layer.
            var species = await _speciesService.GetAllAsync();

            return Ok(species);
        }

        /// <summary>
        /// Creates a new species.
        /// </summary>
        /// <param name="dto">
        /// Data required to create a species, including name, power level and special ability.
        /// </param>
        /// <returns>
        /// The created species.
        /// </returns>
        /// <response code="201">
        /// Species created successfully.
        /// </response>
        /// <response code="400">
        /// The request data is invalid.
        /// </response>
        /// <response code="409">
        /// A species with the same name already exists.
        /// </response>
        [HttpPost]
        [ProducesResponseType(typeof(SpeciesDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(CreateSpeciesDto dto)
        {
            try
            {
                // The service validates business rules such as:
                // - Required fields
                // - Unique species name
                // - Valid power level
                // - Valid special ability
                var species = await _speciesService.CreateAsync(dto);

                // Returns HTTP 201 Created when the species is created successfully.
                // CreatedAtAction points to the GetAll action as a reference endpoint.
                return CreatedAtAction(nameof(GetAll), new { id = species.Id }, species);
            }
            catch (InvalidOperationException ex)
            {
                // Returns HTTP 409 Conflict when the operation violates a business rule,
                // for example when trying to create a species with a duplicated name.
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing species.
        /// </summary>
        /// <param name="id">
        /// Identifier of the species to update.
        /// </param>
        /// <param name="dto">
        /// New data for the species.
        /// </param>
        /// <returns>
        /// The updated species.
        /// </returns>
        /// <response code="200">
        /// Species updated successfully.
        /// </response>
        /// <response code="400">
        /// The request data is invalid.
        /// </response>
        /// <response code="404">
        /// The species was not found.
        /// </response>
        /// <response code="409">
        /// Another species with the same name already exists.
        /// </response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SpeciesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, UpdateSpeciesDto dto)
        {
            try
            {
                // Sends the species ID and the new data to the service layer.
                var species = await _speciesService.UpdateAsync(id, dto);

                // If the service returns null, it means the species does not exist.
                if (species == null)
                {
                    return NotFound();
                }

                // Returns HTTP 200 OK with the updated species.
                return Ok(species);
            }
            catch (InvalidOperationException ex)
            {
                // Returns HTTP 409 Conflict if the update violates a business rule,
                // for example if the new name is already used by another species.
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing species.
        /// </summary>
        /// <param name="id">
        /// Identifier of the species to delete.
        /// </param>
        /// <returns>
        /// No content if the species was deleted successfully.
        /// </returns>
        /// <response code="204">
        /// Species deleted successfully.
        /// </response>
        /// <response code="404">
        /// The species was not found.
        /// </response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            // The service returns true if the species was deleted,
            // or false if the species does not exist.
            var deleted = await _speciesService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            // Returns HTTP 204 No Content because the delete operation
            // completed successfully and there is no response body to return.
            return NoContent();
        }
    }
}