using LTUD.Models;
using Microsoft.AspNetCore.Mvc;
using LTUD.ViewModels;
using LTUD.DTO;

namespace LTUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _evn;
        public ProductController(AppDbContext context, IWebHostEnvironment evn)
        {
            _context = context;
            _evn = evn;
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

 
        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProductViewModel model)
        {
            var dto = model.Request;
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId,
                Image = ""
            };
            if(dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine(_evn.WebRootPath, "Upload");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);
                using (var filelStream = new FileStream(filePath, FileMode.Create))
                {
                   await dto.ImageFile.CopyToAsync(filelStream);
                }
                product.Image = filePath;
            }
           
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult LoadProduct(int idCategory)
        {
            var products = _context.Products
                .Where(e => (idCategory == 0 ? true : e.CategoryId == idCategory))
                .Select(e => new ProductDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    Image = e.Image,
                    CategoryId = e.CategoryId
                }).ToList();
            var model = new ProductViewModel();
            model.Products = products;
            return PartialView("_List", model);
        }
    }
}
