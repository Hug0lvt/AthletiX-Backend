using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to profiles.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Profile APIs")]
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="profileService">The profile service.</param>
        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        /// <summary>
        /// Creates a new profile.
        /// </summary>
        /// <param name="profile">The profile to create.</param>
        /// <returns>The newly created profile.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateProfile([FromBody] Profile profile)
        {
            var createdProfile = _profileService.CreateProfile(profile);
            return CreatedAtAction(nameof(GetProfileById), new { profileId = createdProfile.Id }, createdProfile);
        }

        /// <summary>
        /// Gets a profile by its identifier.
        /// </summary>
        /// <param name="profilEmail">The profile identifier.</param>
        /// <returns>The profile with the specified identifier.</returns>
        [HttpGet("email/{profilEmail}", Name = "GET - Entrypoint for get Profile by Email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProfileByEmail(string profilEmail)
        {
            var profile = _profileService.GetProfileByEmail(profilEmail);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        /// <summary>
        /// Gets a profile by its identifier.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <returns>The profile with the specified identifier.</returns>
        [HttpGet("{profileId}", Name = "GET - Entrypoint for get Profile by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProfileById(int profileId)
        {
            var profile = _profileService.GetProfileById(profileId);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        /// <summary>
        /// Updates a profile.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="updatedProfile">The updated profile information.</param>
        /// <returns>The updated profile.</returns>
        [HttpPut("{profileId}", Name = "PUT - Entrypoint for update Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateProfile(int profileId, [FromBody] Profile updatedProfile)
        {
            try
            {
                if(updatedProfile.Id != profileId) updatedProfile.Id = profileId;
                var profile = _profileService.UpdateProfile(updatedProfile);
                return Ok(profile);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a profile by its identifier.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <returns>The deleted profile.</returns>
        [HttpDelete("{profileId}", Name = "DELETE - Entrypoint for remove Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProfile(int profileId)
        {
            try
            {
                var deletedProfile = _profileService.DeleteProfile(profileId);
                return Ok(deletedProfile);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
