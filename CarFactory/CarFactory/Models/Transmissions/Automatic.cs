namespace CarFactory.Models.Transmissions;

public class Automatic : ITransmission
{
    public string Name => "Автоматическая";

    public int Gears => 6;

    public int MaxSpeed => 15;
}
