using System;
using System.Collections.Generic;
using System.Threading;

// I exceeded requirements by including the Visualization Activity.

class MindfulnessApp
{
    List<Activity> activities;

    public MindfulnessApp()
    {
        activities = new List<Activity>
        {
            new BreathingActivity(),
            new ReflectionActivity(),
            new ListingActivity(),
            new VisualizationActivity()
        };
    }

    public void DisplayMenu()
    {
        Console.WriteLine("Mindfulness App");
        Console.WriteLine("---------------");
        Console.WriteLine("1. Breathing Activity");
        Console.WriteLine("2. Reflection Activity");
        Console.WriteLine("3. Listing Activity");
        Console.WriteLine("4. Visualization Activity");
        Console.WriteLine("5. Quit");
    }

    public void SelectActivity()
    {
        int choice;
        do
        {
            DisplayMenu();
            Console.Write("Select an activity (1-5): ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 5)
            {
                if (choice == 5)
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                activities[choice - 1].Start();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please select a number between 1 and 5.");
            }
        } while (choice != 5);
    }

    static void Main(string[] args)
    {
        MindfulnessApp app = new MindfulnessApp();
        app.SelectActivity();
    }
}

abstract class Activity
{
    protected string name;
    protected string description;
    protected int duration;

    public virtual void Start()
    {
        Console.WriteLine($"\nStarting {name}");
        Console.WriteLine(description);
        Console.Write("Enter duration in seconds: ");
        if (int.TryParse(Console.ReadLine(), out duration) && duration > 0)
        {
            Console.WriteLine("Prepare to begin...");
            ShowSpinner(3);
            PerformActivity();
            End();
        }
        else
        {
            Console.WriteLine("Invalid duration. Please enter a positive integer.");
        }
    }

    protected abstract void PerformActivity();

    protected void End()
    {
        Console.WriteLine($"\nGood job! You have completed the {name} for {duration} seconds.");
        ShowSpinner(3);
    }

    protected void ShowSpinner(int seconds)
    {
        char[] spinner = { '/', '-', '\\', '|' };
        for (int i = 0; i < seconds * 4; i++)
        {
            Console.Write($"\r{spinner[i % 4]}");
            Thread.Sleep(250);
        }
        Console.Write("\r ");
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity()
    {
        name = "Breathing Activity";
        description = "This activity will help you relax by guiding you through slow breathing. Clear your mind and focus on your breathing.";
    }

    protected override void PerformActivity()
    {
        int interval = 4;
        int cycles = duration / (2 * interval);
        for (int i = 0; i < cycles; i++)
        {
            Console.WriteLine("\nBreathe in...");
            ShowCountdown(interval);
            Console.WriteLine("Breathe out...");
            ShowCountdown(interval);
        }
    }

    private void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"{i} ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

class ReflectionActivity : Activity
{
    private List<string> prompts;
    private List<string> questions;

    public ReflectionActivity()
    {
        name = "Reflection Activity";
        description = "This activity will help you reflect on times in your life when you have shown strength and resilience.";
        prompts = new List<string>
        {
            "Think of a time when you stood up for someone.",
            "Think of a time when you achieved a significant goal.",
            "Recall a time when you overcame a difficult challenge."
        };
        questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "How did you feel during this experience?",
            "What did you learn about yourself?",
            "How can you apply this experience in the future?"
        };
    }

    protected override void PerformActivity()
    {
        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Count)];
        Console.WriteLine($"\n{prompt}");
        ShowSpinner(3);

        int questionInterval = duration / questions.Count;
        foreach (string question in questions)
        {
            Console.WriteLine($"\n{question}");
            ShowCountdown(questionInterval);
        }
    }

    private void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"{i} ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

class ListingActivity : Activity
{
    private List<string> prompts;

    public ListingActivity()
    {
        name = "Listing Activity";
        description = "This activity will help you list as many things as you can in a certain area of strength or positivity.";
        prompts = new List<string>
        {
            "List as many personal strengths as you can.",
            "List as many things you're grateful for.",
            "List as many goals you've achieved."
        };
    }

    protected override void PerformActivity()
    {
        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Count)];
        Console.WriteLine($"\n{prompt}");
        ShowSpinner(3);

        Console.WriteLine("Start listing items:");
        DateTime endTime = DateTime.Now.AddSeconds(duration);
        while (DateTime.Now < endTime)
        {
            Console.Write("- ");
            Console.ReadLine();
        }
    }
}

class VisualizationActivity : Activity
{
    private List<string> scenarios;

    public VisualizationActivity()
    {
        name = "Visualization Activity";
        description = "This activity will help you visualize a peaceful and relaxing scene to calm your mind.";
        scenarios = new List<string>
        {
            "Imagine yourself on a quiet beach, feeling the warm sun on your skin and hearing the gentle waves.",
            "Picture yourself in a peaceful forest, surrounded by tall trees and the sounds of birds chirping.",
            "Visualize a serene mountain top, feeling the fresh breeze and looking at the breathtaking view."
        };
    }

    protected override void PerformActivity()
    {
        Random random = new Random();
        string scenario = scenarios[random.Next(scenarios.Count)];
        Console.WriteLine($"\n{scenario}");
        ShowSpinner(duration);
    }
}