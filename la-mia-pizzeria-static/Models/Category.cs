using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        [Column("id")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "La categoria deve avere un nome")]
        [MaxLength(50, ErrorMessage = "Il nome della categoria può avere un massimo di 50 caratteri")]
        public string Name { get; set; }

        //relazione 1 a n con le pizze
        public List<Pizza>? Pizzas { get; set; }

        public Category() { }
       
    }
}
