using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Infrastructure.Services;

public class IdentityService(UserManager<ApplicationUser> userManager,ApplicationDbContext context,SignInManager<ApplicationUser> signInManager) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password) => await _userManager.CreateAsync(user, password);
    

    public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword) => await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

    public async Task<ApplicationUser?> GetByIdAsync(string id) => await _userManager.FindByIdAsync(id);

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user,string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }
    public async Task<IList<string>> GetRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return [];

        return await _userManager.GetRolesAsync(user);
    }
    public async Task<ApplicationUser?> GetUserWithTrainerAsync(string userId)
    {
        return await _context.Users
            .Include(x => x.Trainer)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user) => await _userManager.UpdateAsync(user);

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string role) => await _userManager.IsInRoleAsync(user, role);



    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
    {
        return await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: lockoutOnFailure);
    }

    public  async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public  async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user, token);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
    {
        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<bool> VerifyUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose, string token)
    {
        return await _userManager.VerifyUserTokenAsync(user, tokenProvider, purpose, token);
    }

    public async Task<string> GenerateUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose)
    {
        return await _userManager.GenerateUserTokenAsync(user, tokenProvider, purpose);
    }

    public async Task<ApplicationUser?> GetUserWithRefreshTokensAsync(string userId)
    {
        return await _context.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
