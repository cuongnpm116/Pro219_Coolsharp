using Application.Cqrs.Customer;
using Application.Cqrs.Customer.UpdateAvatar;
using Application.Cqrs.Customer.UpdateProfile;
using Application.IRepositories;
using Application.IServices;
using Common.Consts;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
    private readonly IStorageService _storageService;
    
    public CustomerRepository(AppDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
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
}
