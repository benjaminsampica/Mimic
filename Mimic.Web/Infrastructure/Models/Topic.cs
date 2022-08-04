namespace Mimic.Web.Infrastructure.Models;

public class Topic : IEntity
{
    public string Id { get; set; } = nameof(Topic) + Guid.NewGuid();
    public string Summary { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string[] Tags { get; set; } = null!;
}
