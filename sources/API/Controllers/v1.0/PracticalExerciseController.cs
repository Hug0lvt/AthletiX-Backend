using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Shared.Exceptions;

namespace API.Controllers.v1._0
{
    /// <summary>
    /// Controller for managing operations related to exercises.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Practical Exercise APIs")]
    [ApiController]
    [Route("api/practicalexercise")]
    [Authorize]
    public class PracticalExerciseController : ControllerBase
    {
        private readonly PracticalExerciseService _practicalExerciseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseController"/> class.
        /// </summary>
        /// <param name="exerciseService">The exercise service.</param>
        public PracticalExerciseController(PracticalExerciseService exerciseService)
        {
            _practicalExerciseService = exerciseService;
        }

        /// <summary>
        /// Creates a new exercise.
        /// </summary>
        /// <param name="exercise">The exercise to create.</param>
        /// <returns>The newly created exercise.</returns>
        [HttpPost(Name = "POST - Entrypoint for create PracticalExercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateExercise([FromBody] PracticalExercise exercise)
        {
            var createdExercise = await _practicalExerciseService.CreatePracticalExerciseAsync(exercise);
            return CreatedAtAction(nameof(GetExerciseById), new { exerciseId = createdExercise.Id }, createdExercise);
        }

        /// <summary>
        /// Gets all Exercise with pages.
        /// </summary>
        /// <returns>A list of all Exercise.</returns>
        [HttpGet("pages", Name = "GET - Entrypoint for get all PracticalExercise with pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllExerciseWithPages(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0,
            bool includeSet = false)
        {
            var exercise = _practicalExerciseService.GetAllPracticalExercisesWithPages(pageSize, pageNumber, includeSet);
            return Ok(exercise);
        }

        /// <summary>
        /// Gets all Exercise with pages.
        /// </summary>
        /// <returns>A list of all Exercise.</returns>
        [HttpGet("category/{categoryId}", Name = "GET - Entrypoint for get all PracticalExercise from one category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllExerciseWithPages(int categoryId, bool includeSet = false)
        {
            var exercise = _practicalExerciseService.GetPracticalExercisesFromCategory(categoryId, includeSet);
            return Ok(exercise);
        }

        /// <summary>
        /// Gets an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The exercise identifier.</param>
        /// <returns>The exercise with the specified identifier.</returns>
        [HttpGet("{exerciseId}", Name = "GET - Entrypoint for get PracticalExercise by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetExerciseById(int exerciseId)
        {
            var exercise = _practicalExerciseService.GetPracticalExerciseById(exerciseId);

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        /// <summary>
        /// Deletes an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The exercise identifier.</param>
        /// <returns>The deleted exercise.</returns>
        [HttpDelete("{exerciseId}", Name = "DELETE - Entrypoint for remove PracticalExercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteExercise(int exerciseId)
        {
            try
            {
                var deletedExercise = _practicalExerciseService.DeletePracticalExercise(exerciseId);
                return Ok(deletedExercise);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
