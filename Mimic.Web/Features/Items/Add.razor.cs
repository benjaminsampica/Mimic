using Blazored.TextEditor;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Mimic.Web.Features.Items;

public partial class Add : IDisposable
{
    [Inject] private IRepository<Item> ItemRepository { get; set; } = null!;

    [Parameter, EditorRequired] public EventCallback OnSuccessfulSubmit { get; set; }

    private BlazoredTextEditor _quillHtml = null!;
    private AddItemRequest _form = new();
    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmit(EditContext context)
    {
        var body = await _quillHtml.GetHTML();
        // Strip out any html tags since by default the input puts in a blank paragraph tag.
        _form.Body = Regex.Replace(body, "<.*?>", string.Empty);

        if (context.Validate())
        {
            var item = new Item
            {
                Topic = _form.Topic,
                Body = body
            };

            await ItemRepository.AddAsync(item, _cts.Token);

            await OnSuccessfulSubmit.InvokeAsync();

            _form = new();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

public class AddItemRequest
{
    [Required]
    public string Topic { get; set; }
    [Required]
    public string Body { get; set; }
}
