using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.DataBase;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize(Roles = "USER,ADMIN")]
    public class PizzaController : Controller
    {
        private ICustomLogger _myLogger;
        private PizzaContext _myDatabase;

        public PizzaController(ICustomLogger logger, PizzaContext db )
        {
            _myLogger = logger;
            _myDatabase = db;
        }
        public IActionResult Index()
        {
            _myLogger.WriteLog("Utente in pizza index");

            List<Pizza> pizzas = _myDatabase.Pizzas.ToList<Pizza>();

            return View("Index", pizzas);
            
        }
        public IActionResult Details(int id)
        {
            Pizza? pizzaDetail = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaDetail == null)
            {
                return NotFound("La pizza che hai cercato non è stata trovata...");
            }
            else
            {
                return View("Details", pizzaDetail);
            }
               
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> categories = _myDatabase.Categories.ToList();

            //reperisco i dati per la select
            List<SelectListItem> ingredientsSelectList = new List<SelectListItem>();
            List<Ingredient> ingredients = _myDatabase.Ingredients.ToList();

            //popoliamo la lista di ingredienti
            foreach(Ingredient ingredient in ingredients)
            {
                ingredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString()});
            }

            //aggiungiamo la lista al model
            PizzaFormModel model = new PizzaFormModel { Pizza = new Pizza(), Categories = categories, Ingredients = ingredientsSelectList };

            return View("Create", model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                //reperisco i dati per la select
                List<SelectListItem> ingredientsSelectList = new List<SelectListItem>();
                List<Ingredient> ingredients = _myDatabase.Ingredients.ToList();

                //popoliamo la lista di ingredienti
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString() });
                }

                data.Ingredients = ingredientsSelectList;

                return View("Create", data);
            }

            //popolo la lista degli ingredienti della pizza creata
            data.Pizza.Ingredients = new List<Ingredient>();

            if(data.SelectedIngredientsId != null)
            {
                foreach(string ingredientSelected in data.SelectedIngredientsId)
                {
                    //converto le stringhe che arrivano dal form in interi
                    int intIngredientSelectedId = int.Parse(ingredientSelected);
                    Ingredient? ingredients = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                    if(ingredients != null)
                    {
                        data.Pizza.Ingredients.Add(ingredients);
                    }
                }
            }

            _myDatabase.Pizzas.Add(data.Pizza);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Pizza? pizzaToEdit = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if(pizzaToEdit == null)
            {
                return NotFound("La pizza non è stata trovata...");
            }else
            {
                List<Category> categories = _myDatabase.Categories.ToList();

                //reperisco i dati per la select
                List<SelectListItem> ingredientsSelectList = new List<SelectListItem>();
                List<Ingredient> ingredients = _myDatabase.Ingredients.ToList();

                //popoliamo la lista di ingredienti
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredientsSelectList.Add(new SelectListItem 
                        {   Text = ingredient.Name, 
                            Value = ingredient.Id.ToString(),
                            //aggiungo gli ingredenti già associati 
                            Selected = pizzaToEdit.Ingredients.Any(associatedIngredient => associatedIngredient.Id == ingredient.Id)
                        });
                }

                PizzaFormModel model = new PizzaFormModel { Pizza = pizzaToEdit, Categories = categories, Ingredients = ingredientsSelectList };

                return View("Edit", model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                //reperisco i dati per la select
                List<SelectListItem> ingredientsSelectList = new List<SelectListItem>();
                List<Ingredient> ingredients = _myDatabase.Ingredients.ToList();

                //popoliamo la lista di ingredienti
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString() });
                }

                data.Ingredients = ingredientsSelectList;

                return View("Edit", data);
            }

            Pizza? pizzaToEdit = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if(pizzaToEdit != null)
            {
                //svuoto la lista esistente di ingredienti
                pizzaToEdit.Ingredients.Clear();

                pizzaToEdit.Name = data.Pizza.Name;
                pizzaToEdit.Description = data.Pizza.Description;
                pizzaToEdit.Price = data.Pizza.Price;
                pizzaToEdit.Image = data.Pizza.Image;
                pizzaToEdit.CategoryId = data.Pizza.CategoryId;

                if (data.SelectedIngredientsId != null)
                {
                    foreach (string ingredientSelected in data.SelectedIngredientsId)
                    {
                        //converto le stringhe che arrivano dal form in interi
                        int intIngredientSelectedId = int.Parse(ingredientSelected);
                        Ingredient? ingredients = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                        if (ingredients != null)
                        {
                            pizzaToEdit.Ingredients.Add(ingredients);
                        }
                    }
                }

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }else
            {
                return NotFound("La pizza non è stata trovata...");
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza? pizzaToDelete = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _myDatabase.Pizzas.Remove(pizzaToDelete);

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La pizza non è stata trovata...");
            }
           
        }
    }

}
