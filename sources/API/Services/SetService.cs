using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Shared.Exceptions;
using Shared;
using AutoMapper;
using Dommain.Entities;

namespace API.Services
{
    /// <summary>
    /// Service for managing sets.
    /// </summary>
    public class SetService
    {
        private readonly ILogger<SetService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public SetService(IdentityAppDbContext dbContext, ILogger<SetService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new set.
        /// </summary>
        /// <param name="set">The set to be created.</param>
        /// <returns>The created set.</returns>
        public Set CreateSet(Set set)
        {
            _dbContext.Sets.Add(_mapper.Map<SetEntity>(set));
            _dbContext.SaveChanges();
            return set;
        }

        /// <summary>
        /// Gets all sets.
        /// </summary>
        /// <returns>A list of all sets.</returns>
        public List<Set> GetAllSets()
        {
            return _mapper.Map<List<Set>>(_dbContext.Sets.ToList());
        }

        /// <summary>
        /// Gets all Sets (with pages).
        /// </summary>
        /// <returns>A list of all Sets.</returns>
        public PaginationResult<Set> GetAllSetsWithPages(
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Sets.Count();
            var items = _dbContext.Sets
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<Set>
            {
                Items = _mapper.Map<List<Set>>(items),
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets a set by its identifier.
        /// </summary>
        /// <param name="setId">The identifier of the set.</param>
        /// <returns>The set with the specified identifier.</returns>
        public Set GetSetById(int setId)
        {
            return _mapper.Map<Set>(_dbContext.Sets.FirstOrDefault(s => s.Id == setId));
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
                return _mapper.Map<Set>(existingSet);
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
                return _mapper.Map<Set>(setToDelete);
            }

            _logger.LogTrace("[LOG | SetService] - (DeleteSet): Set not found");
            throw new NotFoundException("[LOG | SetService] - (DeleteSet): Set not found");
        }
    }
}
