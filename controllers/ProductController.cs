using Microsoft.AspNetCore.Mvc;
using netflixdemo.Data;
using netflixdemo.Models; // Chỉnh lại nếu namespace đúng là models
using System.Collections.Generic;
using System.Linq;

namespace netflixdemo.Controllers
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
            return _db.Product.ToList();
        }




        public static  List<Product> lstProduct = new List<Product>();

        // public ProductController()
        // {
        //     // Initialize the list with some products if it's empty
        //     if (!lstProduct.Any())
        //     {
        //         for (int id = 1; id < 6; id++)
        //         {
        //             Product prod = new Product
        //             {
        //                 Id = id,
        //                 Name = $"product {id}",
        //                 Price = id * 1000,
        //                 Description = $"{id} desc product",
        //             };
        //             lstProduct.Add(prod);
        //         }
        //     }
        // }

      

        [HttpGet("get/{id}")]
        public ActionResult<Product> Get(int id)
        {
            var product = lstProduct.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost("create")]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            product.Id = lstProduct.Count + 1;
            lstProduct.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            var existingProduct = lstProduct.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = lstProduct.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            lstProduct.Remove(product);
            return NoContent();
        }
    }
}
