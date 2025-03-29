using System;
using System.Collections.Generic;

class Video
{
    private string _title;
    private string _author;
    private int _length;
    private List<Comment> _comments;

    public Video(string title, string author, int length)
    {
        _title = title;
        _author = author;
        _length = length;
        _comments = new List<Comment>();
    }

    public string GetTitle() => _title;
    public string GetAuthor() => _author;
    public int GetLength() => _length;

    public void AddComment(string commenterName, string text)
    {
        _comments.Add(new Comment(commenterName, text));
    }

    public int GetNumberOfComments() => _comments.Count;

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {_title}");
        Console.WriteLine($"Author: {_author}");
        Console.WriteLine($"Length: {_length} minutes");
        Console.WriteLine($"Number of Comments: {GetNumberOfComments()}");
        Console.WriteLine("Comments:");
        foreach (var comment in _comments)
        {
            Console.WriteLine($"- {comment.GetCommenterName()}: {comment.GetText()}");
        }
        Console.WriteLine();
    }
}

class Comment
{
    private string _commenterName;
    private string _text;

    public Comment(string commenterName, string text)
    {
        _commenterName = commenterName;
        _text = text;
    }

    public string GetCommenterName() => _commenterName;
    public string GetText() => _text;
}

class Program
{
    static void Main()
    {
        List<Video> videos = new List<Video>
        {
            new Video("Introduction to C#", "John Doe", 10),
            new Video("Object-Oriented Programming", "Jane Smith", 15),
            new Video("Advanced C# Concepts", "Michael Brown", 20),
            new Video("Design Patterns in C#", "Emily White", 25)
        };

        videos[0].AddComment("Alice", "Great explanation!");
        videos[0].AddComment("Bob", "Very helpful, thanks!");
        videos[0].AddComment("Charlie", "Can you cover interfaces next?");

        videos[1].AddComment("David", "OOP is really powerful!");
        videos[1].AddComment("Eve", "I finally understand encapsulation.");
        videos[1].AddComment("Frank", "Good job breaking things down!");

        videos[2].AddComment("Grace", "The examples were really helpful.");
        videos[2].AddComment("Hank", "Please do a tutorial on delegates.");
        videos[2].AddComment("Ivy", "Advanced topics explained well!");

        videos[3].AddComment("Jack", "Great video on design patterns!");
        videos[3].AddComment("Karen", "Loved the strategy pattern example.");
        videos[3].AddComment("Leo", "More videos on software architecture, please!");

        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}