namespace Mimic.Web.Features.Items;

public partial class Add : IDisposable
{
    [Inject] private IRepository<Item> ItemRepository { get; set; } = null!;

    [Parameter, EditorRequired] public bool Show { get; set; }
    [Parameter, EditorRequired] public EventCallback OnSuccessfulSubmit { get; set; }

    private AddItemRequest _form = new();
    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmit()
    {
        var item = new Item
        {
            Summary = _form.Summary
        };

        await ItemRepository.AddAsync(item, _cts.Token);

        await OnSuccessfulSubmit.InvokeAsync();

        _form = new();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

public class AddItemRequest
{
    public string Summary { get; set; }
}
