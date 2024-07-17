using Microsoft.AspNetCore.Mvc;
using api.Models; // Chỉnh lại nếu namespace đúng là models
using System.Collections.Generic;
using System.Linq;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
         private readonly ApplicationContext _db;
        public ProductController(ApplicationContext db)
        {
            _db = db;
        }
        [HttpGet("getall")]
        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> lst = _db.Products.ToList();
            return lst;
        }

        [HttpGet("getall/{id}")]
        public Product GetById(int id)
        {
            Product res = _db.Products.SingleOrDefault(n=>n.Id == id);
            return res;
        }
    }
}
