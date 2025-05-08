namespace CarFactory.Transmissions;

public class Mechanical : ITransmission
{
    public string Name => "Mechanical";

    public int Gears => 5;

    public int MaxSpeed => 25;
}
