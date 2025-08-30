using api.Data;
using api.Models;
using api.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{

    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly HtmlSanitizer _htmlSanitizer = new();

        [HttpPost("create")]
        public async Task<ActionResult> AddProduct(ProductPostViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();
            // if (!ModelState.IsValid) return BadRequest();

            // Sanera datat...
            model.Brand = _htmlSanitizer.Sanitize(model.Brand);
            model.Category = _htmlSanitizer.Sanitize(model.Category);
            model.ItemNumber = _htmlSanitizer.Sanitize(model.ItemNumber);
            model.Name = _htmlSanitizer.Sanitize(model.Name);
            model.Description = _htmlSanitizer.Sanitize(model.Description ?? "");

            // Omvalidera modellen...
            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid) return ValidationProblem();

            // Mappa modellen till vår entity(Product klass)...
            var product = new Product
            {
                Brand = model.Brand,
                Category = model.Category,
                ItemNumber = model.ItemNumber,
                Name = model.Name,
                Description = model.Description ?? "",
                Price = model.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(FindProduct), new { id = product.Id });

        }

        [HttpGet]
        public async Task<ActionResult> ListAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(new { success = true, data = products });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> FindProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return NotFound();

            return Ok(new { success = true, data = product });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
