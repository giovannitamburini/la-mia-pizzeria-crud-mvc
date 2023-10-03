using la_mia_pizzeria_crud_mvc.CustomLoggers;
using la_mia_pizzeria_crud_mvc.Database;
using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_crud_mvc.Controllers
{
    public class PizzaController : Controller
    {
        private CustomConsoleLogger _myLogger;

        public PizzaController()
        {
            _myLogger = new CustomConsoleLogger();
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è entrato nella vista Pizza > Index");

            using (PizzeriaContext db = new PizzeriaContext())
            {
                List<Pizza> pizzasList = db.Pizzas.ToList<Pizza>();

                return View("index", pizzasList);
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                // punto interrogativo per mettere in conto che potrei ricevere un oggetto pizza nullo
                Pizza? foundedPizza = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (foundedPizza == null)
                {
                    return NotFound($"La pizza con id {id} non è stata trovata");
                }
                else
                {
                    return View("Details", foundedPizza);
                }
            }
        }

        // sezione per gli user
        public IActionResult UserIndex()
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                _myLogger.WriteLog("L'utente è entrato nella vista Pizza > UserIndex");

                List<Pizza> UserpizzasList = db.Pizzas.ToList<Pizza>();

                return View("UserIndex", UserpizzasList);
            }
        }

        public IActionResult UserDetails(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                // punto interrogativo per mettere in conto che potrei ricevere un oggetto pizza nullo
                Pizza? userFoundedPizza = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

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

        // create

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        //meccanismo di sicurezza che aiuta a prevenire attacchi CSRF
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizzaCreated)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", pizzaCreated);
            }

            using(PizzeriaContext db = new PizzeriaContext())
            {
                db.Pizzas.Add(pizzaCreated);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        // update

        [HttpGet]
        public IActionResult Update(int id)
        {
            using(PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaToUpdate = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if(pizzaToUpdate == null)
                {
                    return NotFound("La pizza che vorresti modificare non è stata trovata");
                }
                else
                {
                    return View("Update", pizzaToUpdate);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(int id, Pizza modifiedPizza)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", modifiedPizza);
            }

            using(PizzeriaContext db = new PizzeriaContext())
            {
                Pizza pizzaToUpdate = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if(pizzaToUpdate != null)
                {
                    pizzaToUpdate.Name = modifiedPizza.Name;
                    pizzaToUpdate.Description = modifiedPizza.Description;
                    pizzaToUpdate.PathImage = modifiedPizza.PathImage;
                    pizzaToUpdate.Price = modifiedPizza.Price;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Non è stata trovata la pizza da modificare");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaToDelete = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Pizzas.Remove(pizzaToDelete);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza che vorresti eliminare non è stata trovata");
                }
            }
        }
    }
}
