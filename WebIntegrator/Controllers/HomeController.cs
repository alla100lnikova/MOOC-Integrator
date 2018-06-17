using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Web.Mvc;
using WebIntegrator.Models;
using System.IO;

namespace WebIntegrator.Controllers
{
    public class HomeController : Controller
    {
        static SearcherViewModel Model = new SearcherViewModel();
        [HttpGet]
        public ActionResult Index()
        {
            return View(new SearcherViewModel());
        }

        [HttpPost]
        public ActionResult Index(SearcherViewModel model)
        {
            Model.IsSearching = true;
            Stopwatch Timer = new Stopwatch();
            Timer.Start();
            Searcher.Administration.ToLog("Start work: " + model.NameText);

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

            Searcher.Administration.ToLog("Characters: SelectedSubjects count = " + Model.SelectedSubjects.Count);
            Searcher.Administration.ToLog("Characters: SelectedStartTime count = " + Model.SelectedStartTime.Count);
            Searcher.Administration.ToLog("Characters: SelectedUniversity count = " + Model.SelectedUniversity.Count);
            Searcher.Administration.ToLog("Characters: SelectedProvider count = " + Model.SelectedProvider.Count);
            Model.Search();

            Timer.Stop();
            Searcher.Administration.ToLog("End work - " + Timer.ElapsedMilliseconds + " ms");
            Searcher.Administration.ToLog("Find: " + Model.SearchingCourses.Count + " | Recommend: " + Model.RecommendedCourses.Count);

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