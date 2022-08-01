using static Mimic.Web.Features.Items.ListItemResponse;

namespace Mimic.Web.Features.Items;

public partial class List : IDisposable
{
    [Inject] private IRepository<Item> ItemRepository { get; set; } = null!;

    private readonly CancellationTokenSource _cts = new();
    private bool _showAddForm = false;
    private ListItemResponse? _result;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _result = null;
        var items = await ItemRepository.GetAllAsync(_cts.Token);

        if (!items.Any()) return;

        var result = new ListItemResponse();
        foreach (var item in items)
        {
            var itemResult = new ItemResponse
            {
                Id = item.Id,
                Summary = item.Summary
            };

            result.Items.Add(itemResult);
        }

        _result = result;
    }

    private async Task DeleteAsync(string id)
    {
        await ItemRepository.RemoveAsync(id, _cts.Token);
        await LoadDataAsync();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

public class ListItemResponse
{
    public List<ItemResponse> Items { get; set; } = new();

    public class ItemResponse
    {
        public string Id { get; set; } = null!;
        public string Summary { get; set; } = null!;
    }
}