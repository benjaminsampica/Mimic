namespace Mimic.Web.Infrastructure.Models;

public class Topic : IEntity
{
    public string Id { get; set; } = nameof(Models.Topic) + Guid.NewGuid();
    public string Topic { get; set; } = null!;
    public string Body { get; set; } = null!;
}
