using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IPropertiesRepository
{
    Task<Guid> Create( Property property );
    Task Delete( Guid id );
    Task<IReadOnlyList<Property>> GetAll();
    Task<Property?> GetById( Guid id );
    Task Update( Property property );
}