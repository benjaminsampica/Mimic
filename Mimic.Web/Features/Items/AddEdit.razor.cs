using Blazored.TextEditor;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Mimic.Web.Features.Items;

public partial class AddEdit
{
    [Parameter, EditorRequired] public EventCallback<AddEditItemRequestAttempt> OnSubmit { get; set; }
    [Parameter, EditorRequired] public AddEditItemRequest Model { get; set; } = null!;

    private BlazoredTextEditor _quillHtml = null!;

    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmitAsync(EditContext context)
    {
        var body = await _quillHtml.GetHTML();
        // Strip out any html tags since by default the input puts in a blank paragraph tag.
        Model.Body = Regex.Replace(body, "<.*?>", string.Empty);

        var attempt = new AddEditItemRequestAttempt
        {
            Model = Model,
            IsValid = context.Validate()
        };

        await OnSubmit.InvokeAsync(attempt);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

public class AddEditItemRequestAttempt
{
    public bool IsValid { get; set; }
    public AddEditItemRequest Model { get; set; } = new();
}

public class AddEditItemRequest
{
    [Required]
    public string Topic { get; set; }
    [Required]
    public string Body { get; set; }
}

