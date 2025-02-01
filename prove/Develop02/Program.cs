using System;
using System.Collections.Generic;

// I exceeded the requirements by implimenting error catches.

class Program
{
    static void Main(string[] args)
    {
        // Initialize the Journal, FileManager, and Menu classes
        Journal myJournal = new();
        FileManager fileManager = new();
        Menu menu = new();
        bool running = true;

        // Loop to display menu and handle user choices
        while (running)
        {
            int choice = Menu.UseMenu();
            
            switch (choice)
            {
                case 1:
                    myJournal.NewEntry(); // Create a new journal entry
                    break;
                case 2:
                    myJournal.Display(); // Display all journal entries
                    break;
                case 3:
                    FileManager.Load(myJournal); // Load journal entries from a file
                    break;
                case 4:
                    FileManager.Save(myJournal); // Save journal entries to a file
                    break;
                case 5:
                    Console.WriteLine("Goodbye!"); // Exit the program
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again."); // Handle invalid inputs
                    break;
            }
        }
    }
}

class Journal
{
    private List<string> entries = []; // List to store journal entries
    private readonly Random random = new(); // Random object for selecting prompts

    public void NewEntry()
    {
        string prompt = RandomPrompt(); // Get a random prompt
        Console.WriteLine($"\n{prompt}");
        Console.Write("> ");
        string response = Console.ReadLine(); // Get user response
        
        // Combine date, prompt, and response into a single entry
        string entry = $"Date: {DateTime.Now} - Prompt: {prompt} \n{response}";
        entries.Add(entry); // Add the new entry to the list
        Console.WriteLine("Entry added!");
    }

    public void Display()
    {
        // Check if there are any entries to display
        if (entries.Count == 0)
        {
            Console.WriteLine("\nNo entries found.");
        }
        else
        {
            Console.WriteLine();
            foreach (var entry in entries)
            {
                Console.WriteLine(entry); // Display each journal entry
                Console.WriteLine();
            }
        }
    }

    public string RandomPrompt()
    {
        // List of prompts for journal entries
        List<string> prompts =
        [
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "Was I more stressed today than yesterday? Why?",
            "Did anyone make me smile or laugh today?",
            "What's my favorite thing I got to do today?",
            "Why was today special?"
        ];

        int index = random.Next(prompts.Count); // Get a random index
        return prompts[index]; // Return the selected prompt
    }

    public List<string> GetEntries()
    {
        return entries; // Return the list of journal entries
    }

    public void SetEntries(List<string> loadedEntries)
    {
        entries = loadedEntries; // Set the entries from loaded data
    }
}


class FileManager
{
    public static void Save(Journal journal)
    {
        Console.Write("What is the filename? (Please exclude '.txt')\n> ");
        string fileName = Console.ReadLine();

        try
        {
            // Save all journal entries to the specified file
            System.IO.File.WriteAllLines(fileName, journal.GetEntries());
            Console.WriteLine("Journal saved.");
        }
        catch (Exception e)
        {
            // Handle any errors that occur during file save
            Console.WriteLine($"An error occurred while saving the journal: {e.Message}");
        }
    }

    public static void Load(Journal journal)
    {
        Console.Write("What is the filename? (Please exclude '.txt')\n> ");
        string fileName = Console.ReadLine();

        try
        {
            // Load all journal entries from the specified file
            List<string> loadedEntries = [.. System.IO.File.ReadAllLines(fileName)];
            journal.SetEntries(loadedEntries);
            Console.WriteLine("Journal loaded successfully.");
        }
        catch (Exception e)
        {
            // Handle any errors that occur during file load
            Console.WriteLine($"An error occurred while loading the journal: {e.Message}");
        }
    }
}

class Menu
{
    public static int UseMenu()
    {
        // Display menu options to the user
        Console.WriteLine("\nPlease select one of the following choices:");
        Console.WriteLine("1. Write");
        Console.WriteLine("2. Display");
        Console.WriteLine("3. Load");
        Console.WriteLine("4. Save");
        Console.WriteLine("5. Quit");
        Console.Write("What would you like to do? ");
        
        // Validate user input
        int userChoice;
        while (!int.TryParse(Console.ReadLine(), out userChoice))
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
        }

        return userChoice; // Return the user's choice
    }
}