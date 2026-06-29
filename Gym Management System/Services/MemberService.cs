using Gym_Management_System.Abstractions;
using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Member;
using Gym_Management_System.Enums;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using GymManagementSystem.Abstractions;
using GymManagementSystem.Contracts.Common;
using GymManagementSystem.Contracts.Member;
using GymManagementSystem.Entities;
using GymManagementSystem.Enums;
using GymManagementSystem.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management_System.Services;

public class MemberService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,ApplicationDbContext context) : IMemberService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaginatedList<MemberSummaryResponse>>> GetAllMembersAsync(RequestFilters filters,CancellationToken cancellationToken = default)
    {
        var query = _context.Users
         .Where(u => _context.UserRoles
        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
        .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name) &&
                (string.IsNullOrEmpty(filters.SearchValue) ||
                 u.FirstName.Contains(filters.SearchValue) ||
                 u.LastName.Contains(filters.SearchValue) ||
                 u.Email!.Contains(filters.SearchValue)));

        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "fullname" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.FirstName)
                : query.OrderBy(u => u.FirstName),

            "email" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),

            _ => query.OrderBy(u => u.FirstName) 
        };

        var finalQuery = sortedQuery.Select(u => new MemberSummaryResponse(
            u.Id,
            $"{u.FirstName} {u.LastName}",
            u.Email!,
            u.PhoneNumber,
            u.Gender,
            u.Trainer == null ? null : $"{u.Trainer.ApplicationUser!.FirstName} {u.Trainer.ApplicationUser.LastName}",
            u.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .Select(s => s.MembershipPlan!.Name)
                .FirstOrDefault(),
            u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow
        ));

        var result = await PaginatedList<MemberSummaryResponse>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return Result.Success(result);
    }
    public async Task<Result<PaginatedList<MemberSummaryResponse>>> GetActiveMembersAsync(RequestFilters filters, CancellationToken cancellationToken)
    {
        var query = _context.Users
    .Where(u => _context.UserRoles
     .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
     .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name) &&
             (string.IsNullOrEmpty(filters.SearchValue) ||
              u.FirstName.Contains(filters.SearchValue) ||
              u.LastName.Contains(filters.SearchValue) ||
              u.Email!.Contains(filters.SearchValue)) && (u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow));

        var sortedQuery = filters.SortColumn?.ToLower() switch
        {
            "fullname" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.FirstName)
                : query.OrderBy(u => u.FirstName),

            "email" => filters.SortDirection == "DESC"
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),

            _ => query.OrderBy(u => u.FirstName)
        };

        var finalQuery = sortedQuery.Select(u => new MemberSummaryResponse(
            u.Id,
            $"{u.FirstName} {u.LastName}",
            u.Email!,
            u.PhoneNumber,
            u.Gender,
            u.Trainer == null ? null : $"{u.Trainer.ApplicationUser!.FirstName} {u.Trainer.ApplicationUser.LastName}",
            u.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .Select(s => s.MembershipPlan!.Name)
                .FirstOrDefault(),
            u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow
        ));

        var result = await PaginatedList<MemberSummaryResponse>.CreateAsync(
            finalQuery,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return Result.Success(result);
    }

    public async Task<Result<UserProfileResponse>> GetMemberAsync(string memberId, CancellationToken cancellationToken = default)
    {
        var m = await _context.Users
            .Include(u => u.ProfileImage)
            .Where(u => u.Id == memberId &&
                        _context.UserRoles
                            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                            .Any(x => x.UserId == u.Id && x.Name == DefaultRoles.Member.Name))
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.DateOfBirth,
                u.Weight,
                u.Height,
                u.Gender,
                u.ProfileImage,
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (m is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        return Result.Success(MapToResponse(m.Id, m.Email!, m.FirstName, m.LastName,
            m.DateOfBirth, m.Weight, m.Height, m.Gender, m.ProfileImage, m.Roles!));
    }

    public async Task<Result<UserProfileResponse>> AddMemberAsync(AddMemberRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Weight = request.Weight,
                Height = request.Height,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Any(e => e.Code == "DuplicateEmail")
                    ? UserErrors.DuplicatedEmail
                    : UserErrors.InvalidCredentials;
                return Result.Failure<UserProfileResponse>(error);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Member.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<UserProfileResponse>(UserErrors.InvalidRoles);
            }

            await transaction.CommitAsync(cancellationToken);

            return Result.Success(MapToResponse(user.Id, user.Email!, user.FirstName, user.LastName,
                user.DateOfBirth, user.Weight, user.Height, user.Gender, null,
                new List<string> { DefaultRoles.Member.Name }));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<UserProfileResponse>(UserErrors.InvalidCredentials);
        }
    }

    public async Task<Result> ToggleStatusAsync(string memberId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(memberId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        user.LockoutEnd = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow
            ? DateTimeOffset.UtcNow.AddYears(100)
            : null;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? Result.Success() : Result.Failure(UserErrors.UpdateFailed);
    }
    private UserProfileResponse MapToResponse(
        string id, string email, string firstName, string lastName,
        DateOnly dateOfBirth, float weight, float height,
        Gender gender, UploadedFile? profileImage, List<string> roles) =>
        new UserProfileResponse(
            id, email, firstName, lastName, dateOfBirth, weight, height,
            profileImage?.RelativePath ?? (gender == Gender.Male
                ? FileSettings.MaleDefaultImage
                : FileSettings.FemaleDefaultImage),
            gender,
            roles
        );
}
