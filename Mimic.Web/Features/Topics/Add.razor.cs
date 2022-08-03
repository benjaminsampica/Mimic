namespace Mimic.Web.Features.Topics;

public partial class Add : IDisposable
{
    [Inject] private IRepository<Topic> TopicRepository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, EditorRequired] public EventCallback OnSuccessfulSubmit { get; set; }

    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmitAsync(AddEditTopicRequest request)
    {
        var topic = new Topic
        {
            Body = request.Body,
            Name = request.Topic,
            Tags = request.FormattedTags
        };

        await TopicRepository.AddAsync(topic, _cts.Token);

        Snackbar.Add("Successfully added item!", Severity.Success);

        await OnSuccessfulSubmit.InvokeAsync();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}