using Gym_Management_System.Abstractions;
using Gym_Management_System.Abstractions.Consts;
using Gym_Management_System.Contracts.Account;
using Gym_Management_System.Contracts.Trainer;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;

namespace Gym_Management_System.Services;

public class TrainerService(UserManager<ApplicationUser> userManager,ApplicationDbContext context,RoleManager<ApplicationRole> roleManager) : ITrainerService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;



    public async Task<Result<IEnumerable<GetTrainerResponse>>> GetAllTrainers()
    {
        var trainersData = await _context.Users
            .Include(u => u.Trainer)
            .Where(u => u.Trainer != null)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Trainer!.Specialization,
                u.Trainer.IsActive,
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
            })
            .ToListAsync();

        var response = trainersData.Select(t => new GetTrainerResponse(
            t.Id,
            t.FirstName,
            t.LastName,
            t.Specialization,
            t.IsActive,
            t.Roles!
        ));

        return Result.Success(response);
    }



    public async Task<Result> AddTrainerAsync(AddTrainerRequest request, CancellationToken cancellationToken = default)
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
                EmailConfirmed = true,
                NormalizedEmail = request.Email.ToUpper(),
                NormalizedUserName = request.Email.ToUpper()
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Any(e => e.Code == "DuplicateEmail")
                            ? UserErrors.DuplicatedEmail
                            : UserErrors.InvalidCredentials;
                return Result.Failure<UserProfileResponse>(error);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Trainer.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(UserErrors.InvalidRoles);
                    
            }
            var trainer = new Trainer
            {
                UserId = user.Id, 
                Specialization = request.Specialization,
                IsActive = true
            };

            await _context.Trainers.AddAsync(trainer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success();

        }
        catch {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure(UserErrors.UpdateFailed);

        }
    }

}
