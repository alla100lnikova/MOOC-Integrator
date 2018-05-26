using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;

namespace Searcher
{
    public class UniversariumParser: MyParser
    {
        //Адрес сайта
        private const string m_UniversariumURL = "http://universarium.org";
        // Адрес списка всех курсов
        private const string m_MainURL = "http://universarium.org/catalog";
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

            StreamReader sr = new StreamReader(@"C:\Universarium.html");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(sr.ReadToEnd());
            HtmlNode Node;
            HtmlNodeCollection c = doc.DocumentNode.SelectNodes("//div[@class='block-course col-md-4 col-sm-6 col-xs-6']");
            if (c != null)
            {
                foreach (HtmlNode n in c)
                {
                    Course NewCourse = new Course();
                    string Href = n.InnerHtml;
                    HtmlDocument docTmp = new HtmlDocument();
                    docTmp.LoadHtml(Href);
                    Node = docTmp.DocumentNode.SelectSingleNode("//h4[@class='block-course-title']");
                    if (Node != null)
                    {
                        doc = new HtmlDocument();
                        doc.LoadHtml(Node.InnerHtml);
                        Node = doc.DocumentNode.SelectSingleNode("//a");
                        string u = Node.Attributes["href"].Value;
                        int Index = IsCourseInDB(u);
                        if (Index != -1)
                        {
                            continue;
                        }
                        NewCourse.URL = u;
                        NewCourse.Name = Node.InnerText;

                        Node = docTmp.DocumentNode.SelectSingleNode("//div[@class='block-course-start-date']");
                        if (Node != null)
                        {
                            doc = new HtmlDocument();
                            doc.LoadHtml(Node.InnerHtml);
                            Node = doc.DocumentNode.SelectSingleNode("//span");
                            if (Node != null)
                            {
                                if (Node.InnerText == "Скоро") NewCourse.StartTime = "Скоро старт";
                                else NewCourse.StartTime = "Идёт";
                            }
                            else
                            {
                                NewCourse.StartTime = "Всегда открыт";
                            }
                        }

                        Node = docTmp.DocumentNode.SelectSingleNode("//span[@class='block-course-institute-and-category']");
                        if (Node != null)
                        {
                            NewCourse.University = Node.ChildNodes[1].InnerText;
                            if (NewCourse.University != "")
                            {
                                NewCourse.Subject = Node.ChildNodes[7].InnerText;
                            }
                            else
                            {
                                NewCourse.Subject = Node.ChildNodes[5].InnerText;
                            }
                        }
                        else
                        {
                            NewCourse.Subject = "Другое";
                        }

                        NewCourse.Provider = "Универсариум";

                        NewCourse.IsUniversity = true;
                        NewCourse.IsSchool = false;
                        NewCourse.IsQualification = true;
                        NewCourse.IsSertificate = false;
                        NewCourses.Add(NewCourse);
                    }
                }
            }
            return NewCourses;
        }

        /// <summary>
        /// Считывает HTML-код и берёт нужные составляющие со страницы
        /// </summary>
        public void StartParse()
        {
            List<Course> NewCourses = Parse(m_MainURL);
            CheckAndSave(NewCourses);
        }
    }

}
