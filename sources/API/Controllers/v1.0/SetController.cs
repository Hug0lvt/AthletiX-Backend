using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to sets.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Set APIs")]
    [ApiController]
    [Route("api/sets")]
    [Authorize]
    public class SetController : ControllerBase
    {
        private readonly SetService _setService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetController"/> class.
        /// </summary>
        /// <param name="setService">The set service.</param>
        public SetController(SetService setService)
        {
            _setService = setService;
        }

        /// <summary>
        /// Creates a new set.
        /// </summary>
        /// <param name="set">The set to create.</param>
        /// <returns>The newly created set.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateSet([FromBody] Set set)
        {
            var createdSet = await _setService.CreateSetAsync(set);
            return CreatedAtAction(nameof(GetSetById), new { setId = createdSet.Id }, createdSet);
        }

        /// <summary>
        /// Gets all sets.
        /// </summary>
        /// <returns>A list of all sets.</returns>
        [HttpGet(Name = "GET - Entrypoint for get all Sets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSets()
        {
            var sets = _setService.GetAllSets();
            return Ok(sets);
        }

        /// <summary>
        /// Gets all Sets with pages.
        /// </summary>
        /// <returns>A list of all Sets.</returns>
        [HttpGet("pages", Name = "GET - Entrypoint for get all Sets with pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSetsWithPages(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0)
        {
            var sets = _setService.GetAllSetsWithPages(pageSize, pageNumber);
            return Ok(sets);
        }

        /// <summary>
        /// Gets a set by its identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns>The set with the specified identifier.</returns>
        [HttpGet("{setId}", Name = "GET - Entrypoint for get Set by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSetById(int setId)
        {
            var set = _setService.GetSetById(setId);

            if (set == null)
            {
                return NotFound();
            }

            return Ok(set);
        }

        /// <summary>
        /// Updates a set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="updatedSet">The updated set information.</param>
        /// <returns>The updated set.</returns>
        [HttpPut("{setId}", Name = "PUT - Entrypoint for update Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateSet(int setId, [FromBody] Set updatedSet)
        {
            try
            {
                if(updatedSet.Id != setId) updatedSet.Id = setId;
                var set = _setService.UpdateSet(updatedSet);
                return Ok(set);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a set by its identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns>The deleted set.</returns>
        [HttpDelete("{setId}", Name = "DELETE - Entrypoint for remove Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteSet(int setId)
        {
            try
            {
                var deletedSet = _setService.DeleteSet(setId);
                return Ok(deletedSet);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
