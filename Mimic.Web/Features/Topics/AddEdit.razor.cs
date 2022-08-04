using Blazored.TextEditor;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Mimic.Web.Features.Topics;

public partial class AddEdit
{
    [Parameter, EditorRequired] public EventCallback<AddEditTopicRequest> OnValidSubmit { get; set; }
    [Parameter, EditorRequired] public AddEditTopicRequest Model { get; set; } = null!;

    private BlazoredTextEditor _quillHtml = null!;

    private readonly CancellationTokenSource _cts = new();

    protected override async Task OnParametersSetAsync()
    {
        await _quillHtml.LoadHTMLContent(Model.Body);
    }

    private async Task OnSubmitAsync(EditContext context)
    {
        var body = await _quillHtml.GetHTML();
        // Strip out any html tags since by default the input puts in a blank paragraph tag.
        Model.Body = Regex.Replace(body, "<.*?>", string.Empty);

        if (context.Validate())
        {
            Model.Body = body;
            await OnValidSubmit.InvokeAsync(Model);
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

public class AddEditTopicRequest
{
    [Required]
    public string Summary { get; set; }
    [Required]
    public string Body { get; set; }
    public string Tags { get; set; }

    public string[]? FormattedTags => !string.IsNullOrEmpty(Tags) ? Tags.Split(",") : null;
}

