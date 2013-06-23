using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebMvc.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<int, Person> persons = new Dictionary<int, Person>();

        static HomeController()
        {
            persons.Add(1, new Person { ID = 1, Name = "Eva", Surname = "Long" });
            persons.Add(2, new Person { ID = 2, Name = "Jon", Surname = "Doe" });
            persons.Add(3, new Person { ID = 3, Name = "Igo", Surname = "Perrin" });
        }

        public ActionResult Index()
        {
            return View(persons.Values.OrderBy(p => p.Surname).ThenBy(p => p.Name));
        }

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Person person = null;
                if (persons.TryGetValue(id.Value, out person))
                    return View(person);
            }
            return View(new Person());
        }

        [HttpPost]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                Person target = null;
                if (persons.TryGetValue(person.ID, out target))
                {
                    target.Name = person.Name;
                    target.Surname = person.Surname;
                }
                else
                {
                    int nextID = persons.Keys.Max() + 1;
                    person.ID = nextID;
                    persons.Add(nextID, person);
                }
                return RedirectToAction("index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (persons.ContainsKey(id))
                persons.Remove(id);

            return RedirectToAction("index");
        }

        public ActionResult About()
        {
            return View();
        }
    }

    public class Person
    {
        public int ID { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please, fill in first name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Your name must contain at least 2 characters")]
        public string Name { get; set; }

        [Display(Name = "Last name")]
        public string Surname { get; set; }
    }
}
