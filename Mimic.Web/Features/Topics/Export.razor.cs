using Microsoft.JSInterop;
using System.Text.Json;

namespace Mimic.Web.Features.Topics;

public partial class Export : IDisposable
{
    [Inject] private IRepository<Topic> ItemRepository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    private readonly CancellationTokenSource _cts = new();

    private async Task ExportDataAsync()
    {
        var items = await ItemRepository.GetAllAsync(_cts.Token);

        if (!items.Any())
        {
            Snackbar.Add("No topics are yet added to export.", Severity.Warning);
            return;
        }

        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(memoryStream, items);
            fileBytes = memoryStream.ToArray();
        }

        var fileName = $"exported-topics{Guid.NewGuid()}.txt";
        await JSRuntime.InvokeVoidAsync("BlazorDownloadFile", fileName, "text/plain", fileBytes);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
