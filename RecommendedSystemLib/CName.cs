using System;
using System.Collections.Generic;

namespace RecommendedSystemLib
{
    //Для расчёта степени схожести названия с запросом
    public class CName : CCharactersBase
    {
        //Список параметров для каждого слова из названия
        public List<CWord> WordsInName;
        static List<string> StopWords = new List<string>()
        {
               "и", "а", "но",
               "еще","него","сказать",
               "а", "ж", "нее", "неё", "со", "её",
               "без", "же", "ней", "совсем",
               "более", "жизнь", "нельзя", "так",
               "больше", "нет", "такой",
               "будет", "зачем", "ни", "там",
               "будто", "здесь", "нибудь", "тебя",
               "бы", "никогда", "тем",
               "был", "ним", "теперь",
               "была", "из","за", "них", "то",
               "были", "или", "ничего", "тогда",
               "было", "им", "но", "того",
               "быть", "иногда", "ну", "тоже",
               "в", "их", "о", "только",
               "вам", "к", "об", "том",
               "вас", "кажется", "один", "тот",
               "вдруг", "как", "он", "три",
               "ведь", "какая", "она", "тут",
               "во", "какой", "они", "ты",
               "вот", "когда", "опять", "у",
               "впрочем", "конечно", "от", "уж",
               "все", "которого", "перед", "уже",
               "всегда", "которые", "по", "хорошо",
               "всего", "кто", "под", "хоть",
               "всех", "куда", "после", "чего",
               "всю", "ли", "потом",
               "вы", "лучше", "потому", "чем",
               "г", "между", "почти", "через",
               "где", "меня", "при что",
               "говорил", "мне", "про", "чтоб",
               "да", "много", "раз", "чтобы",
               "даже", "может", "разве", "чуть",
               "можно", "с", "эти",
               "для", "мой", "сам", "этого",
               "до", "моя", "свое", "этой",
               "другой", "мы", "свою", "этом",
               "его", "на", "себе", " этот",
               "ее", "над", "себя", "эту",
               "ей", "надо", "сегодня", "я",
               "ему", "наконец", "сейчас",
               "если", "нас", "сказал",
               "есть", "не", "сказала",
        };

        private void NameParse(string Name)
        {
            CStemmerPorter StemmerPorterMethod = new CStemmerPorter();
            char[] Separators = { ' ', ',', '.', '/', ':', ';', '|', '\'', '"', '(', ')', '+', '-', '=', '*', '&' };
            string[] WordsTmp = Name.Split(Separators);
            List<string> Words = new List<string>();
            for(int i = WordsTmp.Length - 1; i >= 0; i--)
            {
                if(StopWords.IndexOf(WordsTmp[i]) < 0 && !String.IsNullOrEmpty(WordsTmp[i]))
                {
                    Words.Add(WordsTmp[i]);
                }
            }
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
