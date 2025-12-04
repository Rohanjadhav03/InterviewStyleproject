using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeApi.Models;
using PracticeApi.Services;

namespace PracticeApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService Service;

        public UserController(IUserService _Service)
        {
            Service = _Service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await Service.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUesr([FromBody] User user)
        {
            try
            {
                var result = await Service.CreateUserAsync(user);
                return Ok(new {message = "User Created Successfully",affectedrows=result});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CreateUesr(int id)
        {
            try
            {
                var result = await Service.DeleteUserAsync(id);
                return Ok(new { message = "User Deleted Successfully", affectedrows = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
