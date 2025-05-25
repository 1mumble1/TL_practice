using CarFactory.Models.BodyShapes;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;

namespace CarFactory.Application.CarReader;

public interface ICarReader
{
    string GetCarName();
    IBodyShape GetBodyShape();
    IColor GetColor();
    IEngine GetEngine();
    ITransmission GetTransmission();
}