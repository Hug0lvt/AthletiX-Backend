using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Profile APIs")]
    [ApiController]
    [Route("api/profiles")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost(Name = "POST - Entrypoint for create Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateProfile([FromBody] Profile profile)
        {
            var createdProfile = _profileService.CreateProfile(profile);
            return CreatedAtAction(nameof(GetProfileById), new { profileId = createdProfile.Id }, createdProfile);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Profiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllProfiles()
        {
            var profiles = _profileService.GetAllProfiles();
            return Ok(profiles);
        }

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

        [HttpPut("{profileId}", Name = "PUT - Entrypoint for update Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateProfile(int profileId, [FromBody] Profile updatedProfile)
        {
            try
            {
                var profile = _profileService.UpdateProfile(updatedProfile);

                return Ok(profile);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

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
