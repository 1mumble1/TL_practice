using CarFactory.BodyShapes;
using CarFactory.Colors;
using CarFactory.Engines;
using CarFactory.Transmissions;

namespace CarFactory.Cars;

public class Car : ICar
{
    public string Name { get; }

    public int MaxSpeed => Engine.MaxSpeed + Transmission.MaxSpeed;

    public int Gears => Transmission.Gears;

    public IBodyShape BodyShape { get; }

    public IColor Color { get; }

    public IEngine Engine { get; }

    public ITransmission Transmission { get; }

    public Car( string name, IBodyShape bodyShape, IColor color, IEngine engine, ITransmission transmission )
    {
        Name = name;
        BodyShape = bodyShape;
        Color = color;
        Engine = engine;
        Transmission = transmission;
    }
}
