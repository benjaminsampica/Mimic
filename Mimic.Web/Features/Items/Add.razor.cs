﻿using MudBlazor;

namespace Mimic.Web.Features.Items;

public partial class Add : IDisposable
{
    [Inject] private IRepository<Item> ItemRepository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, EditorRequired] public EventCallback OnSuccessfulSubmit { get; set; }

    private readonly CancellationTokenSource _cts = new();

    private async Task OnSubmitAsync(AddEditItemRequestAttempt attempt)
    {
        if (attempt.IsValid)
        {
            var item = new Item
            {
                Body = attempt.Model.Body,
                Topic = attempt.Model.Topic
            };

            await ItemRepository.AddAsync(item, _cts.Token);

            Snackbar.Add("Successfully added item!", Severity.Success);

            await OnSuccessfulSubmit.InvokeAsync();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}