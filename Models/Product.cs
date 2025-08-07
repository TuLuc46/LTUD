using System.ComponentModel.DataAnnotations.Schema;

namespace LTUD.Models
{
    public class Product
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }

        public int Quantity { get; set; }

        public string Image { get; set; }

        //foreign key to Category
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; } 
    }
}
