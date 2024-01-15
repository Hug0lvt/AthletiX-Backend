using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    /// <summary>
    /// Service for managing exercises.
    /// </summary>
    public class ExerciseService
    {
        private readonly ILogger<ExerciseService> _logger;
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public ExerciseService(AppDbContext dbContext, ILogger<ExerciseService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new exercise.
        /// </summary>
        /// <param name="exercise">The exercise to be created.</param>
        /// <returns>The created exercise.</returns>
        public Exercise CreateExercise(Exercise exercise)
        {
            _dbContext.Exercises.Add(exercise);
            _dbContext.SaveChanges();
            return exercise;
        }

        /// <summary>
        /// Gets all exercises.
        /// </summary>
        /// <returns>A list of all exercises.</returns>
        public List<Exercise> GetAllExercises()
        {
            return _dbContext.Exercises.ToList();
        }

        /// <summary>
        /// Gets an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The identifier of the exercise.</param>
        /// <returns>The exercise with the specified identifier.</returns>
        public Exercise GetExerciseById(int exerciseId)
        {
            return _dbContext.Exercises.FirstOrDefault(e => e.Id == exerciseId);
        }

        /// <summary>
        /// Gets Exercises by category identifier with pagination.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        /// <param name="index">The page index (0-based).</param>
        /// <param name="number">The number of posts per page.</param>
        /// <returns>The paginated list of exercises with the specified category id.</returns>
        public List<Exercise> GetExercisesByCategoryId(int categoryId, int index, int number)
        {
            return _dbContext.Exercises
                .Where(e => e.Category.Id == categoryId)
                .Skip(index * number)
                .Take(number)
                .ToList();
        }

        /// <summary>
        /// Updates an existing exercise.
        /// </summary>
        /// <param name="updatedExercise">The updated exercise.</param>
        /// <returns>The updated exercise.</returns>
        public Exercise UpdateExercise(Exercise updatedExercise)
        {
            var existingExercise = _dbContext.Exercises.Find(updatedExercise.Id);

            if (existingExercise != null)
            {
                existingExercise.Name = updatedExercise.Name;
                existingExercise.Description = updatedExercise.Description;
                existingExercise.Image = updatedExercise.Image;
                _dbContext.SaveChanges();
                return existingExercise;
            }

            _logger.LogTrace("[LOG | ExerciseService] - (UpdateExercise): Exercise not found");
            throw new NotFoundException("[LOG | ExerciseService] - (UpdateExercise): Exercise not found");
        }

        /// <summary>
        /// Deletes an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The identifier of the exercise to be deleted.</param>
        /// <returns>The deleted exercise.</returns>
        public Exercise DeleteExercise(int exerciseId)
        {
            var exerciseToDelete = _dbContext.Exercises.Find(exerciseId);

            if (exerciseToDelete != null)
            {
                _dbContext.Exercises.Remove(exerciseToDelete);
                _dbContext.SaveChanges();
                return exerciseToDelete;
            }

            _logger.LogTrace("[LOG | ExerciseService] - (DeleteExercise): Exercise not found");
            throw new NotFoundException("[LOG | ExerciseService] - (DeleteExercise): Exercise not found");
        }
    }
}
