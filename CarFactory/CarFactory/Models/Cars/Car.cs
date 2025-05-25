using CarFactory.Models.BodyShapes;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;

namespace CarFactory.Models.Cars;

public class Car : ICar
{
    public string Name { get; }

    public int MaxSpeed => Engine.MaxSpeed + Transmission.MaxSpeed;

    public int Gears => Transmission.Gears;

    public IBodyShape BodyShape { get; }

    public IColor Color { get; }

    public IEngine Engine { get; }

    public ITransmission Transmission { get; }

    public Car(
        string name,
        IBodyShape bodyShape,
        IColor color,
        IEngine engine,
        ITransmission transmission )
    {
        Name = name;
        BodyShape = bodyShape;
        Color = color;
        Engine = engine;
        Transmission = transmission;
    }

    public override string ToString()
    {
        return
$@"Название: {Name}
Характеристики:
    двигатель: {Engine.Name},
    трансмиссия: {Transmission.Name},
    кузов: {BodyShape.Name},
    цвет: {Color.Name},
    максимальная скорость: {MaxSpeed},
    кол-во передач: {Gears}";
    }
}
