using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.models;
using api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using api.Filter;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public DemoController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SigninDemo(DemoViewModel demo)
        {

            return Ok(demo);
        }

        [HttpGet]
        // [BlockIp("192.168.1.1", "10.0.0.1", "127.0.0.1")]
        [LogResult]
        public IActionResult GetData()
        {
            return Ok("Okay dữ liệu đã được xử lý");
        }








        // [HttpGet]
        // [Authorize(Roles = Roles.Buyer)]
        // []




    }
}
