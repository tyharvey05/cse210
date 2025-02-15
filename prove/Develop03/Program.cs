using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// I exceeded requirements by including an external txt file in which one can enter as many scriptures as they like for random selection. A great number of random verses are already included in the same directory as the program in github, and must be placed into 'bin\Debug\net8.0'. If the program can not find the scriptures.txt file at this directory, it will create a new file with a short list of default verses.

class Program
{
    static void Main()
    {
        // Define the expected file path for 'scriptures.txt' in the program's directory
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scriptures.txt");

        // Check if the file exists; if not, create it with default scriptures
        if (!File.Exists(filePath))
        {
            CreateDefaultScriptureFile(filePath);
            Console.WriteLine("A new 'scriptures.txt' file has been created with default scriptures.");
        }

        // Load scriptures from the file
        List<Scripture> scriptures = LoadScripturesFromFile(filePath);

        // Ensure scriptures were loaded
        if (scriptures.Count == 0)
        {
            Console.WriteLine("No scriptures found in the file.");
            return;
        }

        // Select a random scripture
        Scripture currentScripture = scriptures[new Random().Next(scriptures.Count)];

        // Start the memorization game
        RunMemorizationGame(currentScripture);
    }

    /// <summary>
    /// Creates a new 'scriptures.txt' file with a short list of default scriptures.
    /// This ensures the program can always run even if the file is missing.
    /// </summary>
    /// <param name="filename">The path where the file should be created.</param>
    static void CreateDefaultScriptureFile(string filename)
    {
        string[] defaultScriptures =
        {
            "John 3:16|For God so loved the world, that he gave his only begotten Son, that whosoever believeth in him should not perish, but have everlasting life.",
            "Proverbs 3:5-6|Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths.",
            "Philippians 4:13|I can do all things through Christ which strengtheneth me.",
            "Romans 8:28|And we know that all things work together for good to them that love God, to them who are the called according to his purpose."
        };

        // Write the default scriptures to a new file
        File.WriteAllLines(filename, defaultScriptures);
    }

    /// <summary>
    /// Loads scriptures from the 'scriptures.txt' file and returns a list of Scripture objects.
    /// Each line in the file should be formatted as "Reference|Text".
    /// </summary>
    /// <param name="filename">The path to the file containing the scriptures.</param>
    /// <returns>A list of Scripture objects.</returns>
    static List<Scripture> LoadScripturesFromFile(string filename)
    {
        List<Scripture> scriptures = new List<Scripture>();

        foreach (string line in File.ReadAllLines(filename))
        {
            string[] parts = line.Split('|');

            if (parts.Length == 2)
            {
                Reference reference = new Reference(parts[0].Trim());
                string text = parts[1].Trim();
                scriptures.Add(new Scripture(reference, text));
            }
        }

        return scriptures;
    }

    /// <summary>
    /// Runs the scripture memorization game, progressively hiding words.
    /// </summary>
    /// <param name="scripture">The Scripture object to be used in the game.</param>
    static void RunMemorizationGame(Scripture scripture)
    {
        Console.Clear();
        Console.WriteLine("Scripture Memorization Game!");
        Console.WriteLine("Press ENTER to start or type 'quit' to exit.");
        Console.ReadLine();

        while (!scripture.AllWordsHidden())
        {
            Console.Clear();
            scripture.DisplayScripture();

            Console.WriteLine("\nPress ENTER to hide words or type 'quit' to exit.");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit") break;

            scripture.HideRandomWords(2);
        }

        Console.Clear();
        Console.WriteLine("All words are hidden. Memorization complete!");
    }
}

class Scripture
{
    private Reference reference; // Holds the scripture reference (e.g., "John 3:16")
    private List<Word> words; // Holds each word of the scripture separately

    /// <summary>
    /// Constructor to initialize the scripture with a reference and text.
    /// </summary>
    public Scripture(Reference reference, string text)
    {
        this.reference = reference;
        words = text.Split(' ').Select(word => new Word(word)).ToList(); // Convert text into Word objects
    }

    /// <summary>
    /// Displays the scripture with hidden words replaced by underscores.
    /// </summary>
    public void DisplayScripture()
    {
        Console.Write($"{reference.CreateReferenceString()}: "); // Display reference
        foreach (Word word in words)
        {
            Console.Write(word.CreateWordString() + " "); // Display words with hidden ones replaced
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Randomly hides a specified number of words that are still visible.
    /// </summary>
    /// <param name="count">Number of words to hide</param>
    public void HideRandomWords(int count)
    {
        Random random = new Random();
        List<Word> visibleWords = words.Where(w => !w.isHidden).ToList(); // Only select visible words

        if (visibleWords.Count > 0)
        {
            for (int i = 0; i < count && visibleWords.Count > 0; i++)
            {
                Word wordToHide = visibleWords[random.Next(visibleWords.Count)]; // Pick a random visible word
                wordToHide.HideWord();
                visibleWords.Remove(wordToHide); // Remove from list to prevent re-hiding
            }
        }
    }

    /// <summary>
    /// Checks if all words in the scripture are hidden.
    /// </summary>
    /// <returns>True if all words are hidden, otherwise false.</returns>
    public bool AllWordsHidden()
    {
        return words.All(w => w.isHidden);
    }
}

class Reference
{
    private string book; // Holds the book name (e.g., "John", "1 Timothy")
    private int chapter; // Holds the chapter number
    private int startVerse; // Holds the starting verse number
    private int endVerse; // Holds the ending verse number (if applicable)

    /// <summary>
    /// Parses a scripture reference string (e.g., "John 3:16" or "1 Timothy 4:12").
    /// </summary>
    /// <param name="reference">The reference string</param>
    public Reference(string reference)
    {
        string[] parts = reference.Split(' '); // Split book name and chapter/verse
        int lastPartIndex = parts.Length - 1; // The last part should be Chapter:Verse

        string chapterVersePart = parts[lastPartIndex]; // Extract "3:16" or "5:6-7"
        book = string.Join(" ", parts, 0, lastPartIndex); // Reconstruct book name

        string[] verseParts = chapterVersePart.Split(':'); // Separate Chapter and Verse
        chapter = int.Parse(verseParts[0]);

        // Check if it's a range (e.g., "5-6")
        if (verseParts[1].Contains("-"))
        {
            string[] range = verseParts[1].Split('-');
            startVerse = int.Parse(range[0]);
            endVerse = int.Parse(range[1]);
        }
        else
        {
            startVerse = int.Parse(verseParts[1]);
            endVerse = startVerse; // Single verse case
        }
    }

    /// <summary>
    /// Creates a formatted reference string.
    /// </summary>
    /// <returns>Formatted reference string</returns>
    public string CreateReferenceString()
    {
        return endVerse > startVerse ? $"{book} {chapter}:{startVerse}-{endVerse}" : $"{book} {chapter}:{startVerse}";
    }
}


class Word
{
    private string text; // The actual word
    public bool isHidden { get; private set; } // Indicates if the word is hidden

    /// <summary>
    /// Constructor to initialize a word.
    /// </summary>
    /// <param name="word">The word as a string</param>
    public Word(string word)
    {
        text = word;
        isHidden = false;
    }

    /// <summary>
    /// Hides the word by setting its hidden status.
    /// </summary>
    public void HideWord()
    {
        isHidden = true;
    }

    /// <summary>
    /// Returns either the word itself or an underscore if hidden.
    /// </summary>
    /// <returns>String representation of the word</returns>
    public string CreateWordString()
    {
        return isHidden ? "_____" : text;
    }
}
