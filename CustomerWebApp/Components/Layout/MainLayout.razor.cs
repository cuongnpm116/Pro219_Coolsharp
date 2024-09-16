using CustomerWebApp.Components.Carts;
using CustomerWebApp.Service.Cart;
using CustomerWebApp.Service.Cart.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Components.Layout;
public partial class MainLayout
{
    [CascadingParameter]
    public Task<AuthenticationState> AuthStateTask { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ICartService CartService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private CartState CartState { get; set; }


    private HubConnection? _hubConnection;
    private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";
    private string _username = string.Empty;
    private CartVm cartVm = new();

    public Guid UserId = Guid.Parse("BCF83D3E-BC97-4813-8E2C-96FD34863EA8");
    private string Search { get; set; } = "";
    public int Quantity;
    public int top = 5;

    protected override async Task OnInitializedAsync()
    {

        await GetQuantityCart();
        _hubConnection = new HubConnectionBuilder()
        .WithUrl(Navigation.ToAbsoluteUri("https://localhost:1000/shophub"))
        .Build();

        _hubConnection.On<string>("ReceiveOrderUpdate", (message) =>
        {
            Snackbar.Add($"{message}", Severity.Success);
            InvokeAsync(StateHasChanged);
        });

        await _hubConnection.StartAsync();
        CartState.OnChange += StateHasChanged;
    }
    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    private async Task GetQuantityCart()
    {
        var response = await CartService.GetCart(UserId);
        if (response.IsSuccess && response.Value != null)
        {
            cartVm = response.Value;
            CartState.Quantity = cartVm.ListCart.Count;
        }
    }


    private bool _isNotificationsVisible;

    private void ToggleNotifications()
    {
        _isNotificationsVisible = !_isNotificationsVisible;
    }
    private void OnPopoverClosed()
    {
        _isNotificationsVisible = false;
    }


    public void Dispose()
    {
        CartState.OnChange -= StateHasChanged;
    }

    private void NavigateToSearchPage()
    {
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Search = Search.Trim();
            Navigation.NavigateTo($"products/search=" + Search);
            Search = string.Empty;
            StateHasChanged();
        }
    }


}