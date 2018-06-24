using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace Searcher
{
    public class MyParser
    {
        /// <summary>
        /// Запрос к веб-странице, получение HTML-кода
        /// </summary>
        /// <param name="URL">Адрес страницы</param>
        /// <returns>Код страницы</returns>
        protected string GetRequest(string URL)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.Method = "GET";
                httpWebRequest.Referer = "http://google.com"; 
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Проверяет, есть ли заданный курс в базе данных
        /// </summary>
        /// <param name="URL">URL для проверки</param>
        /// <returns>Возвращает true, если курс с заданным URL уже существует, иначе false</returns>
        public int IsCourseInDB(string URL)
        {
            int IsCourse = -1;

            var sConnStr = new SqlConnectionStringBuilder
            {
                DataSource = @"ACER\SQLEXPRESS",
                InitialCatalog = "MOOC",
                IntegratedSecurity = true,

            }.ConnectionString;

            using (var sConn = new SqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new SqlCommand
                {//создаем команду
                    Connection = sConn,
                    CommandText = "select id from Описание_MOOC where URL='" + URL + "'"
                };//текст, который должен выполняться 

                var reader = sCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    IsCourse = Convert.ToInt32(reader[0].ToString());
                }
            }
            return IsCourse;
        }

        /// <summary>
        /// Сохраняет данные
        /// </summary>
        /// <param name="NewCourse">Сохраняемые данные</param>
        /// <param name="course">Объект для изменения</param>
        protected void Save(Course NewCourse, Описание_MOOC course)
        {
            using (var ctx = new MOOCEntities())
            {
                //ctx.Описание_MOOC.Attach(course);
                course.URL = NewCourse.URL;
                course.НазваниеКурса = NewCourse.Name;

                course.Институт = NewCourse.University;

                foreach (var provider in ctx.Провайдер)
                {
                    if (provider.Название == NewCourse.Provider)
                    {
                        course.Провайдер = provider.id;
                        break;
                    }
                }

                foreach (var subject in ctx.ПредметнаяОбласть)
                {
                    if (subject.Название == NewCourse.Subject)
                    {
                        course.ПредметнаяОбласть = subject.id;
                        break;
                    }
                }
                if(course.ПредметнаяОбласть <= 0) course.ПредметнаяОбласть = 104;

                if (NewCourse.StartTime == null) course.ВремяНачала = null;
                else
                    foreach (var time in ctx.ВремяНачала)
                    {
                        if (time.Название == NewCourse.StartTime)
                        {
                            course.ВремяНачала = time.id;
                            break;
                        }
                    }
                if (course.ВремяНачала == 0) course.ВремяНачала = 5;

                course.НаличиеСертификата = NewCourse.IsSertificate;
                course.Школа = NewCourse.IsSchool;
                course.ВысшееОбразование = NewCourse.IsUniversity;
                course.ПовышениеКвалификации = NewCourse.IsQualification;
            }
        }

        /// <summary>
        /// Сохраняет новый курс в базу
        /// </summary>
        /// <param name="NewCourse">Курс для сохранения</param>
        /// <param name="CourseUpdate">Если курс изменяли, то параметр равен id изменённого, иначе равен -1</param>
        public void SaveCourse(Course NewCourse, int CourseUpdate)
        {
            Описание_MOOC course = new Описание_MOOC();
            using (var ctx = new MOOCEntities())
            {
                if (CourseUpdate != -1)
                {
                    foreach (var mycourse in ctx.Описание_MOOC)
                    {
                        if (mycourse.id == CourseUpdate)
                        {
                            course = mycourse;
                            break;
                        }
                    }
                    Save(NewCourse, course);
                    ctx.SaveChanges();
                }
                else
                {
                    Save(NewCourse, course);
                    ctx.Описание_MOOC.Add(course);
                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Проверка существования курса в БД и сохранение
        /// </summary>
        /// <param name="CoursesForSaving">Список курсов для сохранения</param>
        protected void CheckAndSave(List<Course> CoursesForSaving)
        {
            foreach (Course course in CoursesForSaving)
            {
                SaveCourse(course, IsCourseInDB(course.URL));
            }
        }


        /// <summary>
        /// Считывает HTML-код и берёт нужные составляющие со страницы
        /// </summary>
        virtual public void StartParse()
        {
        }
    }
}
