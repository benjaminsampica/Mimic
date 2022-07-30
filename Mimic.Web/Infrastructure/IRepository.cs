using Mimic.Web.Infrastructure.Models;

namespace Mimic.Web.Infrastructure;

public interface IRepository<T> where T : IEntity
{
    ValueTask<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
    ValueTask<T?> FindAsync(string id, CancellationToken cancellationToken);
    ValueTask AddAsync(T item, CancellationToken cancellationToken);
    ValueTask RemoveAsync(string id, CancellationToken cancellationToken);
}
