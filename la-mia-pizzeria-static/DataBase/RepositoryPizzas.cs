using la_mia_pizzeria_static.Models;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.DataBase
{
    public class RepositoryPizzas : IRepositoryPizzas
    {

        private PizzaContext _myDatabase;

        public RepositoryPizzas(PizzaContext myDatabase)
        {
            _myDatabase = myDatabase;
        }
        public bool CreatePizza(Pizza newPizza)
        {
            try
            {
                _myDatabase.Add(newPizza);
                _myDatabase.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeletePizza(int id)
        {
            Pizza? pizzaToDelete = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete == null)
            {
                return false;
            }

            _myDatabase.Remove(pizzaToDelete);
            _myDatabase.SaveChanges();


            return true;
        }

        public bool EditPizza(int id, Pizza editedPizza)
        {
            Pizza? pizzaToEdit = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToEdit != null)
            {
                pizzaToEdit.Name = editedPizza.Name;
                pizzaToEdit.Description = editedPizza.Description;
                pizzaToEdit.Price = editedPizza.Price;
                pizzaToEdit.Image = editedPizza.Image;
                pizzaToEdit.CategoryId = editedPizza.CategoryId;

                _myDatabase.SaveChanges();

                return true;
            }
            else
            {
                return false;
            } 

        }

        public Pizza GetPizzaById(int id)
        {
            Pizza? pizza = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category)
                                .Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizza != null)
            {
                return pizza;
            }
            else
            {
                throw new Exception("Pizza non trovata");
            }
        }

        public List<Pizza> GetPizzaByName(string name)
        {
            List<Pizza> searchedPizzas = _myDatabase.Pizzas.Where(pizza => pizza.Name.ToLower().Contains(name.ToLower()))
                                            .Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();
            return searchedPizzas;
        }

        public List<Pizza> GetPizzas()
        {
            List<Pizza> pizzas = _myDatabase.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();

            return pizzas;
        }
    }
}
