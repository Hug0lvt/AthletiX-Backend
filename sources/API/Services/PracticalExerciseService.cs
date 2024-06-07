using AutoMapper;
using Dommain.Entities;
using Microsoft.EntityFrameworkCore;
using Model;
using Shared.Exceptions;
using Shared;
using Domain.Entities;

namespace API.Services
{
    public class PracticalExerciseService
    {
        private readonly ILogger<PracticalExerciseService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PracticalExerciseService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public PracticalExerciseService(IdentityAppDbContext dbContext, ILogger<PracticalExerciseService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new PracticalExercise.
        /// </summary>
        /// <param name="practicalExercise">The PracticalExercise to be created.</param>
        /// <returns>The created exercise.</returns>
        public async Task<PracticalExercise> CreatePracticalExerciseAsync(int sessionId, int exerciseId)
        {
            try
            {
                var existingExercise = await _dbContext.Exercises
                    .Include(e => e.Category)
                    .FirstOrDefaultAsync(c => c.Id == exerciseId);
        
                if (existingExercise == null)
                    throw new NotCreatedExecption("Exercise does not exist.");

                var existingSession = await _dbContext.Sessions
                    .Include(s => s.Profile)
                    .FirstOrDefaultAsync(s => s.Id == sessionId);
                if (existingSession == null)
                    throw new NotCreatedExecption("Session does not exist.");

                var entity = new PracticalExerciseEntity
                {
                    Id = 0,
                    ExerciseId = exerciseId,
                    SessionId = sessionId,
                    Exercise = existingExercise,
                    Session = existingSession
                };

                _dbContext.PracticalExercises.Add(entity);
                await _dbContext.SaveChangesAsync();
        
                var result = _mapper.Map<PracticalExercise>(entity);
                result.Exercise.Category = _mapper.Map<Category>(existingExercise.Category);
                result.Session.Profile = _mapper.Map<Model.Profile>(existingSession.Profile);
                result.Sets = new List<Set>();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create PracticalExercise.", ex);
            }
        }

        /// <summary>
        /// Gets all PracticalExercise.
        /// </summary>
        /// <returns>A list of all PracticalExercise.</returns>
        public List<PracticalExercise> GetAllPracticalExercises()
        {
            return _mapper.Map<List<PracticalExercise>>(_dbContext.PracticalExercises.ToList());
        }

        /// <summary>
        /// Gets all PracticalExercise (with pages).
        /// </summary>
        /// <returns>A list of all PracticalExercise.</returns>
        public PaginationResult<PracticalExercise> GetAllPracticalExercisesWithPages(
            int pageSize = 10,
            int pageNumber = 0,
            bool includeSet = false)
        {
            var totalItems = _dbContext.PracticalExercises.Count();
            var items = _mapper.Map<List<PracticalExercise>>(_dbContext.PracticalExercises
                .Include(e => e.Exercise)
                .Include(e => e.Session)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList());

            if (includeSet)
            {
                foreach (var exercise in items)
                {
                    if (exercise != null)
                    {
                        List<Set> sets = _mapper.Map<List<Set>>(_dbContext.Sets
                        .Where(s => s.PracticalExerciseId == exercise.Id).ToList());
                        exercise.Sets = sets;
                    }
                }
            }

            return new PaginationResult<PracticalExercise>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets all PracticalExercise from category (with pages).
        /// </summary>
        /// <returns>A list of all PracticalExercise in category.</returns>
        public PaginationResult<PracticalExercise> GetPracticalExercisesFromCategory(int categoryId, bool includeSet = false)
        {
            var items = _mapper.Map<List<PracticalExercise>>(_dbContext.PracticalExercises
                .Include(e => e.Exercise)
                .Include(e => e.Session)
                .Where(q => q.Exercise.Category.Id == categoryId)
                .ToList());
            var totalItems = items.Count();

            if (includeSet)
            {
                foreach (var exercise in items)
                {
                    if (exercise != null)
                    {
                        List<Set> sets = _mapper.Map<List<Set>>(_dbContext.Sets
                        .Where(s => s.PracticalExerciseId == exercise.Id).ToList());
                        exercise.Sets = sets;
                    }
                }
            }

            return new PaginationResult<PracticalExercise>
            {
                Items = items,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets an PracticalExercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The identifier of the PracticalExercise.</param>
        /// <returns>The PracticalExercise with the specified identifier.</returns>
        public PracticalExercise GetPracticalExerciseById(int practicalExerciseId)
        {
            PracticalExercise exercise = _mapper.Map<PracticalExercise>(_dbContext.PracticalExercises
                .Include(e => e.Exercise).ThenInclude(ex => ex.Category)
                .Include(e => e.Session)
                .FirstOrDefault(e => e.Id == practicalExerciseId));
            if (exercise != null)
            {
                List<Set> sets = _mapper.Map<List<Set>>(_dbContext.Sets
                .Where(s => s.PracticalExerciseId == exercise.Id).ToList());
                exercise.Sets = sets;
            }
            return exercise;
        }

        /// <summary>
        /// Deletes an exercise by its identifier.
        /// </summary>
        /// <param name="exerciseId">The identifier of the exercise to be deleted.</param>
        /// <returns>The deleted exercise.</returns>
        public PracticalExercise DeletePracticalExercise(int exerciseId)
        {
            var exerciseToDelete = _dbContext.PracticalExercises.Find(exerciseId);

            if (exerciseToDelete != null)
            {
                _dbContext.PracticalExercises.Remove(exerciseToDelete);
                _dbContext.SaveChanges();
                return _mapper.Map<PracticalExercise>(exerciseToDelete);
            }

            _logger.LogTrace("[LOG | PracticalExerciseService] - (DeleteExercise): Exercise not found");
            throw new NotFoundException("[LOG | PracticalExerciseService] - (DeleteExercise): Exercise not found");
        }
    }
}
