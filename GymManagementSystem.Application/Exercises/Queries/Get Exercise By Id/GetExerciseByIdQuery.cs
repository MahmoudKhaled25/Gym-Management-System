using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.Exercises.Queries.Get_Exercise_By_Id;

public record GetExerciseByIdQuery(int Id) : IRequest<Result<ExerciseDto>>;
