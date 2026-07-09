using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Domain.Repositories;

public interface IIdentityService
{

    Task<IdentityResult> CreateAsync(ApplicationUser user,string password);

    Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);

    Task<IdentityResult> UpdateAsync(ApplicationUser user);

    Task<IdentityResult> AddToRoleAsync(ApplicationUser user,string role);

    Task<ApplicationUser?> GetUserWithTrainerAsync(string userId);

    Task<bool> IsInRoleAsync(ApplicationUser user, string role);

    Task<ApplicationUser?> GetByIdAsync(string id);

    Task<IList<string>> GetRolesAsync(string userId);

    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure);
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
    Task<bool> VerifyUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose, string token);
    Task<string> GenerateUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose);
    Task<ApplicationUser?> GetUserWithRefreshTokensAsync(string userId);
}
