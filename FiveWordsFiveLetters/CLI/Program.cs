using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordProgram;

namespace CLI
{
    internal class Program
    {
        static bool stopProgram = false; // Flag to control the program's loop

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the word finder program, press any key to continue");
            Console.ReadKey(); // Wait for user input to proceed

            do
            {
                Console.Clear(); // Clear the console for a fresh start
                Console.WriteLine("Please enter the path to the file you wish to use:");
                string importFilePath = Console.ReadLine(); // Get the file path from the user

                Console.WriteLine("Please enter the length of words you wish to find (1-26):");
                int wordLength = 5; // Default word length
                bool correctWordLength = false; // Flag to validate word length input

                do
                {
                    string tempWordLength = Console.ReadLine();
                    // Try to parse the input to an integer and check if it is within the valid range
                    if (int.TryParse(tempWordLength, out wordLength) && wordLength >= 1 && wordLength <= 26)
                    {
                        correctWordLength = true; // Valid input
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 26:");
                    }
                }
                while (!correctWordLength); // Loop until a valid word length is entered

                try
                {
                    WordFinder wordFinder = new WordFinder(); // Create an instance of WordFinder
                    var res = wordFinder.GatherWords(importFilePath, wordLength); // Find words of specified length in the file

                    // Iterate through the list of word lists and print them
                    foreach (var wordList in res)
                    {
                        string joinedWords = string.Join(", ", wordList);
                        Console.WriteLine(joinedWords); // Display the words found
                    }

                    Console.WriteLine($"\nFound {res.Count} of word length {wordLength}\n"); // Print summary of results
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while reading the file:"); // Handle errors in file reading
                    Console.WriteLine(ex.Message); // Display the error message
                }

                Console.WriteLine("\nDo you wish to exit (y/n)?"); // Prompt user to exit or continue
                var exitInput = Console.ReadKey().KeyChar; // Read user's choice
                stopProgram = exitInput == 'y' || exitInput == 'Y'; // Set flag based on user's choice

            }
            while (!stopProgram); // Loop until the user chooses to exit
        }
    }
}
