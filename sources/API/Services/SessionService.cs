using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using API.Exceptions;
using API.Repositories;
using Shared.Mappers;

namespace API.Services
{
    /// <summary>
    /// Service for managing sessions.
    /// </summary>
    public class SessionService
    {
        private readonly ILogger<SessionService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public SessionService(IdentityAppDbContext dbContext, ILogger<SessionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="session">The session to be created.</param>
        /// <returns>The created session.</returns>
        public Session CreateSession(Session session)
        {
            _dbContext.Sessions.Add(session);
            _dbContext.SaveChanges();
            return session;
        }

        /// <summary>
        /// Gets all sessions.
        /// </summary>
        /// <returns>A list of all sessions.</returns>
        public List<Session> GetAllSessions()
        {
            return _dbContext.Sessions.ToList();
        }

        /// <summary>
        /// Gets all Session (with pages).
        /// </summary>
        /// <returns>A list of all Session.</returns>
        public PaginationResult<Session> GetAllSessionsWithPages(
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Sessions.Count();
            var items = _dbContext.Sessions
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<Session>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets Session from user id.
        /// </summary>
        /// <returns>A list of all Session for one user.</returns>
        public PaginationResult<Session> GetSessionsFromUser(int id)
        {
            var items = _dbContext.Sessions.Where(q => q.Profile.Id == id).ToList();
            var totalItems = items.Count();

            return new PaginationResult<Session>
            {
                Items = items,
                NextPage = -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets a session by its identifier.
        /// </summary>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>The session with the specified identifier.</returns>
        public Session GetSessionById(int sessionId)
        {
            return _dbContext.Sessions.FirstOrDefault(s => s.Id == sessionId);
        }

        /// <summary>
        /// Updates an existing session.
        /// </summary>
        /// <param name="updatedSession">The updated session.</param>
        /// <returns>The updated session.</returns>
        public Session UpdateSession(Session updatedSession)
        {
            var existingSession = _dbContext.Sessions.Find(updatedSession.Id);

            if (existingSession != null)
            {
                existingSession.Name = updatedSession.Name;
                existingSession.Date = updatedSession.Date;
                existingSession.Duration = updatedSession.Duration;
                _dbContext.SaveChanges();
                return existingSession;
            }

            _logger.LogTrace("[LOG | SessionService] - (UpdateSession): Session not found");
            throw new NotFoundException("[LOG | SessionService] - (UpdateSession): Session not found");
        }

        /// <summary>
        /// Deletes a session by its identifier.
        /// </summary>
        /// <param name="sessionId">The identifier of the session to be deleted.</param>
        /// <returns>The deleted session.</returns>
        public Session DeleteSession(int sessionId)
        {
            var sessionToDelete = _dbContext.Sessions.Find(sessionId);

            if (sessionToDelete != null)
            {
                _dbContext.Sessions.Remove(sessionToDelete);
                _dbContext.SaveChanges();
                return sessionToDelete;
            }

            _logger.LogTrace("[LOG | SessionService] - (DeleteSession): Session not found");
            throw new NotFoundException("[LOG | SessionService] - (DeleteSession): Session not found");
        }
    }
}
