using GymManagementSystem.Application.WorkoutPlanExercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Queries.GetWorkoutPlanExercise;

public record GetWorkoutPlanExerciseQuery(int WorkoutPlanId) : IRequest<Result<WorkoutPlanExercisesGroupedDto>>;
