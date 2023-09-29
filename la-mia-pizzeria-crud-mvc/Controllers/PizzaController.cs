﻿using la_mia_pizzeria_crud_mvc.Database;
using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_crud_mvc.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                List<Pizza> pizzasList = db.Pizzas.ToList<Pizza>();
                
                return View("index", pizzasList);
            }
        }

        public IActionResult Details(int id)
        {
            using(PizzeriaContext db = new PizzeriaContext())
            {
                // punto interrogativo per mettere in conto che potrei ricevere un oggetto pizza nullo
                Pizza? foundedPizza = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                return View("Details", foundedPizza);
            }
        }
    }
}
