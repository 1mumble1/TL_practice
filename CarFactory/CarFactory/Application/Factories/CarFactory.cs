using CarFactory.Application.CarReader;
using CarFactory.Models.BodyShapes;
using CarFactory.Models.Cars;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;

namespace CarFactory.Application.Factories;

public class CarFactory : ICarFactory
{
    private readonly ICarReader _carReader;

    public CarFactory( ICarReader carReader )
    {
        _carReader = carReader;
    }

    public ICar CreateCar()
    {
        string name = _carReader.GetCarName();

        IBodyShape bodyShape = _carReader.GetBodyShape();
        IColor color = _carReader.GetColor();
        IEngine engine = _carReader.GetEngine();
        ITransmission transmission = _carReader.GetTransmission();

        return new Car( name, bodyShape, color, engine, transmission );
    }
}
