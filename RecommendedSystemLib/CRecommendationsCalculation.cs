using System.Collections.Generic;
using System.Linq;
using Searcher;

namespace RecommendedSystemLib
{
    public class CRecommendationsCalculation
    {
        SortedList<double, CRecommendCourse> RecCoursesListByName;
        SortedList<double, CRecommendCourse> RecCoursesListByChar;
        static List<Course> m_NamesCourseList;
        static List<CRecommendCourse> m_NamesList;
        static SortedList<string, int> m_CountWordMap;

        public static int WordCounter(string Word)
        {
            int Count = 0;
            Word = Word.ToLower();
            if (m_CountWordMap.Keys.Contains(Word))
            {
                Count = m_CountWordMap[Word];
            }
            else
            {
                for (int i = 0; i < m_NamesCourseList.Count; i++)
                {
                    string Name = (m_NamesCourseList[i]).Name.ToLower();
                    if (Name.IndexOf(Word.ToLower()) >= 0)
                    {
                        Count++;
                    }
                }
                if (m_CountWordMap.IndexOfKey(Word) == -1) m_CountWordMap.Add(Word, Count);
                else m_CountWordMap[Word] += Count;
            }
            return Count;
        }

        private void ToRecommendCourse(List<Course> Courses)
        {
            m_NamesList = new List<CRecommendCourse>();
            foreach(var course in Courses)
            {
                m_NamesList.Add(new CRecommendCourse(course));
            }
        }

        private void CheckKey(SortedList<double, CRecommendCourse> Courses, ref double Value)
        {
            int Index = Courses.IndexOfKey(Value);
            while (Index >= 0)
            {
                if (Index != Courses.Count - 1)
                {
                    if (Courses.Keys[Index + 1] - Value >= 0.00000000001)
                    {
                        Value = (Courses.Keys[Index] + Courses.Keys[Index + 1]) / 2;
                    }
                    else
                    {
                        Value -= 0.00000000001;
                    }
                }
                else
                {
                    Value -= 0.00000000001;
                }
                Index = Courses.IndexOfKey(Value);
            }
        }

        private double CalcSimilarityDegree(CCharactersBase Char, SortedList<double, CRecommendCourse> Courses, 
            CRecommendCourse UserRequest, int CourseIndex, bool IsName = true)
        {
            double CalcName;
            if (Char is CName)
            {
                CalcName = Char.CalcSimilarityDegree(UserRequest.NameCharacters);
            }
            else
            {
                CalcName = Char.CalcSimilarityDegree(UserRequest.CourseCharacters);
            }
            CheckKey(Courses, ref CalcName);            
            return CalcName;
        }

        private void FindSubAndTime(CRecommendCourse course, List<int> SelectSub, List<int> SelectTime, ref int Sub, ref int Time)
        {
            int i = 0, MinDiffSub = 1000, MinDiffTime = 1000;
            while (i < SelectSub.Count || i < SelectTime.Count)
            {
                if(i < SelectSub.Count)
                {
                    int Diff = System.Math.Abs(SelectSub[i] - course.SubjectValue);
                    Sub = Diff < MinDiffSub ? SelectSub[i] : Sub;
                    MinDiffSub = System.Math.Min(Diff, MinDiffSub);
                }

                if (i < SelectTime.Count)
                {
                    int Diff = System.Math.Abs(SelectTime[i] - course.StartTimeValue);
                    Time = Diff < MinDiffTime ? SelectTime[i] : Time;
                    MinDiffTime = System.Math.Min(Diff, MinDiffTime);
                }

                i++;
            }
        }

