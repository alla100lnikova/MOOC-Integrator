using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebIntegrator.Models;

namespace WebIntegrator.Controllers
{
    public class HomeController : Controller
    {
        private SearcherViewModel Model;
        public ActionResult Index()
        {
            Model = new SearcherViewModel();
            return View(Model);
        }

        public ActionResult Search(string name,
            List<string> subjects, 
            List<string> times,
            List<string> univers,
            List<string> providers,
            bool isSert,
            bool isSchool,
            bool isUniver,
            bool isQual)
        {
            Model.Subjects = subjects;
            Model.StartTime = times;
            Model.Provider = providers;
            Model.University = univers;
            Model.IsQulification = isQual;
            Model.IsSchool = isSchool;
            Model.IsSertificate = isSert;
            Model.IsUniversity = isUniver;
            Model.NameText = name;
            Model.Search();

            return View(Model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Описание сервиса";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Контакты";

            return View();
        }
    }
}