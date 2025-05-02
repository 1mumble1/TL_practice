namespace CarFactory.Transmissions;

public class Robotic : ITransmission
{
    public string Name => "Robotic";

    public int Gears => 8;

    public int MaxSpeed => 20;
}