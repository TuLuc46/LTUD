using System.ComponentModel.DataAnnotations;

namespace LTUD.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for related products
        public ICollection<Product> Products { get; set; }
    }
}
