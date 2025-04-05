using System;
using System.Collections.Generic;

abstract class Activity
{
    protected string _date;
    protected int _lengthMinutes;

    public Activity(string date, int lengthMinutes)
    {
        _date = date;
        _lengthMinutes = lengthMinutes;
    }

    public abstract double GetDistance(); // in miles
    public abstract double GetSpeed();    // in mph
    public abstract double GetPace();     // in min/mile

    public virtual string GetSummary()
    {
        return $"{_date} {GetType().Name} ({_lengthMinutes} min): " +
               $"Distance: {GetDistance():0.0} miles, " +
               $"Speed: {GetSpeed():0.0} mph, " +
               $"Pace: {GetPace():0.00} min per mile";
    }
}

class Running : Activity
{
    private double _distance;

    public Running(string date, int lengthMinutes, double distance)
        : base(date, lengthMinutes)
    {
        _distance = distance;
    }

    public override double GetDistance() => _distance;
    public override double GetSpeed() => (_distance / _lengthMinutes) * 60;
    public override double GetPace() => _lengthMinutes / _distance;
}

class Cycling : Activity
{
    private double _speed;

    public Cycling(string date, int lengthMinutes, double speed)
        : base(date, lengthMinutes)
    {
        _speed = speed;
    }

    public override double GetDistance() => (_speed * _lengthMinutes) / 60;
    public override double GetSpeed() => _speed;
    public override double GetPace() => 60 / _speed;
}

class Swimming : Activity
{
    private int _laps;

    public Swimming(string date, int lengthMinutes, int laps)
        : base(date, lengthMinutes)
    {
        _laps = laps;
    }

    public override double GetDistance()
    {
        // Convert meters to miles: 1 lap = 50m, so total_meters / 1000 * 0.62
        return (_laps * 50 / 1000.0) * 0.62;
    }

    public override double GetSpeed() => (GetDistance() / _lengthMinutes) * 60;
    public override double GetPace() => _lengthMinutes / GetDistance();
}

class Program
{
    static void Main()
    {
        List<Activity> activities = new List<Activity>
        {
            new Running("03 Nov 2022", 30, 3.0),
            new Cycling("05 Nov 2022", 45, 15.0),
            new Swimming("06 Nov 2022", 40, 30) // 30 laps = 0.93 miles
        };

        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}