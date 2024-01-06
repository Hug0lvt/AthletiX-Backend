using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class ExerciseService
    {
        private readonly ILogger<ExerciseService> _logger;
        private readonly AppDbContext _dbContext;

        public ExerciseService(AppDbContext dbContext, ILogger<ExerciseService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Exercise CreateExercise(Exercise exercise)
        {
            _dbContext.Exercises.Add(exercise);
            _dbContext.SaveChanges();
            return exercise;
        }

        public List<Exercise> GetAllExercises()
        {
            return _dbContext.Exercises.ToList();
        }

        public Exercise GetExerciseById(int exerciseId)
        {
            return _dbContext.Exercises.FirstOrDefault(e => e.Id == exerciseId);
        }

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
