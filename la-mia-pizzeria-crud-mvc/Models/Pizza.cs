using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_crud_mvc.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [MaxLength(500)]
        public string PathImage { get; set; }
        public float Price { get; set; }

        public Pizza(string name, string description, string pathImage, float price)
        {
            this.Name = name;
            this.Description = description;
            this.PathImage = pathImage;
            this.Price = price;
        }
    }
}
