using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Set APIs")]
    [ApiController]
    [Route("api/sets")]
    public class SetController : ControllerBase
    {
        private readonly SetService _setService;

        public SetController(SetService setService)
        {
            _setService = setService;
        }

        [HttpPost(Name = "POST - Entrypoint for create Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateSet([FromBody] Set set)
        {
            var createdSet = _setService.CreateSet(set);
            return CreatedAtAction(nameof(GetSetById), new { setId = createdSet.Id }, createdSet);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Sets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSets()
        {
            var sets = _setService.GetAllSets();
            return Ok(sets);
        }

        [HttpGet("{setId}", Name = "GET - Entrypoint for get Set by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSetById(int setId)
        {
            var set = _setService.GetSetById(setId);

            if (set == null)
            {
                return NotFound();
            }

            return Ok(set);
        }

        [HttpPut("{setId}", Name = "PUT - Entrypoint for update Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateSet(int setId, [FromBody] Set updatedSet)
        {
            try
            {
                var set = _setService.UpdateSet(updatedSet);

                return Ok(set);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{setId}", Name = "DELETE - Entrypoint for remove Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteSet(int setId)
        {
            try
            {
                var deletedSet = _setService.DeleteSet(setId);

                return Ok(deletedSet);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
