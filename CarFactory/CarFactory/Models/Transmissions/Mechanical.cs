namespace CarFactory.Models.Transmissions;

public class Mechanical : ITransmission
{
    public string Name => "Механическая";

    public int Gears => 5;

    public int MaxSpeed => 25;
}
