using CustomerWebApp.Components.Carts;
using CustomerWebApp.Service.Cart;
using CustomerWebApp.Service.Cart.ViewModel;
using CustomerWebApp.Service.Customer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;
using WebAppIntegrated.Constants;
using WebAppIntegrated.SignalR;

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
    private ICustomerService CustomerService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private CartState CartState { get; set; }
    [Inject]
    private SignalRService SignalRService { get; set; } = null!;

    private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";
    private string _username = string.Empty;
    private CartVm cartVm = new();

    public Guid UserId;
    private string Search { get; set; } = "";
    public int Quantity;

    protected override async Task OnInitializedAsync()
    {
        AuthenticationState authState = await AuthStateTask;
        List<Claim> claims = authState.User.Claims.ToList();

        if (!authState.User.Identity.IsAuthenticated)
        {
            return;
        }

        string stringUserId = authState.User?.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;
        UserId = new(stringUserId);
        var result = await CustomerService.GetUserProfile(UserId);
        _imageUrl += result.Value.ImageUrl;
        _username = result.Value.Username;

        await SignalRService.InitializeSignalRConnection(claims);
        if (SignalRService == null || SignalRService._hubConnection == null)
        {
            throw new ArgumentNullException(nameof(SignalRService), "SignalRService is null or HubConnection is not initialized.");
        }

        SignalRService._hubConnection.On<string>("ReceiveMessage", (message) =>
        {
            Snackbar.Add($"{message}", Severity.Success);
        });

        await GetQuantityCart();
        CartState.OnChange += StateHasChanged;
    }

    public async ValueTask DisposeAsync()
    {
        await SignalRService.DisposeAsync();
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