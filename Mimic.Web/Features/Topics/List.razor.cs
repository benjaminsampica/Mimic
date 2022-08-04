using Microsoft.JSInterop;
using static Mimic.Web.Features.Topics.ListTopicResponse;

namespace Mimic.Web.Features.Topics;

public partial class List : IDisposable
{
    [Inject] private IRepository<Topic> TopicRepository { get; set; } = null!;
    [Inject] private IConfirmDialogService ConfirmDialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    private readonly CancellationTokenSource _cts = new();
    private bool _showAddForm = false;
    private ListTopicResponse? _result;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task OnSuccessfulAddAsync()
    {
        _showAddForm = false;
        await LoadDataAsync();
    }

    private async Task OnCopyClickAsync(string id)
    {
        var item = await TopicRepository.FindAsync(id, _cts.Token);

        Snackbar.Add("Copied topic body into clipboard.", Severity.Info);

        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", item!.Body);
    }

    private async Task LoadDataAsync()
    {
        _result = null;
        var topics = await TopicRepository.GetAllAsync(_cts.Token);

        if (!topics.Any()) return;

        var result = new ListTopicResponse();
        foreach (var topic in topics)
        {
            var topicResponse = new TopicResponse
            {
                Id = topic.Id,
                Summary = topic.Summary,
                Tags = topic.Tags
            };

            result.Topics.Add(topicResponse);
        }

        _result = result;
    }

    private async Task RemoveAsync(string id)
    {
        if (await ConfirmDialogService.IsCancelledAsync()) return;

        await TopicRepository.RemoveAsync(id, _cts.Token);

        Snackbar.Add("Successfully removed item!", Severity.Success);

        await LoadDataAsync();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

public class ListTopicResponse
{
    public List<TopicResponse> Topics { get; set; } = new();

    public class TopicResponse
    {
        public string Id { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
        public bool ShowDetails { get; set; }
    }
}
