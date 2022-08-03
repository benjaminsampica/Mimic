namespace Mimic.Web.Features.Items;

public partial class Add : IDisposable
{
    [Inject] private IRepository<Topic> TopicRepository { get; set; } = null!;

    [Parameter, EditorRequired] public EventCallback OnSuccessfulSubmit { get; set; }

    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmitAsync(AddEditItemRequestAttempt attempt)
    {
        if (attempt.IsValid)
        {
            var topic = new Topic
            {
                Body = attempt.Model.Body,
                Topic = attempt.Model.Topic
            };

            await TopicRepository.AddAsync(, _cts.Token);

            await OnSuccessfulSubmit.InvokeAsync();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}