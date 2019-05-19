using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Business;
using UsersAPI.Models;
using Microsoft.AspNetCore.Authorization; 
  
namespace UsersAPI.Controllers  
{  
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _service.ListAll();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _service.Retrieve(id);
            if (user != null)
                return new ObjectResult(user);
            else
                return NotFound();
        }

        [HttpPost]
        public Result Post([FromBody]User user)
        {
            return _service.Add(user);
        }

        [HttpPut]
        public Result Put([FromBody]User user)
        {
            return _service.Update(user);
        }

        [HttpDelete("{id}")]
        public Result Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}