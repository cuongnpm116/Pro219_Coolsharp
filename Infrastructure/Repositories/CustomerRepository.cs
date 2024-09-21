using Application.Cqrs.Customer;
using Application.Cqrs.Customer.Create;
using Application.Cqrs.Customer.GetCustomerWithPagination;
using Application.Cqrs.Customer.UpdateAvatar;
using Application.Cqrs.Customer.UpdateProfile;
using Application.IRepositories;
using Application.IServices;
using Application.ValueObjects.Email;
using Application.ValueObjects.Pagination;
using Common.Consts;
using Common.Utilities;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Repositories;
internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IStorageService _storageService;
    private readonly IEmailService _emailService;

    public CustomerRepository(AppDbContext context, ITokenService tokenService, IStorageService storageService, IEmailService emailService)
    {
        _context = context;
        _tokenService = tokenService;
        _storageService = storageService;
        _emailService = emailService;
    }
    public async Task<Result<bool>> AddUser(CreateUserCommand request)
    {
        Customer customer = new Customer
        {
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            EmailAddress = request.EmailAddress,
            HashedPassword = HashUtility.Hash(request.Password)
        };

        await _context.Customers.AddAsync(customer);
        await CreateCartForCustomerAsync(customer.Id);

        return Result<bool>.Success(true);
    }

    private async Task CreateCartForCustomerAsync(Guid customerId)
    {

        Cart newCart = new()
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
        };
        await _context.Carts.AddAsync(newCart);

    }

    public async Task<Result<string>> Authenticate(string username, string password)
    {
        Customer? exist = await _context.Customers.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username);
        if (exist == null)
        {
            return Result<string>.Invalid("Tên tài khoản không tồn tại. Vui lòng nhập lại tên tài khoản");
        }

        if (!HashUtility.Verify(password, exist.HashedPassword))
        {
            return Result<string>.Invalid("Mật khẩu không chính xác. Vui lòng nhập lại mật khẩu");
        }


        Claim[] claims =
            [
                new("UserId", exist.Id.ToString()),
                new(ClaimTypes.Name, exist.Username),
                new(ClaimTypes.Email, exist.EmailAddress),

            ];

        string token = _tokenService.GenerateToken(claims);

        return Result<string>.Success(token);
    }

    public async Task<Result<bool>> ChangePassword(string username, string newPassword)
    {
        Customer? exist = await _context.Customers
            .FirstOrDefaultAsync(x => x.EmailAddress == username
            || x.Username == username);

        if (exist is null)
        {
            return Result<bool>.Invalid("người dùng không tồn tại");
        }

        exist.HashedPassword = HashUtility.Hash(newPassword);
        _context.Customers.Update(exist);

        return Result<bool>.Success(true);
    }

    public async Task<Result<string>> ForgetPassword(string request)
    {
        Customer? exist = await _context.Customers.AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmailAddress == request
            || x.Username == request);

        if (exist is null)
        {
            return Result<string>.Invalid("người dùng không tồn tại");
        }

        string confirmCode = StringUtility.GenerateOrderCode(6);
        var message = new Message([exist.EmailAddress], "Quên mật khẩu", confirmCode);
        await _emailService.SendEmail(message);
        return Result<string>.Success(confirmCode);
    }

    public async Task<Result<CustomerInfoVm>> GetCustomerById(Guid id, CancellationToken token)
    {
        CustomerInfoVm? customer = await _context.Customers
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new CustomerInfoVm
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                EmailAddress = u.EmailAddress,
                Gender = u.Gender,
                DateOfBirth = u.DateOfBirth,
                ImageUrl = u.ImageUrl,
            }).FirstOrDefaultAsync(token);

        if (customer == null)
        {
            return Result<CustomerInfoVm>.Invalid("Người dùng không tồn tại");
        }

        return Result<CustomerInfoVm>.Success(customer);
    }

    public async Task<Result<PaginationResponse<CustomerVm>>> GetUsersWithPagination(GetCustomerWithPaginationQuery query)
    {
        var customer = _context.Customers.AsNoTracking();
        var customerQuery = from s in _context.Customers
                            select new
                            {
                                s.Id,
                                FullName = s.LastName + " " + s.FirstName,
                                s.DateOfBirth,
                                s.EmailAddress,
                                s.Username,
                                s.ImageUrl,
                                s.Gender,
                                s.Status
                            };
        if (!string.IsNullOrEmpty(query.SearchString))
        {
            customerQuery = customerQuery.Where(s =>
            s.FullName.Contains(query.SearchString, StringComparison.CurrentCultureIgnoreCase) ||
            s.Username.Contains(query.SearchString, StringComparison.CurrentCultureIgnoreCase) ||
            s.EmailAddress.Contains(query.SearchString, StringComparison.CurrentCultureIgnoreCase));
        }
        var groupedStaffQuery = customerQuery
                .GroupBy(s => new { s.Id, s.FullName, s.DateOfBirth, s.EmailAddress, s.Username, s.Status, s.ImageUrl })
                .Select(g => new CustomerVm
                {
                    Id = g.Key.Id,
                    FullName = g.Key.FullName,
                    Dob = g.Key.DateOfBirth,
                    EmailAddress = g.Key.EmailAddress,
                    Username = g.Key.Username,
                    ImageUrl = g.Key.ImageUrl,
                });
        var response = await groupedStaffQuery.ToPaginatedResponseAsync(query.PageNumber, query.PageSize);
        foreach (var item in response.Data.Select(x => x))
        {
            var userOrderData = from c in _context.Customers
                                join o in _context.Orders on c.Id equals o.CustomerId
                                where c.Id == item.Id
                                group o by c.Id into g
                                select new
                                {
                                    g.Key,
                                    TotalOrder = g.Count(),
                                    TotalSpent = g.Sum(x => x.TotalPrice)
                                };
            var userOrder = await userOrderData.FirstOrDefaultAsync(x => x.Key == item.Id);
            item.TotalOrders = userOrder?.TotalOrder ?? 0;
            item.TotalSpent = userOrder?.TotalSpent ?? 0;
        }
        return Result<PaginationResponse<CustomerVm>>.Success(response);
    }

    public async Task<Result<bool>> IsUniqueEmailAddress(string emailAddress, CancellationToken token)
    {
        bool check = await CheckUniqueEmail(emailAddress, token);
        if (check)
        {
            Result<bool>.Invalid("Email đã được sử dụng. Vui lòng sử dụng Email khác");
        }
        return Result<bool>.Success(check);
    }

    public async Task<Result<bool>> IsUniqueUsername(string username, CancellationToken token)
    {
        bool check = await CheckUniqueUsername(username, token);

        return check ? Result<bool>.Success(check)
            : Result<bool>.Invalid("Tên tài khoản đã tồn tại. Vui lòng sử dụng tên tài khoản khác");
    }

    public async Task<Result<bool>> UpdateEmailAddress(Guid userId, string emailAddress, CancellationToken token)
    {
        bool isUniqueEmail = await CheckUniqueEmail(emailAddress, token);
        if (!isUniqueEmail)
        {
            return Result<bool>.Invalid("Email đã được sử dụng. Vui lòng sử dụng Email khác");
        }
        Customer? exist = await GetUserById(userId, token);
        if (exist is null)
        {
            return Result<bool>.Invalid("Người dùng này không tồn tại");
        }

        exist.EmailAddress = emailAddress;
        _context.Customers.Update(exist);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdatePersonalInformation(UpdateProfileCommand command, CancellationToken token)
    {
        Customer? exist = await GetUserById(command.CustomerId, token);
        if (exist is null)
        {
            return Result<bool>.Invalid("Người dùng không tồn tại");
        }

        bool isModified = false;

        if (exist.FirstName != command.FirstName)
        {
            exist.FirstName = command.FirstName;
            isModified = true;
        }

        if (exist.LastName != command.LastName)
        {
            exist.LastName = command.LastName;
            isModified = true;
        }

        if (exist.Gender != command.Gender)
        {
            exist.Gender = command.Gender;
            isModified = true;
        }

        if (exist.DateOfBirth != command.DateOfBirth)
        {
            exist.DateOfBirth = command.DateOfBirth;
            isModified = true;
        }

        if (isModified)
        {
            _context.Customers.Update(exist);
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdateUserAvatar(UpdateAvatarCommand command)
    {
        Customer? exist = await _context.Customers.FirstOrDefaultAsync(x => x.Id == command.CustomerId);
        if (exist is null)
        {
            return Result<bool>.Invalid("Người dùng không tồn tại");
        }
        string newImageUrl = await _storageService.SaveFileAsync(command.NewImage, StorageDirectory.UserContent);

        if (command.OldImageUrl != "default-profile.png")
        {
            await _storageService.DeleteFileAsync(command.OldImageUrl, StorageDirectory.UserContent);
        }

        exist.ImageUrl = newImageUrl;
        _context.Customers.Update(exist);

        return Result<bool>.Success(true);
    }

    private async Task<Customer?> GetUserById(Guid id, CancellationToken token)
       => await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, token);

    private async Task<bool> CheckUniqueEmail(string email, CancellationToken token)
       => await _context.Customers.AnyAsync(x => x.EmailAddress.Equals(email), token);

    private async Task<bool> CheckUniqueUsername(string username, CancellationToken token)
        => await _context.Customers.AnyAsync(x => x.Username.Equals(username), token);


}
