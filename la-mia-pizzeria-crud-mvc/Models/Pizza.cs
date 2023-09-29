namespace la_mia_pizzeria_crud_mvc.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PathImage { get; set; }
        public float Price { get; set; }

        public Pizza(string name, string description, string urlImage, float price)
        {
            this.Name = name;
            this.Description = description;
            this.PathImage = urlImage;
            this.Price = price;
        }
    }
}
