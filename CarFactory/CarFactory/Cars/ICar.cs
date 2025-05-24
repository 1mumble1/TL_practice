using CarFactory.BodyShapes;
using CarFactory.Colors;
using CarFactory.Engines;
using CarFactory.Transmissions;

namespace CarFactory.Cars;

public interface ICar
{
    string Name { get; }
    int MaxSpeed { get; }
    int Gears { get; }
    IBodyShape BodyShape { get; }
    IColor Color { get; }
    IEngine Engine { get; }
    ITransmission Transmission { get; }

    public string GetDescription()
    {
        return
$@"Характеристики:
    двигатель: {Engine.Name},
    трансмиссия: {Transmission.Name},
    кузов: {BodyShape.Name},
    цвет: {Color.Name},
    максимальная скорость: {MaxSpeed},
    кол-во передач: {Gears}";
    }
}
