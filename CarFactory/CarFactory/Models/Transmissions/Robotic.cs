namespace CarFactory.Models.Transmissions;

public class Robotic : ITransmission
{
    public string Name => "Роботизированная";
    public int Gears => 8;
    public int MaxSpeed => 20;
}