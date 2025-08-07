using LTUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LTUD.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        //phan trang Category
        public IActionResult Index(string keyWord = "", int pageIndex = 1, int pageSize = 10)
        {
            var pagingInfo = new PagingInfo
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
            CategoryViewModel model = new CategoryViewModel();
            model.txtKey = keyWord;
            if (string.IsNullOrEmpty(keyWord))
            {
                var ls = _context.Categories.
                    Select(e => new CategoryDTO { Id = e.Id, Name = e.Name });

                pagingInfo.Total = ls.Count();
                model.Categories = ls.Skip(pagingInfo.PageSize * (pagingInfo.PageIndex - 1))
                                     .Take(pagingInfo.PageSize)
                                     .ToList();
            }
            else
            {
                var ls = _context.Categories.Where(e => e.Name.Contains(keyWord)).
                    Select(e => new CategoryDTO { Id = e.Id, Name = e.Name });
                pagingInfo.Total = ls.Count();
                model.Categories = ls.ToList();
            }
            model.PagingInfo = pagingInfo;
            return View(model);
        }

        ////tim kiem Category
        //public IActionResult Index(string txtKey = " ")
        //{
        //    CategoryViewModel model = new CategoryViewModel();
        //    if (txtKey.IsNullOrEmpty())
        //    {
        //        model.Categories = _context.Categories.Select(e => new CategoryDTO { Id = e.Id, Name = e.Name }).ToList();
        //    }
        //    else
        //    {
        //        var ls = _context.Categories.Where(e => e.Name.Contains(txtKey)).Select(e => new CategoryDTO { Id = e.Id, Name = e.Name }).ToList();
        //        model.Categories = ls;
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Create(int Id)
        {
            var category = _context.Categories.Where(e => e.Id == Id).FirstOrDefault();
            CategoryViewModel model = new CategoryViewModel();
            if (category != null)
            {
                model.Request = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
            else
            {
                model.Request = new CategoryDTO();
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            var category = _context.Categories.Where(e => e.Name == model.Request.Name).FirstOrDefault();
            if (category != null)
            {
                ViewData["name"] = model.Request.Name;
                ViewData["message"] = "Category already exists!";
                return View();
            }
            else
            {
                category = new Category
                {
                    Name = model.Request.Name
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }

        //Update Category
        [HttpGet]
        public IActionResult Update(long Id)
        {
            CategoryViewModel model = new CategoryViewModel();
            var category = _context.Categories.Where(e => e.Id == Id).Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name
            }).FirstOrDefault();
            model.Respone = category;
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            var category = _context.Categories.Where(e => e.Id == model.Request.Id).FirstOrDefault();
            if (category != null)
            {
                category.Name = model.Request.Name;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["name"] = model.Request.Name;
                ViewData["message"] = "Cập nhật không thành công!";
                return View("Update");
            }
        }
        //Delete Category
        [HttpGet]
        public IActionResult Delete(long id)
        {
            var category = _context.Categories.Where(e => e.Id == id).FirstOrDefault();
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["message"] = "Category not found!";
                return View("Index");
            }
        }
    
    
    }

    //doi tuong lien ket giua View va Model
    public class CategoryViewModel
    {
        public string txtKey { get; set; }
        public List<CategoryDTO> Categories { get; set; }

        public CategoryDTO Request { get; set; } // Dùng để lưu thông tin Category khi tạo mới

        public CategoryDTO Respone { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
    
    //cau truc de van chuyen Category
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    //trang cho Category
    public class PagingInfo
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int Total { get; set; }
        public int PageCount
        {
            get
            {
                return Total / PageSize + (Total % PageSize != 0 ? 1 : 0);
            }
        }

    }
}
