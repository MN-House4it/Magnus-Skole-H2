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


        // Test af Magnus 1234
        Dictionary<int, List<int>> test1 = new Dictionary<int, List<int>>();

        public List<List<string>> GatherWords(string filepath, int length)
        {
            this.wordLength = length;

            string[] readFile = File.ReadAllLines(filepath);

            for (int i = 0; i < readFile.Count(); i++)
            {
                if (CheckLength(readFile[i], length) && CheckDoubleLetter(readFile[i]))
                {
                    FromStringToBits(readFile[i]);
                }
            }

            test1 = test1.OrderBy(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);


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
            ProgressChanged?.Invoke(100);

            return result;
        }

        private void WordsArray(int length)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            CalculateStartValues();

  
            WordMatches(0, 0, new Stack<int>());
            //TESTCAL(0, 0, new List<int>());

            stopwatch.Stop();
            TimeChanged?.Invoke(stopwatch);
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



        //private void WordMatches(int usedBits, int pointer, List<int> matchedBits, bool isFirst)
        //{


        //    object lockObj = new object();


        //    if (sortedAlphabet is null) return;
        //    if (matchedBits.Count() == numberOfWords)
        //    {
        //        List<string> newDataSet = new List<string>();
        //        foreach (var bit in matchedBits)
        //        {
        //            newDataSet.Add(dictionary[bit]);
        //        }
        //        result.Add(newDataSet);
        //        return;
        //    }

        //    var gd = alphabetLists.Count() - (alphabetLists.Count() - numberOfAlphabeticWordsToRun - matchedBits.Count() * numberOfWords);


        //    //int letter = pointer; letter < alphabetLists.Count() - (alphabetLists.Count() - numberOfAlphabeticWordsToRun - matchedBits.Count() * numberOfWords) && alphabetLists.Count() > letter && alphabetLists[letter] != null; letter++
        //    for (int letter = pointer; letter < alphabetLists.Count() - (alphabetLists.Count() - numberOfAlphabeticWordsToRun - matchedBits.Count() * numberOfWords) && alphabetLists.Count() > letter && alphabetLists[letter] != null; letter++)
        //    {
        //        if ((sortedAlphabet[letter].Key & usedBits) != 0) continue;
        //        if (matchedBits.Count() == 0)
        //        {
        //            Parallel.ForEach(alphabetLists[letter], bit =>
        //            {                      
        //                var fourBitCollection = new List<int>(matchedBits);
        //                lock (lockObj)
        //                {
        //                    fourBitCollection.Add(bit);
        //                }

        //                WordMatches(usedBits | bit, letter + 1, fourBitCollection, false);

        //                doneCount++;
        //                double hf = ((double)doneCount / doneFinalCount) * 100;
        //                int progress = Convert.ToInt32(((double)doneCount / doneFinalCount) * 100);
        //                ProgressChanged?.Invoke(progress);
        //            });

        //        }
        //        else
        //        {
        //            foreach (var bit in dictionary.Where(x => (x.Key & usedBits) == 0))
        //            {
        //                var fourBitCollection = new List<int>(matchedBits);
        //                fourBitCollection.Add(bit.Key);

        //                //if (isFirst)
        //                //{
        //                //    doneCount++;
        //                //}
        //                WordMatches(usedBits | bit.Key, letter + 1, fourBitCollection, false);
        //            }

        //        }
        //    }
        //}





        //private void TESTCAL(int usedBits, int pointer, List<int> matchedBits, bool isFirst)
        //{


        //    object lockObj = new object();


        //    if (matchedBits.Count() >= 4)
        //    {

        //    }

        //    if (sortedAlphabet is null) return;
        //    if (matchedBits.Count() == numberOfWords)
        //    {
        //        List<string> newDataSet = new List<string>();
        //        foreach (var bit in matchedBits)
        //        {
        //            newDataSet.Add(dictionary[bit]);
        //        }
        //        result.Add(newDataSet);
        //        return;
        //    }


        //    if (matchedBits.Count() == 0)
        //    {
        //        for (int letter = 0; letter < numberOfAlphabeticWordsToRun; letter++)
        //        {
        //            Parallel.ForEach(test1[letter + 1], bit =>
        //            {
        //                var fourBitCollection = new List<int>(matchedBits);
        //                lock (lockObj)
        //                {
        //                    fourBitCollection.Add(bit);
        //                }

        //                TESTCAL(usedBits | bit, letter + 1, fourBitCollection, false);

        //                doneCount++;
        //                double hf = ((double)doneCount / doneFinalCount) * 100;
        //                int progress = Convert.ToInt32(((double)doneCount / doneFinalCount) * 100);
        //                //ProgressChanged?.Invoke(progress);
        //            });
        //        }              
        //    }
        //    else
        //    {
        //        for (int letter = pointer; letter < alphabetLists.Count() - (alphabetLists.Count() - numberOfAlphabeticWordsToRun - matchedBits.Count() * numberOfWords) && alphabetLists.Count() > letter && alphabetLists[letter] != null; letter++)
        //        {
        //            foreach (var bit in dictionary.Where(x => (x.Key & usedBits) == 0))
        //            {
        //                var fourBitCollection = new List<int>(matchedBits);
        //                fourBitCollection.Add(bit.Key);

        //                //if (isFirst)
        //                //{
        //                //    doneCount++;
        //                //}
        //                TESTCAL(usedBits | bit.Key, letter + 1, fourBitCollection, false);
        //            }
        //        }                
        //    }
        //}

         
        // Best 16 sec debug 10 sec release
        int lastProgress = -1;
        private void WordMatches(int usedBits, int pointer, Stack<int> matchedBits)
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

                        WordMatches(usedBits | bit, letter + 1, newMatchedBits);

                        Interlocked.Increment(ref doneCount);
                        int progress = (int)(((double)doneCount / doneFinalCount) * 100);
                        //if (progress != lastProgress)
                        //{
                        //    lastProgress = progress;
                            
                        //}
                        ProgressChanged?.Invoke(progress);
                    });
                }
                else
                {
                    foreach (var bit in alphabetLists[letter].Where(bit => (bit & usedBits) == 0))
                    {
                        matchedBits.Push(bit);
                        WordMatches(usedBits | bit, letter + 1, matchedBits);
                        matchedBits.Pop();
                    }
                }   
            }
        }
        //private void WordMatches(int usedBits, int pointer, int matchedCount, int[] matchedBits)
        //{
        //    if (sortedAlphabet == null) return;

        //    // Base case: if we've matched the required number of words
        //    if (matchedCount == numberOfWords)
        //    {
        //        lock (result)
        //        {
        //            result.Add(matchedBits.Take(matchedCount).Select(bit => dictionary[bit]).ToList());
        //        }
        //        return;
        //    }

        //    int maxIndex = alphabetLists.Length - (alphabetLists.Length - numberOfAlphabeticWordsToRun - matchedCount * numberOfWords);

        //    // Loop through potential letter groups
        //    for (int letter = pointer; letter < maxIndex && alphabetLists.Length > letter && alphabetLists[letter] != null; letter++)
        //    {
        //        if ((sortedAlphabet[letter].Key & usedBits) != 0) continue;

        //        if (matchedCount == 0)
        //        {
        //            Parallel.ForEach(alphabetLists[letter], bit =>
        //            {
        //                if ((bit & usedBits) != 0) return;

        //                int[] newMatchedBits = new int[numberOfWords];
        //                Array.Copy(matchedBits, newMatchedBits, matchedCount);
        //                newMatchedBits[matchedCount] = bit;

        //                WordMatches(usedBits | bit, letter + 1, matchedCount + 1, newMatchedBits);

        //                Interlocked.Increment(ref doneCount);
        //                int progress = (int)(((double)doneCount / doneFinalCount) * 100);
        //                ProgressChanged?.Invoke(progress);
        //            });
        //        }
        //        else
        //        {
        //            foreach (var bit in alphabetLists[letter].Where(bit => (bit & usedBits) == 0))
        //            {
        //                matchedBits[matchedCount] = bit;
        //                WordMatches(usedBits | bit, letter + 1, matchedCount + 1, matchedBits);
        //            }
        //        }
        //    }
        //}

















        private void TESTCAL(int usedBits, int pointer, List<int> matchedBits)
        {
            if (sortedAlphabet == null) return;
            if (matchedBits.Count == numberOfWords)
            {
                var newDataSet = matchedBits.Select(bit => dictionary[bit]).ToList();
                lock (result)
                {
                    result.Add(newDataSet);
                }
                return;
            }

            for (int letter = pointer; letter < alphabetLists.Length - (alphabetLists.Length - numberOfAlphabeticWordsToRun - matchedBits.Count * numberOfWords) && alphabetLists.Length > letter && alphabetLists[letter] != null; letter++)
            {
                if ((sortedAlphabet[letter].Key & usedBits) != 0) continue;

                if (matchedBits.Count == 0)
                {
                    Parallel.ForEach(alphabetLists[letter], bit =>
                    {
                        if ((bit & usedBits) != 0) return;

                        var newMatchedBits = new List<int>(matchedBits) { bit };
                        TESTCAL(usedBits | bit, letter + 1, newMatchedBits);

                        Interlocked.Increment(ref doneCount);
                        int progress = (int)(((double)doneCount / doneFinalCount) * 100);
                        ProgressChanged?.Invoke(progress);
                    });
                }
                else
                {
                    foreach (var bit in alphabetLists[letter].Where(bit => (bit & usedBits) == 0))
                    {
                        var newMatchedBits = new List<int>(matchedBits) { bit };
                        TESTCAL(usedBits | bit, letter + 1, newMatchedBits);
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


                if (!test1.ContainsKey(bitnum))
                {
                    test1.Add(bitnum, new List<int> { bit });
                }
                else
                {
                    test1[bitnum].Add(bit);
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