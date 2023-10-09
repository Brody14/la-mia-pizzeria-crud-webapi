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

            List<Pizza> searchedPizzas = _myDatabase.Pizzas.Where(pizza => pizza.Name.ToLower().Contains(search.ToLower())).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();

            if(searchedPizzas.Count > 0)
            {
                return Ok(searchedPizzas);
            }
            else
            {
                return NotFound();
            }
        }

       

        
    }
}
