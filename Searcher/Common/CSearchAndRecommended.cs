using System;
using System.Collections.Generic;
using System.Linq;
using RecommendedSystemLib;
using System.Xml;

namespace Searcher
{
    public class CSearchAndRecommended
    {
        private SortedList<int, List<Course>> RecommendedCourses;
        private List<Course> RecommendedResult;
        private List<List<string>> Synonyms;
        private List<int> SubjectsVal;
        private List<int> TimesVal;
        private bool IsSert;
        private List<bool> Auditory;

        public CSearchAndRecommended() { }

        public List<Course> GetReccomend()
        {
            return RecommendedResult;
        }

        private List<string> ParseName(string Name)
        {
            CStemmerPorter StemmerPorter = new CStemmerPorter();
            return StemmerPorter.NameParseStem(Name);
        }

        private void ReadFromXml(ref List<string> Syns)
        {
            string Name = Syns[0];
            string Address = @"C:\Users\User\Documents\Visual Studio 2015\Projects\WebSite\RecommendedSystemLib\Dictionary\" + Name[0] + ".xml";
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Address);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                // получаем атрибут name
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("Name");
                    if (attr != null && attr.Value == Name)
                    {
                        XmlNode child = xnode.ChildNodes[0];
                        foreach (XmlNode childnode in child.ChildNodes)
                        {
                            if (childnode.Name == "string")
                            {
                                Syns.Add(childnode.InnerText);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void GetSynonims(string Request)
        {
            Synonyms = new List<List<string>>();
            List<string> WordsRequest = ParseName(Request);

            foreach (string word in WordsRequest)
            {
                List<string> Syns = new List<string>();
                if (word != "")
                {
                    Syns.Add(word);
                    ReadFromXml(ref Syns);
                    Synonyms.Add(Syns);
                }
            }
        }

        private bool CheckText(string CourseName, string Request)
        {
            bool MyText = true;
            if (Request != "")
            {
                List<string> Names = ParseName(Request);
                int i = 0;
                CourseName = CourseName.ToLower();
                while (i < Names.Count)
                {
                    MyText = CourseName.IndexOf(Names[i]) >= 0 && MyText;
                    i++;
                }
            }
            else MyText = true;

            return MyText;
        }

        enum eCourseType
        {
            ctNotMatch,
            ctFound,
            ctRecommended
        }

        int HasSynonims(string CurName)
        {
            List<string> Name = ParseName(CurName.ToLower());
            int CountSyn = 0;

            foreach (var syns in Synonyms)
            {
                foreach (var word in Name)
                {
                    if (syns.Find(x => x == word) == word)
                    {
                        CountSyn++;
                        break;
                    }
                }
            }

            return CountSyn;
        }

        private void CharactersToValue(List<string> Subjects, List<string> Times)
        {
            SubjectsVal = new List<int>();
            TimesVal = new List<int>();
            using (var ctx = new MOOCEntities())
            {
                var Subs = ctx.Группа_ПредметнаяОбласть.ToList();
                for (int i = 0; i < Subjects.Count; i++)
                {
                    var Value = Subs.Find(x => x.Название == Subjects[i]);
                    if (Value != null) SubjectsVal.Add((int)Value.Значение); 
                }

                var TimeList = ctx.Группа_ВремяНачала.ToList();
                for (int i = 0; i < Times.Count; i++)
                {
                    var Value = TimeList.Find(x => x.Название == Times[i]);
                    if (Value != null) TimesVal.Add((int)Value.Значение); 
                }
            }
        }

        private string GetUniversityAbbreviation(string Name)
        {
            using (var ctx = new MOOCEntities())
            {
                foreach(var Abb in ctx.Институт)
                {
                    if (Name == Abb.ПолноеНазвание) return Abb.Аббревиатура;
                }
            }

            return "";
        }

        public List<Course> SearchCourse(
            string Name,
            List<string> Subjects = null,
            List<string> Times = null,
            List<string> Providers = null,
            List<string> Universities = null,
            bool IsSertificate = false,
            bool IsSchool = false,
            bool IsUniversity = false,
            bool IsQualification = false)
        {
            RecommendedCourses = new SortedList<int, List<Course>>();
            List<Course> FoundCourses = new List<Course>();

            GetSynonims(Name);
            CharactersToValue(Subjects, Times);

            using (var ctx = new MOOCEntities())
            {
                foreach (var course in ctx.Описание_MOOC)
                {
                    #region Условия
                    Course CurCourse = new Course(
                        course.НазваниеКурса,
                        course.URL,
                        course.Провайдер1.Название,
                        course.ПредметнаяОбласть == null ? "" : course.ПредметнаяОбласть1.Группа_ПредметнаяОбласть.Название,
                        course.ВремяНачала == null ? "" : course.ВремяНачала1.Название,
                        course.Институт == null ? course.Институт : "",
                        course.НаличиеСертификата == null ? false : (bool)course.НаличиеСертификата,
                        course.Школа == null ? false : (bool)course.Школа,
                        course.ВысшееОбразование == null ? false : (bool)course.ВысшееОбразование,
                        course.ПовышениеКвалификации == null ? false : (bool)course.ПовышениеКвалификации);
                    eCourseType Subject, IsRec, Time;
                    Subject = IsRec = Time = eCourseType.ctNotMatch;
                    bool Sertificate = false, School = false, Student = false, Qualification = false, University = false, Provider = false;

                    int i = 0;
                    //Если предметная область не выбрана, то считаем курс подходящим
                    if (Subjects == null || Subjects.Count == 0) Subject = eCourseType.ctFound;
                    //иначе проверяем соответствие предметной области курса хотя бы одной из выбранных
                    else
                    {
                        while (Subject != eCourseType.ctFound && i < SubjectsVal.Count)
                        {
                            Subject = CurCourse.SubjectValue == SubjectsVal[i] ? eCourseType.ctFound : eCourseType.ctNotMatch;
                            IsRec = IsRec == eCourseType.ctRecommended ? eCourseType.ctRecommended
                                : Math.Abs(CurCourse.SubjectValue - SubjectsVal[i]) < 200 ? eCourseType.ctRecommended : eCourseType.ctNotMatch;
                            i++;
                        }
                        if (Subject != eCourseType.ctFound && IsRec == eCourseType.ctRecommended) Subject = eCourseType.ctRecommended; 
                    }

                    i = 0;
                    IsRec = eCourseType.ctNotMatch;
                    //Аналогично, для времени начала, провайдера и института
                    if (Times == null) Time = eCourseType.ctFound;
                    else
                    {
                        if (CurCourse.StartTime != "")
                        {
                            while (Time != eCourseType.ctFound && Time != eCourseType.ctFound && i < Times.Count)
                            {
                                Time = CurCourse.StartTimeValue == TimesVal[i] ? eCourseType.ctFound : eCourseType.ctNotMatch;
                                IsRec = IsRec == eCourseType.ctRecommended ? eCourseType.ctRecommended
                                 : Math.Abs(CurCourse.StartTimeValue - TimesVal[i]) < 200 ? eCourseType.ctRecommended : eCourseType.ctNotMatch;
                                i++;
                            }
                            if (Time != eCourseType.ctFound && IsRec == eCourseType.ctRecommended) Time = eCourseType.ctRecommended;
                        }
                    }

                    i = 0;
                    if (Providers == null || Providers.Count == 0) Provider = true;
                    else
                    {
                        while (!Provider && i < Providers.Count)
                        {
                            Provider = CurCourse.Provider == Providers[i];
                            i++;
                        }
                    }

                    i = 0;
                    if (Universities == null || Universities.Count == 0) University = true;
                    else
                    {
                        if (CurCourse.University != "")
                        {
                            string UnAbbreviation = GetUniversityAbbreviation(CurCourse.University);
                            while (!University && i < Universities.Count)
                            {
                                University = CurCourse.University == Universities[i] || UnAbbreviation == Universities[i];
                                i++;
                            }
                        }
                    }

                    //Если кнопка была выбрана - наличие сертификата должно быть = true
                    if (IsSertificate)
                    {
                        if (course.НаличиеСертификата != null)
                            Sertificate = Convert.ToBoolean(course.НаличиеСертификата);
                    }
                    else Sertificate = true;

                    IsSert = Sertificate;
                    Auditory = new List<bool>();

                    //Аналогично для целевой аудитории
                    #region Выбор целевой аудитории
                    if (IsSchool)
                    {
                        if (course.Школа != null)
                            School = Convert.ToBoolean(course.Школа);
                    }
                    else School = true;

                    Auditory.Add(IsSchool);

                    if (IsUniversity)
                    {
                        if (course.ВысшееОбразование != null)
                            Student = Convert.ToBoolean(course.ВысшееОбразование);
                    }
                    else Student = true;

                    Auditory.Add(IsUniversity);

                    if (IsQualification)
                    {
                        if (course.ПовышениеКвалификации != null)
                            Qualification = Convert.ToBoolean(course.ПовышениеКвалификации);
                    }
                    else Qualification = true;

                    Auditory.Add(IsQualification);

                    #endregion
                    #endregion

                    //Если курс подходит по всем условиям, то добавляем его в список результатов
                    if (CheckText(CurCourse.Name, Name)
                       && Sertificate && School && Student && Qualification
                       && Provider && University
                       && Time == eCourseType.ctFound
                       && Subject == eCourseType.ctFound)
                    {
                        FoundCourses.Add(CurCourse);
                    }
                    else
                    {
                        //Иначе проверяем, стоит ли отожить для рекомендации
                        int SynCount = HasSynonims(CurCourse.Name);
                        if (Time == eCourseType.ctRecommended
                            && Subject == eCourseType.ctRecommended
                            && SynCount > 0)
                        {
                            if (RecommendedCourses.ContainsKey(SynCount))
                                RecommendedCourses[SynCount].Add(CurCourse);
                            else
                                RecommendedCourses.Add(SynCount, new List<Course> { CurCourse });
                        }
                    }
                }
            }

            RecommendedResult = RecommendCalculation(Name);

            return FoundCourses;
        }

        private List<Course> RecommendCalculation(string Request)
        {
            CRecommendationsCalculation RecSystem = new CRecommendationsCalculation();
            if (RecommendedCourses.Count > 0)
            {
               return RecSystem.FindResult(RecommendedCourses, Request, SubjectsVal, TimesVal, IsSert, Auditory);
            }

            return null;
        }
    }
}