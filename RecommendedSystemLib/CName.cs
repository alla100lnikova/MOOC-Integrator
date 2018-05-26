using System;
using System.Collections.Generic;

namespace RecommendedSystemLib
{
    //Для расчёта степени схожести названия с запросом
    public class CName : CCharactersBase
    {
        //Список параметров для каждого слова из названия
        public List<CWord> WordsInName;
        private void NameParse(string Name)
        {
            CStemmerPorter StemmerPorterMethod = new CStemmerPorter();
            char[] Separators = { ' ', ',', '.', '/', ':', ';', '|', '\'', '"', '(', ')', '+', '-', '=', '*', '&' };
            string[] Words = Name.Split(Separators);
            foreach(string word in Words)
            {
                CWord Word = StemmerPorterMethod.StemmerPorter(word);
                WordsInName.Add(Word);
            }
            WordsInName.Sort();
        }

        protected override void CalcD1()
        {
            double SumSqr = 0;

            foreach (var Ch in WordsInName)
            {
                SumSqr += Ch.Weight * Ch.Weight;
            }

            m_D1 = Math.Sqrt(SumSqr);
        }

        /// <summary>
        /// Конструктор для расчета характеристик названия
        /// </summary>
        /// <param name="Name">Название курса</param>
        /// <param name="NewIdCourse">id курса в БД</param>
        public CName(string Name)
        {
            WordsInName = new List<CWord>();
            NameParse(Name);
            CalcD1();
        }

        protected override double CalcDP(CCharactersBase UserRequest)
        {
            CName UserReq = (CName)UserRequest;
            double DP = 0;
            int Count = UserReq.WordsInName.Count > WordsInName.Count ?
                        WordsInName.Count :
                        UserReq.WordsInName.Count;
            for (int i = 0; i < Count; i++)
            {
                DP += WordsInName[i].Weight * UserReq.WordsInName[i].Weight;
            }
            return DP;
        }
    }
}
