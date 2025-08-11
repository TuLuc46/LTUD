using LTUD.Controllers;
using LTUD.DTO;

namespace LTUD.ViewModels
{
    public class ProductViewModel
    {
        public List<CategoryDTO> Categories { get; set; }
        public List<ProductDTO> Products { get; set; } 
        public ProductDTO Request { get; set; }
        public ProductDTO Respone { get; set; }
    }
}
