namespace Mimic.Web.Infrastructure.Models;

public class Item : IEntity
{
    public string Id { get; set; } = nameof(Item) + Guid.NewGuid();
    public string Summary { get; set; } // Change to Topic.
}
