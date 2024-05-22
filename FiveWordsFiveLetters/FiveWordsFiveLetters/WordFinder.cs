using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FiveWordsFiveLetters
{
    public class WordFinder
    {
        string _dataFilePath;
        string _outputFilePath;
        int _wordLength;

        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        Dictionary<int, int> alphabets = new Dictionary<int, int>();

        int letterValue;
        //List<int> bitList = new List<int>();
        //Dictionary<int, int> alphabetDictionary = new Dictionary<int, int>();
        //int value = 1;
        //int[][] alphabetLists;
        //List<KeyValuePair<int, int>> sortedAlphabet;
        //int bit;

        public string Run(string dataFilePath, string outputFilePath, int wordLength)
        {
            _dataFilePath = dataFilePath;
            _outputFilePath = outputFilePath;
            _wordLength = wordLength;

            ReadWordFile();
            alphabets = alphabets.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);


            //sortedAlphabet = (
            //    from entry in alphabetDictionary
            //    orderby entry.Value ascending
            //    select entry
            //).ToList();

            //alphabetLists = new int[sortedAlphabet.Count][];

            //for (int i = 0; i < sortedAlphabet.Count; i++)
            //{
            //    alphabetLists[i] = bitList.Where(x => (x & sortedAlphabet[i].Key) != 0).ToArray();
            //    bitList = bitList.Where(x => (x & sortedAlphabet[i].Key) == 0).ToList();
            //}

            //foreach (Alphabet alphabet in alphabets)
            //{
            //    Dictionary<int, string> jj = dictionary.Where(x => (x.Key & alphabet.letterBitnum) != 0).ToDictionary(x => x.Key, x => x.Value);


            //    alphabet.test = dictionary.Where(x => (x.Key & alphabet.letterBitnum) != 0).ToDictionary(x => x.Key, x => x.Value);
            //}

            //FiveWordsMatches(0, 0, new List<int>(), 5);

            return "";
        }


        public List<string> ReadWordFile()
        {
            var result = new List<string>();
            var lines = File.ReadAllLines(_dataFilePath);
            object lockObj = new object();

            Parallel.ForEach(lines, line =>
            {
                if (line.Count() == _wordLength && !HasDuplicateLetters(line))
                {
                    lock (lockObj)
                    {
                        WordToAdd(line);
                    }
                }
            });
            return result;
        }
        private void WordToAdd(string word)
        {
            int bit = 0;
            int bitnum = 0;
            for (int i = 0; i < word.Length; i++)
            {
                bit |= 1 << (word[i] - 'a');

                bitnum = 1 << (word[i] - 'a');

                if (!alphabets.ContainsKey(bitnum))
                {
                    letterValue = 1;
                    alphabets.Add(bitnum, letterValue);
                }
                //else
                //{
                //    letterValue++;
                //    alphabets.Where(x => x.letterBitnum == bitnum).FirstOrDefault().Value = letterValue;
                //}
            }

            if (!dictionary.ContainsKey(bit))
            {
                dictionary.Add(bit, word);
            }
        }
        public static bool HasDuplicateLetters(string word)
        {
            return word.GroupBy(c => c).Any(g => g.Count() > 1);
        }


        
























        private void FourWordsMatches(int usedBits, int pointer, List<int> matchedBits, int wordLength)
        {
            ////if (sortedAlphabet is null) return;
            //if (matchedBits.Count() == 4)
            //{
            //    string temps = string.Empty;
            //    for (int i = 0; i >= matchedBits.Count; i++)
            //    {
            //        temps += matchedBits[i] + " ";
            //    }
            //    Console.WriteLine(temps);
            //    //Console.WriteLine("{0} {1} {2} {3}", dictionary[matchedBits[0]], dictionary[matchedBits[1]], dictionary[matchedBits[2]], dictionary[matchedBits[3]]);
            //    //fiveMatches++;
            //    return;
            //}

            //for (int letter = pointer; letter <= alphabetLists.Count() - (26 - 2 - matchedBits.Count() * 6) && alphabetLists[letter] != null; letter++)
            //{
            //    if ((sortedAlphabet[letter].Key & usedBits) != 0) continue;
            //    if (matchedBits.Count() == 0)
            //    {
            //        Parallel.ForEach(alphabetLists[letter].Where(x => (x & usedBits) == 0), bit =>
            //        {
            //            var fourBitCollection = new List<int>(matchedBits);
            //            fourBitCollection.Add(bit);
            //            FourWordsMatches(usedBits | bit, letter + 1, fourBitCollection);
            //        });

            //    }
            //    else
            //    {
            //        foreach (int bit in alphabetLists[letter].Where(x => (x & usedBits) == 0))
            //        {
            //            var fourBitCollection = new List<int>(matchedBits);
            //            fourBitCollection.Add(bit);
            //            FourWordsMatches(usedBits | bit, letter + 1, fourBitCollection);
            //        }
            //    }


            //}

        }

    }
    public class Alphabet
    {
        public int letterBitnum { get; set;}
        public int Value { get; set;}
        public Dictionary<int, string> test { get; set;}
    }
}
