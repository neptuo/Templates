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
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(new Person { Name = "Eva", Surname = "Longoria" });
        }

        public ActionResult About()
        {
            return View();
        }

        [ActionName("about-framework")]
        public ActionResult AboutFramework()
        {
            return View();
        }
    }

    public class Person
    {
        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please, fill in first name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Your name must contain at least 2 characters")]
        public string Name { get; set; }

        [Display(Name = "Last name")]
        public string Surname { get; set; }
    }
}
