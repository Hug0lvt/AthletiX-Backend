using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class SetService
    {
        private readonly ILogger<SetService> _logger;
        private readonly AppDbContext _dbContext;

        public SetService(AppDbContext dbContext, ILogger<SetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Set CreateSet(Set set)
        {
            _dbContext.Sets.Add(set);
            _dbContext.SaveChanges();
            return set;
        }

        public List<Set> GetAllSets()
        {
            return _dbContext.Sets.ToList();
        }

        public Set GetSetById(int setId)
        {
            return _dbContext.Sets.FirstOrDefault(s => s.Id == setId);
        }

        public Set UpdateSet(Set updatedSet)
        {
            var existingSet = _dbContext.Sets.Find(updatedSet.Id);

            if (existingSet != null)
            {
                existingSet.Reps = updatedSet.Reps;
                existingSet.Weight = updatedSet.Weight;
                existingSet.Rest = updatedSet.Rest;
                existingSet.Mode = updatedSet.Mode;
                _dbContext.SaveChanges();
                return existingSet;
            }
            _logger.LogTrace("[LOG | SetService] - (UpdateSet): Set not found");
            throw new NotFoundException("[LOG | SetService] - (UpdateSet): Set not found");
        }

        public Set DeleteSet(int setId)
        {
            var setToDelete = _dbContext.Sets.Find(setId);

            if (setToDelete != null)
            {
                _dbContext.Sets.Remove(setToDelete);
                _dbContext.SaveChanges();
                return setToDelete;
            }
            _logger.LogTrace("[LOG | SetService] - (DeleteSet): Set not found");
            throw new NotFoundException("[LOG | SetService] - (DeleteSet): Set not found");
        }
    }
}
