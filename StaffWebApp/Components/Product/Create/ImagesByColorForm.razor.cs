using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Product.Vms.Create;

namespace StaffWebApp.Components.Product.Create;
public partial class ImagesByColorForm
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Parameter]
    public ColorForSelectVm Color { get; set; }

    public List<ImageVm> Images { get; set; } = [];

    private MudFileUpload<IReadOnlyList<IBrowserFile>> _fileUpload;
    private string ErrMsg = string.Empty;
    private string _msg = string.Empty;

    protected override void OnInitialized()
    {
        ErrMsg = $"Hãy tải lên ít nhất 1 ảnh cho màu {Color.Name}";
    }

    private Task OpenFilePickerAsync() => _fileUpload.OpenFilePickerAsync() ?? Task.CompletedTask;

    private async Task OnInputFileChanged(InputFileChangeEventArgs e)
    {
        _msg = string.Empty;
        var files = e.GetMultipleFiles(99);

        if (!ValidateFileCount(files) || !ValidateFiles(files))
        {
            return;
        }

        await AddValidFiles(files);
    }

    private async Task AddValidFiles(IReadOnlyList<IBrowserFile> files)
    {
        foreach (var file in files)
        {
            Images.Add(new ImageVm
            {
                File = file,
                Binary = await GetImageBinary(file)
            });
        }
    }

    private async Task<string> GetImageBinary(IBrowserFile file)
    {
        using var stream = file.OpenReadStream(10 * 1024 * 1024);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        string base64 = Convert.ToBase64String(memoryStream.ToArray());
        string imageBinary = $"data:{file.ContentType};base64,{base64}";
        return imageBinary;
    }

    private bool ValidateFileCount(IReadOnlyList<IBrowserFile> files)
    {
        if (files.Count > 5)
        {
            Snackbar.Add("Số lượng ảnh tối đa là 5", Severity.Error);
            return false;
        }
        return true;
    }

    private bool ValidateFiles(IReadOnlyList<IBrowserFile> files)
    {
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".jfif" };
        string overSizeFiles = string.Empty;
        string invalidFiles = string.Empty;

        foreach (var item in files)
        {
            if (item.Size > 10 * 1024 * 1024)
            {
                overSizeFiles += $" {item.Name},";
            }
            if (!allowedExtensions.Contains(Path.GetExtension(item.Name).ToLower()))
            {
                invalidFiles += $" {item.Name},";
            }
        }

        bool valid = true;

        if (!string.IsNullOrEmpty(overSizeFiles))
        {
            Snackbar.Add($"Các file {overSizeFiles} vượt quá kích thước cho phép. Vui lòng kiểm tra lại tệp trước khi tải lên", Severity.Error);
            valid = false;
        }

        if (!string.IsNullOrEmpty(invalidFiles))
        {
            Snackbar.Add($"Các file {invalidFiles} không đúng định dạng. Vui lòng kiểm tra lại tệp trước khi tải lên", Severity.Error);
            valid = false;
        }

        return valid;
    }

    public bool Validate()
    {
        if (Images.Count == 0)
        {
            _msg = ErrMsg;
            return false;
        }
        return true;
    }

    private async Task ClearImages()
    {
        await (_fileUpload.ClearAsync() ?? Task.CompletedTask);
        Images.Clear();
    }

    private void RemoveImage(ImageVm img) => Images.Remove(img);
}