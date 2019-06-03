using Microsoft.AspNetCore.Mvc;
using UsersAPI.Business;
using System.Collections.Generic;
using UsersAPI.Models;

namespace UsersAPI.Controllers
{
    [Route("api/v1/[controller]")]
    public class CallbackController : Controller
    {
        private UserService _service;

        public CallbackController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{access_token}")]        
        public string Get(string access_token)
        {
            return _service.StoreSpotifyToken(access_token);
        }
    }
}