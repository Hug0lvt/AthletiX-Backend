﻿using System.Collections.Generic;
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
    /// Service for managing sessions.
    /// </summary>
    public class SessionService
    {
        private readonly ILogger<SessionService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public SessionService(IdentityAppDbContext dbContext, ILogger<SessionService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="session">The session to be created.</param>
        /// <returns>The created session.</returns>
        public async Task<Session> CreateSessionAsync(Session session)
        {
            try
            {
                var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == session.Profile.Id);
                if (existingProfile == null)
                    throw new NotCreatedExecption("Profile does not exist.");

                var entity = _mapper.Map<SessionEntity>(session);

                entity.ProfileId = existingProfile.Id;
                entity.Profile = null;
                _dbContext.Entry(existingProfile).State = EntityState.Unchanged;

                _dbContext.Sessions.Add(entity);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<Session>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create session.", ex);
            }
        }

        /// <summary>
        /// Gets all sessions.
        /// </summary>
        /// <returns>A list of all sessions.</returns>
        public List<Session> GetAllSessions()
        {
            return _mapper.Map<List<Session>>(_dbContext.Sessions.ToList());
        }

        /// <summary>
        /// Gets all Session (with pages).
        /// </summary>
        /// <returns>A list of all Session.</returns>
        public PaginationResult<Session> GetAllSessionsWithPages(
            int pageSize = 10,
            int pageNumber = 0,
            bool includeExercise = false)
        {
            var totalItems = _dbContext.Sessions.Count();
            var items = _mapper.Map<List<Session>>(_dbContext.Sessions
                .Include(s => s.Profile)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList());

            if (includeExercise)
            {
                foreach (var session in items)
                {
                    if (session != null)
                    {
                        List<PracticalExercise> exercises = _mapper.Map<List<PracticalExercise>>(_dbContext.PracticalExercises
                            .Include(p=> p.Exercise)
                            .Where(e => e.SessionId == session.Id).ToList());
                        session.Exercises = exercises;
                    }
                }
            }

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
        public PaginationResult<Session> GetSessionsFromUser(int id, bool includeExercise = false)
        {
            var items = _mapper.Map<List<Session>>(_dbContext.Sessions
                .Include(s => s.Profile)
                .Where(q => q.Profile.Id == id).ToList());
            var totalItems = items.Count();

            if (includeExercise)
            {
                foreach (var session in items)
                {
                    if (session != null)
                    {
                        List<PracticalExercise> exercises = _mapper.Map<List<PracticalExercise>>(_dbContext.PracticalExercises
                            .Include(p=> p.Exercise)
                            .Where(e => e.SessionId == session.Id).ToList());
                        session.Exercises = exercises;
                    }
                }
            }

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
            Session session = _mapper.Map<Session>(_dbContext.Sessions.Include(s => s.Profile).FirstOrDefault(s => s.Id == sessionId));
            if (session != null)
            {
                List<PracticalExercise> exercises = _mapper.Map<List<PracticalExercise>>(_dbContext.PracticalExercises
                    .Include(p=> p.Exercise).ThenInclude(e => e.Category)
                    .Where(e => e.SessionId == session.Id).ToList());
                session.Exercises = exercises;
            }
            return session;
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
                existingSession.Status = updatedSession.Status;
                _dbContext.SaveChanges();
                return _mapper.Map<Session>(existingSession);
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
                return _mapper.Map<Session>(sessionToDelete);
            }

            _logger.LogTrace("[LOG | SessionService] - (DeleteSession): Session not found");
            throw new NotFoundException("[LOG | SessionService] - (DeleteSession): Session not found");
        }
        
        /// <summary>
        /// Updates status of an existing session.
        /// </summary>
        public Session UpdateStatusSession(int sessionId, int value)
        {
            var existingSession = _dbContext.Sessions
                .FirstOrDefault(s => s.Id == sessionId);
            if (existingSession != null)
            {
                int activeSessions = _dbContext.Sessions
                    .Where(s => s.ProfileId == existingSession.ProfileId)
                    .Select(s => s.Status == 1).Count();

                if (activeSessions != 1 && value != 1)
                {
                    existingSession.Status = value;
                    _dbContext.SaveChanges();
                    return _mapper.Map<Session>(existingSession);
                }
                else
                {
                    throw new ConflictExecption($"{activeSessions} Session(s) is already active");
                }
            }

            _logger.LogTrace("[LOG | SessionService] - (UpdateSession): Session not found");
            throw new NotFoundException("[LOG | SessionService] - (UpdateSession): Session not found");
        }
    }
}
