using System;
using System.Collections.Generic;
using System.Linq;
using RecommendedSystemLib;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Searcher
{
    public class CSearchAndRecommended
    {
        private SortedList<int, List<Course>> RecommendedCourses;
        private List<Course> RecommendedResult;
        private List<List<string>> Synonyms;
        private List<int> SubjectsVal;
        private List<int> TimesVal;
        private List<bool> Auditory;
        string UniversityQuery = "(select Аббревиатура from Институт where ПолноеНазвание=Описание_MOOC.Институт)='";

        //Параметры поиска
        string Name;
        List<string> Subjects;
        List<string> Times;
        List<string> Providers;
        List<string> Universities;
        bool IsSertificate;
        bool IsSchool;
        bool IsUniversity;
        bool IsQualification;

        private Dictionary<string, string> Translate = new Dictionary<string, string>()
        {
            {"a","а" },
            {"b","б"},
            {"c","ц"},
            {"d","д"},
            {"e","е"},
            {"f","ф"},
            {"g","г"},
            {"h","х"},
            {"i","и"},
            {"j","ж"},
            {"k","к"},
            {"l","л"},
            {"m","м"},
            {"n","н"},
            {"o","о"},
            {"p","п"},
            {"q","к"},
            {"r","р"},
            {"s","с"},
            {"t","т"},
            {"u","у"},
            {"x","к"},
            {"y","и"},
            {"z","з"},
            {"w","в"},
            {"v","в"},
            {"линукс","linux"},
            {"линух","linux"},
            {"веб","web"},
            {"майкрософт","microsoft"},
            {"эксель","excel"},
            {"ворд","word"},
            {"вижуал","visual"},
            {"визуал","visual"},
            {"студио","studio"},
            {"ит","it"},
            {"сервер","server"},
            {"оракл","oracle"},
            {"питон","python"},
            {"лисп","lisp"},
            {"андроид","android"},
            {"впф","wpf"},
            {"интернет","internet"},
            {"виндовс","windows"},
            {"виндоус","windows"},
            {"маткад","matcad"},
            {"интел","intel"},
            {"асп","asp"},
            {"нет","net"},
            {"юникс","unix"},
            {"пролог","prolog"},
            {"опенофис","openoffice"},
            {"фотошоп","photoshop"},
            {"яндекс","yandex"},
            {"пхп","php"},
            {"делфи","delphi"},
            {"дэлфи","delphi"},
            {"паскаль","pascal"},
            {"компьютер","computer"},
            {"хеш","hash"},
            {"хэш","hash"},
            {"дизайн","design"},
            {"тайм","time"},
            {"1","один"},
            {"2","два"},
            {"3","три"},
            {"4","четыре"},
            {"5","пять"},
            {"6","шесть"},
            {"7","семь"},
            {"8","восемь"},
            {"9","девять"},
            {"10","десять"},
            {"100","сто"},
            {"1000","тысяча"},
            {"linux","линукс"},
            {"web","веб"},
            {"microsoft","майкрософт"},
            {"excel","эксель"},
            {"word","ворд"},
            {"visual","вижуал"},
            {"studio","студио"},
            {"it","ит"},
            {"server","сервер"},
            {"oracle","оракл"},
            {"python","питон"},
            {"lisp","лисп"},
            {"android","андроид"},
            {"wpf","впф"},
            {"internet","интернет"},
            {"windows","виндовс"},
            {"matcad","маткад"},
            {"intel","интел"},
            {"asp","асп"},
            {"net","нет"},
            {"unix","юникс"},
            {"prolog","пролог"},
            {"openoffice","опенофис"},
            {"photoshop","фотошоп"},
            {"yandex","яндекс"},
            {"php","пхп"},
            {"delphi","делфи"},
            {"pascal","паскаль"},
            {"computer","компьютер"},
            {"hash","хэш"},
            {"design","дизайн"},
            {"time","тайм"},
            {"один","1"},
            {"два","2"},
            {"три","3"},
            {"четыре","4"},
            {"пять","5"},
            {"шесть","6"},
            {"семь","7"},
            {"восемь","8"},
            {"девять","9"},
            {"десять","10"},
            {"сто","100"},
            {"тысяча","1000"},
            {"google", "гугл" },
            {"гугл","google" }
        };

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
            string Name = Syns[0].ToLower();
            string Name0 = Name[0].ToString();
            if(Translate.Keys.Contains(Name0))
            {
                Name0 = Translate[Name0];
            }

            string Address = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Dictionary\" + Name0 + ".xml");
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(Address);
            }
            catch
            {
                return;
            }
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

        string GetJoinGroupPart(string TableName, List<string> Elements)
        {
            string Query = "inner join Группа_" + TableName
                            + " on((select Группа from "
                            + TableName + " where id = Описание_MOOC." + TableName + ") = Группа_" + TableName + ".id ";

            for(int i = 0, icount = Elements.Count; i < icount; i++)
            {
                if (i == 0)
                    Query += "and (";
                if (i > 0)
                    Query += " or ";
                Query += "Группа_" + TableName + ".Название='" + Elements[i] + "'";
                if (i == icount - 1)
                    Query += ")";
            }
            Query += ")";

            return Query;
        }

        string GetProvidersQuery(bool IsSearch)
        {
            string Query = "inner join Провайдер on (Описание_MOOC.Провайдер=Провайдер.id ";

            if (IsSearch)
            {
                for (int i = 0, icount = Providers.Count; i < icount; i++)
                {
                    if (i == 0)
                        Query += "and (";
                    if (i > 0)
                        Query += " or ";
                    Query += "Провайдер.Название='" + Providers[i] + "'";
                    if (i == icount - 1)
                        Query += ")";
                }
            }
            Query += ")";

            return Query;
        }

        string GetNameQuery(bool IsSearch)
        {
            string Query = "";
            List<string> Words = ParseName(Name);
            if (!IsSearch)
            {
                foreach (var syns in Synonyms)
                {
                    Words.Concat(syns);
                }
            }

            string Oper = IsSearch ? " and " : " or ";
            for(int i = 0, icount = Words.Count; i < icount; i++)
            {
                if (i > 0)
                    Query += Oper;

                bool IsTranslate = Translate.Keys.Contains(Words[i].ToLower());
                if (IsTranslate)
                {
                    Query += "(НазваниеКурса like '%" + Translate[Words[i]] + "%' or ";
                }

                Query += "НазваниеКурса like '%" + Words[i] + "%'";
                if (IsTranslate)
                    Query += ")";
            }

            return Query;
        }

        string GetUniversityQuery()
        {
            string Query = "(";

            for (int i = 0, icount = Universities.Count; i < icount; i++)
            {
                if (i > 0)
                    Query += " or ";
                Query += UniversityQuery + Universities[i] + "'";
            }
            Query += ")";

            return Query;
        }

        string GetWhereQuery(bool IsSearch)
        {
            if (!IsSearch && (String.IsNullOrWhiteSpace(Name) || String.IsNullOrEmpty(Name)))
                return "";

            string Query = " where ";
            bool HasName = Name != null && Name.Length > 0;
            if (HasName)
                Query += GetNameQuery(IsSearch);
            
            if(IsSearch)
            {

                if (IsSchool)
                {
                    if (HasName)
                        Query += " and ";
                    Query += "Школа=1";
                }
                if (IsSertificate)
                {
                    if (IsSchool || HasName)
                        Query += " and ";
                    Query += "НаличиеСертификата=1";
                }
                if (IsUniversity)
                {
                    if (IsSchool || HasName || IsSertificate)
                        Query += " and ";
                    Query += "ВысшееОбразование=1";
                }
                if (IsQualification)
                {
                    if (IsSchool || HasName || IsSertificate || IsUniversity)
                        Query += " and ";
                    Query += "ПовышениеКвалификации=1";
                }

                if (Universities.Count > 0)
                {
                    if (IsSchool || HasName || IsSertificate || IsUniversity || IsQualification)
                        Query += " and ";
                    GetFullUniversityName();
                    Query += GetUniversityQuery();
                }
            }

            return Query;
        }

        //Составляем запрос
        string GetSqlQuery(bool IsSearch = true)
        {
            string Query = "select НазваниеКурса 'CName', URL 'Curl', НаличиеСертификата 'CSert', Школа 'CSchool', ВысшееОбразование 'CUniv', ПовышениеКвалификации 'CQual',"
                                           + "Группа_ПредметнаяОбласть.Название 'SubName', Группа_ПредметнаяОбласть.Значение 'SubVal',"
                                           + "Группа_ВремяНачала.Название 'TimeName', Группа_ВремяНачала.Значение 'TimeVal', Провайдер.Название 'ProvName', Институт 'UnName' from Описание_MOOC ";

            Query += GetJoinGroupPart("ПредметнаяОбласть", Subjects);
            Query += GetJoinGroupPart("ВремяНачала", Times);
            Query += GetProvidersQuery(IsSearch);
            Query += GetWhereQuery(IsSearch);

            return Query;
        }

        //Собираем полученные из запроса курсы
        void GetCourses(SqlCommand sCommand, ref List<Course> Courses)
        {
            var reader = sCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Course course = new Course();
                    course.Name = reader["CName"].ToString();
                    course.URL = reader["Curl"].ToString();
                    course.University = reader["UnName"].ToString();
                    course.Subject = reader["SubName"].ToString();
                    course.SubjectValue = Convert.ToInt32(reader["SubVal"].ToString());
                    course.StartTime = reader["TimeName"].ToString();
                    course.StartTimeValue = Convert.ToInt32(reader["TimeVal"].ToString());
                    course.Provider = reader["ProvName"].ToString();
                    course.IsSertificate = reader["CSert"].ToString() == "1";
                    course.IsSchool = reader["CSchool"].ToString() == "1";
                    course.IsUniversity = reader["CUniv"].ToString() == "1";
                    course.IsQualification = reader["CQual"].ToString() == "1";

                    Courses.Add(course);
                }
            }
            reader.Close();
        }

        //Ищем близкие предметные области или время начала
        string GetTimeOrSub(string TableName, List<int> Values)
        {
            string Query = "select Название from Группа_" + TableName + " where ";
            int i = 0;
            foreach (var val in Values)
            {
                if (i++ > 0)
                    Query += " or ";
                Query += "Значение-" + val + " < 200";
            }

            return Query;
        }

        string GetFullNameQuery()
        {
            string Query = "select ПолноеНазвание from Институт where ";

            int i = 0;
            foreach (var un in Universities)
            {
                if (i++ > 0)
                    Query += " or ";
                Query += "Аббревиатура='" + un + "'";
            }

            return Query;
        }

        //Добавим полные названия институтов
        void GetFullUniversityName()
        {
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
                    CommandText = GetFullNameQuery()
                };//текст, который должен выполняться 

                var reader = sCommand.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        Universities.Add(reader[0].ToString());
                    }
                }
            }
        }
        
        //Добавляем для запроса предметы и время, которые близки к выбранным
        void AddTimeOrSub(SqlCommand sCommand, ref List<string> Values)
        {
            var reader = sCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                   Values.Add(reader[0].ToString());
                }
            }
            reader.Close();
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
            List<Course> PendingCourses = new List<Course>();
            this.Name = Name;
            this.Subjects = Subjects;
            this.Providers = Providers;
            this.Universities = Universities;
            this.IsSertificate = IsSertificate;
            this.IsSchool = IsSchool;
            this.IsQualification = IsQualification;
            this.IsUniversity = IsUniversity;
            this.Times = Times;

            Stopwatch Timer = new Stopwatch();
            Timer.Start();
            Administration.ToLog("Start search courses...");

            CharactersToValue(Subjects, Times);
            GetSynonims(Name);

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
                    CommandText = GetSqlQuery()
                };//текст, который должен выполняться 

                GetCourses(sCommand, ref FoundCourses);
                if (SubjectsVal.Count > 0)
                {
                    sCommand = new SqlCommand
                    {//создаем команду
                        Connection = sConn,
                        CommandText = GetTimeOrSub("ПредметнаяОбласть", SubjectsVal)
                    };//текст, который должен выполняться 

                    AddTimeOrSub(sCommand, ref Subjects);
                }

                if (TimesVal.Count > 0)
                {
                    sCommand = new SqlCommand
                    {//создаем команду
                        Connection = sConn,
                        CommandText = GetTimeOrSub("ВремяНачала", TimesVal)
                    };//текст, который должен выполняться 

                    AddTimeOrSub(sCommand, ref Times);
                }

                sCommand = new SqlCommand
                {//создаем команду
                    Connection = sConn,
                    CommandText = GetSqlQuery(false)
                };//текст, который должен выполняться 

                GetCourses(sCommand, ref PendingCourses);
            }

            AddRecommend(PendingCourses, FoundCourses);

            Timer.Stop();
            Administration.ToLog("End search courses - " + Timer.ElapsedMilliseconds + " ms");
            RecommendedResult = RecommendCalculation(Name);

            return FoundCourses;
        }

        //Проверяем, стоит ли отожить для рекомендации (не должен попадать в уже выбранные)
        private void AddRecommend(List<Course> PendingCourses, List<Course> FoundCourses)
        {
            foreach (var course in PendingCourses)
            {
                if (FoundCourses.FindIndex(x=>x.URL == course.URL) >= 0)
                    continue;

                int SynCount = HasSynonims(course.Name);
                if (SynCount > 0 || String.IsNullOrWhiteSpace(Name) || String.IsNullOrEmpty(Name))
                {
                    Auditory = new List<bool>();
                    Auditory.Add(IsSchool);
                    Auditory.Add(IsUniversity);
                    Auditory.Add(IsQualification);

                    if (RecommendedCourses.ContainsKey(SynCount))
                        RecommendedCourses[SynCount].Add(course);
                    else
                        RecommendedCourses.Add(SynCount, new List<Course> { course });
                }
            }
        }

        private List<Course> RecommendCalculation(string Request)
        {
            CRecommendationsCalculation RecSystem = new CRecommendationsCalculation(); 
            if (RecommendedCourses.Count > 0)
            {
                Stopwatch Timer = new Stopwatch();
                Timer.Start();
                Administration.ToLog("Start calc recommendations...");
                List<Course> Result = RecSystem.FindResult(RecommendedCourses, Request, SubjectsVal, TimesVal, IsSertificate, Auditory);
                Timer.Stop();
                Administration.ToLog("End calc recommendations - " + Timer.ElapsedMilliseconds + " ms");
                return Result;
            }

            return new List<Course>();
        }
    }
}