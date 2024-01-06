using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class ProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly AppDbContext _dbContext;

        public ProfileService(AppDbContext dbContext, ILogger<ProfileService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Profile CreateProfile(Profile profile)
        {
            _dbContext.Profiles.Add(profile);
            _dbContext.SaveChanges();
            return profile;
        }

        public List<Profile> GetAllProfiles()
        {
            return _dbContext.Profiles.ToList();
        }

        public Profile GetProfileById(int profileId)
        {
            return _dbContext.Profiles.FirstOrDefault(p => p.Id == profileId);
        }

        public Profile UpdateProfile(Profile updatedProfile)
        {
            var existingProfile = _dbContext.Profiles.Find(updatedProfile.Id);

            if (existingProfile != null)
            {
                existingProfile.Username = updatedProfile.Username;
                existingProfile.Role = updatedProfile.Role;
                existingProfile.Age = updatedProfile.Age;
                existingProfile.Email = updatedProfile.Email;
                existingProfile.Weight = updatedProfile.Weight;
                existingProfile.Height = updatedProfile.Height;
                _dbContext.SaveChanges();
                return existingProfile;
            }
            _logger.LogTrace("[LOG | ProfileService] - (UpdateProfile): Profile not found");
            throw new NotFoundException("[LOG | ProfileService] - (UpdateProfile): Profile not found");
        }

        public Profile DeleteProfile(int profileId)
        {
            var profileToDelete = _dbContext.Profiles.Find(profileId);

            if (profileToDelete != null)
            {
                _dbContext.Profiles.Remove(profileToDelete);
                _dbContext.SaveChanges();
                return profileToDelete;
            }
            _logger.LogTrace("[LOG | ProfileService] - (DeleteProfile): Profile not found");
            throw new NotFoundException("[LOG | ProfileService] - (DeleteProfile): Profile not found");
        }
    }
}
