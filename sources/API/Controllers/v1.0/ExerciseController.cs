using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to exercises.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Exercise APIs")]
    [ApiController]
    [Route("api/exercises")]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseService _exerciseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseController"/> class.
        /// </summary>
        /// <param name="exerciseService">The exercise service.</param>
        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        /// <summary>
        /// Creates a new exercise.
        /// </summary>
        /// <param name="exercise">The exercise to create.</param>
        /// <returns>The newly created exercise.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Exercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateExercise([FromBody] Exercise exercise)
        {
            var createdExercise = await _exerciseService.CreateExerciseAsync(exercise);
            return CreatedAtAction(nameof(GetExerciseById), new { exerciseId = createdExercise.Id }, createdExercise);
        }

        /// <summary>
        /// Gets all Exercise with pages.
        /// </summary>
        /// <returns>A list of all Exercise.</returns>
        [HttpGet("pages", Name = "GET - Entrypoint for get all Exercise with pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllExerciseWithPages(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0,
            bool includeSet = false)
        {
            var exercise = _exerciseService.GetAllExercisesWithPages(pageSize, pageNumber, includeSet);
            return Ok(exercise);
        }

        /// <summary>
        /// Gets all Exercise with pages.
        /// </summary>
        /// <returns>A list of all Exercise.</returns>
        [HttpGet("category/{categoryId}", Name = "GET - Entrypoint for get all Exercise from one category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllExerciseWithPages(int categoryId, bool includeSet = false)
        {
            var exercise = _exerciseService.GetExercisesFromCategory(categoryId, includeSet);
            return Ok(exercise);
        }

        /// <summary>
        /// Gets an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The exercise identifier.</param>
        /// <returns>The exercise with the specified identifier.</returns>
        [HttpGet("{exerciseId}", Name = "GET - Entrypoint for get Exercise by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetExerciseById(int exerciseId)
        {
            var exercise = _exerciseService.GetExerciseById(exerciseId);

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        /// <summary>
        /// Updates an exercise.
        /// </summary>
        /// <param name="exerciseId">The exercise identifier.</param>
        /// <param name="updatedExercise">The updated exercise information.</param>
        /// <returns>The updated exercise.</returns>
        [HttpPut("{exerciseId}", Name = "PUT - Entrypoint for update Exercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateExercise(int exerciseId, [FromBody] Exercise updatedExercise)
        {
            try
            {
                if (updatedExercise.Id != exerciseId) updatedExercise.Id = exerciseId;
                var exercise = _exerciseService.UpdateExercise(updatedExercise);
                return Ok(exercise);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The exercise identifier.</param>
        /// <returns>The deleted exercise.</returns>
        [HttpDelete("{exerciseId}", Name = "DELETE - Entrypoint for remove Exercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteExercise(int exerciseId)
        {
            try
            {
                var deletedExercise = _exerciseService.DeleteExercise(exerciseId);
                return Ok(deletedExercise);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
