using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors.Infrastructure;
using MudBlazor;
using System.Security.Claims;
using WebAppIntegrated.Constants;
using CustomerWebApp.Components.Carts;
using CustomerWebApp.Components.Carts.ViewModel;
using CustomerWebApp.Service.Cart;

namespace CustomerWebApp.Components.Layout
{
    public partial class MainLayout
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; } = null!;

        [Inject]
        private NavigationManager Navigation { get; set; } = null!;

        [Inject]
        private ICartService CartService { get; set; } = null!;

        //[Inject]
        //private INotificationService NotiService { get; set; } = null!;

        
        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        private CartState CartState { get; set; }


        //private HubConnection? _hubConnection;
        private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";
        private string _username = string.Empty;
        private CartVm cartVm = new();
       // private List<NotificationVm> _notificationVm = new();
        //private MarkAsReadRequest _mARrequest = new();
        public Guid UserId = Guid.Parse("BCF83D3E-BC97-4813-8E2C-96FD34863EA8");
        private string Search { get; set; } = "";
        public int Quantity;
        public int top = 5;

        protected override async Task OnInitializedAsync()
        {
            //AuthenticationState authState = await AuthStateTask;
            //List<Claim> claims = authState.User.Claims.ToList();

            //if (!authState.User.Identity.IsAuthenticated)
            //{
            //    return;
            //}

            //string stringUserId = authState.User?.Claims.FirstOrDefault(x => x.Type == "userId")!.Value;
            //if (stringUserId != null)
            //{
            //    await SigNalR(claims);

            //}
            //UserId = new(stringUserId);

            //Result<NavInfo> result = await UserService.GetNavInfo(UserId);
            //_imageUrl += result.Value.ImageUrl;
            //_username = result.Value.Username;

            await GetQuantityCart();
            //await GetNotification();
            CartState.OnChange += StateHasChanged;
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

        //private async Task GetNotification()
        //{
        //    var result = await NotiService.GetNotification(UserId, top);
        //    if (result.IsSuccess && result.Value != null)
        //    {
        //        _notificationVm = result.Value;
        //    }
        //}

        //private async Task MarkAsRead(Guid id)
        //{
        //    _mARrequest.Id = id;

        //    var result = await NotiService.MarkAsRead(_mARrequest);

        //    if (result.IsSuccess)
        //    {

        //        var notification = _notificationVm.FirstOrDefault(n => n.Id == id);
        //        if (notification != null)
        //        {
        //            notification.IsRead = true;
        //        }
        //        StateHasChanged();
        //    }

        //}

        private bool _isNotificationsVisible;

        private void ToggleNotifications()
        {
            _isNotificationsVisible = !_isNotificationsVisible;
        }
        private void OnPopoverClosed()
        {
            _isNotificationsVisible = false;
        }
        //private async Task SigNalR(List<Claim> claims)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var identity = new ClaimsIdentity(claims, "token");
        //    var principal = new ClaimsPrincipal(identity);
        //    var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        //    {
        //        Subject = identity,
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(
        //            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FTeJLDt3wY7zTvH4X0i8g5iPANBh6J4x")),
        //            SecurityAlgorithms.HmacSha256Signature)
        //    });

        //    var tokenString = tokenHandler.WriteToken(token);

        //    _hubConnection = new HubConnectionBuilder()
        //        .WithUrl("https://localhost:1000/shophub", options =>
        //        {
        //            options.AccessTokenProvider = async () => await Task.FromResult(tokenString);
        //        }).Build();

        //    _hubConnection.On<string>("SendOrderUpdate", async (message) =>
        //    {
        //        _notificationVm.Add(new NotificationVm
        //        {
        //            Message = message,
        //            IsRead = false,
        //            CreateDate = DateTime.Now
        //        });

        //        await GetNotification();
        //        StateHasChanged();
        //        Snackbar.Add("B?n có 1 thông báo m?i.", Severity.Success);
        //        //InvokeAsync(StateHasChanged);
        //        //Snackbar.Add("B?n có 1 thông báo m?i.T?i l?i trang ?? ??c", Severity.Success);
        //    });
        //    await _hubConnection.StartAsync();
        //}

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
}