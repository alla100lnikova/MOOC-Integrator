using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;

namespace Searcher
{
    public class OpenEduParser : MyParser
    {
        //Адрес сайта
        private const string m_OpenEduURL = "https://openedu.ru";
        // Адрес списка всех курсов
        private const string m_MainURL = "https://openedu.ru/course/";
        string content;
        HtmlDocument doc;

        /// <summary>
        /// Парсинг конкретной страницы для различий в видах образования
        /// </summary>
        /// <param name="URL">Адрес страницы</param>
        /// <returns>Список курсов</returns>
        private List<Course> Parse(string URL)
        {
            List<Course> NewCourses = new List<Course>();

            string content = GetRequest(URL);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);

            string json = content.Substring(content.IndexOf("COURSES ="), content.IndexOf("UNIVERSITIES = "));

            HtmlNode Node;
            HtmlNodeCollection c = doc.DocumentNode.SelectNodes("//div[@class='col-md-4 col-sm-6 col-xs-12 col']");
            if (c != null)
            {


                foreach (HtmlNode n in c)
                {
                    Course NewCourse = new Course();
                    string Href = n.InnerHtml;
                    HtmlAgilityPack.HtmlDocument docTmp = new HtmlAgilityPack.HtmlDocument();
                    docTmp.LoadHtml(Href);
                    Node = docTmp.DocumentNode.SelectSingleNode("//div[@class='course-title']");
                    if (Node != null)
                    {
                        docTmp = new HtmlDocument();
                        docTmp.LoadHtml(Node.InnerHtml);
                        Node = docTmp.DocumentNode.SelectSingleNode("//a");
                        string u = Node.Attributes["href"].Value;
                        NewCourse.URL = u;
                        NewCourse.Name = Node.InnerText;

                        string res_cn = GetRequest(u);
                        HtmlAgilityPack.HtmlDocument d = new HtmlAgilityPack.HtmlDocument();
                        d.LoadHtml(res_cn);

                        Node = d.DocumentNode.SelectSingleNode("//div[@class='course_groups-box']");
                        if (Node != null)
                        {
                            docTmp = new HtmlDocument();
                            docTmp.LoadHtml(Node.InnerHtml);
                            Node = docTmp.DocumentNode.SelectSingleNode("//a");
                            if (Node != null)
                            {
                                NewCourse.Subject = Node.InnerText;
                            }
                            else
                            {
                                NewCourse.Subject = "Другое";
                            }
                        }

                        Node = d.DocumentNode.SelectSingleNode("//a[@class='univercity-title']");
                        if (Node != null)
                        {
                            NewCourse.University = Node.InnerText;
                        }

                        NewCourse.Provider = "Национальная платформа";

                        if (res_cn.IndexOf("До конца записи") > 0) NewCourse.StartTime = "Можно записаться";
                        else if (res_cn.IndexOf("Запись на курс закрыта") > 0 && res_cn.IndexOf("Курс уже начался") > 0) NewCourse.StartTime = "Идёт";
                        else if (res_cn.IndexOf("Запись на курс закрыта") > 0 && res_cn.IndexOf("Старт через") > 0) NewCourse.StartTime = "Скоро старт";
                        else NewCourse.StartTime = "В архиве";

                        NewCourse.IsUniversity = true;
                        NewCourse.IsSchool = false;
                        NewCourse.IsQualification = true;
                        NewCourse.IsSertificate = res_cn.IndexOf("Сертификат") > 0;
                        NewCourses.Add(NewCourse);
                    }
                }
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
