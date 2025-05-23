using Fighters.Models.Fighters;

namespace Fighters.Application.Factory;

public interface IFactory
{
    public IFighter CreateFighter();
}
