using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Errors;
using GymManagementSystem.Domain.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Queries.Get_Exercise_By_Id;

public class GetExerciseByIdQueryHandler(IRepository<Exercise> repository) : IRequestHandler<GetExerciseByIdQuery, Result<ExerciseDto>>
{
    private readonly IRepository<Exercise> _repository = repository;

    public async Task<Result<ExerciseDto>> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var exercise = await _repository.GetByIdAsync(request.Id);
        if (exercise == null)
            return Result.Failure<ExerciseDto>(ExerciseErrors.ExerciseNotFound);

        var exerciseDto = exercise.Adapt<ExerciseDto>();

        return Result.Success(exerciseDto);

    }
}
