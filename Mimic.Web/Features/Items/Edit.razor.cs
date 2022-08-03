using MudBlazor;

namespace Mimic.Web.Features.Items;

public partial class Edit : IDisposable
{
    [Inject] private IRepository<Item> ItemRepository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, EditorRequired] public string Id { get; set; } = null!;
    [Parameter, EditorRequired] public EventCallback OnSuccessfulSubmit { get; set; }

    private readonly AddEditItemRequest _form = new();
    private readonly CancellationTokenSource _cts = new();

    protected override async Task OnInitializedAsync()
    {
        var item = (await ItemRepository.FindAsync(Id, _cts.Token))!;
        _form.Topic = item.Topic;
        _form.Body = item.Body;
    }

    private async Task OnSubmitAsync(AddEditItemRequestAttempt attempt)
    {
        if (attempt.IsValid)
        {
            var item = (await ItemRepository.FindAsync(Id, _cts.Token))!;
            item.Topic = attempt.Model.Topic;
            item.Body = attempt.Model.Body;

            await ItemRepository.RemoveAsync(Id, _cts.Token);
            await ItemRepository.AddAsync(item, _cts.Token);

            Snackbar.Add("Successfully edited item!", Severity.Success);

            await OnSuccessfulSubmit.InvokeAsync();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}