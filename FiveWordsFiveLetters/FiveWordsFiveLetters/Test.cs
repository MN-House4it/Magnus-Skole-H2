using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FiveWordsFiveLetters
{
    public class Test
    {
        const int alphabetLetterCount = 26;
        const string seperator = " - ";
        int wordLength;
        int alphabetLetterRemaning;
        int numberOfWords;

        string outputFileData = string.Empty;

        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        List<int> bitList = new List<int>();
        int fiveMatches = 0;
        Dictionary<int, int> alphabetDictionary = new Dictionary<int, int>();
        int value = 1;
        int[][] alphabetLists;
        List<KeyValuePair<int, int>> sortedAlphabet;
        int bit;
        public void gatherWords(string filepath, int length)
        {
            this.wordLength = length;

            string[] readFile = File.ReadAllLines(filepath);

            for (int i = 0; i < readFile.Count(); i++)
            {
                if (CheckLength(readFile[i], length) && CheckDouble(readFile[i]))
                {
                    FromString(readFile[i]);
                }
            }

            sortedAlphabet = (
                from entry in alphabetDictionary
                orderby entry.Value ascending
                select entry
            ).ToList();

            alphabetLists = new int[sortedAlphabet.Count][];

            for (int i = 0; i < sortedAlphabet.Count; i++)
            {
                alphabetLists[i] = bitList.Where(x => (x & sortedAlphabet[i].Key) != 0).ToArray();
                bitList = bitList.Where(x => (x & sortedAlphabet[i].Key) == 0).ToList();
            }

            WordsArray(length);
        }

        private int WordsArray(int length)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            CalculateRemaningNumAlphabet();

            WordMatches(0, 0, new List<int>());



            stopwatch.Stop();
            Console.WriteLine(outputFileData);
            return fiveMatches;
        }

        private void CalculateRemaningNumAlphabet()
        {
            // Calculate how many letters are remaining in the alphabet
            alphabetLetterRemaning = alphabetLetterCount % wordLength;
            double numberOfWords = alphabetLetterCount / wordLength;
            this.numberOfWords = (int)Math.Ceiling(numberOfWords);
        }


        List<List<int>> result = new List<List<int>>();
        private void WordMatches(int usedBits, int pointer, List<int> matchedBits)
        {
            object lockObj = new object();

            if (sortedAlphabet is null) return;
            if (matchedBits.Count() == numberOfWords)
            {
                //string lineToPrint = string.Empty;
                //foreach (var bit in matchedBits)
                //{
                //    lineToPrint += dictionary[bit] + seperator;
                //}
                //// Save to file later on
                ////Console.WriteLine(lineToPrint);
                //outputFileData += lineToPrint + "\n";
                result.Add(matchedBits);
                return;
            }



            for (int letter = pointer; letter <= alphabetLists.Count() - (alphabetLetterCount - alphabetLetterRemaning - matchedBits.Count() * numberOfWords) && alphabetLists.Count() > letter && alphabetLists[letter] != null; letter++)
            {
                if ((sortedAlphabet[letter].Key & usedBits) != 0) continue;
                if (matchedBits.Count() == 0)
                {
                    Parallel.ForEach(alphabetLists[letter].Where(x => (x & usedBits) == 0), bit =>
                    {
                        var fourBitCollection = new List<int>(matchedBits);
                        lock (lockObj)
                        {
                            fourBitCollection.Add(bit);
                        }
                        
                        WordMatches(usedBits | bit, letter + 1, fourBitCollection);
                    });
                }
                else
                {
                    foreach (int bit in alphabetLists[letter].Where(x => (x & usedBits) == 0))
                    {
                        var fourBitCollection = new List<int>(matchedBits);
                        fourBitCollection.Add(bit);
                        WordMatches(usedBits | bit, letter + 1, fourBitCollection);
                    }

                    //Parallel.ForEach(alphabetLists[letter].Where(x => (x & usedBits) == 0), bit =>
                    //{
                    //    var fourBitCollection = new List<int>(matchedBits);
                    //    lock (lockObj)
                    //    {
                    //        fourBitCollection.Add(bit);
                    //    }
                    //    //lock (lockObj)
                    //    //{
                    //    //    WordMatches(usedBits | bit, letter + 1, fourBitCollection);
                    //    //}
                    //    WordMatches(usedBits | bit, letter + 1, fourBitCollection);

                    //});

                }
            }
        }








        private void FromString(string word)
        {
            int bit = 0;
            int bitnum = 0;
            for (int i = 0; i < word.Length; i++)
            {
                bit |= 1 << (word[i] - 'a');
            }

            if (!dictionary.ContainsKey(bit))
            {
                dictionary.Add(bit, word);
                bitList.Add(bit);
            }

            for (int i = 0; i < word.Length; i++)
            {

                bitnum = 1 << (word[i] - 'a');

                if (!alphabetDictionary.ContainsKey(bitnum))
                {
                    value = 1;
                    alphabetDictionary.Add(bitnum, value);
                }
                else
                {
                    value++;
                    alphabetDictionary[bitnum] = value;
                }
            }

        }

        // Test if the word length is correct
        private bool CheckLength(string word, int length)
        {
            return word.Length == length;
        }

        // Test if the word is the right length
        private bool CheckDouble(string word)
        {
            bool notDoubleLetter = true;
            int wordLength = word.Length;
            for (int x = 0; x < wordLength; x++)
            {
                for (int y = 0; y < wordLength; y++)
                {
                    if (word[x] == word[y] && x != y)
                    {
                        notDoubleLetter = false;
                    }
                }
            }
            return notDoubleLetter;
        }
    }
}
