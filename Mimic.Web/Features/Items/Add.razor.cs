namespace Mimic.Web.Features.Items;

public partial class Add : IDisposable
{
    [Inject] private IRepository<Item> ItemRepository { get; set; } = null!;

    private readonly AddItemRequest _form = new();
    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmit()
    {
        var item = new Item
        {
            Summary = _form.Summary
        };

        await ItemRepository.AddAsync(item, _cts.Token);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public class AddItemRequest
{
    public string Summary { get; set; }
}
