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
        static SearcherViewModel Model = new SearcherViewModel();
        [HttpGet]
        public ActionResult Index()
        {
            return View(Model);
        }

        [HttpPost]
        public ActionResult Index(SearcherViewModel model)
        {
            Model.IsSertificate = Request.Form["IsSertificate"] ==  "on";
            Model.IsSchool = Request.Form["IsSchool"] == "on";
            Model.IsUniversity = Request.Form["IsUniversity"] == "on";
            Model.IsQulification = Request.Form["IsQulification"] == "on";

            Model.SelectedSubjects = new List<string>();
            var Keys = Model.Subjects.Keys;
            for (int i = 0, icount = Model.Subjects.Count; i < icount; i++)
            {
                bool IsVal = Model.Subjects[Keys[i]] = Request.Form[Keys[i]] == "on";
                if (IsVal) Model.SelectedSubjects.Add(Keys[i]);
            }

            Model.SelectedProvider = new List<string>();
            Keys = Model.Provider.Keys;
            for (int i = 0, icount = Model.Provider.Count; i < icount; i++)
            {
                bool IsVal = Model.Provider[Keys[i]] = Request.Form[Keys[i]] == "on";
                if (IsVal) Model.SelectedProvider.Add(Keys[i]);
            }

            Model.SelectedStartTime = new List<string>();
            Keys = Model.StartTime.Keys;
            for (int i = 0, icount = Model.StartTime.Count; i < icount; i++)
            {
                bool IsVal = Model.StartTime[Keys[i]] = Request.Form[Keys[i]] == "on";
                if (IsVal) Model.SelectedStartTime.Add(Keys[i]);
            }

            Model.SelectedUniversity = new List<string>();
            Keys = Model.University.Keys;
            for (int i = 0, icount = Model.University.Count; i < icount; i++)
            {
                bool IsVal = Model.University[Keys[i]] = Request.Form[Keys[i]] == "on";
                if (IsVal) Model.SelectedUniversity.Add(Keys[i]);
            }

            Model.NameText = model.NameText;

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