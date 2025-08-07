using LTUD.Models;
using Microsoft.AspNetCore.Mvc;
using LTUD.ViewModels;
using LTUD.DTO;

namespace LTUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new ProductViewModel();
            var cats = _context.Categories
                .Select(e => new CategoryDTO { Id = e.Id, Name = e.Name }) // lấy ra để không phụ thuộc database
                .ToList();
            model.Categories = cats;
            var products = _context.Products
                .Select(e => new ProductDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    Image = e.Image,
                    CategoryId = e.CategoryId
                })
                .ToList();
            model.Products = products;

            return View(model);
        }
    }
}
