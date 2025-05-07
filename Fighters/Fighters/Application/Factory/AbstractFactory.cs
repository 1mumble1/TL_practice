using Fighters.Models.Fighters;

namespace Fighters.Application.Factory;

public abstract class AbstractFactory
{
    public abstract IFighter CreateFighter();
}
