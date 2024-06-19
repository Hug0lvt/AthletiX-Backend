using System.Collections.Generic;
using Dommain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Shared;
using Shared.Exceptions;

namespace API.Services
{
    /// <summary>
    /// Service for managing user profiles.
    /// </summary>
    public class ProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly AutoMapper.IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public ProfileService(IdentityAppDbContext dbContext, ILogger<ProfileService> logger, AutoMapper.IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new user profile.
        /// </summary>
        /// <param name="profile">The profile to be created.</param>
        /// <returns>The created profile.</returns>
        public Profile CreateProfile(Profile profile)
        {
            try
            {
                _dbContext.Profiles.Add(_mapper.Map<ProfileEntity>(profile));
                _dbContext.SaveChanges();
                return profile;
            } catch (DbUpdateException e)
            {
                _logger.LogTrace("[LOG | ProfileService] - (CreateProfile): Profile exist");
                return _mapper.Map<Profile>(_dbContext.Profiles.FirstOrDefault(p => p.Email == profile.Email));   
            }
        }

        /// <summary>
        /// Gets all user profiles.
        /// </summary>
        /// <returns>A list of all user profiles.</returns>
        public List<Profile> GetAllProfiles()
        {
            return _mapper.Map<List<Profile>>(_dbContext.Profiles.ToList());
        }

        /// <summary>
        /// Gets a user profile by its identifier.
        /// </summary>
        /// <param name="profileId">The identifier of the profile.</param>
        /// <returns>The profile with the specified identifier.</returns>
        public Profile GetProfileById(int profileId)
        {
            return _mapper.Map<Profile>(_dbContext.Profiles.FirstOrDefault(p => p.Id == profileId));
        }

        /// <summary>
        /// Gets a user profile by email identifier.
        /// </summary>
        /// <param name="profileEmail">The identifier of the profile.</param>
        /// <returns>The profile with the specified identifier.</returns>
        public Profile GetProfileByEmail(string profileEmail)
        {
            return _mapper.Map<Profile>(_dbContext.Profiles.FirstOrDefault(p => p.Email == profileEmail));
        }

        /// <summary>
        /// Updates an existing user profile.
        /// </summary>
        /// <param name="updatedProfile">The updated profile.</param>
        /// <returns>The updated profile.</returns>
        public Profile UpdateProfile(Profile updatedProfile)
        {
            var existingProfile = _dbContext.Profiles.Find(updatedProfile.Id);

            if (existingProfile != null)
            {
                existingProfile.Username = updatedProfile.Username;
                existingProfile.Role = updatedProfile.Role;
                existingProfile.Age = updatedProfile.Age;
                //existingProfile.Email = updatedProfile.Email;
                existingProfile.Weight = updatedProfile.Weight;
                existingProfile.Height = updatedProfile.Height;
                existingProfile.Picture = updatedProfile.Picture;
                existingProfile.Gender = updatedProfile.Gender;
                _dbContext.SaveChanges();
                return _mapper.Map<Profile>(existingProfile);
            }

            _logger.LogTrace("[LOG | ProfileService] - (UpdateProfile): Profile not found");
            throw new NotFoundException("[LOG | ProfileService] - (UpdateProfile): Profile not found");
        }

        /// <summary>
        /// Updates an existing user profile.
        /// </summary>
        /// <param name="updatedProfile">The updated profile.</param>
        /// <returns>The updated profile.</returns>
        public Profile UpdateUniqueNotificationToken(Profile updatedProfile)
        {
            var existingProfile = _dbContext.Profiles.Find(updatedProfile.Id);

            if (existingProfile != null)
            {
                existingProfile.UniqueNotificationToken = updatedProfile.UniqueNotificationToken;
                _dbContext.SaveChanges();
                return _mapper.Map<Profile>(existingProfile);
            }

            _logger.LogTrace("[LOG | ProfileService] - (UpdateUniqueNotificationToken): Profile not found");
            throw new NotFoundException("[LOG | ProfileService] - (UpdateUniqueNotificationToken): Profile not found");
        }


        /// <summary>
        /// Deletes a user profile by its identifier.
        /// </summary>
        /// <param name="profileId">The identifier of the profile to be deleted.</param>
        /// <returns>The deleted profile.</returns>
        public Profile DeleteProfile(int profileId)
        {
            var profileToDelete = _dbContext.Profiles.Find(profileId);

            if (profileToDelete != null)
            {
                _dbContext.Profiles.Remove(profileToDelete);
                _dbContext.SaveChanges();
                return _mapper.Map<Profile>(profileToDelete);
            }

            _logger.LogTrace("[LOG | ProfileService] - (DeleteProfile): Profile not found");
            throw new NotFoundException("[LOG | ProfileService] - (DeleteProfile): Profile not found");
        }

        public async Task<PaginationResult<Profile>> SearchProfilesAsync(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new PaginationResult<Profile>
                {
                    Items = new List<Profile>(),
                    TotalItems = 0
                };
            }

            var returnedProfiles = await _dbContext.Profiles
                .Where(p => p.Username.Contains(value) || p.Email.Contains(value))
                .ToListAsync();

            return new PaginationResult<Profile>
            {
                Items = _mapper.Map<List<Profile>>(returnedProfiles),
                TotalItems = returnedProfiles.Count()
            };
        }
    }
}
