namespace CarFactory.Transmissions;

public interface ITransmission
{
    string Name { get; }
    int Gears { get; }
    int MaxSpeed { get; }
}
