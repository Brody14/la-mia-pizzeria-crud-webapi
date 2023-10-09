using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'ingrediente deve avere un nome")]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Name { get; set; }

        public List<Pizza>? Pizzas { get; set; }

        public Ingredient() { }

    }
}
