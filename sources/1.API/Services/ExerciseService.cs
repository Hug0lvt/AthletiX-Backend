using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class ExerciseService
    {
        private readonly AppDbContext _dbContext;

        public ExerciseService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

            throw new NotFoundException("[LOG | ExerciseService] - (DeleteExercise): Exercise not found");
        }
    }
}
