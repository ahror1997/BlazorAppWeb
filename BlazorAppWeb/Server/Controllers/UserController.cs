using BlazorAppWeb.Server.Interfaces;
using BlazorAppWeb.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await Task.FromResult(_user.GetUsers());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = _user.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost]
        public void Post(User user)
        {
            _user.AddUser(user);
        }

        [HttpPut]
        public void Put(User user)
        {
            _user.UpdateUser(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _user.DeleteUser(id);
            return Ok();
        }
    }
}
