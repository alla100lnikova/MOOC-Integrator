using System.Collections.Generic;
using HtmlAgilityPack;

namespace Searcher
{
    public class LektoriumParser: MyParser
    {
        //Адрес сайта
        private const string m_LektoriumURL = "https://www.lektorium.tv";
        // Адрес списка всех курсов
        private const string m_MainURL = "https://www.lektorium.tv/medialibrary?type%5B%5D=course&search_api_views_fulltext=&recorded_from%5Bdate%5D=&recorded_to%5Bdate%5D=&sort_by=created&sort_order=DESC&";

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

            string LastAddress;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNode Node = doc.DocumentNode.SelectSingleNode("//a[@title='На последнюю страницу']");
            if(Node != null)
            {
                LastAddress = m_MainURL + Node.Attributes["href"].Value;
            }
            //Это на всякий
            else
            {
                LastAddress = m_MainURL + "page" + 8;
            }
            //Не остановится, пусть пока
            while(i < 9)
            //while ((URL + "page=" + i) != LastAddress)
            {
                doc = new HtmlDocument();
                doc.LoadHtml(content);
                HtmlNodeCollection c = doc.DocumentNode.SelectNodes("//div[@class='row result-item']");
                if (c != null)
                {
                    foreach (HtmlNode n in c)
                    {
                        Course NewCourse = new Course();
                        string Href = n.InnerHtml;
                        HtmlDocument docTmp = new HtmlDocument();
                        docTmp.LoadHtml(Href);
                        Node = docTmp.DocumentNode.SelectSingleNode("//a");
                        if (Node != null)
                        {
                            string u = m_LektoriumURL + Node.Attributes["href"].Value;
                            NewCourse.URL = u;
                            string res_cn = GetRequest(u);
                            HtmlDocument d = new HtmlDocument();
                            d.LoadHtml(res_cn);

                            Node = d.DocumentNode.SelectSingleNode("//div[@id='page-title']");
                            if (Node != null)
                            {
                                docTmp = new HtmlDocument();
                                docTmp.LoadHtml(Node.InnerHtml);
                                Node = docTmp.DocumentNode.SelectSingleNode("//h1");
                                if (Node != null)
                                {
                                    NewCourse.Name = Node.InnerText;
                                }
                            }

                            Node = d.DocumentNode.SelectSingleNode("//div[@id='meta-desc-fields']");
                            if (Node != null)
                            {
                                docTmp = new HtmlDocument();
                                docTmp.LoadHtml(Node.ChildNodes[1].InnerHtml);
                                HtmlNode NodeTmp = docTmp.DocumentNode.SelectSingleNode("//a");
                                if (NodeTmp != null)
                                {
                                    NewCourse.University = NodeTmp.InnerText;
                                }
                                docTmp.LoadHtml(Node.ChildNodes[3].InnerHtml);
                                NodeTmp = docTmp.DocumentNode.SelectSingleNode("//div[@class='textformatter-list']");
                                if (NodeTmp != null)
                                {
                                    if ((NodeTmp.InnerText).IndexOf(",") > 0)
                                    {
                                        NewCourse.Subject = (NodeTmp.InnerText).Split(',')[0];
                                    }
                                    else
                                    {
                                        NewCourse.Subject = NodeTmp.InnerText;
                                    }
                                }
                                else
                                {
                                    NewCourse.Subject = "Другое";
                                }
                            }
                            else
                            {
                                NewCourse.Subject = "Другое";
                            }

                            NewCourse.Provider = "Лекториум";
                            NewCourse.StartTime = "В архиве";
                            NewCourse.IsUniversity = true;
                            NewCourse.IsSchool = true;
                            NewCourse.IsQualification = true;
                            NewCourse.IsSertificate = false;
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
