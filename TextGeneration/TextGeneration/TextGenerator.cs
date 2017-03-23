using System;
using System.Collections.Generic;
using System.Text;

namespace TextGeneration
{
    class TextGenerator
    {
        static Random rnd = new Random();

        class NGram
        {
            public List<string> values = new List<string>();
        }

        static Dictionary<string, List<NGram>> MakeNgrams(string text, int n)
        {
            Dictionary<string, List<NGram>> ngrams = new Dictionary<string, List<NGram>>();

            List<string> words = new List<string>();
            words.AddRange(text.Split(' ', '\n'));

            for (int i = 0; i < words.Count; i++)
            {
                if (!ngrams.ContainsKey(words[i]))
                {
                    ngrams[words[i]] = new List<NGram>();
                }

                NGram ngram = new NGram();
                for (int j = 0; j < n && i + j + 1 < words.Count; j++)
                {
                    ngram.values.Add(words[i + j + 1]);
                }
                ngrams[words[i]].Add(ngram);
            }

            return ngrams;
        }

        static string GenerateNextUnit(ref string unit, Dictionary<string, List<NGram>> ngrams)
        {
            List<NGram> unitngrams = ngrams[unit];

            if (unitngrams == null) { return null; }

            int pos = rnd.Next(0, unitngrams.Count);
            string result = "";
            List<string> values = unitngrams[pos].values;
            foreach (string s in values)
            {
                result += " " + s;
            }
            
            if (values.Count != 0) { unit = values[values.Count - 1]; }


            return result;
        }

        static string GenerateText(Dictionary<string, List<NGram>> ngrams, int length)
        {
            List<string> units = new List<string>();

            foreach (KeyValuePair<string, List<NGram>> entry in ngrams)
            {
                units.Add(entry.Key);
            }


            int startpos = rnd.Next(0, ngrams.Count);

            string currentunit = units[startpos];

            string result = currentunit;

            while (length > 0)
            {
                string s = GenerateNextUnit(ref currentunit, ngrams);

                if (s == null) break;

                result += s;
                length--;
            }

            return result;
        }

        public static string GenerateText(int length, int n, List<string> texts)
        {
            string text = "";

            for (int i = 0; i < texts.Count; i++)
            {
                text += texts[i];
            }

            Dictionary<string, List<NGram>> ngrams = MakeNgrams(text, n);

            return GenerateText(ngrams, length);
        }
    }
}
