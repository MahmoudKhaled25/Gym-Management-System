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



    public async Task<Result<IEnumerable<GetTrainerResponse>>> GetAllTrainersAsync()
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

    public async Task<Result<IEnumerable<GetTrainerResponse>>> GetActiveTrainersAsync()
    {
        var activeTrainers = await _context.Users
            .Include(x => x.Trainer)
            .Where(x => x.Trainer != null && x.Trainer.IsActive == true)
            .Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.Trainer!.Specialization,
                x.Trainer.IsActive,
                Roles = _context.UserRoles.Where(ur => ur.UserId == x.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
            }).ToListAsync();

        var response = activeTrainers.Select(t => new GetTrainerResponse(
           t.Id,
           t.FirstName,
           t.LastName,
           t.Specialization,
           t.IsActive,
           t.Roles!
       ));

        return Result.Success(response);

    }

    public async Task<Result<GetTrainerResponse>> GetTrainerByIdAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        var trainer = await _userManager.Users.Include(x => x.Trainer).FirstOrDefaultAsync(u => u.Id == trainerId, cancellationToken);
        if(trainer == null)
            return Result.Failure<GetTrainerResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(trainer);

        var response = new GetTrainerResponse(
            trainer.Id,
            trainer.FirstName,
            trainer.LastName,
            trainer.Trainer!.Specialization,
            trainer.Trainer.IsActive,
            roles
        );
        return Result.Success(response);
    }

    public async Task<Result<GetTrainerResponse>> AddTrainerAsync(AddTrainerRequest request, CancellationToken cancellationToken = default)
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
                return Result.Failure<GetTrainerResponse>(error);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Trainer.Name);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<GetTrainerResponse>(UserErrors.InvalidRoles);
                    
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

            var response = new GetTrainerResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                trainer.Specialization,
                trainer.IsActive,
                new List<string> { DefaultRoles.Trainer.Name }
            );

            return Result.Success(response);

        }
        catch {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<GetTrainerResponse>(UserErrors.UpdateFailed);

        }
    }

    public async Task<Result> UpdateTrainerAsync(string trainerId, UpdateTrainerRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Include(u => u.Trainer)
            .FirstOrDefaultAsync(u => u.Id == trainerId, cancellationToken);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        request.Adapt(user);
        if (user.Trainer != null)
        {
            user.Trainer.Specialization = request.Specialization;
        }
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return Result.Failure(UserErrors.UpdateFailed);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        var trainer = await _context.Trainers
            .FirstOrDefaultAsync(t => t.UserId == trainerId, cancellationToken);

        if (trainer is null)
            return Result.Failure(UserErrors.UserNotFound);

        trainer.IsActive = !trainer.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
