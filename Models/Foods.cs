using System.ComponentModel.DataAnnotations.Schema;

namespace LTUD.Models
{
    public class Foods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        // Foreign key to Product
        [ForeignKey("ProductID")]
        public int ProductId { get; set; }

        // Navigation property

        public Product Product { get; set; }
    }
}
