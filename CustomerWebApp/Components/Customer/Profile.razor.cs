using CustomerWebApp.Service.Customer;
using CustomerWebApp.Service.Customer.Request;
using CustomerWebApp.Service.Customer.Vms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Components.Customer;

public partial class Profile
{
    [Parameter]
    public Guid CustomerId { get; set; }

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ICustomerService UserService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private CustomerInfoVm _customerProfileModel = new();
    private DateTime? _dob;
    private UpdateAvatarRequest _updateAvatarModel = new();
    private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";

    protected override async Task OnInitializedAsync()
    {
        Result<CustomerInfoVm> result = await UserService.GetUserProfile(CustomerId);
        if (!result.IsSuccess)
        {
            Snackbar.Add("Tải thông tin người dùng thất bại");
        }

        _customerProfileModel = result.Value!;
        _dob = _customerProfileModel.DateOfBirth; // phải gán sang field vì không thể gọi @bind-Date="_model.DateOfBirth"

        _updateAvatarModel.CustomerId = CustomerId;
        _updateAvatarModel.OldImageUrl = _customerProfileModel.ImageUrl;

        _imageUrl += _customerProfileModel.ImageUrl;
    }

    private async Task UpdateProfile()
    {
        UpdateProfileRequest updateProfile = new()
        {
            CustomerId = CustomerId,
            FirstName = _customerProfileModel.FirstName,
            LastName = _customerProfileModel.LastName,
            Gender = _customerProfileModel.Gender,
            DateOfBirth = (DateTime)_dob!,
        };

        Result<bool> result = await UserService.UpdateProfile(updateProfile);
        if (result.Value)
        {
            Snackbar.Add("Cập nhật hồ sơ thành công");
            await Task.Delay(1000);
            Navigation.NavigateTo(Navigation.Uri, true);
        }
        else
        {
            Snackbar.Add($"Cập nhật hồ sơ thất bại. {result.Message}");
        }
    }

    private async Task UpdateAvatar(IBrowserFile file)
    {
        _updateAvatarModel.NewImage = file;

        Result<bool> result = await UserService.UpdateAvatar(_updateAvatarModel);
        if (result.Value)
        {
            Snackbar.Add("Cập nhật ảnh đại diện thành công");
            await Task.Delay(1000);
            Navigation.NavigateTo(Navigation.Uri, true);
        }
        else
        {
            Snackbar.Add($"Cập nhật ảnh đại diện thất bại. {result.Message}");
        }
    }
}