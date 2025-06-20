namespace CarFactory.Models.Engines;

public class Electric : IEngine
{
    public string Name => "Электрический";
    public int MaxSpeed => 200;
}