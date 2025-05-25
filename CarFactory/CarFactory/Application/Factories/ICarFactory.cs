using CarFactory.Models.Cars;

namespace CarFactory.Application.Factories;

public interface ICarFactory
{
    public ICar CreateCar();
}