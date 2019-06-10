using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Business;
using UsersAPI.Models;
using Microsoft.AspNetCore.Authorization; 
  
namespace UsersAPI.Controllers  
{  
    [Route("api/v1/avaliacoes")]
    public class RatingsController : Controller
    {
        private RatingService _service;

        public RatingsController(RatingService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Rating> Get()
        {
            return _service.ListAll();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var rating = _service.Retrieve(id);
            if (rating != null)
                return new ObjectResult(rating);
            else
                return NotFound();
        }

        [HttpPost]
        public Result Post([FromBody]Rating rating)
        {
            return _service.Add(rating);
        }

        [HttpPut]
        public Result Put([FromBody]Rating rating)
        {
            return _service.Update(rating);
        }

        [HttpDelete("{id}")]
        public Result Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}