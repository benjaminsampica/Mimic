namespace Mimic.Web.Features.Topics;

public partial class Edit : IDisposable
{
    [Inject] private IRepository<Topic> TopicRepository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, EditorRequired] public string Id { get; set; } = null!;
    [Parameter, EditorRequired] public EventCallback OnSuccessfulSubmit { get; set; }

    private readonly AddEditTopicRequest _form = new();
    private readonly CancellationTokenSource _cts = new();

    protected override async Task OnInitializedAsync()
    {
        var topic = (await TopicRepository.FindAsync(Id, _cts.Token))!;
        _form.Topic = topic.Name;
        _form.Body = topic.Body;
        _form.Tags = string.Join(",", topic.Tags);
    }

    private async Task OnSubmitAsync(AddEditTopicRequest request)
    {
        var topic = (await TopicRepository.FindAsync(Id, _cts.Token))!;
        topic.Name = request.Topic;
        topic.Body = request.Body;
        topic.Tags = request.FormattedTags;

        await TopicRepository.RemoveAsync(Id, _cts.Token);
        await TopicRepository.AddAsync(topic, _cts.Token);

        Snackbar.Add("Successfully edited item!", Severity.Success);

        await OnSuccessfulSubmit.InvokeAsync();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}