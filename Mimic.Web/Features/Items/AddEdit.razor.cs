using Blazored.TextEditor;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Mimic.Web.Features.Items;

public partial class AddEdit
{
    [Parameter, EditorRequired] public EventCallback<AddEditItemRequestAttempt> OnSubmit { get; set; }

    private BlazoredTextEditor _quillHtml = null!;
    private AddEditItemRequest _form = new();
    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmitAsync(EditContext context)
    {
        var body = await _quillHtml.GetHTML();
        // Strip out any html tags since by default the input puts in a blank paragraph tag.
        _form.Body = Regex.Replace(body, "<.*?>", string.Empty);

        var attempt = new AddEditItemRequestAttempt
        {
            Model = _form,
            IsValid = context.Validate()
        };

        await OnSubmit.InvokeAsync(attempt);

        if (attempt.IsValid) _form = new();
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

