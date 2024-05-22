using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveWordsFiveLetters
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string wordFilePath = "C:\\H2\\FiveLetterFiveWordsTestData\\AllWords.txt"; // AllWords.txt SortedWords.txt

            var wordFinder = new WordFinder();
            //wordFinder.Run(wordFilePath, "", 5);


            var fiveWords = new Test();
            fiveWords.gatherWords(wordFilePath, 5);


            var rawData = ReadWordFile(wordFilePath);
            var filteredWordList = FilterWordList(rawData);
            var hh = filteredWordList.OrderBy(x => x).ToList();
            TEST(hh);
        }

        public static List<string> ReadWordFile(string path)
        {
            var result = new List<string>();
            var lines = File.ReadAllLines(path);
            object lockObj = new object();

            Parallel.ForEach(lines, line =>
            {
                if (line.Count() == 5)
                {
                    lock (lockObj)
                    {
                        result.Add(line);
                    }
                }
            });

            return result;
        }
        public static List<string> FilterWordList(List<string> allWords)
        {
            var result = new List<string>();
            object lockObj = new object();

            Parallel.ForEach(allWords, word =>
            {
                if (!HasDuplicateLetters(word))
                {
                    lock (lockObj)
                    {
                        result.Add(word);
                    }
                }
            });

            return result;
        }
        public static bool HasDuplicateLetters(string word)
        {
            return word.GroupBy(c => c).Any(g => g.Count() > 1);
        }
        public static List<string> GetPossibleWords(string words, List<string> allWords)
        {
            // Convert the letters string to a HashSet for faster lookup
            HashSet<char> lettersSet = new HashSet<char>(words);

            // Filter out the words that do not contain any letter from the given string
            return allWords.Where(word => word.All(c => !lettersSet.Contains(c))).ToList();
        }
        public static List<string> TEST(List<string> words)
        {

            Parallel.ForEach(words, word1 =>
            {
                List<string> wordsPossibleForWord2 = GetPossibleWords(word1, words);
                var word2List = wordsPossibleForWord2.Where(x => !HasDuplicateLetters(word1 + x));

                Parallel.ForEach(word2List, word2 =>
                {
                    List<string> wordsPossibleForWord3 = GetPossibleWords(word1 + word2, words);
                    var word3List = wordsPossibleForWord3.Where(x => !HasDuplicateLetters(word1 + word2 + x));
                    Parallel.ForEach(word3List, word3 =>
                    {
                        List<string> wordsPossibleForWord4 = GetPossibleWords(word1 + word2 + word3, words);
                        var word4List = wordsPossibleForWord4.Where(x => !HasDuplicateLetters(word1 + word2 + word3 + x));
                        Parallel.ForEach(word4List, word4 =>
                        {
                            List<string> wordsPossibleForWord5 = GetPossibleWords(word1 + word2 + word3 + word4, words);
                            var word5List = wordsPossibleForWord5.Where(x => !HasDuplicateLetters(word1 + word2 + word3 + word4 + x));
                            if (word5List.Count() > 0)
                            {
                                Console.WriteLine($"Found: {word1} {word2} {word3} {word4} {word5List.FirstOrDefault()}");
                            }
                        });
                    });
                });
            });



            //foreach (var word1 in words)
            //{
            //    var word2List = words.Where(x => !HasDuplicateLetters(word1 + x));
            //    foreach (var word2 in word2List)
            //    {
            //        var word3List = words.Where(x => !HasDuplicateLetters(word1 + word2 + x));
            //        foreach (var word3 in word3List)
            //        {
            //            var word4List = words.Where(x => !HasDuplicateLetters(word1 + word2 + word3 + x));
            //            foreach (var word4 in word4List)
            //            {
            //                var word5List = words.Where(x => !HasDuplicateLetters(word1 + word2 + word3 + word4 + x));
            //                if (word5List.Count() > 0)
            //                {
            //                    Console.WriteLine($"Found: {word1} {word2} {word3} {word4} {word5List.FirstOrDefault()}");
            //                }
            //            }
            //        }
            //    }
            //}
















            List<Words> result = new List<Words>();

            foreach (var word1 in words)
            {
                foreach (var word2 in words)
                {
                    if (result.Where(x => x.result().Contains(word1) && x.result().Contains(word2)).Count() == 0)
                    {
                        if (!HasDuplicateLetters(word1 + word2))
                        {
                            var newPair = new Words() { word1 = word1, word2 = word2 };
                            result.Add(newPair);
                        }
                    }                 
                }
            }

            List<Words> result2 = new List<Words>();
            //foreach (var pair1  in result)
            //{
            //    foreach (var pair2 in result)
            //    {
            //        if (!HasDuplicateLetters(pair1.word1 + pair1.word2 + pair2.word1 + pair2.word2))
            //        {
            //            var newPair = new Words() { word1 = pair1.word1, word2 = pair1.word2, word3 = pair2.word1, word4 = pair2.word2 };
            //            result2.Add(newPair);
            //        }
            //    }
            //}

            object lockObj = new object(); // Lock object for synchronization

            // Iterate over each pair of words in parallel
            Parallel.ForEach(result, pair1 =>
            {
                foreach (var pair2 in result)
                {
                    if (!HasDuplicateLetters(pair1.word1 + pair1.word2 + pair2.word1 + pair2.word2))
                    {
                        var newPair = new Words() { word1 = pair1.word1, word2 = pair1.word2, word3 = pair2.word1, word4 = pair2.word2 };
                        lock (lockObj) // Ensure thread-safe access to the result list
                        {
                            result2.Add(newPair);
                        }
                    }
                }
            });
            return null;
        }
    }
    public class Words
    {
        public string word1 { get; set; }
        public string word2 { get; set; }
        public string word3 { get; set; }
        public string word4 { get; set; }
        public string word5 { get; set; }
        public List<string> result()
        {
            return new List<string> { word1, word2, word3, word4, word5 };
        }
    }
}
