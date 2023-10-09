using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static.DataBase
{
    public interface IRepositoryPizzas
    {
        public Pizza GetPizzaById(int id);
        public List<Pizza> GetPizzas();
        public List<Pizza> GetPizzaByName(string name);
        public bool CreatePizza(Pizza newPizza);
        public bool EditPizza(int id, Pizza editedPizza);
        public bool DeletePizza(int id);

    }
}
