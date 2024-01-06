using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class SessionService
    {
        private readonly ILogger<SessionService> _logger;
        private readonly AppDbContext _dbContext;

        public SessionService(AppDbContext dbContext, ILogger<SessionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Session CreateSession(Session session)
        {
            _dbContext.Sessions.Add(session);
            _dbContext.SaveChanges();
            return session;
        }

        public List<Session> GetAllSessions()
        {
            return _dbContext.Sessions.ToList();
        }

        public Session GetSessionById(int sessionId)
        {
            return _dbContext.Sessions.FirstOrDefault(s => s.Id == sessionId);
        }

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
