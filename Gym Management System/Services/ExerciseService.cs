using Gym_Management_System.Contracts.Exercise;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;

namespace Gym_Management_System.Services;

public class ExerciseService(ApplicationDbContext context) : IExerciseService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<ExerciseResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var results = await _context.Exercises
                    .ProjectToType<ExerciseResponse>()
                     .ToListAsync(cancellationToken);
        return Result.Success(results.AsEnumerable());
    }
    public async Task<Result<ExerciseResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _context.Exercises.FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        if (result is null)
        {
            return Result.Failure<ExerciseResponse>(ExerciseErrors.ExerciseNotFound);
        }
        var response = result.Adapt<ExerciseResponse>();
        return Result.Success(response);
    }
    public async Task<Result<ExerciseResponse>> AddAsync(ExerciseRequest request, CancellationToken cancellationToken)
    {
        var exercise = request.Adapt<Exercise>();

        await _context.Exercises.AddAsync(exercise, cancellationToken);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Failure<ExerciseResponse>(ExerciseErrors.ExerciseExists);
        }

        return Result.Success(exercise.Adapt<ExerciseResponse>());
    }
    public async Task<Result> UpdateAsync(int id, ExerciseRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
            return Result.Failure(ExerciseErrors.ExerciseNotFound);

        request.Adapt(entity);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result.Failure(ExerciseErrors.ExerciseExists);
        }

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (exercise is null)
            return Result.Failure(ExerciseErrors.ExerciseNotFound);

        exercise.IsActive = !exercise.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();


    }
}
