using System.ComponentModel;
using Searcher;
using System.Collections.Generic;

namespace WebIntegrator.Models
{
    public enum eSubject
    {
        [Description("Астрономия")]
        Astronomy,
        [Description("Биология")]
        Biology,
        [Description("География")]
        Geography,
        [Description("Геология")]
        Geology,
        [Description("Информатика")]
        Informatics,
        [Description("Искусство")]
        Art,
        [Description("История")]
        History,
        [Description("Культурология")]
        Culture,
        [Description("Математика")]
        Mathem,
        [Description("Медицина")]
        Medicine,
        [Description("Менеджмент")]
        Management,
        [Description("Педагогика")]
        Education,
        [Description("Политология")]
        Political,
        [Description("Психология")]
        Psihology,
        [Description("Социология")]
        Sociology,
        [Description("Физика")]
        Physics,
        [Description("Филология")]
        Philology,
        [Description("Философия")]
        Philosophy,
        [Description("Химия")]
        Chemistry,
        [Description("Экономика")]
        Economics,
        [Description("Юриспруденция")]
        Jurisprudence,
        [Description("Другое")]
        Other
    }

    public enum eStartTime
    {
        [Description("Идёт")]
        Run,
        [Description("Скоро старт")]
        SoonStart,
        [Description("Можно записаться")]
        CanRegister,
        [Description("Всегда открыт")]
        Open,
        [Description("Завершён")]
        Closed
    }

    public enum eProvider
    {
        [Description("Интуит")]
        Intuit,
        [Description("Универсариум")]
        Universarium,
        [Description("Лекториум")]
        Lectorium,
        [Description("ПостНаука")]
        PostNauka,
        [Description("УниверТВ")]
        UniverTV,
        [Description("Arzamas Academy")]
        Arzamas,
        [Description("Stepic")]
        Stepic,
        [Description("Национальная платформа")]
        NationalPlatform,
        [Description("Университет без границ")]
        OpenUniversity,
        [Description("Distant.msu")]
        DistantMSU,
    }

    public enum eUniversity
    {
        [Description("МГУ")]
        MSU,
        [Description("СПбГУ")]
        SPbSU,
        [Description("СПбГПУ")]
        SPPolitec,
        [Description("НИУ ВШЭ")]
        HSE,
        [Description("УрФУ")]
        Ural,
        [Description("МФТИ")]
        MPhysics,
        [Description("НГУ")]
        NSU,
    }

    public class SearcherViewModel
    {
        /// <summary>
        /// Характеристики для поиска
        /// </summary>
        public List<string> Subjects { get; set; }
        public List<string> University { get; set; }
        public List<string> StartTime { get; set; }
        public List<string> Provider { get; set; }
        public string NameText { get; set; }
        public bool IsSertificate { get; set; }
        public bool IsSchool { get; set; }
        public bool IsUniversity { get; set; }
        public bool IsQulification { get; set; }
 
        /// <summary>
        /// Найденные курсы
        /// </summary>
        public List<Course> SearchingCourses { get; set; }
        public List<Course> RecommendedCourses { get; set; }

        public SearcherViewModel()
        {
            SearchingCourses = new List<Course>();
            RecommendedCourses = new List<Course>();
        }

        public void Search()
        {
            //CSearchAndRecommended Searcher = new CSearchAndRecommended();
            //SearchingCourses = new List<Course>()
            //{
            //    new Course ("Курс 1", "http://mybootstrap.ru/components/#pagination", "", "Математика", "Скоро старт", "", true, true, true, false),
            //    new Course ("Курс 2", "http://mybootstrap.ru/components/#pagination", "", "Социология", "Идёт", "", true, true, true, false),
            //    new Course ("Курс 3 мегаинтересный", "http://mybootstrap.ru/components/#pagination", "", "Физика", "Скоро старт", "", true, true, true, false),
            //    new Course ("Курс 4, оч интересный, крутой, надо посмотреть, тырыпыры", "https://bootswatch.com/minty/", "", "Биология", "Скоро старт", "", true, true, true, false),
            //    new Course ("Курс 1102319392103", "http://localhost:11479/Home/About", "ПостНаука", "Математика", "Всегда открыт", "", true, true, true, false)
            //};
            //RecommendedCourses = new List<Course>()
            //{
            //    new Course ("Курс 1", "https://bootswatch.com/minty/", "Интуит", "Математика", "Скоро старт", "МГУ", true, true, true, false),
            //    new Course ("Курс 2", "https://bootswatch.com/minty/", "Лекториум", "Социология", "Идёт", "", true, true, true, false),
            //    new Course ("Курс 3 мегаинтересный", "http://localhost:11479/Home/About", "Stepik", "Физика", "Скоро старт", "Крутой институт", true, true, true, false),
            //    new Course ("Курс 4, оч интересный, крутой, надо посмотреть, тырыпыры", "http://localhost:11479/Home/About", "Arzamas Academy", "Биология", "Скоро старт", "МФТИ", true, true, true, false),
            //    new Course ("Курс 1123", "http://localhost:11479/Home/About", "ПостНаука", "Математика", "Всегда открыт", "Высшая школа экономики", true, true, true, false)
            //};

            //SearchingCourses = Searcher.SearchCourse(NameText, Subjects, StartTime,
            //    Provider, University, IsSertificate, IsSchool, IsUniversity, IsQulification);
            //RecommendedCourses = Searcher.GetReccomend();
        }
    }
}