using System;
using System.Collections.Generic;

class Event
{
    protected string _title;
    protected string _description;
    protected string _date;
    protected string _time;
    protected string _address;

    public Event(string title, string description, string date, string time, string address)
    {
        _title = title;
        _description = description;
        _date = date;
        _time = time;
        _address = address;
    }

    public virtual string GetStandardDetails()
    {
        return $"{_title} - {_description}\nDate: {_date} Time: {_time}\nAddress: {_address}";
    }

    public virtual string GetFullDetails()
    {
        return GetStandardDetails(); // Can be overridden to include more details
    }

    public virtual string GetShortDescription()
    {
        return $"Event Type: General\nTitle: {_title}\nDate: {_date}";
    }
}

class Lecture : Event
{
    private string _speaker;
    private int _capacity;

    public Lecture(string title, string description, string date, string time, string address, string speaker, int capacity)
        : base(title, description, date, time, address)
    {
        _speaker = speaker;
        _capacity = capacity;
    }

    public override string GetFullDetails()
    {
        return $"{GetStandardDetails()}\nSpeaker: {_speaker}\nCapacity: {_capacity}";
    }

    public override string GetShortDescription()
    {
        return $"Event Type: Lecture\nTitle: {_title}\nDate: {_date}";
    }
}

class Reception : Event
{
    private string _rsvpEmail;

    public Reception(string title, string description, string date, string time, string address, string rsvpEmail)
        : base(title, description, date, time, address)
    {
        _rsvpEmail = rsvpEmail;
    }

    public override string GetFullDetails()
    {
        return $"{GetStandardDetails()}\nRSVP at: {_rsvpEmail}";
    }

    public override string GetShortDescription()
    {
        return $"Event Type: Reception\nTitle: {_title}\nDate: {_date}";
    }
}

class OutdoorGathering : Event
{
    private string _weatherForecast;

    public OutdoorGathering(string title, string description, string date, string time, string address, string weatherForecast)
        : base(title, description, date, time, address)
    {
        _weatherForecast = weatherForecast;
    }

    public override string GetFullDetails()
    {
        return $"{GetStandardDetails()}\nWeather Forecast: {_weatherForecast}";
    }

    public override string GetShortDescription()
    {
        return $"Event Type: Outdoor Gathering\nTitle: {_title}\nDate: {_date}";
    }
}

class Program
{
    static void Main()
    {
        List<Event> events = new List<Event>
        {
            new Lecture("Bio Talk", "Latest Inovations in Virus Transmission", "04/10/2025", "2:00 PM", "123 Tech St", "Dr. Albert Wesker", 50),
            new Reception("Alumni Meet", "Networking for former students", "04/15/2025", "6:00 PM", "University Hall", "rsvp@university.edu"),
            new OutdoorGathering("Spring Festival", "Celebrate the season with music", "04/20/2025", "1:00 PM", "Central Park", "Sunny, 72°F")
        };

        foreach (Event e in events)
        {
            Console.WriteLine("=== Standard Details ===");
            Console.WriteLine(e.GetStandardDetails());
            Console.WriteLine("\n=== Full Details ===");
            Console.WriteLine(e.GetFullDetails());
            Console.WriteLine("\n=== Short Description ===");
            Console.WriteLine(e.GetShortDescription());
            Console.WriteLine("\n-----------------------------\n");
        }
    }
}