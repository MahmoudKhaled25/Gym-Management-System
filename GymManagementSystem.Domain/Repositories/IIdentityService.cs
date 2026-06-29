using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Domain.Repositories;

public interface IIdentityService
{

    Task<IdentityResult> CreateAsync(ApplicationUser user,string password);

    Task<IdentityResult> UpdateAsync(ApplicationUser user);

    Task<IdentityResult> AddToRoleAsync(ApplicationUser user,string role);

    Task<ApplicationUser?> GetUserWithTrainerAsync(string userId);

    Task<bool> IsInRoleAsync(ApplicationUser user, string role);

    Task<ApplicationUser?> GetByIdAsync(string id);

    Task<IList<string>> GetRolesAsync(string userId);
}
