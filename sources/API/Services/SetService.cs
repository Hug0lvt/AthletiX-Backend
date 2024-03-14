using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using API.Exceptions;
using API.Repositories;

namespace API.Services
{
    /// <summary>
    /// Service for managing sets.
    /// </summary>
    public class SetService
    {
        private readonly ILogger<SetService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public SetService(IdentityAppDbContext dbContext, ILogger<SetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new set.
        /// </summary>
        /// <param name="set">The set to be created.</param>
        /// <returns>The created set.</returns>
        public Set CreateSet(Set set)
        {
            _dbContext.Sets.Add(set);
            _dbContext.SaveChanges();
            return set;
        }

        /// <summary>
        /// Gets all sets.
        /// </summary>
        /// <returns>A list of all sets.</returns>
        public List<Set> GetAllSets()
        {
            return _dbContext.Sets.ToList();
        }

        /// <summary>
        /// Gets a set by its identifier.
        /// </summary>
        /// <param name="setId">The identifier of the set.</param>
        /// <returns>The set with the specified identifier.</returns>
        public Set GetSetById(int setId)
        {
            return _dbContext.Sets.FirstOrDefault(s => s.Id == setId);
        }

        /// <summary>
        /// Updates an existing set.
        /// </summary>
        /// <param name="updatedSet">The updated set.</param>
        /// <returns>The updated set.</returns>
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

        /// <summary>
        /// Deletes a set by its identifier.
        /// </summary>
        /// <param name="setId">The identifier of the set to be deleted.</param>
        /// <returns>The deleted set.</returns>
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
