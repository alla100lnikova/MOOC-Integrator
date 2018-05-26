using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendedSystemLib
{
    //Слово + его вес
    public class CWord : IComparable
    {
        //Количество названий всего !!! не забыть задать
        public static int NamesNumber;
        //Вес слова
        private double m_Weight;
        //Слово в общей форме (без суффиксов и окончаний)
        private string m_Word;

        public double Weight
        {
            get { return m_Weight; }
        }

        public string Word
        {
            get { return m_Word; }
        }

        /// <summary>
        /// Конструктор для расчета все характеристик (полной инициализации)
        /// </summary>
        /// <param name="NamesWithWord">Количество названий, в которых есть слово</param>
        /// <param name="Word">Слово в общем виде (без суффиксов и окончания)</param>
        public CWord(int NamesWithThisWord, string Word)
        {
            m_Word = Word;

            if (NamesNumber != 0)
            {
                double Frequency = NamesWithThisWord / Convert.ToDouble(NamesNumber);
                double IDF = Math.Log(1 / Frequency, 2);
                double Noise = Frequency * IDF;
                double Signal = Math.Log(NamesNumber, 2) - Noise;
                m_Weight = Signal / Noise;
            }
        }

        public int CompareTo(object obj)
        {
            CWord Obj = obj as CWord;
            return m_Word.CompareTo(Obj.Word);
        }
    }
}
