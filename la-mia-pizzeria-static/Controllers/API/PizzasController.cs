using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.DataBase;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private PizzaContext _myDatabase;

        public PizzasController(PizzaContext db)
        {
            _myDatabase = db;
        }

        [HttpGet]
        public IActionResult GetPizzas()
        {
            List<Pizza> pizzas = _myDatabase.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();

            return Ok(pizzas);
        }

        [HttpGet]
        public IActionResult SearchPizzas(string? search)
        {
            if(search == null)
            {
                return BadRequest(new { Message = "Non hai inserito nessun valore per la ricerca" });
            }

            List<Pizza> searchedPizzas = _myDatabase.Pizzas.Where(pizza => pizza.Name.ToLower().Contains(search.ToLower()))
                                            .Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();

            if(searchedPizzas.Count > 0)
            {
                return Ok(searchedPizzas);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult SearchPizzaById(int id)
        {
            Pizza? pizza = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category)
                                .Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizza != null)
            {
                return Ok(pizza);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult CreatePizza([FromBody] Pizza newPizza)
        {
            try
            {
                _myDatabase.Add(newPizza);
                _myDatabase.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditPizza(int id, [FromBody] Pizza editedPizza)
        {
            Pizza? pizzaToEdit = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToEdit == null)
            {
                return NotFound();
            }

            pizzaToEdit.Name = editedPizza.Name;
            pizzaToEdit.Description = editedPizza.Description;
            pizzaToEdit.Price = editedPizza.Price;
            pizzaToEdit.Image = editedPizza.Image;
            pizzaToEdit.CategoryId = editedPizza.CategoryId;

            _myDatabase.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            Pizza? pizzaToDelete = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete == null)
            {
                return NotFound();
            }

            _myDatabase.Remove(pizzaToDelete);
            _myDatabase.SaveChanges();


            return Ok();
        }

    }
}
