using Microsoft.AspNetCore.Mvc;

using ppawproject.Database.Entities;
using ppawproject.Interfaces;

namespace ppawproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registeredUser = await _userService.RegisterUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = registeredUser.Id }, registeredUser);
        }

        // PUT: api/user/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            try 
            {
                var user = await _userService.UpdateUser(id, updatedUser);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/user/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            
            if (result)
                return Ok("Utilizatorul a fost șters cu succes.");
            
            return NotFound("Utilizatorul nu a fost găsit.");
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
                return NotFound("Utilizatorul nu a fost găsit.");

            return Ok(user);
        }

        // GET: api/user/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();

            if (!users.Any())
                return NotFound("Nu s-au găsit utilizatori.");

            return Ok(users);
        }
    }
}
