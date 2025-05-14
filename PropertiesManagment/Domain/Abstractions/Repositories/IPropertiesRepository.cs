using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IPropertiesRepository
{
    Task<Guid> Create( Property property );
    Task<Guid> Delete( Guid id );
    Task<List<Property>> GetAll();
    Task<Property?> GetById( Guid id );
    Task<Guid> Update( Property property );
}