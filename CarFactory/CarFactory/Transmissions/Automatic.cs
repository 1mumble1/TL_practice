namespace CarFactory.Transmissions;

public class Automatic : ITransmission
{
    public string Name => "Automatic";

    public int Gears => 6;

    public int MaxSpeed => 15;
}
