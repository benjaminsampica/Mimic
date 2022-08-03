using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace Mimic.Web.Features.Topics;

public partial class Import : IDisposable
{
    [Inject] private IRepository<Topic> ItemRepository { get; set; } = null!;

    [Parameter, EditorRequired] public EventCallback OnImportSuccess { get; set; }
    private readonly CancellationTokenSource _cts = new();

    private async Task ImportDataAsync(InputFileChangeEventArgs e)
    {
        var importedTopics = new List<Topic>();
        foreach (var file in e.GetMultipleFiles())
        {
            using Stream stream = file.OpenReadStream();

            importedTopics = await JsonSerializer.DeserializeAsync<List<Topic>>(stream);
        }

        foreach (var topic in importedTopics)
        {
            await ItemRepository.AddAsync(topic, _cts.Token);
        }

        await OnImportSuccess.InvokeAsync();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
