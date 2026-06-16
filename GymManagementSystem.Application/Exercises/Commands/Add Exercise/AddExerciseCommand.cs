using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
namespace GymManagementSystem.Application.Exercises.Commands.Add_Exercise;

public record AddExerciseCommand(string Name, string Description, string MuscleGroup) : IRequest<Result<ExerciseDto>>;

