using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace RecommendedSystemLib
{
    
    public class CStemmerPorter
    {
        private string EMPTY = "";
        private string S1 = "$1";
        private string S13 = "$1$3";
        private string SN = "н";
        private Regex PERFECTIVEGROUND = new Regex("(ив|ивши|ившись|ыв|ывши|ывшись|вши|вшись)$");
        private Regex REFLEXIVE = new Regex("(ся|сь)$");
        private Regex ADJECTIVE = new Regex("(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$");
        private Regex PARTICIPLE = new Regex("(.*)(ивш|ывш|ующ)$|(?<=[ая])(ем|нн|вш|ющ|щ)$");
        private Regex VERB = new Regex("(.*)(ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю)$|(?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)$");
        private Regex NOUN = new Regex("(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$");
        private Regex I = new Regex("и$");
        private Regex P = new Regex("ь$");
        private Regex NN = new Regex("нн$");
        private Regex DERIVATIONAL = new Regex(".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$");
        private Regex DER = new Regex("ость?$");
        private Regex SUPERLATIVE = new Regex("(ейше|ейш)$");
        private char[] const1 = { 'а', 'е', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
        
        private string GetWord(string Word)
        {
            Word.Replace('ё', 'е');
            int pos = Word.IndexOfAny(const1);
            if (pos >= 0)
            {
                string pre = Word.Substring(0, pos + 1);
                string rv = Word.Substring(pos + 1);
                string temp = PERFECTIVEGROUND.Replace(rv, EMPTY);
                if (rv.Length != temp.Length)
                {
                    rv = temp;
                }
                else
                {
                    rv = REFLEXIVE.Replace(rv, EMPTY);
                    temp = ADJECTIVE.Replace(rv, EMPTY);
                    if (rv.Length != temp.Length)
                    {
                        rv = temp;
                        rv = PARTICIPLE.Replace(rv, S13);
                    }
                    else
                    {
                        temp = VERB.Replace(rv, S13);
                        if (rv.Length != temp.Length)
                        {
                            rv = temp;
                        }
                        else
                        {
                            rv = NOUN.Replace(temp, EMPTY);
                        }
                    }
                }
                rv = I.Replace(rv, EMPTY);
                if (DERIVATIONAL.Match(rv) != null)
                {
                    rv = DER.Replace(rv, EMPTY);
                }
                temp = P.Replace(rv, EMPTY);
                if (temp.Length != rv.Length)
                {
                    rv = temp;
                }
                else
                {
                    rv = SUPERLATIVE.Replace(rv, EMPTY);
                    rv = NN.Replace(rv, SN);
                }
                Word = pre + rv;
            }

            return Word;
        }


        public List<string> NameParseStem(string Name)
        {
            List<string> WordList = new List<string>();
            char[] Separators = { ' ', ',', '.', '/', ':', ';', '|', '\'', '"', '(', ')', '+', '-', '=', '*', '&' };
            string[] Words = Name.Split(Separators);
            foreach (string word in Words)
            {
                string Word = GetWord(word).ToLower();
                WordList.Add(Word);
            }

            return WordList;
        }

        public CWord StemmerPorter(string StrWord)
        {
            int CountWord = 0;

            StrWord = GetWord(StrWord).ToLower();
            CountWord = CRecommendationsCalculation.WordCounter(StrWord);

            return new CWord(CountWord, StrWord);
        }
    }
}