        private void FindSortedCourseList(List<Course> NamesList, Course UserRequestText, 
            List<int> SelectSub, List<int> SelectTime)
        {
            m_NamesCourseList = NamesList;
            CWord.NamesNumber = NamesList.Count;
            m_CountWordMap = new SortedList<string, int>();
            ToRecommendCourse(NamesList);
            RecCoursesListByName = new SortedList<double, CRecommendCourse>();
            RecCoursesListByChar = new SortedList<double, CRecommendCourse>();
            CWord.NamesNumber = NamesList.Count + 1;
            CRecommendCourse UserRequest = new CRecommendCourse(UserRequestText);
            //Проход по очередному списку "отложенных"
            for (int i = 0; i < m_NamesList.Count; i++)
            {
                double CalcName = 0;
                //Если название не было задано, не расчитываем для него
                if (UserRequestText.Name != "")
                {
                    CalcName = CalcSimilarityDegree(m_NamesList[i].NameCharacters, RecCoursesListByName, UserRequest, i);
                    RecCoursesListByName.Add(CalcName, m_NamesList[i]);
                }
                //Может быть несколько выбранных чекбоксов, перебирать все сочетания долго
                //Курс был отложен из-за близости к конкретному значению, поэтому ищем его 
                int Sub = 0, Time = 0;
                FindSubAndTime(m_NamesList[i], SelectSub, SelectTime, ref Sub, ref Time);
                //Формируем курс, с которым нужно сравнивать характеристики
                UserRequest.StartTimeValue = Time;
                UserRequest.SubjectValue = Sub;
                UserRequest.RecalcD1();
                //Расчитываем степень схожести
                CalcName = CalcSimilarityDegree(m_NamesList[i].CourseCharacters, RecCoursesListByChar, UserRequest, i);
                RecCoursesListByChar.Add(CalcName, m_NamesList[i]);
            }
        }

        private List<Course> IntersectResults(List<SortedList<double, CRecommendCourse>> ResultName,
                                                                  List<SortedList<double, CRecommendCourse>> ResultChar)
        {
            List<Course> Result = new List<Course>();
            SortedList<double, Course> SortRes = new SortedList<double, Course>();
            //Цикл по всем спискам
            for (int i = 0; i < ResultName.Count; i++)
            {
                //Цикл по подсписку, ищем место для курса в результате
                //Смотрим на его место в обоих списках и берем среднее
                SortedList<double, CRecommendCourse> Res = new SortedList<double, CRecommendCourse>();
                for (int j = ResultName[i].Count - 1; j >= 0; j--)
                {
                    int Index = ResultChar[i].IndexOfValue(ResultName[i].ElementAt(j).Value);
                    //Место в списке названий важнее, чем место в списке характеристик
                    double ResVal = ((double)j * 2 + Index) / 3;
                    CheckKey(Res, ref ResVal);
                    Res.Add(ResVal, ResultName[i].ElementAt(j).Value);
                }
                if(ResultName[i].Count == 0)
                {
                    foreach (var course in ResultChar[i])
                    {
                        Result.Add(course.Value);
                        if (Result.Count >= 20)
                            return Result;
                    }
                    return Result;
                }

                foreach(var course in Res)
                {
                    Result.Add(course.Value);
                    if (Result.Count >= 15)
                        return Result;
                }
            }          

            return Result;
        }

        public List<Course> FindResult (SortedList<int, List<Course>> NamesList, 
            string UserRequestText, List<int> SelectSub, List<int> SelectTime, bool IsSertificate, List<bool> SelectAuditory)
        {
            List<SortedList<double, CRecommendCourse>> ResultName = new List<SortedList<double, CRecommendCourse>>();
            List<SortedList<double, CRecommendCourse>> ResultChar = new List<SortedList<double, CRecommendCourse>>();

            List<int> Keys = NamesList.Keys.ToList();
            Keys.Reverse();
            int k = 0;
            foreach (var Key in Keys)
            {
                Administration.ToLog("Synonyms count = " + Key + " Courses count = " + NamesList[Key].Count);
                FindSortedCourseList(NamesList[Key], 
                    new Course(UserRequestText, "", "", "", "", "", IsSertificate, SelectAuditory[0], SelectAuditory[1], SelectAuditory[2]), 
                    SelectSub, SelectTime);
                k++;

                ResultChar.Add(RecCoursesListByChar);
                ResultName.Add(RecCoursesListByName);
                
                if (ResultName.Count >= 15) break;
            }

            return IntersectResults(ResultName, ResultChar);
        }
    }
}
