﻿using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to exercises.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Exercise APIs")]
    [ApiController]
    [Route("api/exercises")]
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
        public IActionResult CreateExercise([FromBody] Exercise exercise)
        {
            var createdExercise = _exerciseService.CreateExercise(exercise);
            return CreatedAtAction(nameof(GetExerciseById), new { exerciseId = createdExercise.Id }, createdExercise);
        }

        /// <summary>
        /// Gets all exercises.
        /// </summary>
        /// <returns>A list of all exercises.</returns>
        [HttpGet(Name = "GET - Entrypoint for get all Exercises")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllExercises()
        {
            var exercises = _exerciseService.GetAllExercises();
            return Ok(exercises);
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