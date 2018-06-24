using System.ComponentModel;
using Searcher;
using System.Collections.Generic;

namespace WebIntegrator.Models
{
    public class SearcherViewModel
    {
        /// <summary>
        /// Характеристики для поиска
        /// </summary>
        public SortedList<string, bool> Subjects { get; set; }
        public SortedList<string, bool> University { get; set; }
        public SortedList<string, bool> StartTime { get; set; }
        public SortedList<string, bool> Provider { get; set; }
        public List<string> SelectedSubjects { get; set; }
        public List<string> SelectedUniversity { get; set; }
        public List<string> SelectedStartTime { get; set; }
        public List<string> SelectedProvider { get; set; }
        public string NameText { get; set; }
        public bool IsSertificate { get; set; }
        public bool IsSchool { get; set; }
        public bool IsUniversity { get; set; }
        public bool IsQulification { get; set; }
        public bool IsSearching { get; set; }
 
        /// <summary>
        /// Найденные курсы
        /// </summary>
        public List<Course> SearchingCourses { get; set; }
        public List<Course> RecommendedCourses { get; set; }

        public SearcherViewModel()
        {
            SearchingCourses = new List<Course>();
            RecommendedCourses = new List<Course>();
            Subjects = new SortedList<string, bool>()
            {
                 {"Астрономия", false },
                 {"Биология", false},
                 {"География", false},
                 {"Геология", false},
                 {"Информатика", false},
                 {"Искусство", false},
                 {"История", false},
                 {"Культурология", false},
                 {"Математика", false},
                 {"Медицина", false},
                 {"Менеджмент", false},
                 {"Педагогика", false},
                 {"Политология", false},
                 {"Психология", false},
                 {"Социология", false},
                 {"Физика", false},
                 {"Филология", false},
                 {"Философия", false},
                 {"Химия", false},
                 {"Экономика", false},
                 {"Юриспруденция", false },
                 { "Другое", false}
            };

            Provider = new SortedList<string, bool>()
            {
                {"Интуит", false },
                {"Лекториум", false},
                {"Универсариум", false},
                {"Distant.msu", false},
                {"Университет без границ", false},
                {"Национальная платформа", false},
                {"Arzamas Academy", false},
                {"ПостНаука", false},
                { "УниверТВ", false}
            };

            University = new SortedList<string, bool>()
            {
                 {"МГУ", false },
                 {"СПбГУ", false},
                 {"СПбГПУ", false},
                 {"УрФУ", false},
                 {"НГУ", false},
                 {"МФТИ", false},
                 { "НИУ ВШЭ", false}
            };

            StartTime = new SortedList<string, bool>()
            {
                { "Скоро старт", false },
                { "Идёт", false },
                 {"Можно записаться", false },
                 {"Всегда открыт", false },
                 {"Завершён", false }
            }; 
        }

        public void Search()
        {
            CSearchAndRecommended Searcher = new CSearchAndRecommended();

            SearchingCourses = Searcher.SearchCourse(NameText, SelectedSubjects, SelectedStartTime,
                SelectedProvider, SelectedUniversity, IsSertificate, IsSchool, IsUniversity, IsQulification);
            RecommendedCourses = Searcher.GetReccomend();
        }
    }
}