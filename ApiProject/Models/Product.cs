using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public int Stock {get; set;}
    }
}
