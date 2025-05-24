using CarFactory.Cars;

namespace CarFactory.Factories;

public abstract class AbstractFactory
{
    public abstract ICar CreateCar();
}