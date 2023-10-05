using la_mia_pizzeria_crud_mvc.CustomLoggers;
using la_mia_pizzeria_crud_mvc.Database;
using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace la_mia_pizzeria_crud_mvc.Controllers
{
    public class PizzaController : Controller
    {
        // private CustomConsoleLogger _myConsoleLogger;
        // private CustomFileLogger _myFileLogger;

        //private ICustomLogger _myConsoleLogger;
        private ICustomLogger _myFileLogger;

        private PizzeriaContext _myDataBase;

        public PizzaController(ICustomLogger _fileLogger, PizzeriaContext db)
        {
            //_myConsoleLogger = new CustomConsoleLogger();
            //_myFileLogger = new CustomFileLogger();
            //_myConsoleLogger = _consoleLogger;

            _myFileLogger = _fileLogger;
            _myDataBase = db;
        }


        public IActionResult Index()
        {
            //_myConsoleLogger.WriteLog("L'utente è entrato nella vista Pizza > Index");

            _myFileLogger.WriteLog("L'utente è entrato nella vista Pizza > Index");

            List<Pizza> pizzasList = _myDataBase.Pizzas.ToList<Pizza>();

            return View("index", pizzasList);
        }


        // DETAILS -------------------------------

        public IActionResult Details(int id)
        {
            // punto interrogativo per mettere in conto che potrei ricevere un oggetto pizza nullo
            // aggiungo l'include degli ingredienti
            Pizza? foundedPizza = _myDataBase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (foundedPizza == null)
            {
                return NotFound($"La pizza con id {id} non è stata trovata");
            }
            else
            {
                return View("Details", foundedPizza);
            }
        }



        // CREATE ---------------------------------

        [HttpGet]
        public IActionResult Create()
        {
            // devo passare la lista alla create
            List<Category> categories = _myDataBase.Categories.ToList();

            // istanzio una nuova lista di tipo SelectListItem che andrò a popolare
            List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();

            // prendo dal database la lista degli ingredienti
            List<Ingredient> dbAllIngredients = _myDataBase.Ingredients.ToList();

            foreach (Ingredient ingredient in dbAllIngredients)
            {
                allIngredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString() });
            }

            // aggiungo la lista degli ingredienti al modello che passo alla vista
            PizzaFormModel model = new PizzaFormModel { Pizza = new Pizza(), Categories = categories, Ingredients = allIngredientsSelectList };

            return View("Create", model);
        }


        [HttpPost]
        //meccanismo di sicurezza che aiuta a prevenire attacchi CSRF
        [ValidateAntiForgeryToken]
        //public IActionResult Create(Pizza pizzaCreated)
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDataBase.Categories.ToList();
                data.Categories = categories;

                // ingredienti
                List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
                List<Ingredient> dbAllIngredients = _myDataBase.Ingredients.ToList();

                foreach (Ingredient ingredient in dbAllIngredients)
                {
                    allIngredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString() });
                }

                data.Ingredients = allIngredientsSelectList;

                return View("Create", data);
            }

            data.Pizza.Ingredients = new List<Ingredient>();

            if (data.SelectedIngredientsId != null)
            {
                foreach (string ingredientSelectedId in data.SelectedIngredientsId)
                {
                    int intIngredientSelectedId = int.Parse(ingredientSelectedId);

                    Ingredient? ingredientInDb = _myDataBase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                    if (ingredientInDb != null)
                    {
                        data.Pizza.Ingredients.Add(ingredientInDb);
                    }
                }
            }

            //_myDataBase.Pizzas.Add(pizzaCreated);

            _myDataBase.Pizzas.Add(data.Pizza);
            _myDataBase.SaveChanges();

            return RedirectToAction("Index");
        }


        // UPDATE ---------------------------------

        [HttpGet]
        public IActionResult Update(int id)
        {
            Pizza? pizzaToUpdate = _myDataBase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToUpdate == null)
            {
                return NotFound("La pizza che vorresti modificare non è stata trovata");
            }
            else
            {
                List<Category> categories = _myDataBase.Categories.ToList();

                PizzaFormModel model = new PizzaFormModel { Pizza = pizzaToUpdate, Categories = categories };

                return View("Update", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDataBase.Categories.ToList();
                data.Categories = categories;

                return View("Update", data);
            }

            // oppure metodo alternativo
            //Pizza? pizzaToUpdate = _myDataBase.Pizzas.Find(id);

            Pizza? pizzaToUpdate = _myDataBase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();


            if (pizzaToUpdate != null)
            {
                //pizzaToUpdate.Name = modifiedPizza.Name;
                //pizzaToUpdate.Description = modifiedPizza.Description;
                //pizzaToUpdate.PathImage = modifiedPizza.PathImage;
                //pizzaToUpdate.Price = modifiedPizza.Price;

                EntityEntry<Pizza> entityEntry = _myDataBase.Entry(pizzaToUpdate);
                entityEntry.CurrentValues.SetValues(data.Pizza);

                _myDataBase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Non è stata trovata la pizza da modificare");
            }
        }


        // DELETE -------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza? pizzaToDelete = _myDataBase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _myDataBase.Pizzas.Remove(pizzaToDelete);

                _myDataBase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La pizza che vorresti eliminare non è stata trovata");
            }
        }


        // USER SECTION -----------------------------------

        // sezione per gli user
        public IActionResult UserIndex()
        {
            //_myConsoleLogger.WriteLog("L'utente è entrato nella vista Pizza > UserIndex");

            _myFileLogger.WriteLog("L'utente è entrato nella vista Pizza > UserIndex");

            List<Pizza> UserpizzasList = _myDataBase.Pizzas.ToList<Pizza>();

            return View("UserIndex", UserpizzasList);
        }

        public IActionResult UserDetails(int id)
        {
            // punto interrogativo per mettere in conto che potrei ricevere un oggetto pizza nullo
            Pizza? userFoundedPizza = _myDataBase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (userFoundedPizza == null)
            {
                return NotFound($"La pizza con id {id} non è stata trovata");
            }
            else
            {
                return View("UserDetails", userFoundedPizza);
            }
        }
    }
}
