using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        GoalManager goalManager = new GoalManager();
        Menu menu = new Menu();
        bool running = true;

        while (running)
        {
            menu.DisplayMenu();
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    goalManager.AddGoal(InputHandler.GetGoalInput());
                    Console.WriteLine();
                    break;
                case "2":
                    goalManager.RecordEvent(InputHandler.GetGoalNameInput());
                    Console.WriteLine();
                    Console.WriteLine("Event recorded successfully!");
                    break;
                case "3":
                    goalManager.DisplayGoals();
                    break;
                case "4":
                    goalManager.SaveGoals(InputHandler.GetFilenameInput("save"));
                    Console.WriteLine("Goals saved successfully!");
                    Console.WriteLine();
                    break;
                case "5":
                    goalManager.LoadGoals(InputHandler.GetFilenameInput("load"));
                    Console.WriteLine();
                    break;
                case "6":
                    running = false;
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    Console.WriteLine();
                    break;
            }
        }
    }
}

class Menu
{
    public void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("1. Add Goal");
        Console.WriteLine("2. Record Event");
        Console.WriteLine("3. Display Goals");
        Console.WriteLine("4. Save Goals");
        Console.WriteLine("5. Load Goals");
        Console.WriteLine("6. Exit");
        Console.WriteLine();
    }
}

class InputHandler
{
    public static Goal GetGoalInput()
    {
        Console.WriteLine("Choose the type of goal to add:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");
        string choice = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter goal points: ");
        int points = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case "1":
                return new SimpleGoal(name, description, points);
            case "2":
                return new EternalGoal(name, description, points);
            case "3":
                Console.Write("Enter the target count for Checklist Goal: ");
                int targetCount = int.Parse(Console.ReadLine());
                Console.Write("Enter the bonus points for Checklist Goal: ");
                int bonusPoints = int.Parse(Console.ReadLine());
                return new ChecklistGoal(name, description, points, targetCount, bonusPoints);
            default:
                Console.WriteLine("Invalid choice. Returning a Simple Goal by default.");
                return new SimpleGoal(name, description, points);
        }
    }

    public static string GetGoalNameInput()
    {
        Console.Write("Enter goal name to record event: ");
        return Console.ReadLine();
    }

    public static string GetFilenameInput(string action)
    {
        Console.Write($"Enter filename to {action} goals: ");
        return Console.ReadLine();
    }
}

class GoalManager
{
    private List<Goal> _goals = new List<Goal>();
    private int _totalScore = 0;

    public void AddGoal(Goal goal)
    {
        _goals.Add(goal);
    }

    public void RecordEvent(string goalName)
    {
        foreach (var goal in _goals)
        {
            if (goal.Name == goalName)
            {
                _totalScore += goal.RecordEvent();
                Console.WriteLine($"Goal '{goalName}' has been successfully completed!");
                break;
            }
        }
    }

    public void DisplayGoals()
    {
        foreach (var goal in _goals)
        {
            Console.WriteLine(goal);
        }
        Console.WriteLine($"Total Score: {_totalScore}");
    }

    public void SaveGoals(string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var goal in _goals)
                {
                    writer.WriteLine($"{goal.GetType().Name},{goal.Name},{goal.Description},{goal.Points},{goal.IsComplete}");
                    if (goal is ChecklistGoal checklistGoal)
                    {
                        writer.WriteLine($"{checklistGoal.TargetCount},{checklistGoal.BonusPoints}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving goals: {ex.Message}");
        }
    }

    public void LoadGoals(string filename)
    {
        _goals.Clear();
        if (!File.Exists(filename))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 5)
                    {
                        string type = parts[0];
                        string name = parts[1];
                        string description = parts[2];
                        if (!int.TryParse(parts[3], out int points))
                        {
                            Console.WriteLine("Error: Invalid file format.");
                            continue;
                        }
                        if (!bool.TryParse(parts[4], out bool isComplete))
                        {
                            Console.WriteLine("Error: Invalid file format.");
                            continue;
                        }

                        Goal goal = type switch
                        {
                            "SimpleGoal" => new SimpleGoal(name, description, points),
                            "EternalGoal" => new EternalGoal(name, description, points),
                            "ChecklistGoal" when parts.Length >= 7 => new ChecklistGoal(name, description, points, int.Parse(parts[5]), int.Parse(parts[6])),
                            _ => null
                        };

                        if (goal != null)
                        {
                            goal.IsComplete = isComplete;
                            _goals.Add(goal);
                        }
                    }
                }
            }
            Console.WriteLine("Goals loaded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading goals: {ex.Message}");
        }
    }
}

abstract class Goal
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
    public bool IsComplete { get; set; }

    protected Goal(string name, string description, int points)
    {
        Name = name;
        Description = description;
        Points = points;
        IsComplete = false;
    }

    public abstract int RecordEvent();

    public override string ToString()
    {
        return $"{Name}: {Description} ({Points} points) - {(IsComplete ? "Completed" : "Incomplete")}";
    }
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, string description, int points) : base(name, description, points) { }

    public override int RecordEvent()
    {
        IsComplete = true;
        return Points;
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points) : base(name, description, points) { }

    public override int RecordEvent()
    {
        return Points;
    }

    public override string ToString()
    {
        // Eternal goals do not show completed/incomplete
        return $"{Name}: {Description} ({Points} points) - Eternal goal";
    }
}

class ChecklistGoal : Goal
{
    public int TargetCount { get; private set; }
    public int BonusPoints { get; private set; }

    private int _currentCount;
    private bool _targetReached;

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints) 
        : base(name, description, points)
    {
        TargetCount = targetCount;
        BonusPoints = bonusPoints;
        _currentCount = 0;
        _targetReached = false;
    }

    public override int RecordEvent()
    {
        _currentCount++;

        int earnedPoints = 0;

        if (_currentCount >= TargetCount && !_targetReached)
        {
            earnedPoints = Points;
            _targetReached = true;
        }

        if (_targetReached)
        {
            earnedPoints += BonusPoints;
        }

        if (_currentCount >= TargetCount)
        {
            IsComplete = true;
        }

        return earnedPoints;
    }

    public override string ToString()
    {
        return $"{Name}: {Description} ({Points} points) - {(_currentCount >= TargetCount ? "Completed" : "Incomplete")} - {_currentCount}/{TargetCount} completed";
    }
}