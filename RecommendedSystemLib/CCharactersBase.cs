using System;
using System.Collections.Generic;

namespace RecommendedSystemLib
{
    public abstract class CCharactersBase
    {
        //Параметр для расчета степени схожести
        protected double m_D1;

        public double D1
        {
            get { return m_D1; }
        }

        protected virtual void CalcD1()
        {
        }

        protected virtual double CalcDP(CCharactersBase UserRequest)
        {
            return 1;
        }

        /// <summary>
        /// Расчет степени схожести текущего названия курса с запросом пользователя
        /// </summary>
        /// <param name="UserRequest">Параметры пользовательского запроса</param>
        /// <returns></returns>
        public double CalcSimilarityDegree(CCharactersBase UserRequest)
        {
            double DP = CalcDP(UserRequest);
            return DP / (m_D1 * UserRequest.D1);
        }
    }

    public class CCharacters : CCharactersBase
    {
        public List<int> CharWeight { get; set; }

        public CCharacters(int subject, int startTime, bool isSertificate, bool isSchool, bool isUniversity, bool isQualification)
        {
            //Чтобы нигде не появилось деление на 0, учитывая важность не самих значений, а только их разности:
            //пусть 1 - критерий выбран, а 3 - критерий не выбран
            CharWeight = new List<int>();
            CharWeight.Add(subject);
            CharWeight.Add(startTime);
            CharWeight.Add(isSertificate ? 1 : 3);
            CharWeight.Add(isSchool ? 1 : 3);
            CharWeight.Add(isUniversity ? 1 : 3);
            CharWeight.Add(isQualification ? 1 : 3);

            CalcD1();
        }

        protected override void CalcD1()
        {
            double SumSqr = 0;
            foreach (var Ch in CharWeight)
            {
                SumSqr += Ch * Ch;
            }

            m_D1 = Math.Sqrt(SumSqr);
        }

        public void RecalcD1(int newsubject, int newstartTime)
        {
            CharWeight[0] = newsubject;
            CharWeight[1] = newstartTime;
            CalcD1();
        }

        protected override double CalcDP(CCharactersBase UserRequest)
        {
            CCharacters UserChar = (CCharacters)UserRequest;
            double DP = 0;
            int Count = CharWeight.Count;
            for (int i = 0; i <  Count; i++)
            {
                DP += CharWeight[i] * UserChar.CharWeight[i];
            }
            return DP;
        }
    }
}
