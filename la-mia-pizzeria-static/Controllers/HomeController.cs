using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.DataBase;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    public class HomeController : Controller
    {
        private ICustomLogger _myLogger;
        private IRepositoryPizzas _repoPizzas;
        private PizzaContext _myDatabase;

        public HomeController(ICustomLogger logger, IRepositoryPizzas repoPizzas, PizzaContext myDatabase)
        {
            _myLogger = logger;
            _repoPizzas = repoPizzas;
            _myDatabase = myDatabase;
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("Utente in home index");

            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizzas = db.Pizzas.ToList<Pizza>();

                return View("Index", pizzas);
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza? pizzaDetail = db.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (pizzaDetail == null)
                {
                    return NotFound($"La pizza che hai cercato non è stata trovata...");
                }
                else
                {
                    return View("Details", pizzaDetail);
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Create()
        {
            //List<Category> categories = _myDatabase.Categories.ToList();

            ////reperisco i dati per la select
            //List<SelectListItem> ingredientsSelectList = new List<SelectListItem>();
            //List<Ingredient> ingredients = _myDatabase.Ingredients.ToList();

            ////popoliamo la lista di ingredienti
            //foreach (Ingredient ingredient in ingredients)
            //{
            //    ingredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString() });
            //}

            ////aggiungiamo la lista al model
            //PizzaFormModel model = new PizzaFormModel { Pizza = new Pizza(), Categories = categories, Ingredients = ingredientsSelectList };

            return View("Create", "Home");
        }
    }
}