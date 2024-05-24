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
        // Events to return status and time to the front end
        public event Action<int> ProgressChanged;
        public event Action<Stopwatch> TimeChanged;
        
        // Constans for the staic values
        const int alphabetLetterCount = 26;


        int wordLength;
        int numberOfWords;
        int numberOfAlphabeticWordsToRun = 1;

        
        List<List<string>> result = new List<List<string>>();

        // Used to calculate the percentage of completion to send to the front end
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

            // Setting the target word length
            this.wordLength = length;

            // Reading all lines from the specified file
            string[] readFile = File.ReadAllLines(filepath);

            // Loop through each line in the read file
            for (int i = 0; i < readFile.Count(); i++)
            {
                // Check if the current word meets the specified length and doesn´t contains double letters
                if (CheckLength(readFile[i], length) && CheckDoubleLetter(readFile[i]))
                {
                    // Convert the valid word to its bit representation and process it
                    FromStringToBits(readFile[i]);
                }
            }

            // Sorting the alphabet dictionary by values in ascending order
            sortedAlphabet = (
                from entry in alphabetDictionary
                orderby entry.Value ascending
                select entry
            ).ToList();

            // Initialize an array to hold lists of bits grouped by sorted alphabet
            alphabetLists = new int[sortedAlphabet.Count][];

            for (int i = 0; i < sortedAlphabet.Count; i++)
            {
                // Filter bitList to include only those bits that match the current sorted alphabet key
                alphabetLists[i] = bitList.Where(x => (x & sortedAlphabet[i].Key) != 0).ToArray();
                // Update bitList to remove the processed bits
                bitList = bitList.Where(x => (x & sortedAlphabet[i].Key) == 0).ToList();
            }
            Calculate(length);

            // Stop the timer and invoke the TimeChanged event with the elapsed time, for the front end to display
            stopwatch.Stop();
            TimeChanged?.Invoke(stopwatch);

            // Invoke ProgressChanged for the front end to mark as complete 
            ProgressChanged?.Invoke(100);

            // Return the result of the run
            return result;
        }

        private void Calculate(int length)
        {
            // Sends 0% to the GUI to start showing the progressbar
            ProgressChanged?.Invoke(0);
            
            // Calculate the start values like the number of words per combination
            CalculateStartValues();

            // Rund the recursive function to gather all the possible word combinations
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



        // Best 12 sec debug 4.5 sec release
        private void WordCombinationFinder(int usedBits, int pointer, Stack<int> matchedBits)
        {
            if (sortedAlphabet == null) return;

            // Check if the matchedBits stack has reached the required number of words
            if (matchedBits.Count == numberOfWords)
            {
                // Thread-safe addition of the matched combination to the result list
                lock (result)
                {
                    result.Add(matchedBits.Select(bit => dictionary[bit]).ToList());
                }
                return;
            }

            // Calculate the maximum index to iterate over the alphabet lists
            int maxIndex = alphabetLists.Length - (alphabetLists.Length - numberOfAlphabeticWordsToRun - matchedBits.Count * numberOfWords);

            // Iterate over the alphabet lists starting from the given pointer
            for (int letter = pointer; letter < maxIndex && alphabetLists.Length > letter && alphabetLists[letter] != null; letter++)
            {
                // Skip if the current letter's key has already been used
                if ((sortedAlphabet[letter].Key & usedBits) != 0) continue;

                // If no bits are matched yet, use parallel processing for better performance on the first of the five words
                if (matchedBits.Count == 0)
                {
                    Parallel.ForEach(alphabetLists[letter], bit =>
                    {
                        // Skip if the current bit overlaps with usedBits
                        if ((bit & usedBits) != 0) return;

                        // Create a new stack for matched bits and push the current bit
                        var newMatchedBits = new Stack<int>(matchedBits);
                        newMatchedBits.Push(bit);

                        // Recursive call to find further combinations for the next words
                        WordCombinationFinder(usedBits | bit, letter + 1, newMatchedBits);

                        // Increment the done count safely and update progress
                        Interlocked.Increment(ref doneCount);
                        int progress = (int)(((double)doneCount / doneFinalCount) * 100);
                        ProgressChanged?.Invoke(progress);
                    });
                }
                else
                {
                    // Sequential processing for subsequent levels of recursion to find the words possible after the first one
                    var bitsList = alphabetLists[letter];
                    int count = bitsList.Count();

                    for (int i = 0; i < count; i++)
                    {
                        var bit = bitsList[i];

                        // Proceed if the current bit does not overlap with usedBits
                        if ((bit & usedBits) == 0)
                        {
                            // Push the current bit to the stack
                            matchedBits.Push(bit);
                            // Recursive call to find further combination
                            WordCombinationFinder(usedBits | bit, letter + 1, matchedBits);
                            // Pop the bit after returning from recursion
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