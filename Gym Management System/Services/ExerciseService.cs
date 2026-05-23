using Gym_Management_System.Contracts.Exercise;
using Gym_Management_System.Errors;
using Gym_Management_System.Persistence;
using Microsoft.Extensions.Caching.Memory;

namespace Gym_Management_System.Services;

public class ExerciseService(ApplicationDbContext context, IMemoryCache memoryCache) : IExerciseService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _exercisesCacheKey = "exercises_cache";

    public async Task<Result<IEnumerable<ExerciseResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var results = await _context.Exercises
                    .ProjectToType<ExerciseResponse>()
                     .ToListAsync(cancellationToken);
        
        _memoryCache.Set(_exercisesCacheKey, results, TimeSpan.FromMinutes(30));

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
