using la_mia_pizzeria_static.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome della pizza è obbligatorio")]
        [MaxLength(50, ErrorMessage = "Il nome può avere un massimo di 50 caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [MoreThenFiveWordValidation]
        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Required(ErrorMessage = "L'immagine è obbligatoria")]
        [Url(ErrorMessage = "Devi inserire un URL valido")]
        [MaxLength(500, ErrorMessage = "L'URL può avere un massimo di 500 caratteri")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Range(0, 999, ErrorMessage = "Il prezzo non può superare i 999 Euro")]
        [Column(TypeName = "decimal(5, 2)")]
        public double Price { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }

        //relazione 1 a n con le categorie

        public int? CategoryId { get; set; }
        public Category? Category {get; set;}

        //relazione n a n con gli ingredienti
        public List<Ingredient>? Ingredients { get; set; }

        public Pizza() { }

        public Pizza(string name, string description, string image, double price, int rating)
        {
            this.Name = name;
            this.Description = description;
            this.Image = image;
            this.Price = price;
            this.Rating = rating;
        }
    }
}
