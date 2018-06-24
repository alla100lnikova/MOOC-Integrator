using System.Collections.Generic;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Searcher
{
    public class PostNaukaParser : MyParser
    {
        [JsonObject(MemberSerialization.OptIn)]
        struct MyJsonObject
        {
            [JsonProperty("title")]
            public string Name { get; set; }

            [JsonProperty("link")]
            public string Url { get; set; }
        }

        //Адрес сайта
        private const string m_PostNaukaURL = "https://postnauka.ru";
        // Адрес списка всех курсов
        private const string m_MainURL = "https://postnauka.ru/api/v1/posts?page=1&term=courses";

        /// <summary>
        /// Парсинг конкретной страницы для различий в видах образования
        /// </summary>
        /// <param name="URL">Адрес страницы</param>
        /// <returns>Список курсов</returns>
        private List<Course> Parse(string URL)
        {
            List<Course> NewCourses = new List<Course>();

            string content = GetRequest(URL);
            MyJsonObject[] objArr = JsonConvert.DeserializeObject<MyJsonObject[]>(content);
            foreach (var obj in objArr)
            {
                Course NewCourse = new Course();
                NewCourse.URL = obj.Url.IndexOf(m_PostNaukaURL) >= 0 ? obj.Url : m_PostNaukaURL + obj.Url;
                NewCourse.Name = obj.Name;
                NewCourse.University = "";
                NewCourse.Subject = "Другое";
                NewCourse.Provider = "ПостНаука";
                NewCourse.StartTime = "Всегда открыт";
                NewCourse.IsUniversity = true;
                NewCourse.IsSchool = false;
                NewCourse.IsQualification = true;
                NewCourse.IsSertificate = false;
                NewCourses.Add(NewCourse);
            }
            return NewCourses;
        }

        /// <summary>
        /// Считывает HTML-код и берёт нужные составляющие со страницы
        /// </summary>
        public override void StartParse()
        {
            List<Course> NewCourses = Parse(m_MainURL);
            CheckAndSave(NewCourses);
        }
    }
}