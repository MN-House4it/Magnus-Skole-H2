using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WordProgram
{
    public class WordFinder
    {
        // Events to return status and time
        public event Action<int> ProgressChanged;
        public event Action<Stopwatch> TimeChanged;
        
        // Constans for the staic values
        const int alphabetLetterCount = 26;


        int wordLength;
        //int alphabetLetterRemaning;
        int numberOfWords;

        int numberOfAlphabeticWordsToRun = 1;

        
        List<List<string>> result = new List<List<string>>();
  

        int doneCount = 0;
        int doneFinalCount = 0;


        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        List<int> bitList = new List<int>();
        Dictionary<int, int> alphabetDictionary = new Dictionary<int, int>();
        int[][] alphabetLists;
        List<KeyValuePair<int, int>> sortedAlphabet;


        public List<List<string>> GatherWords(string filepath, int length)
        {
            // Timer to see how long it takes to complet
            var stopwatch = new Stopwatch();
            stopwatch.Start();


            this.wordLength = length;

            string[] readFile = File.ReadAllLines(filepath);

            for (int i = 0; i < readFile.Count(); i++)
            {
                if (CheckLength(readFile[i], length) && CheckDoubleLetter(readFile[i]))
                {
                    FromStringToBits(readFile[i]);
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


            stopwatch.Stop();
            TimeChanged?.Invoke(stopwatch);


            ProgressChanged?.Invoke(100);

            return result;
        }

        private void WordsArray(int length)
        {
            // Sends 0% to the GUI to start showing the progressbar
            ProgressChanged?.Invoke(0);
            
            CalculateStartValues();

  
            WordCombinationFinder(0, 0, new Stack<int>());
        }

        private void CalculateStartValues()
        {
            // Calculate how many words i possible with the choose word length
            double numberOfWords = alphabetLetterCount / wordLength;
            this.numberOfWords = (int)Math.Floor(numberOfWords);

            // Calculate how many letters in the alphabetic list to run
            var alphabetLetterRemaning = alphabetLetterCount % wordLength;
            numberOfAlphabeticWordsToRun += alphabetLetterRemaning;

            // Calculate the the total number of parallel loop runs
            for (int i = 0; i < numberOfAlphabeticWordsToRun; i++)
            {
                if (alphabetLists.Count() > i)
                {
                    doneFinalCount += alphabetLists[i].Count();
                }
            }            
        }




        // Best 16 sec debug 10 sec release
        private void WordCombinationFinder(int usedBits, int pointer, Stack<int> matchedBits)
        {
            if (sortedAlphabet == null) return;

            if (matchedBits.Count == numberOfWords)
            {
                lock (result)
                {
                    result.Add(matchedBits.Select(bit => dictionary[bit]).ToList());
                }
                return;
            }

            int maxIndex = alphabetLists.Length - (alphabetLists.Length - numberOfAlphabeticWordsToRun - matchedBits.Count * numberOfWords);
            for (int letter = pointer; letter < maxIndex && alphabetLists.Length > letter && alphabetLists[letter] != null; letter++)
            {
                if ((sortedAlphabet[letter].Key & usedBits) != 0) continue;

                if (matchedBits.Count == 0)
                {
                    Parallel.ForEach(alphabetLists[letter], bit =>
                    {
                        if ((bit & usedBits) != 0) return;

                        var newMatchedBits = new Stack<int>(matchedBits);
                        newMatchedBits.Push(bit);

                        WordCombinationFinder(usedBits | bit, letter + 1, newMatchedBits);

                        Interlocked.Increment(ref doneCount);
                        int progress = (int)(((double)doneCount / doneFinalCount) * 100);
                        ProgressChanged?.Invoke(progress);
                    });

                }
                else
                {
                    //foreach (var bit in alphabetLists[letter].Where(bit => (bit & usedBits) == 0))
                    //{
                    //    matchedBits.Push(bit);
                    //    WordMatches(usedBits | bit, letter + 1, matchedBits);
                    //    matchedBits.Pop();
                    //}

                    var bitsList = alphabetLists[letter];
                    int count = bitsList.Count();

                    for (int i = 0; i < count; i++)
                    {
                        var bit = bitsList[i];
                        if ((bit & usedBits) == 0)
                        {
                            matchedBits.Push(bit);
                            WordCombinationFinder(usedBits | bit, letter + 1, matchedBits);
                            matchedBits.Pop();
                        }
                    }
                }
            }
        }





        // Converts the string words into bits and store them in the lists
        private void FromStringToBits(string word)
        {
            int bit = 0;
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

                int bitnum = 1 << (word[i] - 'a');

                if (!alphabetDictionary.ContainsKey(bitnum))
                {
                    alphabetDictionary.Add(bitnum, 1);
                }
                else
                {
                    alphabetDictionary[bitnum] += 1;
                }
            }
        }

        // Test if the word length is correct
        private bool CheckLength(string word, int length)
        {
            return word.Length == length;
        }

        // Test if the word is the right length
        private bool CheckDoubleLetter(string word)
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