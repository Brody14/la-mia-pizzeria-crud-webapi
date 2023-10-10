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
        private IRepositoryPizzas _repoPizzas;
       
        public PizzasController(IRepositoryPizzas repoPizzas)
        {
            _repoPizzas = repoPizzas;
        }

        [HttpGet]
        public IActionResult GetPizzas()
        {
            List<Pizza> pizzas = _repoPizzas.GetPizzas();

            return Ok(pizzas);
        }

        [HttpGet]
        public IActionResult SearchPizza(string? search)
        {
            List<Pizza> searchedPizzas;

            if (search == null)
            {
                searchedPizzas = _repoPizzas.GetPizzas();
            }
            else
            {
                searchedPizzas = _repoPizzas.GetPizzaByName(search);
            }

            return Ok(searchedPizzas);
      
        }

        [HttpGet("{id}")]
        public IActionResult GetPizzaById(int id)
        {
            Pizza? pizza = _repoPizzas.GetPizzaById(id);

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
                bool result = _repoPizzas.CreatePizza(newPizza);

                if(result)
                {
                 return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditPizza(int id, [FromBody] Pizza editedPizza)
        {
            bool result = _repoPizzas.EditPizza(id, editedPizza);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
           
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            bool result = _repoPizzas.DeletePizza(id);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

    }
}
