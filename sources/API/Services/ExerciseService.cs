using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Shared.Exceptions;
using Shared;
using AutoMapper;
using Dommain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    /// <summary>
    /// Service for managing exercises.
    /// </summary>
    public class ExerciseService
    {
        private readonly ILogger<ExerciseService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public ExerciseService(IdentityAppDbContext dbContext, ILogger<ExerciseService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new exercise.
        /// </summary>
        /// <param name="exercise">The exercise to be created.</param>
        /// <returns>The created exercise.</returns>
        public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            try
            {
                var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == exercise.Category.Id);
                if (existingCategory == null)
                    throw new NotCreatedExecption("Category does not exist.");

                var existingSession = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == exercise.Session.Id);
                if (existingSession == null)
                    throw new NotCreatedExecption("Session does not exist.");

                var entity = _mapper.Map<ExerciseEntity>(exercise);

                entity.CategoryId = existingCategory.Id;
                entity.SessionId = existingSession.Id;
                entity.Session = null;
                entity.Category = null;

                _dbContext.Entry(existingCategory).State = EntityState.Unchanged;
                _dbContext.Entry(existingSession).State = EntityState.Unchanged;

                _dbContext.Exercises.Add(entity);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<Exercise>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create exercise.", ex);
            }
        }

        /// <summary>
        /// Gets all exercises.
        /// </summary>
        /// <returns>A list of all exercises.</returns>
        public List<Exercise> GetAllExercises()
        {
            return _mapper.Map<List<Exercise>>(_dbContext.Exercises.ToList());
        }

        /// <summary>
        /// Gets all Exercise (with pages).
        /// </summary>
        /// <returns>A list of all Exercise.</returns>
        public PaginationResult<Exercise> GetAllExercisesWithPages(
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Exercises.Count();
            var items = _dbContext.Exercises
                .Include(e => e.Category)
                .Include(e => e.Session)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<Exercise>
            {
                Items = _mapper.Map<List<Exercise>>(items),
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets all Exercise from category (with pages).
        /// </summary>
        /// <returns>A list of all Exercise in category.</returns>
        public PaginationResult<Exercise> GetExercisesFromCategory(int categoryId)
        {
            var items = _dbContext.Exercises
                .Include(e => e.Category)
                .Include(e => e.Session)
                .Where(q => q.Category.Id == categoryId)
                .ToList();
            var totalItems = items.Count();

            return new PaginationResult<Exercise>
            {
                Items = _mapper.Map<List<Exercise>>(items),
                NextPage = -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The identifier of the exercise.</param>
        /// <returns>The exercise with the specified identifier.</returns>
        public Exercise GetExerciseById(int exerciseId)
        {
            return _mapper.Map<Exercise>(_dbContext.Exercises
                .Include(e => e.Category)
                .Include(e => e.Session)
                .FirstOrDefault(e => e.Id == exerciseId));
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
                return _mapper.Map<Exercise>(existingExercise);
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
                return _mapper.Map<Exercise>(exerciseToDelete);
            }

            _logger.LogTrace("[LOG | ExerciseService] - (DeleteExercise): Exercise not found");
            throw new NotFoundException("[LOG | ExerciseService] - (DeleteExercise): Exercise not found");
        }
    }
}
