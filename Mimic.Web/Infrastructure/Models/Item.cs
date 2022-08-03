namespace Mimic.Web.Infrastructure.Models;

public class Item : IEntity
{
    public string Id { get; set; } = nameof(Item) + Guid.NewGuid();
    public string Topic { get; set; } = null!;
    public string Body { get; set; } = null!;
}
