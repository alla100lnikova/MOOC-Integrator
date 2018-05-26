using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

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
            using (var ctx = new MOOC())
            {
                foreach (var course in ctx.Описание_MOOC)
                {
                    if (course.URL == URL)
                    {
                        IsCourse = course.id;
                        break;
                    }
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
            using (var ctx = new MOOC())
            {
                ctx.Описание_MOOC.Attach(course);
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
                if(course.ПредметнаяОбласть <= 0) course.ПредметнаяОбласть = 113;

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
            using (var ctx = new MOOC())
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
                    ctx.Описание_MOOC.Attach(course);
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
            StreamWriter sw = new StreamWriter(@"C:\Test.txt");
            foreach (Course course in CoursesForSaving)
            {
                SaveCourse(course, -1);
                sw.WriteLine(course.URL + " // " + course.Name + " // " + course.Provider + " // " + course.StartTime +
                    " // " + course.Subject + " // " + course.University + " // " + course.IsSchool + " // " + course.IsUniversity +
                    " // " + course.IsQualification + " // " + course.IsSertificate);
            }
           sw.Close();
        }
    }
}
