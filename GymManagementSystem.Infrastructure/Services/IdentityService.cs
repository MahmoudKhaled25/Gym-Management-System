using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Infrastructure.Services;

public class IdentityService(UserManager<ApplicationUser> userManager,ApplicationDbContext context) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password) => await _userManager.CreateAsync(user, password);
    

    public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword) => await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

    public async Task<ApplicationUser?> GetByIdAsync(string id) => await _userManager.FindByIdAsync(id);

    public async Task<IdentityResult> AddToRoleAsync(
     ApplicationUser user,
     string role)
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
   
}
