using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Searcher
{
    public class UniversariumParser: MyParser
    {
        [JsonObject(MemberSerialization.OptIn)]
        struct MyJsonObject
        {
            [JsonProperty("id")]
            public string id { get; set; }

            [JsonProperty("title")]
            public string Name { get; set; }

            [JsonProperty("institute")]
            public string University { get; set; }

            [JsonProperty("status_ru")]
            public string Time { get; set; }

            [JsonProperty("category")]
            public string Sub { get; set; }
        }

        //Адрес сайта
        private const string m_UniversariumURL = "http://universarium.org/course/";
        // Адрес списка всех курсов
        private const string m_MainURL = "https://universarium.org/mapi/fcourses";

        /// <summary>
        /// Парсинг конкретной страницы для различий в видах образования
        /// </summary>
        /// <param name="URL">Адрес страницы</param>
        /// <returns>Список курсов</returns>
        private List<Course> Parse(string URL)
        {
            List<Course> NewCourses = new List<Course>();
            string content = GetRequest(URL);
            JObject jObj = JObject.Parse(content);
            MyJsonObject[] objArr = JsonConvert.DeserializeObject<MyJsonObject[]>(jObj["response"].ToString());
            foreach (var obj in objArr)
            {
                Course NewCourse = new Course();
                NewCourse.URL = m_UniversariumURL + obj.id;
                NewCourse.Name = obj.Name;
                NewCourse.University = obj.University;
                NewCourse.Subject = obj.Sub;
                NewCourse.Provider = "Универсариум";
                NewCourse.StartTime = obj.Time;
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
