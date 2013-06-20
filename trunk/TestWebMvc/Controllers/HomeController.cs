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
            persons.Add(1, new Person { Name = "Eva", Surname = "Long" });
            persons.Add(2, new Person { Name = "Jon", Surname = "Doe" });
            persons.Add(3, new Person { Name = "Igo", Surname = "Perrin" });
        }

        public ActionResult Index()
        {
            return View(persons.Values);
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
