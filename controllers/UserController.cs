// Thư viện cung cấp các class và phương thức MVC như Controller và các thuộc tính như Route, HttpGet, HttpPost, v.v.
using Microsoft.AspNetCore.Mvc;
using netflixdemo.Models; // Chỉnh lại nếu namespace đúng là models
// Thư viện hỗ trợ các hàm cho kiểu dữ liệu collection (List, Dictionary, IEnumerable, v.v.)
using System.Collections.Generic;
using System.Linq;

namespace netflixdemo.Controllers
{
    [Route("api/users")]
    [ApiController]   
    public class UserController : ControllerBase 
    {

        public static List<User> lstUser = new List<User>();

        public UserController()
        {
            // Initialize the list with some users if it's empty
            if (!lstUser.Any())
            {
                for (int id = 1; id < 6; id++)
                {
                    User us = new User
                    {
                        Id = id,
                        Password = "123456789",
                        Email = $"user{id}@gmail.com",
                        Phone = $"09090909{id}"
                    };
                    lstUser.Add(us);
                }
            }
        }

        [HttpGet("getall")]
        public IEnumerable<User> GetAll()
        {
            return lstUser;
        }

        [HttpGet("get/{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = lstUser.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost("create")]
        public ActionResult<User> Create([FromBody] User user)
        {
            user.Id = lstUser.Count + 1;
            lstUser.Add(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            var existingUser = lstUser.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Password = user.Password;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var user = lstUser.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            lstUser.Remove(user);
            return NoContent();
        }
    }
}
