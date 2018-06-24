using System.Collections.Generic;
using HtmlAgilityPack;

namespace Searcher
{
    public class IntuitParser: MyParser
    {
        //Адрес сайта
        private const string m_IntuitURL = "http://www.intuit.ru";
        // Адрес списка всех курсов
        private const string m_MainURL = "http://www.intuit.ru/studies/courses?idfilter=0&sort=3&sort_order=0&search_data=&tab=4&_";

        /// <summary>
        /// Парсинг конкретной страницы для различий в видах образования
        /// </summary>
        /// <param name="URL">Адрес страницы</param>
        /// <returns>Список курсов</returns>
        private List<Course> Parse(string URL)
        {
            List<Course> NewCourses = new List<Course>();

            int i = 0;
            string content = GetRequest(URL + "page=" + i);
            int CurrentIndexCourse;

           while ((CurrentIndexCourse = content.IndexOf("<div class=\"td information\">")) != -1)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content);
                HtmlNodeCollection c = doc.DocumentNode.SelectNodes("//div[@class='td information']");
                if (c != null)
                {
                    foreach (HtmlNode n in c)
                    {
                        Course NewCourse = new Course();
                        string Href = n.InnerHtml;
                        Href = Href.Substring(Href.IndexOf("href") + 6, Href.IndexOf("/info") - Href.IndexOf("href") - 1);
                        if (Href != null)
                        {
                            string u = m_IntuitURL + Href;
                           
                            NewCourse.URL = u;
                            string res_cn = GetRequest(u);
                            HtmlDocument d = new HtmlDocument();
                            d.LoadHtml(res_cn);

                            HtmlNode Node = d.DocumentNode.SelectSingleNode("//h1");
                            if (Node != null)
                            {
                                try
                                {
                                    NewCourse.Name = Node.ChildNodes[2].InnerText;
                                    if (NewCourse.Name == ": ")
                                    {
                                        NewCourse.Name = Node.ChildNodes[4].InnerText;
                                    }
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            NewCourse.IsSertificate = true;

                            int j = 6;
                            Node = null;
                            while(j <= 9 && Node == null)
                            {
                                string address = "//*[@id=\"info - showcase - wrapper - block\"]/div[2]/div/div[1]/div[" + j + "]/a[1]";
                                Node = d.DocumentNode.SelectSingleNode(address);
                                if(Node == null)
                                {
                                    address = "//*[@id=\"info-showcase-wrapper-block\"]/div[2]/div/div[1]/div[" + j + "]/a[1]";
                                    Node = d.DocumentNode.SelectSingleNode(address);
                                }
                                j++;
                            }
                            if (Node != null)
                            {
                                NewCourse.Subject = Node.InnerText;
                            }
                            else
                            {
                                NewCourse.Subject = "Другое";
                            }

                            Node = d.DocumentNode.SelectSingleNode("//*[@id=\"info-showcase-wrapper-block\"]/div[2]/div/div[1]/div[2]/div/div[1]/div[3]/div/div[1]/div[2]/span");
                            if (Node != null)
                            {
                                if (Node.InnerText == "Для всех")
                                {
                                    NewCourse.IsUniversity = true;
                                    NewCourse.IsSchool = true;
                                    NewCourse.IsQualification = true;
                                }
                                else if (Node.InnerText == "Специалист")
                                {
                                    NewCourse.IsQualification = true;
                                    NewCourse.IsUniversity = true;
                                }
                                else
                                {
                                    NewCourse.IsQualification = true;
                                }
                            }

                            Node = d.DocumentNode.SelectSingleNode("//div[@class='text university-name']");
                            if (Node != null)
                            {
                                HtmlDocument dUn = new HtmlDocument();
                                dUn.LoadHtml(Node.InnerHtml);
                                Node = dUn.DocumentNode.SelectSingleNode("//span");
                                if (Node != null)
                                {
                                    NewCourse.University = Node.InnerText;
                                }
                            }
                            NewCourse.Provider = "Интуит";
                            NewCourse.StartTime = "Всегда открыт";
                            NewCourses.Add(NewCourse);
                        }
                    }
                }
                i++;
                content = GetRequest(URL + "page=" + i);
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
