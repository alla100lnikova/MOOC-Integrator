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
        public List<int> CharWeight { get; }

        public CCharacters(int subject, int startTime, bool isSertificate, bool isSchool, bool isUniversity, bool isQualification)
        {
            CharWeight = new List<int>();
            CharWeight.Add(subject);
            CharWeight.Add(startTime);
            CharWeight.Add(isSertificate ? 1 : 0);
            CharWeight.Add(isSchool ? 1 : 0);
            CharWeight.Add(isUniversity ? 1 : 0);
            CharWeight.Add(isQualification ? 1 : 0);

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

        public void RecalcD1()
        {
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
